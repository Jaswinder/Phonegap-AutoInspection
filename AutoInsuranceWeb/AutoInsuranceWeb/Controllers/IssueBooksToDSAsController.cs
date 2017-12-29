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
    public class IssueBooksToDSAsController : BaseController
    {
        private AutoInsuranceContext db = new AutoInsuranceContext();

        // GET: IssueBooksToDSAs
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            if (User.IsInRole("MIS")) {
                var objList = db.IssueBooksToDSA.ToList();
                objList.ForEach(x =>
                {
                    x.IssuedBy = GetFullname(x.IssuedBy);
                    x.IssueTo = GetFullname(x.IssueTo);

                });
                return View(objList);
            }
            else
            {
                var objList = db.IssueBooksToDSA.Where(x => x.IssuedBy == userid).ToList();//db.IssueBooksToDSA.ToList();
                objList.ForEach(x =>
                {
                    x.IssuedBy = GetFullname(x.IssuedBy);
                    x.IssueTo = GetFullname(x.IssueTo);

                });
                return View(objList);

               // return View(db.IssueBooksToDSA.Where(x => x.IssuedBy == userid).ToList());

            }
            
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

        // GET: IssueBooksToDSAs/Create
        public ActionResult Create(string pid="",string uid="")
        {
            var userid = User.Identity.GetUserId();
           
            var userlist = GetMyChildUsers(userid);
            var context = new ApplicationDbContext();
            var role = (from r in context.Roles where r.Name.Contains("DSA") select r).FirstOrDefault();
            IssueBooksToDSA issuebooks = new IssueBooksToDSA();
            if (User.IsInRole("MIS"))
            { 
                var users = from u in context.Users
                            where u.Roles.Any(r => r.RoleId == role.Id)
                            select u;
                ViewBag.UsersList = new SelectList(users, "Id", "FullName", uid);
                var uu = users.Where(x => x.Id == uid).FirstOrDefault();
                if(uu!=null)
                {
                    pid = uu.ParentUserid;
                    issuebooks.SendToAddress = uu.Address;
                }
                List<long> issueddbooks = db.IssueBooksToDSA.Select(x => x.StartSNo).ToList();

                var books = from b in db.IssueBooksToRO where b.IssueTo == pid && !issueddbooks.Contains(b.StartSNo) select b;
                ViewBag.StartSNo = books;
            }
            else
            {
                var users = from u in context.Users
                            where u.Roles.Any(r => r.RoleId == role.Id) && userlist.Contains(u.Id)
                            select u;
                ViewBag.UsersList = new SelectList(users, "Id", "FullName");
                List<long> issueddbooks = db.IssueBooksToDSA.Select(x => x.StartSNo).ToList();

                var books = from b in db.IssueBooksToRO where b.IssueTo == userid && !issueddbooks.Contains(b.StartSNo) select b;
                ViewBag.StartSNo = books;
            }

            ViewBag.CourierList = new SelectList(db.CourierCompanyMaster, "CourierCompanyID", "CompanyName");
            ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName");
            ViewBag.VehicleCategories = new SelectList(db.VehicleCategories, "VehicleCategoryID", "VehicleCategoryName");
           
            return View(issuebooks);
        }

        // POST: IssueBooksToDSAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IssueBookID,IssueTo,IssuedDate,SendToAddress,Status,IssuedBy,StartSNo,EndSno,Remarks,Location,IssuedFrom")] IssueBooksToDSA issueBooksToDSA, string[] StartSNo, string pid = "", string uid = "")
        {
            var userid = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                for (int i = 0; i < StartSNo.Length; i++)
                {
                    long StNo = Convert.ToInt64(StartSNo[i]);
                    long endNo = StNo + 49;
                    IssueBooksToDSA book = new IssueBooksToDSA();
                    book.IssuedBy = userid;
                    book.IssuedDate = DateTime.Now;
                    book.NumberOfBooks = 1;
                    book.StartSNo = StNo;
                    book.EndSno = endNo;
                    book.SendToAddress = issueBooksToDSA.SendToAddress;
                    book.Status = issueBooksToDSA.Status;
                    book.Remarks = issueBooksToDSA.Remarks;
                    book.IssuedFrom = issueBooksToDSA.IssuedFrom;
                    book.VehicleCategoryID = 1;// issueBooksToDSA.VehicleCategoryID;
                    book.IssueTo = issueBooksToDSA.IssueTo;
                    db.IssueBooksToDSA.Add(book);
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var context = new ApplicationDbContext();
            var role = (from r in context.Roles where r.Name.Contains("DSA") select r).FirstOrDefault();
            ViewBag.uid = uid;
            if (User.IsInRole("MIS"))
            {
                var users = from u in context.Users
                            where u.Roles.Any(r => r.RoleId == role.Id)
                            select u;
                ViewBag.UsersList = new SelectList(users, "Id", "FullName",uid);
                var uu = users.Where(x => x.Id == uid).FirstOrDefault();
                if (uu != null)
                {
                    pid = uu.ParentUserid;
                }
                List<long> issueddbooks = db.IssueBooksToDSA.Select(x => x.StartSNo).ToList();

                var books = from b in db.IssueBooksToRO where b.IssueTo == pid && !issueddbooks.Contains(b.StartSNo) select b;
                ViewBag.StartSNo = books;
            }
            else
            {
                
                
                var users = from u in context.Users
                            where u.Roles.Any(r => r.RoleId == role.Id)
                            select u;
                ViewBag.UsersList = new SelectList(users, "Id", "FullName", issueBooksToDSA.IssueTo);
                ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName", issueBooksToDSA.Status);
                ViewBag.VehicleCategories = new SelectList(db.VehicleCategories, "VehicleCategoryID", "VehicleCategoryName");
                List<long> issueddbooks = db.IssueBooksToDSA.Select(x => x.StartSNo).ToList();

                var books = from b in db.IssueBooksToRO where b.IssueTo == userid && !issueddbooks.Contains(b.StartSNo) select b;
                ViewBag.StartSNo = books;
            }
            ViewBag.CourierList = new SelectList(db.CourierCompanyMaster, "CourierCompanyID", "CompanyName", issueBooksToDSA.CourierCompanyID);
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
            ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName", issueBooksToDSA.Status);
            ViewBag.VehicleCategories = new SelectList(db.VehicleCategories, "VehicleCategoryID", "VehicleCategoryName");
            return View(issueBooksToDSA);
        }

        // POST: IssueBooksToDSAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IssueBookID,IssueTo,IssuedDate,SendToAddress,Status,IssuedBy,StartSNo,EndSno,Remarks,Location,IssuedFrom")] IssueBooksToDSA issueBooksToDSA)
        {
            if (ModelState.IsValid)
            {
                IssueBooksToDSA book = db.IssueBooksToDSA.Find(issueBooksToDSA.IssueBookID);
                book.Status = issueBooksToDSA.Status;
                book.SendToAddress = issueBooksToDSA.SendToAddress;
                book.Remarks = issueBooksToDSA.Remarks;
                book.Location = issueBooksToDSA.Location;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var context = new ApplicationDbContext();
            var role = (from r in context.Roles where r.Name.Contains("DSA") select r).FirstOrDefault();
            var users = from u in context.Users
                        where u.Roles.Any(r => r.RoleId == role.Id)
                        select u;
            ViewBag.UsersList = new SelectList(users, "Id", "FullName", issueBooksToDSA.IssueTo);
            ViewBag.DeliveryStatus = new SelectList(db.DeliveryStatus, "Status", "DeliveryStatusName", issueBooksToDSA.Status);
            ViewBag.VehicleCategories = new SelectList(db.VehicleCategories, "VehicleCategoryID", "VehicleCategoryName");
            return View(issueBooksToDSA);
        }

        // GET: IssueBooksToDSAs/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: IssueBooksToDSAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IssueBooksToDSA issueBooksToDSA = db.IssueBooksToDSA.Find(id);
            db.IssueBooksToDSA.Remove(issueBooksToDSA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CheckValidNumber(string startNumber, string bookId = "0")
        {
            int vstartNumber = Convert.ToInt32(startNumber);
            if (bookId == "0")
            {
                var bookNumber = db.IssueBooksToDSA.Where(x => vstartNumber >= x.StartSNo && vstartNumber <= x.EndSno).FirstOrDefault();
                if (bookNumber == null)
                {
                    return Json("YES");
                }
                else
                {
                    return Json("NO");
                }
            }
            else
            {
                int vbookId = Convert.ToInt32(bookId);
                var bookNumber = db.IssueBooksToDSA.Where(x => vstartNumber >= x.StartSNo && vstartNumber <= x.EndSno && x.IssueBookID != vbookId).FirstOrDefault();
                if (bookNumber == null)
                {
                    return Json("YES");
                }
                else
                {
                    return Json("NO");
                }
            }
        }
        [HttpPost]
        public ActionResult CheckValidBookTitle(string bookTitle, string bookId = "0")
        {
            if (bookId == "0")
            {
                var bookNumber = db.BookMasters.Where(x => x.BookTitle == bookTitle).FirstOrDefault();
                if (bookNumber == null)
                {
                    return Json("YES");
                }
                else
                {
                    return Json("NO");
                }
            }
            else
            {
                int vbookId = Convert.ToInt32(bookId);
                var bookNumber = db.BookMasters.Where(x => x.BookTitle == bookTitle && x.BookMasterID != vbookId).FirstOrDefault();
                if (bookNumber == null)
                {
                    return Json("YES");
                }
                else
                {
                    return Json("NO");
                }
            }
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
