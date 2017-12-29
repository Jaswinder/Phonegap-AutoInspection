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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AutoInsuranceWeb.Controllers
{
    public class DSABookManagerController : BaseController
    {
        private AutoInsuranceContext db = new AutoInsuranceContext();
        // GET: DSABookManager
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var reportpreview = db.IssueBooksToDSA.Where(x => x.IssueTo == userid).ToList();
            reportpreview.ForEach(x => {
                x.IssueTo = GetUsername(x.IssueTo);
                x.IssuedBy = GetUsername(x.IssuedBy);
            });
            return View(reportpreview);
        }

        // GET: IssueBooksToDSAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueBooksToDSA issueBooksToDSA = db.IssueBooksToDSA.Find(id);
            if (issueBooksToDSA == null)
            {
                return HttpNotFound();
            }
            return View(issueBooksToDSA);
        }
        // GET: IssueBooksToDSAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueBooksToDSA issueBooksToDSA = db.IssueBooksToDSA.Find(id);
            if (issueBooksToDSA == null)
            {
                return HttpNotFound();
            }
            var context = new ApplicationDbContext();
            var role = (from r in context.Roles where r.Name.Contains("DSA") select r).FirstOrDefault();
            var users = from u in context.Users
                        where u.Roles.Any(r => r.RoleId == role.Id)
                        select u;
            ViewBag.UsersList = new SelectList(users, "Id", "FullName", issueBooksToDSA.IssueTo);
            ViewBag.VehicleCategories = new SelectList(db.VehicleCategories, "VehicleCategoryID", "VehicleCategoryName",issueBooksToDSA.VehicleCategoryID);
            ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName", issueBooksToDSA.Status);
            return View(issueBooksToDSA);
        }

        // POST: IssueBooksToDSAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IssueBookID,IssueTo,IssuedDate,SendToAddress,Status,IssuedBy,StartSNo,EndSno,Remarks,Location,IssuedFrom,VehicleCategoryID")] IssueBooksToDSA issueBooksToDSA)
        {
            if (ModelState.IsValid)
            {
                IssueBooksToDSA book = db.IssueBooksToDSA.Find(issueBooksToDSA.IssueBookID);
                book.Status = issueBooksToDSA.Status;
                book.SendToAddress = issueBooksToDSA.SendToAddress;
                book.Remarks = issueBooksToDSA.Remarks;
                book.Location = issueBooksToDSA.Location;
                book.VehicleCategoryID = issueBooksToDSA.VehicleCategoryID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var context = new ApplicationDbContext();
            var role = (from r in context.Roles where r.Name.Contains("DSA") select r).FirstOrDefault();
            var users = from u in context.Users
                        where u.Roles.Any(r => r.RoleId == role.Id)
                        select u;
            ViewBag.UsersList = new SelectList(users, "Id", "FullName", issueBooksToDSA.IssueTo);
            ViewBag.VehicleCategories = new SelectList(db.VehicleCategories, "VehicleCategoryID", "VehicleCategoryName",issueBooksToDSA.VehicleCategoryID);
            ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName", issueBooksToDSA.Status);
            return View(issueBooksToDSA);
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