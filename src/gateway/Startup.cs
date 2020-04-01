using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        .AddAuthentication()
        .AddJwtBearer(authenticationProviderKey, cfg =>
        {
          cfg.Authority = "https://localhost:5004";
          cfg.Audience = "gateway";
          cfg.RequireHttpsMetadata = false;
        });

      // services
      //   .AddAuthentication(options =>
      //   {
      //     options.DefaultScheme = authenticationProviderKey;
      //     options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
      //   })
      //   .AddJwtBearer(authenticationProviderKey, cfg =>
      //   {
      //     cfg.Authority = "https://localhost:5004";
      //     cfg.Audience = "gateway";
      //     cfg.RequireHttpsMetadata = false;
      //   })
      //   .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
      //   {
      //     options.SignInScheme = authenticationProviderKey;
      //     options.Authority = "https://localhost:5004";
      //     options.RequireHttpsMetadata = true;
      //     options.ClientId = "gateway";
      //     // options.ClientSecret = "gateway_client_secret";
      //     options.ResponseType = "code";
      //     options.UsePkce = false;
      //     options.Scope.Add("profile");
      //     options.Scope.Add("offline_access");
      //     options.Scope.Add("catalog");
      //     options.Scope.Add("orders.full_access");
      //     options.Scope.Add("time");
      //     options.SaveTokens = true;
      //   });

      // services
      //   .AddAuthentication(options =>
      //   {
      //     options.DefaultScheme = "Cookies";
      //     options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
      //   })
      //   .AddCookie()
      //   .AddOpenIdConnect(options =>
      //   {
      //     options.SignInScheme = "Cookies";
      //     options.Authority = "https://localhost:5004";
      //     options.RequireHttpsMetadata = true;
      //     options.ClientId = "gateway";
      //     // options.ClientSecret = "codeflow_pkce_client_secret";
      //     options.ResponseType = "code";
      //     options.UsePkce = false;
      //     options.Scope.Add("profile");
      //     options.Scope.Add("offline_access");
      //     options.Scope.Add("catalog");
      //     options.Scope.Add("orders.full_access");
      //     options.Scope.Add("time");
      //     options.SaveTokens = true;
      //   });

      services.AddAuthorization();

      services.AddControllers();

      services.AddOcelot(this.Configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseCors("AllowAllOrigins");

        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseWebSockets();
      app.UseOcelot().Wait();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
