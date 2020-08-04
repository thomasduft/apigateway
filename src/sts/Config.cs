using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace ApiGateway.STS
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> Ids =>
      new IdentityResource[]
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
      };

    public static IEnumerable<ApiResource> Apis =>
      new ApiResource[]
      {
        new ApiResource
        {
          Name = "catalog",
          DisplayName = "catalog api",
          Scopes = {
           "catalog"
          }
        },
        new ApiResource
        {
          Name = "orders",
          DisplayName = "orders api",
          Scopes = {
            "orders.full_access",
            "orders.read_only"
          }
        },
        new ApiResource
        {
          Name = "time",
          DisplayName = "time api",
          Scopes = {
            "time"
          }
        }
      };

    public static IEnumerable<Client> Clients =>
      new Client[]
      {
        // SPA client using code flow + pkce
        new Client
        {
          ClientId = "frontend",
          ClientName = "SPA Client",
          ClientUri = "https://localhost:5000",
          AllowedGrantTypes = GrantTypes.Code,
          RequirePkce = true,
          RequireClientSecret = false,
          RedirectUris =
          {
            "https://localhost:5000",
            "http://localhost:4200"
          },
          PostLogoutRedirectUris = { "https://localhost:5000/index.html", "http://localhost:4200" },
          AllowedCorsOrigins = { "http://localhost:4200", "https://localhost:5000" },
          AllowedScopes = { "openid", "profile", "catalog", "orders.full_access", "time" }
        },
        new Client
        {
          ClientName = "gateway",
          ClientId = "gateway",
          ClientSecrets = {new Secret("gateway_client_secret".Sha256()) },
          AllowedGrantTypes = GrantTypes.Code,
          RequirePkce = false,
          RequireClientSecret = false,
          AllowOfflineAccess = true,
          AlwaysSendClientClaims = true,
          UpdateAccessTokenClaimsOnRefresh = true,
          AlwaysIncludeUserClaimsInIdToken = true,
          RedirectUris = {
            "https://localhost:5000/signin-oidc"
          },
          PostLogoutRedirectUris = {
            "https://localhost:5000/signout-callback-oidc"
          },
          AllowedScopes = new List<string>
          {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            IdentityServerConstants.StandardScopes.OfflineAccess,
            "catalog",
            "orders.full_access",
            "time"
            }
        }
      };

    public static List<TestUser> GetUsers()
    {
      return new List<TestUser>
      {
        new TestUser
        {
          SubjectId = "1",
          Username = "admin",
          Password = "password",
          Claims = {
            new Claim(JwtClaimTypes.Name, "admin")
          }
        },
        new TestUser
        {
          SubjectId = "2",
          Username = "alice",
          Password = "password",
          Claims = {
            new Claim(JwtClaimTypes.Name, "alice")
          }
        },
        new TestUser
        {
          SubjectId = "3",
          Username = "bob",
          Password = "password",
          Claims = {
            new Claim(JwtClaimTypes.Name, "bob")
          }
        }
      };
    }
  }
}