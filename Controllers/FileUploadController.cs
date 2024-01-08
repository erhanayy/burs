using MyAdmin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAdmin.Controllers
{
    public class FileUploadController : BaseController
    {
        public ActionResult GetUploadFileNames(int contactTypeId)
        {
            try
            {
                var contactTypeCode = (from inc in base.DbInstance.ContactTypes
                                   where inc.Id == contactTypeId
                                  select inc.Code).First();
                

                List<FileUploadOut> fileOuts = new List<FileUploadOut>();

                // Veritabanından dosya isimlerini çekme işlemini gerçekleştirin
                // Örnek olarak, Bursiyer tablosundan çekiyorum, ihtiyaca göre diğer tablolardan da çekebilirsiniz.
                //var userFiles = db.BursiyerFiles.Where(f => f.UserId == userId && f.ActorType == actorType).ToList();

                bool isReference = contactTypeCode.StartsWith("Ref-");
                bool isScholar = contactTypeCode == "Scholar";

                var userFiles = (from file in base.DbInstance.FileTypes// db.BursiyerFiles
                                 where (isReference ? file.IsForReference == 1 : file.IsForReference == 0) 
                                       && (isScholar ? file.IsForScholar == 1 : file.IsForScholar == 0)
                                       && file.IsActive == 1
                                 select new FileUploadOut
                                 {
                                     Id = file.Id,
                                     Description = file.Description,
                                     Code = file.Code,
                                     IsRequired = file.IsRequired
                                 }).ToList();

                foreach (var file in userFiles)
                {
                    fileOuts.Add(file);
                }

                return Json(fileOuts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                return Json(new { error = "Bir hata ile karşılaşıldı." /*ex.Message*/ }, JsonRequestBehavior.AllowGet);
            }
        }

        // UploadFile - FormData
        [HttpPost]
        public ActionResult UploadFile()
        {
            try
            {
                /*
                 * contactId parametresi, 
                 * referans için ReferenceFileUpload tablosunun ReferenceId sütununu
                 * burs isteyen için ScholarFileUpload tablosunun ScholarRequestId sütununu işaret eder.
                 * 
                 * */
                int ActiveUID = ActiveUserId;
                // DEBUG ONLY!!!!!
                string debugContactType = null;
                if (Request.Form.AllKeys.Contains("_debugContactType"))
                {
                    debugContactType = Request.Form["_debugContactType"];
                    ActiveUID = debugContactType == "scholar" ? 1 : 2; 
                }
                // <end> DEBUG ONLY!!! (sonra sil)
                int contactId = int.Parse(Request.Form["contactId"]);
                int fileTypeId = int.Parse(Request.Form["fileTypeId"]);

                var contact = (from inc in base.DbInstance.Contacts
                               where inc.Id == ActiveUID
                               select inc).FirstOrDefault();

                if(contact == null)
                {
                    throw new Exception("Contact null olamaz.");
                }

                var contactType = (from cty in base.DbInstance.ContactTypes
                                   join ctr in base.DbInstance.ContactTypeRels on cty.Id equals ctr.ContactTypeId
                                   join cs in base.DbInstance.Contacts on ctr.ContactId equals cs.Id
                                   where cs.Id == ActiveUID
                                   && cs.IsActive
                                   && ctr.IsActive
                                   && cty.IsActive
                                   select cty.Code
                                   ).FirstOrDefault();

                if(contactType == null)
                {
                    throw new Exception("ContactType null olamaz.");
                }

                var fileType = (from inc in base.DbInstance.FileTypes
                                where inc.Id == fileTypeId
                                && inc.IsActive == 1
                                select inc).FirstOrDefault();

                if(fileType == null)
                {
                    throw new Exception("fileType null olamaz.");
                }

                bool isReference = contactType.StartsWith("Ref-");

                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];

                    // güvenlik kontrolü
                    if (file != null)
                    {
                        string originalFileName = Path.GetFileName(file.FileName);
                        string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                        string[] allowedExtensions = new string[] { ".pdf" };
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            throw new Exception("Geçersiz dosya uzantısı. Sadece PDF uzantılı dosyalar kabul edilmektedir.");
                        }

                        string safeFileName = $"{Guid.NewGuid():N}_{contactId}_{fileTypeId}{fileExtension}";

                        string rootDir = Server.MapPath("~/Uploads");
                        var filePath = Path.Combine(rootDir, safeFileName);

                        if (!Directory.Exists(rootDir))
                        {
                            Directory.CreateDirectory(rootDir);

                        }
                        file.SaveAs(filePath);
         

                        if (isReference)
                        {
                            var refFile = new ReferenceFileUpload
                            {
                                IsActive = 1,
                                FileTypeId = fileTypeId,
                                UplodedFileData = filePath,
                                ReferenceId = contactId
                            };

                            base.SetCreateOrUpdateFieldValues(refFile);

                            base.DbInstance.ReferenceFileUploads.Add(refFile);
                            base.DbInstance.SaveChanges();
                        }
                        else
                        {
                            var scholarFile = new ScholarFileUpload
                            {
                                IsActive = 1,
                                FileTypeId = fileTypeId,
                                UploadedFileData = filePath,
                                ScholarRequestId = contactId
                            };

                            base.SetCreateOrUpdateFieldValues(scholarFile);

                            base.DbInstance.ScholarFileUploads.Add(scholarFile);
                            base.DbInstance.SaveChanges();
                        }
                    }
                    else
                    {
                        throw new Exception("Göndermeye çalıştığınız dosya hatalı ya da gönderim sırasında bir hata ile karşılaşıldı.");
                    }
                    
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
