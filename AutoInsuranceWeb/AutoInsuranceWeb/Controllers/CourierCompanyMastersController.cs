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
    public class CourierCompanyMastersController : Controller
    {
        private AutoInsuranceContext db = new AutoInsuranceContext();

        // GET: CompanyMasters
        public ActionResult Index()
        {
            return View(db.CourierCompanyMaster.ToList());
        }

        // GET: CompanyMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourierCompanyMaster CouriercompanyMaster = db.CourierCompanyMaster.Find(id);
            if (CouriercompanyMaster == null)
            {
                return HttpNotFound();
            }
            return View(CouriercompanyMaster);
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
        public ActionResult Create(CourierCompanyMaster CouriercompanyMaster)
        {
            if (ModelState.IsValid)
            {
                db.CourierCompanyMaster.Add(CouriercompanyMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(CouriercompanyMaster);
        }

        // GET: CompanyMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourierCompanyMaster CouriercompanyMaster = db.CourierCompanyMaster.Find(id);
            if (CouriercompanyMaster == null)
            {
                return HttpNotFound();
            }
            return View(CouriercompanyMaster);
        }

        // POST: CompanyMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( CourierCompanyMaster CouriercompanyMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(CouriercompanyMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(CouriercompanyMaster);
        }

        // GET: CompanyMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourierCompanyMaster CouriercompanyMaster = db.CourierCompanyMaster.Find(id);
            if (CouriercompanyMaster == null)
            {
                return HttpNotFound();
            }
            return View(CouriercompanyMaster);
        }

        // POST: CompanyMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourierCompanyMaster CouriercompanyMaster = db.CourierCompanyMaster.Find(id);
            db.CourierCompanyMaster.Remove(CouriercompanyMaster);
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
