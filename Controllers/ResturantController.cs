using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EADWebProj.Models;
using System.Data.Entity;
using System.Net.Mail;
namespace EADWebProj.Controllers
{
    public class ResturantController : Controller
    {
        //
        // GET: /Resturant/
        HotelHubEntities1 db = new HotelHubEntities1();


        public ActionResult Test()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Test(HttpPostedFileBase file)
        {
            var img = Request["im"];
            if (file != null)
            {                
                // HttpPostedFileBase file = Request.Files["im"];
                //HttpPostedFileBase file = Request.Files[0];
                file.SaveAs(Server.MapPath(@"~/MyFiles") + file.FileName);
            }
            return RedirectToAction("Index");
        }
        public JsonResult CheckUserName(string userName)
        {
            var us = db.resturants.Find(userName);
            if (us != null)
            {

                return this.Json(true, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ForgetPassword()
        {
            string rid = Request["fId"];
            resturant us = db.resturants.Find((rid));
            resturant u = new resturant();
            Session["lier"] =  rid;
            
            if (us != null)
            {
                
                return RedirectToAction("Questions", u);

            }
            else
                return RedirectToAction("Usersignin");
        }
        public ActionResult Questions(resturant r)
        {
            return View(r);
        }
        [HttpPost]
        public ActionResult Answers(resturant r)
        {
            resturant u=null;
            r.rid = Session["lier"].ToString();
            ViewBag.lierResponse = "Response From Server";
            if(r.rid!=null)
                u = db.resturants.Find((r.rid));
            if(u!=null)
            {

                if (u.city.Equals(r.city) && (u.phone.Equals(r.phone) || u.capacity.Equals(r.capacity)))
                {
                    Session["rid"] = r.rid;
                    return RedirectToAction("editProfile", u);

                }
                else
                {
                    ViewBag.lierResponse = "Error: Your Answers don`t match to the information";
                    return RedirectToAction("Questions", r);
                }
            }
            return RedirectToAction("Questions", r);
        }
        public ActionResult Index()
        {
            List<resturant> ls = db.resturants.ToList();
            return View(ls);
        }
        public ActionResult Logout()
        {
            Session["rid"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult EditProfile()
        {
            string rid=Session["rid"].ToString();
            resturant r = db.resturants.Find(rid);
            if (r != null)
                return View(r);
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult EditProfile(resturant r)
        {
            db.Entry(r).State = EntityState.Modified;
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(resturant u)
        {
            var us = from x in db.resturants
                     where x.rid == u.rid
                     select x;
            if (!us.Any())
            {
                if (ModelState.IsValid)
                {
                    db.resturants.Add(u);
                    db.SaveChanges();
                    Session["rid"] = u.rid;
                    Session["id"] = null;
                    Session["uid"] = null;
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

        }
        public ActionResult AddItems()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddImage()
        {

            return RedirectToAction("addItems");
        }
        [HttpPost]
        public ActionResult AddItems(HttpPostedFileBase image)
        {
            //var img = Request.Files["image"];
            //var v=@"C:\Users\Imroz\Documents\Visual Studio 2013\Projects\EADWebProj\EADWebProj\MyFiles\ab.JPG";
            //img.SaveAs(v);
            //if (img != null && img.FileName != null)
            //{

            //    var ext = System.IO.Path.GetExtension(img.FileName);
            //    var iname = "ab" + ext;
            //    //physical path
            //    var pth = Server.MapPath("~/MyFiles");
            //    var cmpltPath = System.IO.Path.Combine(pth, iname);
            //    img.SaveAs(cmpltPath);

            //}

            
            //for (int i = 0; i < Request.Files.Count; i++ )
            // if(file != null )
            {
                var img = Request["image"];
                // HttpPostedFileBase file = Request.Files["im"];
                //HttpPostedFileBase file = Request.Files[0];
                //file.SaveAs(Server.MapPath(@"~/MyFiles") + file.FileName);

                //HttpPostedFile f = HttpContext.Request.Files[0];
                var f = HttpContext.Request.Files["image"];
                
                if (f != null && f.FileName != "" && f.ContentLength != 0)
                {

                    f.SaveAs(Server.MapPath(@"~/MyFiles") + f.FileName);
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Usersignin()
        {
            return View();
        }
        [HttpPost, ActionName("Usersignin")] //for same name this will be called on Usersignin page by default
        public ActionResult checkSignIn()
        {
            string u = Request["uid"];
            string password = Request["password"];
            try
            {
                resturant us = db.resturants.First(a => a.rid.Equals(u));
                if (us != null && us.password == password)
                {
                    Session["rid"] = u;
                    Session["id"] = null;
                    Session["uid"] = null;
                    return RedirectToAction("Index");
                }
                else
                    return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }
        public ActionResult About()
        {
            return View();
        }      
        public ActionResult Blog()
        {
            string u = Session["rid"].ToString();
            resturant l = db.resturants.First(a => a.rid.Equals(u));
            return View(l);
        }

        public ActionResult Gallery()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        } 
    [HttpPost]
        public ActionResult ContactUs(resturant s)
        {
            string email = Request["email"];
            string subject = Request["subject"];
            string msg1 = Request["msg"];
            string pass = Request["pass"];

            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string msg = string.Empty;

            try
            {

                MailAddress fromAddress = new MailAddress(email);
                message.From = fromAddress;
                message.To.Add("bcsf14m037@pucit.edu.pk");

                message.Subject = subject;
                message.IsBodyHtml = false;
                message.Body = msg1;
                smtpClient.Host = "smtp.gmail.com";   // We use gmail as our smtp client
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(email, pass);

                smtpClient.Send(message);
                msg = "Successful<BR>";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return RedirectToAction("index", "User");
            }
            Session["confirmation"] = "Congratulation ! Message Sent ...";
            string rid = Session["currentResturant"].ToString();
            if(rid!=null)
            {
                resturant r = db.resturants.Find(rid);
                if (r != null)
                    return RedirectToAction("about", "User",r);
            }
            return RedirectToAction("index","User");
        } 
	}
}