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
        /// <summary>
        /// Attempts to receive payment for an order, and creates the order if successful.
        /// Currently, all orders are discounted to 150 SEK. Yay!
        /// </summary>
        /// <param name="order">The order to be placed.</param>
        /// <returns>The transaction identifier of the newly created order if payment successful. Otherwise, the PaymentStatus received upon payment failure, negated.</returns>
        [HttpPost]
        public int CreateOrder([FromBody] Order order)
        {
            PaymentProvider paymentProvider = new PaymentProvider();
            var payment = paymentProvider.Pay(150, "SEK", order.TransactionID.ToString());
            if (payment.PaymentStatus == PaymentStatus.PaymentApproved)
            {
                return ticketDB.AddCustomerOrder(order.BuyerFirstName, order.BuyerLastName, order.BuyerAddress, order.BuyerCity, payment.PaymentStatus, payment.PaymentReference, order.TicketIDs, order.BuyerEmailAddress);
            }
            else
            {
                Response.StatusCode = 403;
                return -(int)payment.PaymentStatus;
            }
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
            ticketDB.UpdateCustomerOrder(id, order.BuyerLastName, order.BuyerFirstName, order.BuyerAddress, order.BuyerCity);
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
