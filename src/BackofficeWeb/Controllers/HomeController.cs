using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackofficeWeb.Models;
using TicketSystemEngine;
using TicketSystem.RestApiClient;
namespace BackofficeWeb.Controllers
{
    public class HomeController : Controller
    {
        VenueApi venueApi = new VenueApi();
        EventApi eventApi = new EventApi();
        TicketEventDateApi dateApi = new TicketEventDateApi();
        OrderAdministratorApi orderApi = new OrderAdministratorApi();
        public IActionResult Index()
        {
        //    List<Venue> venueList = new List<Venue> { };
        //    venueList = venueApi.VenueGet();
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Venues");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Venues()
        {
            List<Venue> venueList = new List<Venue> { };
            venueList = venueApi.VenueGet();
            if (User.Identity.IsAuthenticated)
            {
                return View(venueList);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Events()
        {
            List<TicketEvent> eventList = new List<TicketEvent> { };
            eventList = eventApi.GetAllEvents();

            if (User.Identity.IsAuthenticated)
            {
                return View(eventList);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Order(string query)
        {
            List<Order> orderList = new List<Order> { };
            if (String.IsNullOrEmpty(query) || query == "query")
            {              
                orderList = orderApi.GetAllOrders();
            }
            else
            {
                orderList = orderApi.GetOrdersByQuery(query);
            }
            if (User.Identity.IsAuthenticated)
            {
                return View(orderList);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult OrderQuery(string query)
        {
            List<Order> orderList = new List<Order> { };
            orderList = orderApi.GetOrdersByQuery(query);

            if (User.Identity.IsAuthenticated)
            {
                return View("Order", orderList);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        public IActionResult VenueAdd()
        {
            Venue venue = new Venue();
            if (User.Identity.IsAuthenticated)
            {
                return View(venue);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult EventAdd()
        {
            List<Venue> venueList = new List<Venue> { };
            venueList = venueApi.VenueGet();
            if (User.Identity.IsAuthenticated)
            {
                return View(venueList);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult DateAdd()
        {
            VenueEventModel hybrid = new VenueEventModel();
            hybrid.venues = venueApi.VenueGet();
            hybrid.events = eventApi.GetAllEvents();
            if (User.Identity.IsAuthenticated)
            {
                return View(hybrid);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult EditEvent()
        {
            OverviewModel overview = new OverviewModel();
            overview.venues = venueApi.VenueGet();
            overview.events = eventApi.GetAllEvents();
            overview.dates = dateApi.GetAllTicketEventDate();
            if (User.Identity.IsAuthenticated)
            {
                return View(overview);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        //[HttpPost]
        //public IActionResult Venue(VenueModel ven)
        //{
        //    VenueModel venData = ven;
        //    return View();
        //}

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
