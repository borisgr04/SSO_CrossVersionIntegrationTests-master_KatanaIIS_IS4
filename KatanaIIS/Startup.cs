using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web;
using System.IO;
using Microsoft.Owin.Extensions;
using System.Web.Http;
using System.Text;
using IdentityServer3.AccessTokenValidation;

[assembly: OwinStartup(typeof(KatanaIIS.Startup))]

namespace KatanaIIS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                RequiredScopes = new[] { "api" },

                DelayLoadMetadata = true
            });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            app.UseWebApi(config);
        }
      
    }

    [RoutePrefix("test")]
    public class TestController : ApiController
    {

        [Authorize]
        public string Get()
        {
            var stringClaimsBuilder = new StringBuilder();
            foreach (var claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
            {

                stringClaimsBuilder.Append($"{claim.Type} {claim.Value}");
            }

            return stringClaimsBuilder.ToString();
        }

        public int GetId(int id)
        {
            return id;
        }
    }
}
