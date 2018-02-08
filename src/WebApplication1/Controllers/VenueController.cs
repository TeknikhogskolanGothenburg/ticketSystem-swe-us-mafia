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

        // GET: api/Venue/query
        [HttpGet("{query}")]
        public IEnumerable<Venue> Get(string query)
        {
            return database.VenuesFind(query);
        }
        
        // POST: api/Venue
        [HttpPost]
        public void Post([FromBody]string name, string address, string city, string country)
        {
            database.VenueAdd(name, address, city, country);
        }
        
        // PUT: api/Venue/id
        // based on id of venue, change values
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string name, string address, string city, string country)
        {
            database.UpdateVenue(id, name, address, city, country);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
