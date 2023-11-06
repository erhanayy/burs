using MyAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAdmin.Controllers
{
    public class LoginController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            //tb_user m = new tb_user();
            //m.UserName = "admin";
            //m.Password = "123";
            return View();
        }

        [HttpPost]
        public ActionResult Check(tb_user model)
        {
            if (model != null && !string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                var result = (from inc in base.DbInstance.tb_user
                              where inc.UserName == model.UserName && inc.Password == model.Password
                              select inc).FirstOrDefault();
                if (result != null)
                {
                    Session[MessageResource.SessionKeyofUserName] = model.UserName;
                    Session[MessageResource.SessionKeyofPassword] = model.Password;
                    return RedirectToAction("Index", "Home");
                }
            }
            base.ShowAlertMessage("Lütfen bilgilerinizi kontrol ediniz !!");
            return View("Login", model);
        }

        public ActionResult Logout()
        {
            Session[MessageResource.SessionKeyofUserName] = null;
            Session[MessageResource.SessionKeyofPassword] = null;
            return View("Login");
        }
    }
}