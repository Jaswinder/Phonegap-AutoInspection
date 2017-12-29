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
    public class ReportStatusController : Controller
    {
        private AutoInsuranceContext db = new AutoInsuranceContext();

        // GET: ReportStatus
        public ActionResult Index()
        {
            return View(db.ReportStatus.ToList());
        }

        // GET: ReportStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportStatus reportStatus = db.ReportStatus.Find(id);
            if (reportStatus == null)
            {
                return HttpNotFound();
            }
            return View(reportStatus);
        }

        // GET: ReportStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportStatusID,ReportStatusName,Status")] ReportStatus reportStatus)
        {
            if (ModelState.IsValid)
            {
                db.ReportStatus.Add(reportStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportStatus);
        }

        // GET: ReportStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportStatus reportStatus = db.ReportStatus.Find(id);
            if (reportStatus == null)
            {
                return HttpNotFound();
            }
            return View(reportStatus);
        }

        // POST: ReportStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReportStatusID,ReportStatusName,Status")] ReportStatus reportStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reportStatus);
        }

        // GET: ReportStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportStatus reportStatus = db.ReportStatus.Find(id);
            if (reportStatus == null)
            {
                return HttpNotFound();
            }
            return View(reportStatus);
        }

        // POST: ReportStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportStatus reportStatus = db.ReportStatus.Find(id);
            db.ReportStatus.Remove(reportStatus);
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
