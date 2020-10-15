using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Proxy
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    public object JwtBearerDefaults { get; private set; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddReverseProxy()
        .LoadFromConfig(Configuration.GetSection("ReverseProxy"));

      services.AddAuthentication()
        .AddJwtBearer(opt =>
        {
          opt.Authority = "https://localhost:5004";
          opt.Audience = "backend-suite";
          opt.RequireHttpsMetadata = false;
          opt.TokenValidationParameters = new TokenValidationParameters()
          {
            ValidateIssuer = true,
            ValidateAudience = false,
          };
        });

      services.AddAuthorization(options =>
        {
          options.AddPolicy("IsAuthorized", builder => builder
              // See AccountController.Login
              // .RequireClaim("CustomClaim", "My Value")
              .RequireAuthenticatedUser());

          // Require authentication for all requests that do not specify another policy
          // options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      ConsiderSpaRoutes(app);

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseWebSockets();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapReverseProxy();
      });
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
