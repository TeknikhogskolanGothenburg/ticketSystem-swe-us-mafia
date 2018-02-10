using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTapi.Controllers
{
    /// <summary>
    /// Här får vi hantera att lägga in ett event på ett specifikt datum och en specifik venue tror jag.
    /// </summary>
    [Produces("application/json")]
    [Route("api/TicketEventDate")]
    public class TicketEventDateController : Controller
    {
        // GET: api/TicketEventDate
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TicketEventDate/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/TicketEventDate
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/TicketEventDate/5
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
