using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin;
using System.Web.Http;
using Topshelf;
using IdentityServer3.AccessTokenValidation;

namespace KatanaServiceWindow
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return (int)HostFactory.Run(x =>
            {
                x.Service<OwinService>(s =>
                {
                    s.ConstructUsing(() => new OwinService());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });
            });
        }
    }

    public class OwinService
    {
        private IDisposable _webApp;

        public void Start()
        {
            _webApp = WebApp.Start<StartOwin>("http://localhost:6080");
        }

        public void Stop()
        {
            _webApp.Dispose();
        }
    }

    public class StartOwin
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            //JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                RequiredScopes = new[] { "api" },

                DelayLoadMetadata = true
            });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            app.UseWebApi(config);
        }
    }

    public class HelloWorldController : ApiController
    {
        public string Get()
        {
            return "Hello, World! Boris";
        }
    }

    
    public class TestController : ApiController
    {

        //[Authorize]
        [Route("test")]
        public string Get()
        {
            var stringClaimsBuilder = new StringBuilder();
            foreach (var claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
            {

                stringClaimsBuilder.Append($"{claim.Type} {claim.Value}");
            }

            return stringClaimsBuilder.ToString();
        }

        [Route("test/{id}")]
        public int GetId(int id)
        {
            return id;
        }
    }
}
