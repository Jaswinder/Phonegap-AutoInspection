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

namespace AutoInsuranceWeb.Controllers
{
    public class TempReportMastersController : BaseController
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

            var reportpreview = db.ReportMasterPreview.Where(x => userlist.Contains(x.UserId) && x.Status != 6).ToList();
            if (User.IsInRole("MIS"))
            {
                reportpreview = db.ReportMasterPreview.Where(x => x.Status != 6).ToList();
            }
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
            reportpreview.ForEach(x =>
            {
                x.CompanyName = GetCompanyName(x.CompanyID);
                x.Comment = GetVehicleCategory(x.VehicleCategoryID);
                x.UserId = GetUsername(x.UserId);
            });
            return View(reportpreview);
        }
        public string RenderViewAsString(string viewName, object model)
        {
            // create a string writer to receive the HTML code
            StringWriter stringWriter = new StringWriter();

            // get the view to render
            ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
            // create a context to render a view based on a model
            ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    new ViewDataDictionary(model),
                    new TempDataDictionary(),
                    stringWriter
                    );

            // render the view to a HTML code
            viewResult.View.Render(viewContext, stringWriter);

            // return the HTML code
            return stringWriter.ToString();
        }


        [HttpPost]
        public ActionResult ConvertToPdf()
        {
            // get the HTML code of this view
            string htmlToConvert = RenderViewAsString("ViewImages", null);

            // the base URL to resolve relative images and css
            String thisPageUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            String baseUrl = thisPageUrl.Substring(0, thisPageUrl.Length - "Home/ConvertThisPageToPdf".Length);

            // instantiate the HiQPdf HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            // hide the button in the created PDF
            htmlToPdfConverter.HiddenHtmlElements = new string[] { "#convertThisPageButtonDiv" };

            // render the HTML code as PDF in memory
            byte[] pdfBuffer = htmlToPdfConverter.ConvertHtmlToMemory(htmlToConvert, baseUrl);

            // send the PDF file to browser
            FileResult fileResult = new FileContentResult(pdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "ThisMvcViewToPdf.pdf";

            return fileResult;
        }


        // GET: ReportMasters/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportMasterPreview reportMaster = db.ReportMasterPreview.Find(id);
            if (reportMaster == null)
            {
                return HttpNotFound();
            }
            return View(reportMaster);
        }

        public ActionResult Approve(long? id)
        {
            //message
            string userid = "";
            if (Request.IsAuthenticated)
            {
                userid = User.Identity.GetUserId();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportMasterPreview reportMaster = db.ReportMasterPreview.Find(id);
            if (reportMaster == null)
            {
                return HttpNotFound();
            }
            if (reportMaster != null)
            {
                long booknumber = reportMaster.BookNumber;
                var bookfromdsa = db.IssueBooksToDSA.Where(x => x.StartSNo ==  booknumber && x.IssueTo== userid).FirstOrDefault();
                if(User.IsInRole("MIS"))
                {
                    bookfromdsa = db.IssueBooksToDSA.Where(x => x.StartSNo == booknumber).FirstOrDefault();
                }
                if (bookfromdsa != null)
                {
                    ReportMaster mainreport = new ReportMaster();
                    mainreport.ReportNumber = reportMaster.ReportNumber;
                    mainreport.UserId = reportMaster.UserId;
                    mainreport.StartDate = DateTime.Now.Date;
                    mainreport.FinishDate = DateTime.Now.Date;
                    mainreport.Status = 6;
                    mainreport.IsLocked = true;
                    mainreport.VehicleCategoryID = reportMaster.VehicleCategoryID;
                    mainreport.VehicleNumber = reportMaster.VehicleNumber;
                    mainreport.ChassisNumber = reportMaster.ChassisNumber;
                    mainreport.BookNumber = reportMaster.BookNumber;
                    mainreport.Comment = reportMaster.Comment;
                    mainreport.CompanyID = reportMaster.CompanyID;
                    mainreport.CompanyName = reportMaster.CompanyName;
                    mainreport.Longitude = reportMaster.Longitude;
                    mainreport.Latitude = reportMaster.Latitude;
                    mainreport.ClientLocation = reportMaster.ClientLocation;
                    mainreport.ApproverID = userid;
                    db.ReportMaster.Add(mainreport);
                    db.SaveChanges();



                    #region Transfer Images
                    var images = db.ReportImages.Where(x => x.ReportID == reportMaster.ReportID).ToList();
                    images.ForEach(a => { a.ReportID = mainreport.ReportID; a.ReportNumber = mainreport.ReportNumber; });

                    db.SaveChanges();
                    #endregion
                    #region Transfer Videos
                    var videos = db.ReportVideos.Where(x => x.ReportID == reportMaster.ReportID).ToList();
                    videos.ForEach(a => { a.ReportID = mainreport.ReportID;a.ReportNumber = mainreport.ReportNumber; });
                    reportMaster.Status = 6;

                    #endregion
                    #region Transfer Attributes
                    var attributes = db.ReportAttributeValue.Where(x => x.ReportID == reportMaster.ReportID).ToList();
                    attributes.ForEach(a => { a.ReportID = mainreport.ReportID; });
                    #endregion
                    reportMaster.Status = 6;

                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("Edit",new { id=id, message ="This book number is not valid."});
                }
            }
            return RedirectToAction("Index");
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
        public ActionResult Create([Bind(Include = "ReportID,ReportNumber,UserId,StartDate,FinishDate,Status,IsLocked,VehicleCategoryID,VehicleNumber,ChassisNumber,BookNumber,Comment,CompanyID,CompanyName,Longitude,Latitude,ClientLocation")] ReportMasterPreview reportMaster)
        {
            if (ModelState.IsValid)
            {
                db.ReportMasterPreview.Add(reportMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportMaster);
        }

        // GET: ReportMasters/Edit/5

        public void loaddropdowns(ReportMasterPreview model)
        {
            var Statu = db.ReportStatus.ToList();
            ViewBag.Status = new SelectList(Statu, "ReportStatusID", "ReportStatusName", model.Status);

            var VehicleCategories = db.VehicleCategories.ToList();
            ViewBag.VehicleCategories = new SelectList(VehicleCategories, "VehicleCategoryID", "VehicleCategoryName", model.Status);

            var CompanyMaster = db.CompanyMaster.ToList();
            ViewBag.CompanyMaster = new SelectList(CompanyMaster, "CompanyID", "CompanyName", model.Status);
        }
        public string GetAttributeValue(long ReportAtrrId,long? reportId,string type)
        {
            var table = db.ReportAttributeValue.Where(v => v.ReportAttributeID == ReportAtrrId && v.ReportID== reportId).FirstOrDefault();
            if(table!=null)
            {
                return table.ReportAttributeValueName;
            }
            else
            {
                
                ReportAttributeValue rpt = new ReportAttributeValue();
                rpt.ReportAttributeID = ReportAtrrId;
                rpt.ReportID =GetLong(reportId.ToString());
                if (type == "COMBO")
                {
                    rpt.ReportAttributeValueName = "Safe";
                }
                else
                {
                    rpt.ReportAttributeValueName = "";
                }
                db.ReportAttributeValue.Add(rpt);
                db.SaveChanges();
                return rpt.ReportAttributeValueName;
            }
        }
        [HttpPost]
        public void SaveAttributeValue(long reportId,long attributeId, string attributeValue="")
        {
            var atValue = db.ReportAttributeValue.Where(x => x.ReportID == reportId && x.ReportAttributeID == attributeId).FirstOrDefault();
            if(atValue==null)
            {
                ReportAttributeValue report = new ReportAttributeValue();
                report.ReportAttributeValueName = attributeValue;
                report.ReportID = reportId;
                report.ReportAttributeID = attributeId;
                db.ReportAttributeValue.Add(report);
                db.SaveChanges();
            }
            else
            {
                atValue.ReportAttributeValueName = attributeValue;
                db.SaveChanges();
            }
        }
        public ActionResult Edit(long? id,string message=null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportMasterPreview reportMaster = db.ReportMasterPreview.Find(id);
            ViewBag.ReportImages = db.ReportImages.Where(x => x.ReportID == id).ToList();
            ViewBag.ReportVideos = db.ReportVideos.Where(x => x.ReportID == id).ToList();
            ViewBag.Message = message;
            if (reportMaster == null)
            {
                return HttpNotFound();
            }
            loaddropdowns(reportMaster);
            var reportAttr = db.ReportAttribute.Where(x => x.VehicleCategoryID == reportMaster.VehicleCategoryID).ToList();
            reportAttr.ForEach(x => {
                x.ReportAttributeDefaultValue = GetAttributeValue(x.ReportAttributeID, id,x.ReportAttributeDataType);
            });
            ViewBag.ReportAttr = reportAttr;
            return View(reportMaster);
        }

        // POST: ReportMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReportID,ReportNumber,UserId,StartDate,FinishDate,Status,IsLocked,VehicleCategoryID,VehicleNumber,ChassisNumber,BookNumber,Comment,CompanyID,CompanyName,Longitude,Latitude,ClientLocation")] ReportMasterPreview reportMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit",new { id= reportMaster.ReportID });
            }
            loaddropdowns(reportMaster);

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
            var ReportObj = db.ReportMasterPreview.Where(x => x.ReportID == id).FirstOrDefault();
            ViewBag.VehicleCategory = "";
            if (ReportObj != null)
            {
                ReportObj.CompanyName = GetCompanyName(Convert.ToInt32(ReportObj.CompanyID));
                ViewBag.VehicleCategory = GetVehicleCategory(ReportObj.VehicleCategoryID);
                ViewBag.Username = GetUsername(ReportObj.UserId);
                ApplicationUser  Info = GetMyParentInfo(ReportObj.UserId);
                if (Info != null)
                {
                    ViewBag.UserFullNameRo = Info.FullName;
                    ViewBag.UserLocationRo = Info.MobileNumber;
                    ViewBag.UserAddRo = Info.Address;
                }
            }
            
                ViewBag.ReportObj = ReportObj;
            var Attibutes = db.ReportAttributeValue.Where(x => x.ReportID == id);
            if (Attibutes != null)
            {
                List<GenricTemp> gtlist = new List<GenricTemp>();
                foreach (ReportAttributeValue item in Attibutes)
                {

                    GenricTemp GenricTemp = new GenricTemp()
                    {
                        Name = GetReportAttributes((int)item.ReportAttributeID),
                        Value = item.ReportAttributeValueName,
                        Type = GetReportAttributesType((int)item.ReportAttributeID),
                    };
                    gtlist.Add(GenricTemp);

                }
                ViewBag.AttibutesVB = gtlist;
            }
            return View(reportMaster);
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
            fileResult.FileDownloadName = "ReportPdf" + ID + ".pdf";

            return fileResult;
        }
        public ActionResult DownloadImages(long? id)
        {
            using (ZipFile zip = new ZipFile())
            {
                var reportMaster = db.ReportImages.Where(x => x.ReportID == id).ToList();
                if (reportMaster != null)
                {
                    string dd = DateTime.Now.ToLongDateString();
                    foreach (var item in reportMaster)
                    {
                        if (item.ImagePathStemped.Length>7)
                        {
                            zip.AddFile(Server.MapPath(item.ImagePathStemped), "Images-"+dd+"-" + id);
                        }
                        else if(item.ImagePath.Length>7)
                        {
                            zip.AddFile(Server.MapPath(item.ImagePath), "Images-"+dd+"-" + id);
                        }
                      
                    }
                   // zip.AddFile(Server.MapPath("/Content/Img/onam.jpg"), "files");

                   // zip.AddFile(Server.MapPath("/Content/Img/onam_stam.jpg"), "files");
                }
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedImages-"+id+".zip");
                Response.ContentType = "application/zip";
                zip.Save(Response.OutputStream);
                Response.End();
                return View();
            }
        }
        public ActionResult DownloadVideos(long? id)
        {
            using (ZipFile zip = new ZipFile())
            {
                var reportMaster = db.ReportVideos.Where(x => x.ReportID == id).ToList();
                if (reportMaster != null)
                {
                    string dd = DateTime.Now.ToLongDateString();
                    foreach (var item in reportMaster)
                    {
                        if (item.VideoPath.Length > 7)
                        {
                            zip.AddFile(Server.MapPath(item.VideoPath), "Videos-" + dd + "-" + id);
                        }
                    }
                    // zip.AddFile(Server.MapPath("/Content/Img/onam.jpg"), "files");

                    // zip.AddFile(Server.MapPath("/Content/Img/onam_stam.jpg"), "files");
                }
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedVideos.zip");
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
            ReportMasterPreview reportMaster = db.ReportMasterPreview.Find(id);
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
            ReportMasterPreview reportMaster = db.ReportMasterPreview.Find(id);
            db.ReportMasterPreview.Remove(reportMaster);
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
    public class GenricTemp
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
