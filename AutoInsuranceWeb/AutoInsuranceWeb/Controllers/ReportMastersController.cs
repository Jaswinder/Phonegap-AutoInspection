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
using Ionic.Zip;
using System.IO;
using HiQPdf;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace AutoInsuranceWeb.Controllers
{
    public class ReportMastersController : BaseController
    {
        private AutoInsuranceContext db = new AutoInsuranceContext();

        // GET: ReportMasters
        public ActionResult Index(string startdate, string enddate)
        {
            string userid = "";
            if (Request.IsAuthenticated)
            {
                userid = User.Identity.GetUserId();
            }
            
            var userlist = GetMyChildUsers(userid);
            var reportpreview = db.ReportMaster.Where(x => userlist.Contains(x.UserId) && x.Status == 6).ToList();
            ViewBag.fromdate = "";
            ViewBag.todate = "";
            if (!string.IsNullOrEmpty(startdate))
            {
                ViewBag.fromdate = startdate;
               DateTime str = DateTime.Parse(startdate);
                reportpreview.Where(x => x.StartDate >= str);
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                ViewBag.todate = enddate;
                DateTime str = DateTime.Parse(enddate);
                reportpreview.Where(x => x.StartDate <= str);
            }
            reportpreview.ForEach(x => {
                x.CompanyName = GetCompanyName(x.CompanyID);
                x.Comment = GetVehicleCategory(x.VehicleCategoryID);
                x.UserId = GetUsername(x.UserId);
                //  x.StartDate = string.Format("{0:d}", (x.StartDate)) //(x.StartDate).Value.ToShortDateString(); 
               // x.StartDate = string.IsNullOrEmpty(string.Format("{0:d}", (x.StartDate))) ? (DateTime?)null : DateTime.Parse(string.Format("{0:d}", (x.StartDate)));
            });
            return View(reportpreview);
        }

        // GET: ReportMasters/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportMaster reportMaster = db.ReportMaster.Find(id);
            if (reportMaster == null)
            {
                return HttpNotFound();
            }
            return View(reportMaster);
        }

        // GET: ReportMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportID,ReportNumber,UserId,StartDate,FinishDate,Status,IsLocked,VehicleCategoryID,VehicleNumber,ChassisNumber,BookNumber,Comment,CompanyID,CompanyName,Longitude,Latitude,ClientLocation")] ReportMaster reportMaster)
        {
            if (ModelState.IsValid)
            {
                db.ReportMaster.Add(reportMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportMaster);
        }

        // GET: ReportMasters/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportMaster reportMaster = db.ReportMaster.Find(id);
            if (reportMaster == null)
            {
                return HttpNotFound();
            }
            return View(reportMaster);
        }

        // POST: ReportMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReportID,ReportNumber,UserId,StartDate,FinishDate,Status,IsLocked,VehicleCategoryID,VehicleNumber,ChassisNumber,BookNumber,Comment,CompanyID,CompanyName,Longitude,Latitude,ClientLocation")] ReportMaster reportMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reportMaster);
        }

        public ActionResult ViewImages(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ReportID = id;
            var reportMaster = db.ReportImages.Where(x=>x.ReportID==id).ToList();
            if (reportMaster == null)
            {
                return HttpNotFound();
            }
            var ReportObj = db.ReportMaster.Where(x => x.ReportID == id).FirstOrDefault();
            ViewBag.VehicleCategory = "";
            if (ReportObj != null)
            {
                ReportObj.CompanyName = GetCompanyName(Convert.ToInt32(ReportObj.CompanyID));
                ViewBag.VehicleCategory = GetVehicleCategory(ReportObj.VehicleCategoryID);
            }
            ViewBag.ReportObj = ReportObj;
            return View(reportMaster);
        }
        public ActionResult DownloadImages(long? id)
        {
            using (ZipFile zip = new ZipFile())
            {
                var reportMaster = db.ReportImages.Where(x => x.ReportID == id).ToList();
                long reportnumber = 1;
                if (reportMaster != null)
                {
                    string dd = DateTime.Now.ToLongDateString();
                    foreach (var item in reportMaster)
                    {
                        if (item.ImagePathStemped.Length > 7)
                        {
                            zip.AddFile(Server.MapPath(item.ImagePathStemped), "Images-Approved-"+ dd +"_" + id);
                        }
                        else if (item.ImagePath.Length > 7)
                        {
                            zip.AddFile(Server.MapPath(item.ImagePath), "Images-Approved-"+ dd +"_" + id);
                        }
                        reportnumber = item.ReportNumber;
                    }
                   // zip.AddFile(Server.MapPath("/Content/Img/onam.jpg"), "files");

                   // zip.AddFile(Server.MapPath("/Content/Img/onam_stam.jpg"), "files");
                }
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedImages-"+ reportnumber + ".zip");
                Response.ContentType = "application/zip";
                zip.Save(Response.OutputStream);
                Response.End();
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Export(string GridHtml,string ID)
        {
            // get the HTML code of this view
            string htmlToConvert = GridHtml;

            // the base URL to resolve relative images and css
            //String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            //String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length - "Home/ConvertThisPageToPdf".Length);

            // instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            // hide the button in the created PDF
            htmlToPdfConverter.HiddenHtmlElements = new string[] { "#convertThisPageButtonDiv" };

            // render the HTML code as PDF in memory
            byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, "http://autoinsurance.flashcontacts.org/");

            // send the PDF file to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "ReportPdf"+ ID+".pdf";

            return fileResult;
        }
        public ActionResult DownloadVideos(long? id)
        {
            using (ZipFile zip = new ZipFile())
            {
                var reportMaster = db.ReportVideos.Where(x => x.ReportID == id).ToList();
                long reportnumber = 1;
                if (reportMaster != null)
                {
                    string dd = DateTime.Now.ToLongDateString();
                    foreach (var item in reportMaster)
                    {
                        if (item.VideoPath.Length > 7)
                        {
                            zip.AddFile(Server.MapPath(item.VideoPath), "Videos-" + dd + "-" + item.ReportNumber);
                        }
                        reportnumber = item.ReportNumber;
                    }
                    // zip.AddFile(Server.MapPath("/Content/Img/onam.jpg"), "files");

                    // zip.AddFile(Server.MapPath("/Content/Img/onam_stam.jpg"), "files");
                }
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedVideos-"+ reportnumber + ".zip");
                Response.ContentType = "application/zip";
                zip.Save(Response.OutputStream);
                Response.End();
                return View();
            }
        }
        // GET: ReportMasters/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportMaster reportMaster = db.ReportMaster.Find(id);
            if (reportMaster == null)
            {
                return HttpNotFound();
            }
            return View(reportMaster);
        }

        // POST: ReportMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ReportMaster reportMaster = db.ReportMaster.Find(id);
            db.ReportMaster.Remove(reportMaster);
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
