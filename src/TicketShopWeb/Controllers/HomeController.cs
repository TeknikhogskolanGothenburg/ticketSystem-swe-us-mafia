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
        Ticket customerTicket = new Ticket();
        private static Random idGenerator = new Random();
        private static Dictionary<int, CustomerSession> CustomerSessions = new Dictionary<int, CustomerSession>();

        public IActionResult Index()
        {
            

            if (User.Identity.IsAuthenticated) 
            { 
                CustomerModel customer = new CustomerModel();
                customer.tEvent = eventapi.GetAllEvents();
                customer.dates = dateapi.GetAllTicketEventDate();
                customer.venues = venueapi.VenueGet();
                HttpContext.Session.SetString("customer");
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
    }
}
