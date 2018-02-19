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
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        public IActionResult OrderAdd()
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

        [HttpPost]
        public ActionResult CreateTicket(string jsonObj)
        {
            string[] jsonArray = jsonObj.Split();
            Ticket ticket = new Ticket();
            ticket.TicketID = Convert.ToInt32(jsonArray[0]);
            ticket.SeatID = Convert.ToInt32(jsonArray[1]);
            ticket.VenueName = jsonArray[2];
            ticket.EventName = jsonArray[3];
            ticket.TicketEventPrice = Convert.ToInt32(jsonArray[4]);
            ticket.EventStartDateTime = Convert.ToDateTime(jsonArray[5]);
            customer.tickets.Add(ticket);
            return View("Index");
        }


        //new Ticket(TicketID, SeatID, VenueName, EventName, TicketEventPrice, EventStartDateTime)
    }
}
