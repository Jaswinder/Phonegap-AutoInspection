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
    public class BookMastersController : Controller
    {
        private AutoInsuranceContext db = new AutoInsuranceContext();

        // GET: BookMasters
        public ActionResult Index()
        {
            return View(db.BookMasters.ToList());
        }

        // GET: BookMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookMasterViewModels bookMasterViewModels = db.BookMasters.Find(id);
            if (bookMasterViewModels == null)
            {
                return HttpNotFound();
            }
            return View(bookMasterViewModels);
        }

        // GET: BookMasters/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: BookMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookMasterID,BookTitle,StartSNo,EndSno,CreatedDate,CreatedBy,NumberOfBooks")] BookMasterViewModels bookMasterViewModels)
        {
            if (ModelState.IsValid)
            {
               
                long numberOfBooks = bookMasterViewModels.NumberOfBooks;
                long StNo = bookMasterViewModels.StartSNo;
                long endNo = StNo + 49;
                for (int i = 1; i <= numberOfBooks; i++)
                {
                    BookMasterViewModels book = new BookMasterViewModels();
                    book.CreatedBy = User.Identity.GetUserId();
                    book.CreatedDate = DateTime.Now;
                    book.NumberOfBooks = 1;
                    book.StartSNo = StNo;
                    book.EndSno = endNo;
                    book.BookTitle = bookMasterViewModels.BookTitle;
                    db.BookMasters.Add(book);
                    StNo = StNo + 50;
                    endNo = endNo + 50;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookMasterViewModels);
        }

        // GET: BookMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookMasterViewModels bookMasterViewModels = db.BookMasters.Find(id);
            if (bookMasterViewModels == null)
            {
                return HttpNotFound();
            }
            return View(bookMasterViewModels);
        }

        // POST: BookMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookMasterID,BookTitle,StartSNo,EndSno,CreatedDate,CreatedBy,NumberOfBooks")] BookMasterViewModels bookMasterViewModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookMasterViewModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookMasterViewModels);
        }

        // GET: BookMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookMasterViewModels bookMasterViewModels = db.BookMasters.Find(id);
            if (bookMasterViewModels == null)
            {
                return HttpNotFound();
            }
            return View(bookMasterViewModels);
        }

        // POST: BookMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookMasterViewModels bookMasterViewModels = db.BookMasters.Find(id);
            db.BookMasters.Remove(bookMasterViewModels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CheckValidNumber(string startNumber,string bookId= "0")
        {
            int vstartNumber = Convert.ToInt32(startNumber);
            if (bookId == "0")
            { 
                var bookNumber = db.BookMasters.Where(x => vstartNumber >= x.StartSNo  && vstartNumber<= x.EndSno ).FirstOrDefault();
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
                var bookNumber = db.BookMasters.Where(x => vstartNumber >= x.StartSNo && vstartNumber <= x.EndSno && x.BookMasterID!=vbookId).FirstOrDefault();
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
                var bookNumber = db.BookMasters.Where(x => x.BookTitle== bookTitle).FirstOrDefault();
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
