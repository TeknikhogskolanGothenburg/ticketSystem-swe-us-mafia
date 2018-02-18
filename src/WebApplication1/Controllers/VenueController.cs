using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketSystem.DatabaseRepository;
using TicketSystemEngine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
namespace RESTapi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class VenueController : Controller
    {
        TicketDatabase database = new TicketDatabase();
        readonly ILogger<VenueController> _logger;

        public VenueController(ILogger<VenueController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method that gets all Venues from database table Venues.
        /// </summary>
        /// <returns>A list (IEnumerable) of all Venues in the database.</returns>
        // GET: /venue
        [HttpGet]
        public IEnumerable<Venue> GetAllVenues()
        {
            _logger.LogInformation("Entered GetAllVenues in RestApi Venues Controller");
            return database.AllVenues();
        }

        /// <summary>
        /// Method that gets all Venues from database table Venues
        /// based on sent in query: VenueID or VenueName/part of VenueName,
        /// Address, City, Country.
        /// </summary>
        /// <param name="query">The search qritera for which we want to apply when searching for Venues.</param>
        /// <returns>A list (IEnumerable) of Venues according to specified search query.</returns>
        // GET: venue/search/query
        [HttpGet("search/{query}")]
        public IEnumerable<Venue> FindVenues (string query)
        {
            _logger.LogInformation("Entered FindVenues in RestApi Venues Controller");
            return database.VenuesFind(query);
        }

        /// <summary>
        /// Method that gets a specific Venue from database table Venues 
        /// based on specified VenueID.
        /// </summary>
        /// <param name="id">ID of the Venue we want information about.</param>
        /// <returns>A Venue object.</returns>
        // GET venue/5
        [HttpGet("{id}")]
        public Venue GetSpecificVenue(int id)
        {
            _logger.LogInformation("Entered GetSecificVenues in RestApi Venues Controller");
            return database.FindVenueByID(id);
        }

        /// <summary>
        /// Method that Adds a Venue to the database table Venues.
        /// </summary>
        /// <param name="venue">The values sent in to method are properties of
        /// a Venue object.</param>
        // POST: /venue
        [HttpPost]
        public void AddNewVenue([FromBody]Venue venue)
        {
            _logger.LogInformation("Entered AddNewVenue in RestApi Venues Controller");
            database.VenueAdd(venue.VenueName, venue.Address, venue.City, venue.Country);
        }
        
        /// <summary>
        /// Method that updates data on a Venue based on provided VenueID
        /// and values to be changed.
        /// </summary>
        /// <param name="id">VenueID of the Venue we want to update.</param>
        /// <param name="venue">Values we want to update that corresponds with the properties of a Venue object.</param>
        // PUT: /venue/5
        // based on id of venue, change values
        [HttpPut("{id}")]
        public void UpdateVenue(int id, [FromBody]Venue venue)
        {
            _logger.LogInformation("Entered UpdateVenue in RestApi Venues Controller");
            if (database.FindVenueByID(id) == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                database.UpdateVenue(id, venue.VenueName, venue.Address, venue.City, venue.Country);
            }    
        }
        
        /// <summary>
        /// Method that deletes a specified Venue from database table Venues
        /// based on VenueID sent in to method.
        /// </summary>
        /// <param name="id">The VenueID of the Venue to be deleted.</param>
        // DELETE: api/Venue/5
        [HttpDelete("{id}")]
        public void DeleteVenue(int id)
        {
            _logger.LogInformation("Entered DeleteVenue in RestApi Venues Controller");
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
