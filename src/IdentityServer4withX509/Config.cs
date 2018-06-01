using IdentityServer4.Models;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer4withX509
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api" }
                },
                new Client
                {
                    ClientId = "urn:owinrp",
                    ProtocolType = ProtocolTypes.WsFederation,

                    //RedirectUris = { "http://localhost:10313/" },
                    //FrontChannelLogoutUri = "http://localhost:10313/home/signoutcleanup",
                    //IdentityTokenLifetime = 36000,

                    AllowedScopes = { "openid", "profile" }
                }
            };
        }
    }
}