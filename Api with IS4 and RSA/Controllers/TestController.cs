using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api_with_IS4_and_RSA.Controllers
{
    

    public class TestController : ApiController
    {
        [Authorize]
        [Route("test")]
        public string Get()
        {
            return "OK";
        }
    }
}
