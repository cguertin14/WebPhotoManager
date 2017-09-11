using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoManager.Controllers
{
    public class AccountsController : Controller
    {
        public ActionResult Login()
        {
            Session["CurrentUser"] = null;
            Models.LoginView loginView = new Models.LoginView();
            return PartialView("Login", loginView);
        }

        public ActionResult UserNameExist(string username)
        {
            Models.User user = DB.Users.GetByName(username);
            return new JsonResult { Data = new { UserNameExist = user != null }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult PasswordMatch(string username, string password)
        {
            Models.User user = DB.Users.GetByName(username);
            return new JsonResult { Data = new { PasswordMatch = ((user != null) ? user.Password == password : false) }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public ActionResult Login(Models.LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                Models.User user = DB.Users.GetByName(loginView.UserName);
                if (user != null)
                {
                    if (user.Password == loginView.Password)
                    {
                        Session["CurrentUser"] = user;
                    }
                 }
            }
            return new JsonResult { Data = new { log = (Session["CurrentUser"] != null) }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Logout()
        {
            Session["CurrentUser"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
            Models.UserView userView = new Models.UserView();
            return PartialView("Create", userView);
        }
        [HttpPost]
        public ActionResult Create(Models.UserView userview)
        {
            bool error = false;
            if (ModelState.IsValid)
            {
                try
                {
                    DB.DataBase.BeginTransaction();
                    DB.Users.Add(userview.ToUser());
                }
                catch (Exception)
                {
                    error = true;
                }
                finally
                {
                    DB.DataBase.EndTransaction(error);
                }
            }
            return new JsonResult { Data = new { status = !error } };
        }

        public ActionResult Edit(int id)
        {
            Models.User user = DB.Users.Get(id);
            if (user != null)
            {
                Models.UserView userView = new Models.UserView(user);
                return View(userView);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Edit(Models.UserView userView)
        {
            if (ModelState.IsValid)
            {
                Session["CurrentUser"] = userView.ToUser();
                bool error = false;
                try
                {
                    DB.DataBase.BeginTransaction();
                    DB.Users.Update((Models.User)Session["CurrentUser"]);
                }
                catch (Exception)
                {
                    error = true;
                }
                finally
                {
                    DB.DataBase.EndTransaction(error);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(userView);
        }
    }
}