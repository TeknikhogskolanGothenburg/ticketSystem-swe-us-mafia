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
using System.Net.Http;
using System.Net;

namespace RESTapi.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        TicketDatabase ticketDB = new TicketDatabase();

        /// <summary>
        /// Method that gets all customer orders (TicketTransactions database table).
        /// </summary>
        /// <returns>A list (IEnumerable of Order objects.</returns>
        // GET: /order
        [HttpGet]
        public List<Order> GetAllCustomerOrders()
        {
            return ticketDB.FindAllCustomerOrder();
        }

        /// <summary>
        /// Method that searches for customer orders in TicketTransactions database table
        /// based on BuyerFirstName, BuyerLastName or BuyerEmailAddress
        /// </summary>
        /// <param name="query">The value to use for filtering out customer orders.</param>
        /// <returns>A list (IEnumerable of Order objects based on query.</returns>
        // GET: order/customername
        [HttpGet("search/{query}")]
        public IEnumerable<Order> FindCustomerOrders(string query)
        {
            return ticketDB.FindCustomerOrders(query);
        }

        /// <summary>
        /// Method that gets a specific order (TicketTransaction)
        /// based on the provided transactionID.
        /// </summary>
        /// <param name="id">TransactionID of the order to get information on.</param>
        /// <returns>An Order object.</returns>
        // GET: order/5
        [HttpGet("{id}")]
        public Order GetSpecificOrder(int id)
        {
            if(ticketDB.FindCustomerOrderByID(id) == null)
            {
                Response.StatusCode = 404;
            }
            //Response.StatusCode = 200;
            return ticketDB.FindCustomerOrderByID(id);           
        }

        /// <summary>
        /// Attempts to receive payment for an order, and creates the order if successful.
        /// Currently, all orders are discounted to 150 SEK. Yay!
        /// </summary>
        /// <param name="order">The order to be placed.</param>
        /// <returns>The transaction identifier of the newly created order if payment successful. 
        /// Otherwise, the PaymentStatus received upon payment failure, negated.</returns>
        // POST: /order
        [HttpPost]
        public int CreateOrder([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                Response.StatusCode = 200;

                PaymentProvider paymentProvider = new PaymentProvider();
                var payment = paymentProvider.Pay(150, "SEK", order.TransactionID.ToString());
                if (payment.PaymentStatus == PaymentStatus.PaymentApproved)
                {
                    // TODO: maybe catch parse error here and give appropriate error, if ticket IDs are bad?
                    int[] ticketIDs = order.TicketIDs.Split(",").Select(int.Parse).ToArray();
                    return ticketDB.AddCustomerOrder(order.BuyerFirstName, order.BuyerLastName, order.BuyerAddress, order.BuyerCity, payment.PaymentStatus, payment.PaymentReference, ticketIDs, order.BuyerEmailAddress);
                }
                else
                {
                    Response.StatusCode = 403;
                    return -(int)payment.PaymentStatus;
                }
            }
            else
            {
                return  Response.StatusCode = 400;
            }
        }

        /// <summary>
        /// Method that updates the order data based on provided parameter values.
        /// </summary>
        /// <param name="id">The TransactionID of the order that we want to update.</param>
        /// <param name="order">The Order object which's value we want to update.</param>
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
            Response.StatusCode = 200;
        }

        /// <summary>
        /// Method that deletes an order (TicketTransaction).
        /// If transactionID (orderID) can't be found, returns 404 error,
        /// else we delete the transaction from TicketTransactions table.
        /// </summary>
        /// <param name="id">TransactionID of the order we want to delete.</param>
        // DELETE: order/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (ticketDB.FindCustomerOrderByID(id) == null)
            {
                Response.StatusCode = 404;
                return;
            }
            ticketDB.DeleteCustomerOrder(id);
            Response.StatusCode = 200;
        }
    }
}
