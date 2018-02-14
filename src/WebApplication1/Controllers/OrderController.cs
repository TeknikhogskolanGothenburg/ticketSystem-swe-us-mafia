using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DatabaseRepository;
using Newtonsoft.Json;
using TicketSystemEngine;


namespace RESTapi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        TicketDatabase ticketDB = new TicketDatabase();
 
        // GET: api/Ticket
        [HttpGet]
        public IEnumerable<Order> GetAllCustomerOrders()
        {
            return ticketDB.FindAllCustomerOrder();
        }

        // GET: Ticket/customername
        [HttpGet("search/{query}")]
        public IEnumerable<Order> FindCustomerOrders(string query)
        {
            return ticketDB.FindCustomerOrders(query);
        }

        // GET: orderadministration/5
        [HttpGet("{id}")]
        public Order GetSpecificOrder(int id)
        {
            return ticketDB.FindCustomerOrderByID(id);
        }
        
        // PUT: orderadministration/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Order order)
        {
            if (ticketDB.FindEventByID(id) == null)
            {
                Response.StatusCode = 404;
                return;
            }
            ticketDB.UpdateCustomerOrder(id, order.PaymentStatus, order.BuyerLastName, order.BuyerFirstName, order.BuyerAddress, order.BuyerCity);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if(ticketDB.FindCustomerOrderByID(id) == null)
            {
                Response.StatusCode = 404;
                return;
            }
            ticketDB.DeleteCustomerOrder(id);
        }
    }
}
