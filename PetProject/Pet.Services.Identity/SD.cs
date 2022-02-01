using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Services.Identity
{
    public static class SD
    {

        public const String Admin = "Admin";
        public const String Customer = "Customer";
        public static IEnumerable<IdentityResource> IdentityResources = new List<IdentityResource>{


            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
             new IdentityResources.Profile(),

            

            };


        public static IEnumerable<ApiScope> Apiscopes = new List<ApiScope> {


            new ApiScope("pet","Pet Server"),
               new ApiScope(name:"read",displayName: "read your data"),
                new ApiScope(name:"write",displayName: "write your data"),
                 new ApiScope(name:"delete",displayName: "delete your data")
        };

        public static IEnumerable<Client> Clients = new List<Client> { new Client
        {
ClientId="pet",
ClientSecrets={new Secret("secret".Sha256()) },
AllowedGrantTypes=GrantTypes.Code,

RedirectUris={ "https://localhost:44321/signin-oidc" },
PostLogoutRedirectUris={ "https://localhost:44321/signout-callback-oidc" },
AllowedScopes   ={IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Email , IdentityServerConstants.StandardScopes.Profile,"pet" }



        }






        };







    }

}

