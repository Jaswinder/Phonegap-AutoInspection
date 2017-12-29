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
    public class IssueBooksToROesController : BaseController
    {
        private AutoInsuranceContext db = new AutoInsuranceContext();

        // GET: IssueBooksToROes
        public ActionResult Index()
        {
            var objList = db.IssueBooksToRO.ToList();
            objList.ForEach(x =>
            {
                x.IssuedBy = GetFullname(x.IssuedBy);
                x.IssueTo = GetFullname(x.IssueTo);

            });
            return View(objList);
        }

        // GET: IssueBooksToROes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueBooksToRO issueBooksToRO = db.IssueBooksToRO.Find(id);
            if (issueBooksToRO == null)
            {
                return HttpNotFound();
            }
            return View(issueBooksToRO);
        }

        // GET: IssueBooksToROes/Create
        public ActionResult Create()
        {
            var context = new ApplicationDbContext();
            var role = (from r in context.Roles where r.Name.Contains("RO") select r).FirstOrDefault();
            var users = from u in context.Users
                        where u.Roles.Any(r => r.RoleId == role.Id)
                        select u;
            ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName");
            ViewBag.UsersList = new SelectList(users, "Id", "FullName");
            ViewBag.CourierList = new SelectList(db.CourierCompanyMaster, "CourierCompanyID", "CompanyName");
            List<long> issueddbooks = db.IssueBooksToRO.Select(x => x.StartSNo).ToList();
            var books = from b in db.BookMasters where !issueddbooks.Contains(b.StartSNo) select b;
            ViewBag.StartSNo = books;
            return View();
        }

        // POST: IssueBooksToROes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IssueBookID,IssueTo,IssuedDate,SendToAddress,Status,IssuedBy,StartSNo,EndSno,Remarks,Location,IssuedFrom")] IssueBooksToRO issueBooksToRO,string[] StartSNo)
        {
            if (ModelState.IsValid)
            {
               
                for (int i = 0; i < StartSNo.Length; i++)
                {
                    long StNo =Convert.ToInt64(StartSNo[i]);
                    long endNo = StNo + 49;
                    IssueBooksToRO book = new IssueBooksToRO();
                    book.IssuedBy = User.Identity.GetUserId();
                    book.IssuedDate = DateTime.Now;
                    book.NumberOfBooks = 1;
                    book.StartSNo = StNo;
                    book.EndSno = endNo;
                    book.SendToAddress = issueBooksToRO.SendToAddress;
                    book.Status = issueBooksToRO.Status;
                    book.Remarks = issueBooksToRO.Remarks;
                    book.IssuedFrom = issueBooksToRO.IssuedFrom;
                    book.IssueTo = issueBooksToRO.IssueTo;
                    db.IssueBooksToRO.Add(book);
                }

                db.SaveChanges();
        
                return RedirectToAction("Index");
            }
            var context = new ApplicationDbContext();
            var role = (from r in context.Roles where r.Name.Contains("RO") select r).FirstOrDefault();
            var users = from u in context.Users
                        where u.Roles.Any(r => r.RoleId == role.Id)
                        select u;
            ViewBag.UsersList = new SelectList(users, "Id", "FullName",issueBooksToRO.IssueTo);
            ViewBag.CourierList = new SelectList(db.CourierCompanyMaster, "CourierCompanyID", "CompanyName",issueBooksToRO.CourierCompanyID);
            ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName",issueBooksToRO.Status);
            var books = from b in db.BookMasters select b;
            ViewBag.StartSNo = books;
            return View(issueBooksToRO);
        }
        public string GetUserAddress(string id)
        {
            var context = new ApplicationDbContext();
            var users = (from u in context.Users
                         where u.Id == id
                         select u).FirstOrDefault();
            if (users != null)
            {
                return users.Address;
            }
            return "";
        }
        // GET: IssueBooksToROes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueBooksToRO issueBooksToRO = db.IssueBooksToRO.Find(id);
            if (issueBooksToRO == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName", issueBooksToRO.Status);
            return View(issueBooksToRO);
        }

        // POST: IssueBooksToROes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IssueBookID,IssueTo,IssuedDate,SendToAddress,Status,IssuedBy,StartSNo,EndSno,Remarks,Location,IssuedFrom")] IssueBooksToRO issueBooksToRO)
        {
            if (ModelState.IsValid)
            {
                IssueBooksToRO book = db.IssueBooksToRO.Find(issueBooksToRO.IssueBookID);
                book.Status = issueBooksToRO.Status;
                book.SendToAddress = issueBooksToRO.SendToAddress;
                book.Remarks = issueBooksToRO.Remarks;
                book.Location = issueBooksToRO.Location;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName", issueBooksToRO.Status);

            return View(issueBooksToRO);
        }

        // GET: IssueBooksToROes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueBooksToRO issueBooksToRO = db.IssueBooksToRO.Find(id);
            if (issueBooksToRO == null)
            {
                return HttpNotFound();
            }
            return View(issueBooksToRO);
        }

        // POST: IssueBooksToROes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IssueBooksToRO issueBooksToRO = db.IssueBooksToRO.Find(id);
            db.IssueBooksToRO.Remove(issueBooksToRO);
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
