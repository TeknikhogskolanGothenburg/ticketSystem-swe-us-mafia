using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public IEnumerable<Venue> GetAllVenues()
        {           
            return database.AllVenues();
        }

        // GET: api/Venue/query
        [HttpGet("{query}")]
        public string GetSpecificVenue(string query)
        {
            return JsonConvert.SerializeObject(database.VenuesFind(query));
        }
        
        // POST: api/Venue
        [HttpPost]
        public void Post([FromBody]Venue venue)
        {
            database.VenueAdd(venue.VenueName, venue.Address, venue.City, venue.Country);
        }
        
        // PUT: api/Venue/id
        // based on id of venue, change values
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Venue venue)
        {
            if (database.FindVenueByID(id) == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                database.UpdateVenue(id, venue.VenueName, venue.Address, venue.City,venue.Country);
            }    
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
