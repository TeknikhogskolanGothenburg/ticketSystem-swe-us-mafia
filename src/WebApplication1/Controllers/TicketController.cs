using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystemEngine;
using TicketSystem.DatabaseRepository;
using Newtonsoft.Json;

namespace RESTapi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class TicketController : Controller
    {
        TicketDatabase db = new TicketDatabase();
        /// <summary>
        /// Method that Gets all the tickets from the database (uses database method
        /// that joins Ticket table together with other tables to get datetime data,
        /// venuname, eventname to the model representation of Ticket.
        /// </summary>
        /// <returns>A List of Ticket objects.</returns>
        // GET: /ticket
        [HttpGet]
        public IEnumerable<Ticket> Get()
        {
            return db.GetAllTickets();
        }

        /// <summary>
        /// Method used to get data on one Ticket object from the database.
        /// </summary>
        /// <param name="id">The TicketID of the ticket that we want data on.</param>
        /// <returns>A Ticket object.</returns>
        // GET: /ticket/5
        [HttpGet("{id}")]
        public Ticket Get(int id)
        {
            return db.FindTicketByTicketID(id);
        }
        
        /// <summary>
        /// Method that creates a Ticket (A row in database table Tickets).
        /// </summary>
        /// <param name="id">ID of the seat that the created ticket is connected to.</param>
        // POST: /ticket
        [HttpPost("{id}")]
        public void CreateTicket(int id)
        {
            db.CreateTicket(id);
        }
        
      /* Don't know if we even need this method?
        // PUT: /ticket/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }*/
        
        /// <summary>
        /// Method that deletes a Ticket object.
        /// If ID is not found in database- returns 404 statuscode,
        /// else- deletes the ticket.
        /// </summary>
        /// <param name="id">The ID of the Ticket to be deleted.</param>
        // DELETE: ticket/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (db.FindTicketByTicketID(id) == null)
            {
                Response.StatusCode = 404;
            }
            else
            {
                db.DeleteTicket(id);
            }
        }
    }
}
