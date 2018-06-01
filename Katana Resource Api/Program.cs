using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Logging;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Topshelf;


namespace Katana_Resource_Api
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Katana API using IS3";

            using (WebApp.Start<Startup>("http://localhost:6080"))
            {
                Console.ReadLine();
            }
        }
    
        internal class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                //app.SetLoggerFactory(new ConsoleLoggerFactory());

                //var config = new HttpConfiguration();
                //config.MapHttpAttributeRoutes();

                //config.Routes.MapHttpRoute(
                //    name: "DefaultApi",
                //    routeTemplate: "{controller}/{id}",
                //    defaults: new { id = RouteParameter.Optional }
                //    );

                //JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();
                //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
                //{
                //    Authority = "http://localhost:5000",
                //    RequiredScopes = new[] { "api" },

                //    DelayLoadMetadata = true
                //});

                //app.UseWebApi(config);


                app.Use((context, next) =>
                {
                    PrintCurrentIntegratedPipelineStage(context, "Middleware 1");
                    return next.Invoke();
                });
                app.Use((context, next) =>
                {
                    PrintCurrentIntegratedPipelineStage(context, "2nd MW");
                    return next.Invoke();
                });
                app.Run(context =>
                {
                    PrintCurrentIntegratedPipelineStage(context, "3rd MW");
                    return context.Response.WriteAsync("Hello world");
                });
            }

            private void PrintCurrentIntegratedPipelineStage(IOwinContext context, string msg)
            {
                var currentIntegratedpipelineStage = HttpContext.Current.CurrentNotification;
                context.Get<TextWriter>("host.TraceOutput").WriteLine(
                    "Current IIS event: " + currentIntegratedpipelineStage
                    + " Msg: " + msg);
            }
        }
       
    }
}
