using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyAdmin.Models;

namespace MyAdmin.Controllers
{
    public class BursTanimlamaController : Controller
    {
        private DbMyadmEntities db = new DbMyadmEntities();

        // GET: BursTanimlama
        public ActionResult Index()
        {
            var scholarships = db.Scholarships.Include(s => s.Contact).Include(s => s.Season).Include(s => s.Year);
            return View(scholarships.ToList());
        }

        // GET: BursTanimlama/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scholarship scholarship = db.Scholarships.Find(id);
            if (scholarship == null)
            {
                return HttpNotFound();
            }
            return View(scholarship);
        }

        // GET: BursTanimlama/Create
        public ActionResult Create()
        {
            ViewBag.ScholarshipOwnerContactId = new SelectList(db.Contacts, "Id", "Name");
            ViewBag.SeasonId = new SelectList(db.Seasons, "Id", "Season1");
            ViewBag.YearId = new SelectList(db.Years, "Id", "Code");
            return View();
        }

        // POST: BursTanimlama/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ScholarshipOwnerContactId,ScholarshipName,ScholarshipDescription,YearId,SeasonId,GiverCount,GiverMonthlyDonateAmount,ScholarCount,ScholarDonateAmount")] Scholarship scholarship)
        {
            if (ModelState.IsValid)
            {
                db.Scholarships.Add(scholarship);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ScholarshipOwnerContactId = new SelectList(db.Contacts, "Id", "Name", scholarship.ScholarshipOwnerContactId);
            ViewBag.SeasonId = new SelectList(db.Seasons, "Id", "Season1", scholarship.SeasonId);
            ViewBag.YearId = new SelectList(db.Years, "Id", "Code", scholarship.YearId);
            return View(scholarship);
        }

        // GET: BursTanimlama/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scholarship scholarship = db.Scholarships.Find(id);
            if (scholarship == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScholarshipOwnerContactId = new SelectList(db.Contacts, "Id", "Name", scholarship.ScholarshipOwnerContactId);
            ViewBag.SeasonId = new SelectList(db.Seasons, "Id", "Season1", scholarship.SeasonId);
            ViewBag.YearId = new SelectList(db.Years, "Id", "Code", scholarship.YearId);
            return View(scholarship);
        }

        // POST: BursTanimlama/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ScholarshipOwnerContactId,ScholarshipName,ScholarshipDescription,YearId,SeasonId,GiverCount,GiverMonthlyDonateAmount,ScholarCount,ScholarDonateAmount")] Scholarship scholarship)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scholarship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ScholarshipOwnerContactId = new SelectList(db.Contacts, "Id", "Name", scholarship.ScholarshipOwnerContactId);
            ViewBag.SeasonId = new SelectList(db.Seasons, "Id", "Season1", scholarship.SeasonId);
            ViewBag.YearId = new SelectList(db.Years, "Id", "Code", scholarship.YearId);
            return View(scholarship);
        }

        // GET: BursTanimlama/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scholarship scholarship = db.Scholarships.Find(id);
            if (scholarship == null)
            {
                return HttpNotFound();
            }
            return View(scholarship);
        }

        // POST: BursTanimlama/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Scholarship scholarship = db.Scholarships.Find(id);
            db.Scholarships.Remove(scholarship);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
