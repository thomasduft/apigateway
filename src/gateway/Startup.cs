using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway.GatewayApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(o =>
      {
        o.AddPolicy("AllowAllOrigins", builder =>
        {
          builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("X-Pagination");
        });
      });

      var authenticationProviderKey = "JwtBearerSchemeKey";

      services
        .AddAuthentication(opt =>
        {
          opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(authenticationProviderKey, opt =>
        {
          opt.Authority = "https://localhost:5004";
          opt.Audience = "gateway";
          opt.RequireHttpsMetadata = false;
          opt.TokenValidationParameters = new TokenValidationParameters()
          {
            ValidateIssuer = true,
            ValidateAudience = false,
          };
        });

      services.AddOcelot(this.Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseCors("AllowAllOrigins");

        IdentityModelEventSource.ShowPII = true;

        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      ConsiderSpaRoutes(app);

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseWebSockets();
      app.UseOcelot().Wait();
    }

    private static void ConsiderSpaRoutes(IApplicationBuilder app)
    {
      var angularRoutes = new[]
      {
        "/home",
        "/catalogs",
        "/orders"
      };

      app.Use(async (context, next) =>
      {
        if (context.Request.Path.HasValue
          && null != angularRoutes.FirstOrDefault(
            (ar) => context.Request.Path.Value.StartsWith(ar, StringComparison.OrdinalIgnoreCase)))
        {
          context.Request.Path = new PathString("/");
        }

        await next();
      });
    }
  }
}
