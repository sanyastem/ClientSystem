using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ClientSystem.Controllers
{
    public class ClaimController : ApiController
    {
        // GET: api/Claim
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Claim/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Claim
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Claim/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Claim/5
        public void Delete(int id)
        {
        }
    }
}
