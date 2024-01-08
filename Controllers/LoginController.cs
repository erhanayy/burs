using MyAdmin.Helper;
using MyAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourNamespace.ViewModels;

namespace MyAdmin.Controllers
{
    public class LoginController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            //tb_user m = new tb_user();
            //m.UserName = "admin";
            //m.Password = "1234";
            Console.WriteLine("laps");
            return View();
        }


        [HttpPost]
        public ActionResult Check(LoginViewModel model)
        {
            if (model != null && !string.IsNullOrEmpty(model.Login.UserName) && !string.IsNullOrEmpty(model.Login.Password))
            {
                using (DbMyadmEntities context = new DbMyadmEntities())
                {
                    var contact = context.Contacts
                                 .FirstOrDefault(c => c.Email == model.Login.UserName || c.Phone == model.Login.UserName);

                    if (contact != null)
                    {
                        // Kullanıcı bulundu, şimdi şifreyi kontrol et
                        var result = context.Logins.AsEnumerable()
                            .FirstOrDefault(l => l.ContactId == contact.Id && l.Password == Crypto.HashPassword(model.Login.Password));

                        if (result != null)
                        {
                            Session[MessageResource.SessionKeyofUserName] = result.UserName;
                            Session[MessageResource.SessionKeyofPassword] = result.Password;
                            Session[MessageResource.SessionKeyofId] = result.ContactId;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            base.ShowAlertMessage("Lütfen bilgilerinizi kontrol ediniz !!");
            return View("Login", model);
        }

public ActionResult Logout()
        {
            Session[MessageResource.SessionKeyofUserName] = null;
            Session[MessageResource.SessionKeyofPassword] = null;
            Session[MessageResource.SessionKeyofId] = 0;

            return View("Login");
        }
    }
}