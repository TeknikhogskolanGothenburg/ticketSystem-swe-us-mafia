using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketShop.Models;

namespace TicketShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Login()
        {
            return View("Login");
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "The amazing ticket shop- search for culture events made easy.";

            return View("About");
        }

        public IActionResult Contact()
        {
            //ViewData["Message"] = "Contact page for Amazing ticketshop.";

            return View("Contact");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
