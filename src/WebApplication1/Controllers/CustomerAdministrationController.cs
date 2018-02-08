using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTapi.Controllers
{
    [Produces("application/json")]
    [Route("api/CustomerAdministration")]
    public class CustomerAdministrationController : Controller
    {
        // GET: api/CustomerAdministration
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CustomerAdministration/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/CustomerAdministration
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/CustomerAdministration/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
