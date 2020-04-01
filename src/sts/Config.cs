// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

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
          Scopes =
          {
            new Scope()
            {
              Name = "catalog",
              DisplayName = "catalog api"
            }
          }
        },
        new ApiResource
        {
          Name = "orders",
          DisplayName = "orders api",
          Scopes =
          {
            new Scope()
            {
              Name = "orders.full_access",
              DisplayName = "full access to orders api"
            },
            new Scope()
            {
              Name = "orders.read_only",
              DisplayName = "read only access to orders api"
            }
          }
        },
        new ApiResource
        {
          Name = "time",
          DisplayName = "time api",
          Scopes =
          {
            new Scope()
            {
              Name = "time",
              DisplayName = "time api"
            }
          }
        }
      };

    public static IEnumerable<Client> Clients =>
      new Client[]
      {
        // SPA client using code flow + pkce
        new Client
        {
          ClientId = "spa",
          ClientName = "SPA Client",
          ClientUri = "http://localhost:5000",
          AllowedGrantTypes = GrantTypes.Code,
          RequirePkce = true,
          RequireClientSecret = false,
          RedirectUris =
          {
            "http://localhost:5000/index.html",
            "http://localhost:5000/callback.html",
            "http://localhost:5000/silent.html",
            "http://localhost:5000/popup.html"
          },
          PostLogoutRedirectUris = { "http://localhost:5000/index.html" },
          AllowedCorsOrigins = { "http://localhost:4200" },
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
  }
}