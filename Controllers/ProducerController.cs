using MyAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAdmin.Controllers
{
    public class ProducerController : BaseController
    {
        // GET: Producer
        public ActionResult Create()
        {
            base.SetCityAndCountySource(null, null);
            return View();
        }

        [HttpPost]
        public ActionResult Create(tb_producer model)
        {
            if (model != null)
            {
                if (model.ContanctId.HasValue)
                    model.tb_contact.Id = model.ContanctId.Value;

                if (model.ContanctId.HasValue)
                {
                    tb_contact _contact = base.GetContactById(model.ContanctId.Value);
                    _contact.ContactName = model.tb_contact.ContactName;
                    _contact.Adress = model.tb_contact.Adress;
                    _contact.Email = model.tb_contact.Email;
                    _contact.PhoneNumber = model.tb_contact.PhoneNumber;
                    _contact.GsmNumber = model.tb_contact.GsmNumber;
                    _contact.Fax = model.tb_contact.Fax;
                    
                    base.SetCreateOrUpdateFieldValues<tb_contact>(_contact);
                }
                else if (model.tb_contact != null)
                {
                    base.SetCreateOrUpdateFieldValues<tb_contact>(model.tb_contact);
                    base.DbInstance.tb_contact.Add(model.tb_contact);
                    base.DbInstance.SaveChanges();
                    model.ContanctId = model.tb_contact.Id;
                }

                if (model.Id > 0)
                {
                    tb_producer _producer = GetProducerById(model.Id);
                    _producer.CityId = model.CityId;
                    _producer.CountyId = model.CountyId;
                    _producer.Name = model.Name;
                    _producer.ContanctId = model.ContanctId;
                    base.SetCreateOrUpdateFieldValues<tb_producer>(_producer);
                }
                else
                {
                    base.SetCreateOrUpdateFieldValues<tb_producer>(model);
                    base.DbInstance.tb_producer.Add(model);
                }

                base.DbInstance.SaveChanges();
            }
            return RedirectToAction("Create");
        }

        public ActionResult Edit(int Id)
        {
            tb_producer result = null;
            if (Id > 0)
            {
                result = (from inc in base.DbInstance.tb_producer
                          where inc.Id == Id
                          select inc).FirstOrDefault();
            }

            base.SetCityAndCountySource(result.CityId, result.CountyId);

            return View("Create", result);
        }
        public ActionResult Delete(int Id)
        {
            tb_producer _producer = null;
            if (Id > 0)
            {
                _producer = (from inc in base.DbInstance.tb_producer
                             where inc.Id == Id
                             select inc).FirstOrDefault();

                if (_producer.ContanctId.HasValue)
                {
                    tb_contact _contact = base.GetContactById(_producer.ContanctId.Value);
                    base.DbInstance.tb_contact.Remove(_contact);
                }
                base.DbInstance.tb_producer.Remove(_producer);
                base.DbInstance.SaveChanges();
            }
            return RedirectToAction("Create");
        }

        [HttpPost]
        public ActionResult Search(tb_producer model)
        {
            var result = (from inc in base.DbInstance.tb_producer
                          where (!model.CityId.HasValue || model.CityId.Value == inc.CityId)
                          && (!model.CountyId.HasValue || model.CountyId.Value == inc.CountyId)
                          && (string.IsNullOrEmpty(model.Name) || inc.Name.StartsWith(model.Name))
                          && (string.IsNullOrEmpty(model.tb_contact.ContactName) || inc.tb_contact.ContactName.StartsWith(model.tb_contact.ContactName))
                          && (string.IsNullOrEmpty(model.tb_contact.Adress) || inc.tb_contact.Adress.StartsWith(model.tb_contact.Adress))
                          && (string.IsNullOrEmpty(model.tb_contact.Email) || inc.tb_contact.Email.StartsWith(model.tb_contact.Email))
                          && (string.IsNullOrEmpty(model.tb_contact.PhoneNumber) || inc.tb_contact.PhoneNumber.StartsWith(model.tb_contact.PhoneNumber))
                          && (string.IsNullOrEmpty(model.tb_contact.GsmNumber) || inc.tb_contact.GsmNumber.StartsWith(model.tb_contact.GsmNumber))
                          && (string.IsNullOrEmpty(model.tb_contact.Fax) || inc.tb_contact.Fax.StartsWith(model.tb_contact.Fax))
                          select inc);

            if (result != null)
                Session[MessageResource.SearchListNameofProducer] = result.ToList();
            else
                Session[MessageResource.SearchListNameofProducer] = null;

            base.SetCityAndCountySource(model.CityId, model.CountyId);

            return View("Create", model);
        }
        public ActionResult GetAllProducers()
        {
            List<tb_producer> result = null;
            if (Session[MessageResource.SearchListNameofProducer] != null)
            {
                result = (List<tb_producer>)Session[MessageResource.SearchListNameofProducer];
                Session[MessageResource.SearchListNameofProducer] = null;
            }
            else
            {
                result = (from inc in base.DbInstance.tb_producer
                          select inc).ToList();
            }
            if (result == null)
                result = new List<tb_producer>();

            return PartialView("_ProducerGrid", result);
        }

        private tb_producer GetProducerById(int Id)
        {
            tb_producer result = (from inc in base.DbInstance.tb_producer
                                  where inc.Id == Id
                                  select inc).FirstOrDefault();
            return result;
        }
    }
}