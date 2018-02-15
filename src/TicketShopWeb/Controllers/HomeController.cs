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
        public IActionResult Index()
        {       
            List<TicketEvent> listEvent = new List<TicketEvent> { };
            listEvent = eventapi.GetAllEvents();

            if (User.Identity.IsAuthenticated) 
            {
                return View(listEvent);
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
            ViewData["Message"] = "The amazing ticket shop- search for culture events made easy.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact page for Amazing ticketshop.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
