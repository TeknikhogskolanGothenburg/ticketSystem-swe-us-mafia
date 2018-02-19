using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketShopWeb.Models;
using TicketSystem.RestApiClient;
using TicketSystemEngine;

namespace TicketShopWeb.Controllers
{
    
    public class HomeController : Controller
    {
        EventApi eventapi = new EventApi();
        VenueApi venueapi = new VenueApi();
        TicketEventDateApi dateapi = new TicketEventDateApi();
        OrderAdministratorApi orderapi = new OrderAdministratorApi();
        static List<Order> orders = new List<Order>();
        public static CustomerModel customer = new CustomerModel();

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated) 
            {
                customer.tEvent = eventapi.GetAllEvents();
                customer.dates = dateapi.GetAllTicketEventDate();
                customer.venues = venueapi.VenueGet();
                return View(customer);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Cart()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(customer);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        public IActionResult OrderAdd()
        {
            Order order = new Order();

            if (User.Identity.IsAuthenticated)
            {
                return View(order);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult SeeOrders(string email)
        {
            
            if (User.Identity.IsAuthenticated && email != null)
            {
                orders = orderapi.GetOrdersByQuery(email);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public IActionResult About()
        {

            return View();
        }

        public IActionResult Contact()
        {

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Produces("application/json")]
        [HttpDelete]
        public JsonResult DeleteTicket(int id)
        {
            if(customer.tickets.Count > 0)
            {
                foreach(Ticket ticket in customer.tickets)
                {
                    if(ticket.TicketID == id)
                    {
                        customer.tickets.Remove(ticket);
                        break;
                    }
                }
               
            }
            return Json(id);
        }


        [Produces("application/json")]
        [HttpPost]
        public JsonResult AddTicket(Ticket ticket)
        {
            customer.tickets.Add(ticket);
            return Json(ticket);
        }
        
    }
}
