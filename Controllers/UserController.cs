using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EADWebProj.Models;
using System.Data.Entity;
namespace EADWebProj.Controllers
{
    public class UserController : Controller
    {
        HotelHubEntities1 db = new HotelHubEntities1();
        [HttpPost]
        public ActionResult Search()
        {
            //ViewBag.city ="lahore";
            return RedirectToAction("Blog", "User");
        }
        public ActionResult Index()
        {
            List<resturant> ls = db.resturants.ToList();
            return View(ls);
        }
        public ActionResult Logout()
        {
            Session["uid"] = null;
            return RedirectToAction("Index","Home");
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(User u)
        {
            var us = from x in db.Users
                     where x.uid == u.uid
                     select x;
           // if (db.Users.First(a => a.uid.Equals(u.uid)) != null)
            //db.Entry(r).State = EntityState.Modified;
            
            if(!us.Any())
            {
                Session["uid"] = u.uid;
                if (ModelState.IsValid)
                {
                    db.Users.Add(u);
                    db.SaveChanges();
                    return RedirectToAction("Index", "User");
                    
                }
                
                Session["id"] = null;
                Session["rid"] = null;
                return View(u);
            }
            else
            {
                return View(); 
            }
            
        }
        public ActionResult Usersignin()
        {
            return View();
        }
        [HttpPost, ActionName("Usersignin")] //for same name this will be called on Usersignin page by default
        public ActionResult checkSignIn()
        {
            string u= Request["uid"];
            string password = Request["password"];
           // User us = new User { uid = "abc", password = "abc", name = "abc" };
            try
            {
                User us = db.Users.First(a => a.uid.Equals(u));
                if (us != null && us.password == password)
                {
                    Session["uid"] = us.uid;
                    Session["aid"] = null;
                    Session["rid"] = null;
                    return RedirectToAction("Index");
                }
                else
                    return View();
            }
            catch(Exception e)
            {
                return View();
            }
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult abcout(string rid, int pic)
        {
            ViewBag.picNo = pic;
            Session["confirmation"] = null;
            resturant r = db.resturants.First(a => a.rid.Equals(rid));
            Session["currentResturant"] = rid;
            return View("About",r);
        }
       
        public ActionResult ViewProfile()
        {
            string u = Session["uid"].ToString();
            User l = db.Users.First(a => a.uid.Equals(u));
            return View(l);
        }

        public ActionResult EditProfile()
        {
            string uid = Session["uid"].ToString();
            User r = db.Users.First(a => a.uid.Equals(uid));
            return View(r);
        }
        [HttpPost]
        public ActionResult EditProfile(User r)
        {
            db.Entry(r).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Blog()
        {
            return View();
        }
        [HttpPost, ActionName("Blog")]
        public ActionResult SearchedResturants()
        {
            string city = Request["city"];
            List<resturant> ls = db.resturants.Where(a => a.city.Equals(city)).ToList();
            return View(ls);
        }
        public ActionResult Gallery()
        {
            return View();
        }

        public object a { get; set; }
    }
}