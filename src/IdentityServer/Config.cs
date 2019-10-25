﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[] {
                new ApiResource("api1", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[] { new Client() {
                ClientId = "client",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "api1" }
            },
            // resource owner password grant client
            new Client
            {
                ClientId = "ro.client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "api1" }
            },

            // OpenID Connect implicit flow client (MVC)
            //new Client
            //{
            //    ClientId = "mvc",
            //    ClientName = "MVC Client",
            //    AllowedGrantTypes = GrantTypes.Implicit,

            //    // where to redirect to after login
            //    RedirectUris = { "http://localhost:5002/signin-oidc" },

            //    // where to redirect to after logout
            //    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

            //    AllowedScopes = new List<string>
            //    {
            //        IdentityServerConstants.StandardScopes.OpenId,
            //        IdentityServerConstants.StandardScopes.Profile
            //    }
            //}
            
            new Client
            {
                ClientId = "mvc",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Hybrid,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                RedirectUris           = { "http://localhost:5002/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                },
                AllowOfflineAccess = true
            },

            // SPA per new security guidance
            new Client
            {
                ClientId = "spa",
                ClientName = "SPA (Code + PKCE)",

                RequireClientSecret = false,
                RequireConsent = false,

                RedirectUris = { "https://notused" },
                PostLogoutRedirectUris = { "https://notused" },

                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = { "openid", "profile", "email", "api" },

                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse
            },

            // JavaScript Client
            new Client
            {
                ClientId = "js",
                ClientName = "JavaScript Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,

                RedirectUris =           { "http://localhost:5003/callback.html" },
                PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                AllowedCorsOrigins =     { "http://localhost:5003" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                }
            }


            };
        }
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",
                    Claims = new []
                        {
                            new Claim("name", "Alice"),
                            new Claim("website", "https://alice.com")
                        }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",
                    Claims = new []
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }
    }
}