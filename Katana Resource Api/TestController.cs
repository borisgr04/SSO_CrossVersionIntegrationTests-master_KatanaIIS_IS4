using System.Text;
using System.Web.Http;

namespace Katana_Resource_Api
{
    [RoutePrefix("test")]
    public class TestController : ApiController
    {

        [Authorize]
        public string Get()
        {
            var stringClaimsBuilder = new StringBuilder();
           foreach(var claim in System.Security.Claims.ClaimsPrincipal.Current.Claims)
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