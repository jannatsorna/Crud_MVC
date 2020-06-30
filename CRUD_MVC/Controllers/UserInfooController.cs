using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUD_MVC.Models;

namespace CRUD_MVC.Controllers
{
    public class UserInfooController : Controller
    {
        [HttpGet]
        // GET: UserInfoo
        public ActionResult SignUp(int id = 0)
        {
            UserInfoo user_obj = new UserInfoo();
            return View(user_obj);
        }
        [HttpPost]
        public ActionResult SignUp(UserInfoo user_obj)
        {
            //to save values into UserInfoo table
            using (Entities eobj = new Entities())
            {
                //to check duplicate values
                if(eobj.UserInfoo.Any(x=>x.UserName == user_obj.UserName && x.Email == user_obj.Email))
                {
                    ViewBag.DuplicateMessage = "Username or Email already exist";
                    return RedirectToAction("SignUp");
                }
                eobj.UserInfoo.Add(user_obj);
                eobj.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(UserInfoo user_obj)
        {
            using (Entities eobj = new Entities())
            {
                var user_check = eobj.UserInfoo.Single(u => u.UserName == user_obj.UserName && u.Email == user_obj.Email && u.Password == user_obj.Password);
                if(user_check != null)
                {
                    Session["UserID"] = user_check.UserID.ToString();
                    Session["UserName"] = user_check.UserName.ToString();
                    Session["Email"] = user_check.UserName.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError(" ", "Username or Password is wrong");
                }
                return View();
            }
        }

        public ActionResult LoggedIn()
        {
            if(Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }

        public ActionResult Signout()
        {
            Session.Abandon();
            return RedirectToAction("SignIn");
        }



        public ActionResult List()
        {
            using (Entities eobj = new Entities())
            {
                var list = eobj.UserInfoo.ToList();
                return View(list);
            }
        }

        public ActionResult Details(int id)
        {
            using (Entities eobj = new Entities())
            {
                var details = eobj.UserInfoo.Where(d => d.UserID == id).FirstOrDefault();

                return View(details);
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            using (Entities eobj = new Entities())
            {
                var update = eobj.UserInfoo.Where(u => u.UserID == id).FirstOrDefault();
                return View(update);
            }
        }
        [HttpPost]
        public ActionResult Update(int id, UserInfoo user_obj)
        {
            try
            {
                using (Entities eobj = new Entities())
                {
                    eobj.Entry(user_obj).State = EntityState.Modified;
                    eobj.SaveChanges();
                }
                return RedirectToAction("List");//after update,the updated value shown in List,so back to /User_Info/List

            }
            catch
            {
                return View(); //Update.cshtml pg e rtn krbe
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (Entities eobj = new Entities())
            {
                var delete = eobj.UserInfoo.Where(d => d.UserID == id).FirstOrDefault();
                return View(delete);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id,UserInfoo user_obj)
        {
            try
            {
                using (Entities eobj = new Entities())
                {
                    var dlt = eobj.UserInfoo.Where(d => d.UserID == id).FirstOrDefault();
                    eobj.UserInfoo.Remove(dlt);
                    eobj.SaveChanges();
                }
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

    }
}