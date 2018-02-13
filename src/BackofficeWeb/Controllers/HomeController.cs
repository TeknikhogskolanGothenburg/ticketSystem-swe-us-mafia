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
        public IActionResult Index()
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

        public IActionResult Customer()
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
        
        //[HttpPost]
        //public IActionResult Venue(VenueModel ven)
        //{
        //    VenueModel venData = ven;
        //    return View();
        //}

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
