using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MAKClaims.Models;
using Microsoft.AspNet.Identity;

namespace MAKClaims.Controllers
{
    public class HomeController : Controller
    {
        private Claimsconfig db = new Claimsconfig();
        public ActionResult Index()
        {          
            var uid = User.Identity.GetUserId();
            var uname = db.AspNetUsers.Where(u => u.Id.Equals(uid, StringComparison.Ordinal)).FirstOrDefault();
            var greeting = "Hello ";
            if (DateTime.Now.Hour < 12)
            {
                greeting = "Good Morning ";
            }
            else if (DateTime.Now.Hour < 17)
            {
                greeting = "Good Afternoon ";
            }
            else if (DateTime.Now.Hour < 24)
            {
                greeting = "Good Evening ";
            }
            else
            {

            }
            ViewBag.ID = 23;
            ViewBag.Greeting = greeting;
            if (uid != null)
            {
                ViewBag.Name = uname.UserName;
            }
            else
            {
                ViewBag.Name = "Please sign in";
            }
            

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}