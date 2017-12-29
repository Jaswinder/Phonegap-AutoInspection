using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoInsuranceWeb.Context;
using AutoInsuranceWeb.Models;

namespace AutoInsuranceWeb.Controllers
{
    public class CompanyMastersController : Controller
    {
        private AutoInsuranceContext db = new AutoInsuranceContext();

        // GET: CompanyMasters
        public ActionResult Index()
        {
            return View(db.CompanyMaster.ToList());
        }

        // GET: CompanyMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyMaster companyMaster = db.CompanyMaster.Find(id);
            if (companyMaster == null)
            {
                return HttpNotFound();
            }
            return View(companyMaster);
        }

        // GET: CompanyMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyID,CompanyName,CompanyRemarks,Status")] CompanyMaster companyMaster)
        {
            if (ModelState.IsValid)
            {
                db.CompanyMaster.Add(companyMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companyMaster);
        }

        // GET: CompanyMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyMaster companyMaster = db.CompanyMaster.Find(id);
            if (companyMaster == null)
            {
                return HttpNotFound();
            }
            return View(companyMaster);
        }

        // POST: CompanyMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyID,CompanyName,CompanyRemarks,Status")] CompanyMaster companyMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyMaster);
        }

        // GET: CompanyMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyMaster companyMaster = db.CompanyMaster.Find(id);
            if (companyMaster == null)
            {
                return HttpNotFound();
            }
            return View(companyMaster);
        }

        // POST: CompanyMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyMaster companyMaster = db.CompanyMaster.Find(id);
            db.CompanyMaster.Remove(companyMaster);
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
