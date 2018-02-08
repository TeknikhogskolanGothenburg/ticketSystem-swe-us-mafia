using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DatabaseRepository;
using TicketSystem.DatabaseRepository.Model;

namespace RESTapi.Controllers
{
    [Produces("application/json")]
    [Route("api/Venue")]
    public class VenueController : Controller
    {
        TicketDatabase database = new TicketDatabase();
        // GET: api/Venue
        [HttpGet]
        public IEnumerable<Venue> Get()
        {           
            return database.AllVenues();
        }

        // GET: api/Venue/5
        [HttpGet("{query}", Name = "Get")]
        public IEnumerable<Venue> Get(string query)
        {
            return database.VenuesFind(query);
        }
        
        // POST: api/Venue
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Venue/5
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
