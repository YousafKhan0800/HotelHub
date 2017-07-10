using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EADWebProj.Models;
using System.Data.Entity;
namespace EADWebProj.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /admin/
        HotelHubEntities1 db = new HotelHubEntities1();

        public JsonResult CheckUserName(string UserName)
        {
            var us = db.admins.Find(UserName);
            if (us == null)
            {
              
                return this.Json(true, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);            
            }       
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Index()
        {
            List<resturant> ls = db.resturants.ToList();
            return View(ls);
        }
        public ActionResult Usersignin()
        {
            return View();
        }
        public ActionResult Signup()
        {
            if (Session["id"] != null)
                return View();
            else
                return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Signup(admin u)
        {
            var us = from x in db.admins
                     where x.id == u.id
                     select x;
            // if (db.Users.First(a => a.uid.Equals(u.uid)) != null)
            if (!us.Any())
            {
                if (u.id != null)
                {
                    db.admins.Add(u);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    return View();
            }
            else
            {
                return View();
            }

        }
        public ActionResult EditProfile()
        {
            string i = Session["id"].ToString();
            admin r = db.admins.First(a => a.id.Equals(i));
            if (r == null)
            {
                return HttpNotFound();
            }
            return View(r);
           
        }
        
        [HttpPost]
        public ActionResult EditProfile(admin a)
        {
            db.Entry(a).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult Logout()
        {
            Session["id"] = null;
            return RedirectToAction("Index","Home");
        }
        
        public ActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost, ActionName("Usersignin")] //for same name this will be called on Usersignin page by default
        public ActionResult checkSignIn()
        {
            string u = Request["uid"];
            string password = Request["password"];
            // User us = new User { uid = "abc", password = "abc", name = "abc" };
            admin us = db.admins.First(a => a.id.Equals(u));
            if (us != null && us.password == password)
            {
                Session["id"]=us.id;
                Session["rid"] = null;
                Session["uid"] = null;
                return RedirectToAction("Index");
            }
            else
                return View();
        }
        public ActionResult ViewUsers()
        {
            List<User> ls = db.Users.ToList();
            return View(ls);
        }
        [HttpPost]
        public ActionResult EditUser(User us)
        {
            db.Entry(us).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewUsers");
        }
        public ActionResult EditUser(string i)
        {
            try
            {
                User r = db.Users.First(a => a.uid.Equals(i));
                if (r == null)
                {
                    return HttpNotFound();
                }
                return View(r);
            }
            catch (Exception e)
            {
                return RedirectToAction("ViewUsers");
            }

        }
        public ActionResult DeleteUser(string id)
        {
            try
            {
                User r = db.Users.First(a => a.uid.Equals(id));
                if (r == null)
                {
                    return HttpNotFound();
                }
                return View("DeleteUser", r);
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult ConfirmDeleteUser(string u)
        {
            try
            {
                User r = db.Users.First(a => a.uid.Equals(u));

                if (r == null)
                {
                    return HttpNotFound();
                }
                db.Users.Remove(r);
                db.SaveChanges();
                return RedirectToAction("ViewUsers");

            }
            catch (Exception e)
            {
                return View();
            }
        }
     




        public ActionResult Resturants()
        {
            List<resturant> ls = db.resturants.ToList();
            return View(ls);
        }
        [HttpPost]
        public ActionResult EditResturant(resturant u)
        {
            db.Entry(u).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Resturants");
        }
        public ActionResult EditResturant(string i)
        {
            try
            {
                resturant r = db.resturants.First(a => a.rid.Equals(i));
                if (r == null)
                {
                    return HttpNotFound();
                }
                return View(r);
            }
            catch (Exception e)
            {
                return RedirectToAction("Resturants");
            }

        }
        public ActionResult DeleteResturant(string id)
        {
            try
            {
                resturant r = db.resturants.First(a => a.rid.Equals(id));
                if (r == null)
                {
                    return HttpNotFound();
                }
                return View("DeleteResturant", r);
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult ConfirmDeleteResturant(string u)
        {
            try
            {
                resturant r = db.resturants.First(a => a.rid.Equals(u));

                if (r == null)
                {
                    return HttpNotFound();
                }
                db.resturants.Remove(r);
                db.SaveChanges();
                return RedirectToAction("Resturants");

            }
            catch (Exception e)
            {
                return View();
            }
        }
        

	}
}