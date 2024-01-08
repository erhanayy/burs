using MyAdmin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAdmin.Controllers
{
    public class DeclarationController : BaseController
    {
        public ActionResult GetQuestions(int contactTypeId)
        {
            try
            {
                /*
                   var contactType = (from inc in DbInstance.ContactTypes
                                   where inc.Id == contactTypeId
                                   && inc.IsActive
                                   select inc).FirstOrDefault();
                */

                var questions = (from inc in DbInstance.DeclarationQuesitons.AsEnumerable() // for datetime.tostring
                                 where inc.ContactTypeId == contactTypeId
                                 && inc.IsActive == 1
                                 orderby inc.QuestionOrder ascending
                                 select new
                                 {
                                     Id = inc.Id,
                                     Question = inc.Question,
                                     QuestionOrder = inc.QuestionOrder,
                                     ContactTypeId = inc.ContactTypeId,
                                     CreatedBy = inc.CreatedBy,
                                     CreatedDate = inc.CreatedDate != null ? inc.CreatedDate.ToString("u") : null,
                                     UpdatedBy = inc.UpdatedBy,
                                     UpdatedDate = inc.UpdatedDate != null ? inc.UpdatedDate.ToString("u") : null
                                 }).ToList();

                return Json(new { questions = questions }, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
                return Json(new { error = $"{ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveAnswers(DeclarationAnswer[] answers)
        {
            try
            {

                foreach(var answer in answers)
                {
                    var finalAnswer = new DeclarationAnswer
                    {
                        Answer = answer.Answer,
                        AnswerOrder = answer.AnswerOrder,
                        DeclarationQuesitonId = answer.DeclarationQuesitonId,
                        IsActive = 1,
                    };

                    base.SetCreateOrUpdateFieldValues(finalAnswer);

                    DbInstance.DeclarationAnswers.Add(finalAnswer);
                    DbInstance.SaveChanges();
                }

                return Json(new { success = true });

            }catch(Exception ex)
            {
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /*
        public ActionResult Declaration()
        {
            int contactTypeId = 1;

            ViewBag.JsonData = GetJsonDataByContactTypeId(contactTypeId);

            return View();

        }

        private string GetJsonDataByContactTypeId(int contactTypeId)
        {
            using (DbMyadmEntities context = new DbMyadmEntities())
            {
                var query = context.DeclarationQuesitons
                                     .Where(q => q.ContactTypeId == contactTypeId)
                                     .Select(q => new
                                     {
                                         q.Id,
                                         q.Question,
                                         q.QuestionOrder,
                                         q.IsActive,
                                         q.CreatedBy,
                                         q.CreatedDate,
                                         q.UpdatedBy,
                                         q.UpdatedDate,
                                         q.ContactTypeId
                                     })
                                     .ToList();

                string jsonData = JsonConvert.SerializeObject(query, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented
                });

                return jsonData;
            }
        }
        */
    }
}
