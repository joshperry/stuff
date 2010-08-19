using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net.Mail;

namespace PartyInvites.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Greeting"] = DateTime.Now.Hour < 12 ? "Morning" : "Afternoon";
            return View();
        }

        [HttpGet]
        public ActionResult RsvpForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RsvpForm(Models.GuestResponse response)
        {
            if (ModelState.IsValid)
            {
                response.Submit();
                return View("Thanks", response);
            }
            else
            {
                return View();
            }
        }
    }
}
