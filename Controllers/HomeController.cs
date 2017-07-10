using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EADWebProj.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost, ActionName("Index")]
        public ActionResult UserIndex()
        {
            return RedirectToAction("Index","User");
        }
        [HttpPost]
        public ActionResult ResturantIndex()
        {
            return RedirectToAction("Index", "Resturant");
        }
        [HttpPost]
        public ActionResult AdminIndex()
        {
            return RedirectToAction("Index", "Admin");
        }
	}
}