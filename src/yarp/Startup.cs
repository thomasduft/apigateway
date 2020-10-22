using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
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

    public Startup(IConfiguration configuration)
    {
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

      Configuration = configuration;
    }

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

      services.AddReverseProxy()
        .LoadFromConfig(Configuration.GetSection("ReverseProxy"));

      services.AddAccessTokenManagement();
      services.AddControllers();
      services.AddDistributedMemoryCache();

      // see: https://github.com/leastprivilege/AspNetCoreSecuritySamples/blob/aspnetcore21/BFF/src/Host/Startup.cs
      services.AddAuthentication(options =>
      {
        options.DefaultScheme = "cookies";
        options.DefaultChallengeScheme = "oidc";
      })
      .AddCookie("cookies", options =>
      {
        options.Cookie.Name = "bff";
        options.Cookie.SameSite = SameSiteMode.Strict;
      })
      .AddOpenIdConnect("oidc", options =>
      {
        // options.SignInScheme = "cookies";
        options.Authority = "https://localhost:5004";
        options.ClientId = "bff";
        // options.ClientSecret = "secret";

        options.ResponseType = "code";
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("backend-suite");
        options.Scope.Add("offline_access");

        options.TokenValidationParameters = new TokenValidationParameters
        {
          NameClaimType = "name",
          RoleClaimType = "role"
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
      app.UseCors("AllowAllOrigins");

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseAuthentication();
      // Force user to be authenticated
      app.Use(async (context, next) =>
      {
        if (!context.User.Identity.IsAuthenticated)
        {
          await context.ChallengeAsync();

          return;
        }

        await next();
      });

      app.UseDefaultFiles();
      app.UseStaticFiles();

      // ConsiderSpaRoutes(app);
      app.Use(async (context, next) =>
      {
        await next();

        if (context.Response.StatusCode == 404 && !context.Request.Path.Value.Contains("/api"))
        {
          context.Request.Path = new PathString("/");

          await next();
        }
      });

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapReverseProxy(proxyPipeline =>
        {
          proxyPipeline.Use((context, next) =>
          {
            var accessToken = context.GetUserAccessTokenAsync().GetAwaiter().GetResult();
            context.Request.Headers.Add("Authorization", $"Bearer {accessToken}");

            return next();
          });

          proxyPipeline.UseAffinitizedDestinationLookup();
          proxyPipeline.UseProxyLoadBalancing();
          proxyPipeline.UseRequestAffinitizer();
        });
        endpoints.MapControllers().RequireAuthorization();
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
