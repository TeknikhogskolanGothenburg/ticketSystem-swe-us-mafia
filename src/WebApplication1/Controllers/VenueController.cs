using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketSystem.DatabaseRepository;
using TicketSystemEngine;

namespace RESTapi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class VenueController : Controller
    {
        TicketDatabase database = new TicketDatabase();
        // GET: /venue
        [HttpGet]
        public IEnumerable<Venue> GetAllVenues()
        {           
            return database.AllVenues();
        }

        // GET: venue/search/query
        [HttpGet("search/{query}")]
        public IEnumerable<Venue> FindVenues (string query)
        {
            return database.VenuesFind(query);
        }

        // GET venue/5
        [HttpGet("{id}")]
        public Venue GetSpecificVenue(int id)
        {
            return database.FindVenueByID(id);
        }

        // POST: /venue
        [HttpPost]
        public void AddNewVenue([FromBody]Venue venue)
        {
            database.VenueAdd(venue.VenueName, venue.Address, venue.City, venue.Country);
        }
        
        // PUT: /venue/5
        // based on id of venue, change values
        [HttpPut("{id}")]
        public void UpdateVenue(int id, [FromBody]Venue venue)
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
        
        // DELETE: api/Venue/5
        [HttpDelete("{id}")]
        public void DeleteVenue(int id)
        {
            if (database.FindVenueByID(id) == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                database.DeleteVenue(id);
            }
        }
    }
}
