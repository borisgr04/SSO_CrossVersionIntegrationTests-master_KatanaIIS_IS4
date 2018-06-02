# SSO_CrossVersionIntegrationTests-master_KatanaIIS_IS4
Este proyecto esta basado en CrossVersionIntegrationTests-master
Y Se implementa un Api basada en Katana/IIS protegida por IS4

# Est CrossVersionIntegrationTests
Test harness to ensure IdentityServer 3/4 compatibility

Please run every project as self-hosted.

Para crear un Proyecto WebApi .Net Framework compatible se debe realizar los siguientes pasos:

# 1. Crear un proyecto web vacio - sin escoger ningun tipo 
# 2. Instalar los siguientes paquetes Nuget:
    2.1 Katana                      :  Install-Package Microsoft.Owin.Host.SystemWeb -Version 4.0.0
    2.2 WebApi                      :  Install-Package Microsoft.AspNet.WebApi -Version 5.2.6
    2.3 WebApi para Owin(Katana)    :  Install-Package Microsoft.AspNet.WebApi.Owin -Version 5.2.6
    2.4 Client de IdentiyServer 3/4 :  Install-Package IdentityServer3.AccessTokenValidation -Version 2.15.1

# 3. Crear una Clase Owin Startup class

    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using IdentityServer3.AccessTokenValidation;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Owin;

    [assembly: OwinStartup(typeof(WebApiNet4x.Startup))]

    namespace WebApiNet4x
    {
        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseCookieAuthentication(new CookieAuthenticationOptions());
                DesactivarLaAsignacionPredeterminadaDeJwt();
                app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = "http://localhost:5000",
                    RequiredScopes = new[] { "Api1" },
                    DelayLoadMetadata = true,
                });

                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();
                config.EnableCors(new EnableCorsAttribute("*","*", "*"));

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                    );

                app.UseWebApi(config);

            }

            private void DesactivarLaAsignacionPredeterminadaDeJwt()
            {
                JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            }
        }
    }
