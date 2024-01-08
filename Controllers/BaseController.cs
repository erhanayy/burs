using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MyAdmin.Helper;
using MyAdmin.Models;

namespace MyAdmin.Controllers
{
    public class BaseController : Controller
    {
        private DbMyadmEntities _dbobj = null;
        protected DbMyadmEntities DbInstance
        {
            get
            {
                if (_dbobj == null)
                    _dbobj = new DbMyadmEntities();
                return _dbobj;
            }
        }
        protected int ActiveUserId
        {
            get
            {
                return 1;
            }
        }

        protected void ShowAlertMessage(string message)
        {
            TempData["ShowAlert"] = "1";
            TempData["AlertMessage"] = message;
        }

        [HttpPost]
        public void SetTempData(string n, string v)
        {
            TempData[n] = v;
        }

        protected void SetCreateOrUpdateFieldValues<T>(T item)
        {
            Type itemtype = item.GetType();
            int Id = (int)itemtype.GetProperty("Id").GetValue(item);
            if (Id > 0)
            {
                itemtype.GetProperty("UpdatedBy").SetValue(item, this.ActiveUserId);
                itemtype.GetProperty("UpdatedDate").SetValue(item, (DateTime.Now));
            }
            else
            {
                itemtype.GetProperty("CreatedBy").SetValue(item, this.ActiveUserId);
                
                itemtype.GetProperty("CreatedDate").SetValue(item, (DateTime.Now));
                itemtype.GetProperty("UpdatedBy").SetValue(item, this.ActiveUserId);
                itemtype.GetProperty("UpdatedDate").SetValue(item, DateTime.Now);
            }
        }

        protected void SetCityAndCountySource(int? cityId, int? countyId)
        {
            ViewBag.CityList = GetAllCity();
            ViewBag.CountyList = Enumerable.Empty<SelectListItem>();
            if (cityId.HasValue)
            {
                var cdata = GetCountyByCityId(cityId.Value);
                if (!countyId.HasValue)
                    cdata.Insert(0, new CustomCounty() { Id = null, Name = MessageResource.PleaseSelectText });
                ViewBag.CountyList = cdata;
            }
        }

        protected List<CustomCity> GetAllCity()
        {
            var result = (from inc in this.DbInstance.tb_city
                          select new CustomCity { Id = inc.Id, Name = inc.Name }).ToList();
            result.Insert(0, new CustomCity() { Id = null, Name = MessageResource.PleaseSelectText });
            return result;
        }

        protected List<CustomCounty> GetCountyByCityId(int cityId)
        {
            var result = (from inc in this.DbInstance.tb_county
                          where inc.CityId == cityId
                          select new CustomCounty { Id = inc.Id, Name = inc.Name }).ToList();
            return result;
        }

        protected tb_contact GetContactById(int Id)
        {
            tb_contact result = (from inc in this.DbInstance.tb_contact
                                 where inc.Id == Id
                                 select inc).FirstOrDefault();
            return result;
        }

        [HttpPost]
        public ActionResult GetCountyList(string cityID)
        {
            int cityId;
            List<CustomCounty> countyList = new List<CustomCounty>();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            if (int.TryParse(cityID, out cityId))
                countyList = this.GetCountyByCityId(cityId);
            countyList.Insert(0, new CustomCounty() { Id = null, Name = MessageResource.PleaseSelectText });
            return Json(javaScriptSerializer.Serialize(countyList), JsonRequestBehavior.AllowGet);
        }

    }
}