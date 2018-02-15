using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.DatabaseRepository;
using Newtonsoft.Json;
using TicketSystemEngine;
using TicketSystem.PaymentProvider;

namespace RESTapi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        TicketDatabase ticketDB = new TicketDatabase();
        Payment payment;
        PaymentProvider paymentProvider;

        // GET: /order
        [HttpGet]
        public IEnumerable<Order> GetAllCustomerOrders()
        {
            return ticketDB.FindAllCustomerOrder();
        }

        // GET: order/customername
        [HttpGet("search/{query}")]
        public IEnumerable<Order> FindCustomerOrders(string query)
        {
            return ticketDB.FindCustomerOrders(query);
        }

        // GET: order/5
        [HttpGet("{id}")]
        public Order GetSpecificOrder(int id)
        {
            return ticketDB.FindCustomerOrderByID(id);
        }

        // POST: /order
        [HttpPost]
        public int CreateOrder([FromBody] Order order)
        {
            return ticketDB.AddCustomerOrder(order.BuyerFirstName, order.BuyerLastName, order.BuyerAddress, order.BuyerCity, order.PaymentStatus, order.PaymentReferenceID, order.TicketID, order.BuyerEmailAddress);
        }

        // PUT: order/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Order order)
        {
            if (ticketDB.FindCustomerOrderByID(id) == null)
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
            if (ticketDB.FindCustomerOrderByID(id) == null)
            {
                Response.StatusCode = 404;
                return;
            }
            ticketDB.DeleteCustomerOrder(id);
        }
    }
}
