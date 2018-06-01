using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;

[assembly: OwinStartup(typeof(Api_with_IS4_and_RSA.Startup))]

namespace Api_with_IS4_and_RSA
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetLoggerFactory(new ConsoleLoggerFactory());
            //app.UseCookieAuthentication(new CookieAuthenticationOptions { AuthenticationType = "Cookies" });
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // turn off any default mapping on the JWT handler
            //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            //{
            //    Authority = "http://localhost:5001",
            //    RequiredScopes = new[] { "api" },

            //    ClientId = "api",
            //    ClientSecret = "secret",
            //    //ValidationMode=ValidationMode.Both,
            //    ValidationMode = ValidationMode.ValidationEndpoint,
            //    DelayLoadMetadata = true
            //});

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                RequiredScopes = new[] { "api" },

                DelayLoadMetadata = true
            });


            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            app.UseWebApi(config);

            //Federado
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = "Cookies"
            //});

            //app.UseWsFederationAuthentication(new WsFederationAuthenticationOptions
            //{
            //    MetadataAddress = "http://localhost:5000/wsfederation",
            //    Wtrealm = "urn:owinrp",

            //    SignInAsAuthenticationType = "Cookies"
            //});
        }
    }

    internal class ConsoleLoggerFactory : ILoggerFactory
    {
        public ILogger Create(string name)
        {
            return new ConsoleLogger();
        }

        private class ConsoleLogger : ILogger
        {
            public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
                Console.WriteLine(formatter(state, exception));
                return true;
            }
        }
    }
}
