//using Microsoft.Extensions.Logging;
using MyAdmin.Helper;
using MyAdmin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YourNamespace.ViewModels;

namespace MyAdmin.Controllers
{
    public class RegisterController : BaseController
    {
        public ActionResult Register()
        {
            using(DbMyadmEntities db = new DbMyadmEntities())
            {
                RegisterViewModel viewModel = new RegisterViewModel
                {
                    ContactTypes = db.ContactTypes.ToList()
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Check(RegisterViewModel model, string Password)
        {
            Console.WriteLine(Password);
            if (ModelCheck(model.Contact))
            {
                if (IsExist(model.Contact))
                {
                    int contactId = ImportContact(model.Contact);
                    if (contactId >= -1 && ImportLogin(model.Contact, contactId, Password) && ImportContactTypeRel(contactId, model.SelectedContactTypeId))
                    {
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        goto alert;
                    }
                }
                else
                {
                    base.ShowAlertMessage("Girdiğiniz bilgiler başka bir hesaba ait !!");
                    return RedirectToAction("Register", "Register");
                }
            }
        alert:
            base.ShowAlertMessage("Lütfen bilgilerinizi kontrol ediniz !!");
            return RedirectToAction("Register", "Register");
        }

        private bool ModelCheck(Contact model)
        {
            if (model != null &&
            !string.IsNullOrEmpty(model.Name) &&
            !string.IsNullOrEmpty(model.Surname) &&
            !string.IsNullOrEmpty(model.Email) &&
            !string.IsNullOrEmpty(model.Phone))
            {
                return true;
            }
            return false;
        }

        private bool IsExist(Contact model)
        {
            var existingContact = (from contact in base.DbInstance.Contacts
                                   where contact.Email == model.Email || contact.Phone == model.Phone
                                   select contact).FirstOrDefault();
            return (existingContact == null);
        }

        private int ImportContact(Contact model)
        {
            try
            {
                using (DbMyadmEntities context = new DbMyadmEntities())
                {
                    Contact newContact = new Contact
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        Email = model.Email,
                        Phone = model.Phone,
                        IsActive = true,
                        CitizenCountryId = 1,
                        CitizenNumber = "123456789"
                    };
                    context.Contacts.Add(newContact);
                    context.SaveChanges();
                    return newContact.Id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private bool ImportLogin(Contact model, int contactId, string password)
        {
            try
            {
                using (DbMyadmEntities context = new DbMyadmEntities())
                {
                    Login newLogin = new Login
                    {
                        ContactId = contactId,
                        UserName = model.Name,
                        Password = Crypto.HashPassword(password),
                        IsActive=true
                    };

                    context.Logins.Add(newLogin);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
                throw;
            }
        }

        private bool ImportContactTypeRel(int contactId, int contactTypeId)
        {
            try
            {
                using (DbMyadmEntities context = new DbMyadmEntities())
                {
                    ContactTypeRel newContactTypeRel = new ContactTypeRel
                    {
                        ContactId= contactId,
                        ContactTypeId = contactTypeId,
                        IsActive = true
                    };

                    context.ContactTypeRels.Add(newContactTypeRel);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
                throw;
            }
        }
    }
}
