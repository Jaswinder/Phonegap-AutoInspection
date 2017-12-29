using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AutoInsuranceWeb.Models;
using System.Collections.Generic;

using AutoInsuranceWeb.Context;
using System.IO;

using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace AutoInsuranceWeb.Controllers
{
    public class APPServicesController : BaseController
    {
        private AutoInsuranceContext db = new AutoInsuranceContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public string AddressByLongLat;
        public APPServicesController()
        {
        }

        public APPServicesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: APPServices
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public JsonResult Login(string username, string password)
        {

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            ApplicationUser user = new ApplicationUser();
            user = UserManager.Find(username, password);
            
            user.PasswordHash = "";
            user.SecurityStamp = "";
            return Json(user, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllMyReports(string userid)
        {
            List<ReportMaster> reportMasterList = new List<ReportMaster>();
            var reportMaster = db.ReportMaster.Where(x=>x.UserId==userid).ToList();
            foreach (var item in reportMaster)
            {
                reportMasterList.Add(item);
            }
            return Json(reportMasterList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StartNewReport(string reportnumber, string userid)
        {
            ReportMaster RMaster = new ReportMaster();
            long Rnumber = Convert.ToInt64(reportnumber);
            var report = db.ReportMaster.Where(x => x.ReportNumber == Rnumber).FirstOrDefault();
            if (report != null)
            {
                if(report.IsLocked==true)
                {
                    RMaster.ReportID = 0;
                    RMaster.Comment = "Report is locked.";
                    return Json(RMaster, JsonRequestBehavior.AllowGet);

                }
                RMaster.ReportID = report.ReportID;
                RMaster.ReportNumber = report.ReportNumber;
                RMaster.UserId = report.UserId;
                RMaster.StartDate = report.StartDate;
                RMaster.FinishDate = report.FinishDate;
                RMaster.Status = report.Status;
                RMaster.IsLocked = report.IsLocked;
                RMaster.VehicleCategoryID = report.VehicleCategoryID;
                RMaster.VehicleNumber = report.VehicleNumber;
                RMaster.ChassisNumber = report.ChassisNumber;
                RMaster.BookNumber = report.BookNumber;
                RMaster.Comment = report.Comment;
                RMaster.CompanyID = report.CompanyID;
                RMaster.CompanyName = report.CompanyName;
            }
            else
            {
                var bookfromdsa = db.IssueBooksToDSA.Where(x => x.StartSNo <= Rnumber && Rnumber <= x.EndSno).FirstOrDefault();
                if (bookfromdsa != null)
                {

                    RMaster.ReportNumber = Rnumber;
                    RMaster.StartDate = DateTime.Now;
                    RMaster.UserId = userid;
                    RMaster.Status = 1;
                    RMaster.BookNumber = bookfromdsa.StartSNo;
                    RMaster.VehicleCategoryID = bookfromdsa.VehicleCategoryID;

                    db.ReportMaster.Add(RMaster);
                    db.SaveChanges();
                }
                else
                {
                    RMaster.ReportID = 0;
                    RMaster.Comment = "Report number does not exist";
                }
            }
            return Json(RMaster, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDriverLoaction(string userid,string latitude,string longitude)
        {
            decimal vlongitude = Convert.ToDecimal(longitude);
            decimal vlatitude = Convert.ToDecimal(latitude);
            UserLocationHistory loc = db.UserLocationHistory.Where(x => x.UserId == userid).FirstOrDefault();
            if(loc==null)
            {
                UserLocationHistory newloc = new UserLocationHistory();
                newloc.Latitude = vlatitude;
                newloc.Longitude = vlongitude;
                newloc.UserId = userid;
                newloc.UpdatedDate = DateTime.UtcNow;
                db.UserLocationHistory.Add(newloc);
                db.SaveChanges();
                return Json(newloc, JsonRequestBehavior.AllowGet);
            }
            else
            {
                loc.Longitude = vlongitude;
                loc.Latitude = vlatitude;
                loc.UpdatedDate = DateTime.UtcNow;
                db.SaveChanges();
                return Json(loc, JsonRequestBehavior.AllowGet);
            }
            
        }
        public JsonResult SaveStepOne(string reportnumber, int vehicletypeid, int booknumber, string companyid, string vehiclenumber, string chassinumber)
        {
            long Rnumber = Convert.ToInt64(reportnumber);
            ReportMaster report = db.ReportMaster.Where(x => x.ReportNumber == Rnumber).FirstOrDefault();
            if (report != null)
            {
                report.VehicleCategoryID = vehicletypeid;
                report.VehicleNumber = vehiclenumber;
                report.ChassisNumber = chassinumber;
                report.BookNumber = booknumber;
                report.CompanyID = companyid;
                report.Status = 1;
                db.SaveChanges();
            }
            else
            {

                report.ReportID = 0;
                report.Comment = "Report number does not exist";

            }
            return Json(report, JsonRequestBehavior.AllowGet);
        }
        private int SaveByteArrayAsImage(string fullOutputPath, string base64String)
        {
            byte[] data = Convert.FromBase64String(base64String);
            using (var stream = new MemoryStream(data, 0, data.Length))
            {
                Image image = Image.FromStream(stream);
                image.Save(fullOutputPath, System.Drawing.Imaging.ImageFormat.Png);
                //TODO: do something with image
            }
            //byte[] bytes = Convert.FromBase64String(base64String);

            //Image image;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            //    image = Image.FromStream(ms);
            //}

            
            return 1;
        }
        private int Base64ToImage(string SignaturePath, string base64String)
        {
            try
            {
                string convert = base64String.Replace("data:image/png;base64,", String.Empty);
                byte[] imageBytes = Convert.FromBase64String(convert);
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    //MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    image.Save(SignaturePath, System.Drawing.Imaging.ImageFormat.Png);
                }
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            return 1;
        }
        public bool SaveImage(string imageString, string location)
        {
            try
            {
               // imageString = imageString.Replace("data:image/png;base64,", String.Empty);
                imageString = imageString.Substring(imageString.IndexOf(',') + 1);
                byte[] bytes = Convert.FromBase64String(imageString);

                using (FileStream fs = new FileStream(location, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            return true;
        }
        [HttpPost]
        public JsonResult SaveSignature(string ReportNumber,string ReportId, string UserId, string UpdatedDate, string Imagename, string ImageType, string ImagePath, string ImagePhonePath, string Longitude, string Latitude, string ClientLocation)
        {
            long vReportId = GetLong(ReportId);
            long vreportnumber = GetLong(ReportNumber);
            ReportImages Imges = db.ReportImages.Where(x => x.UserId == UserId && x.ReportNumber == vreportnumber && x.ImageType == ImageType).FirstOrDefault();
            string _FileName = "SIGN_"+ReportNumber + "-" + ReportId + "-" + UserId + ".png";
            string SignaturePath = Path.Combine(Server.MapPath("~/ReportImages/"), _FileName);
            string _sigPath = Path.Combine("/ReportImages/", _FileName);
            //Path.Combine(Server.MapPath("~" + _Dir), _FileName);
            bool wait = SaveImage( ImagePhonePath, SignaturePath);
            if (Imges != null)
            {

                Imges.ImageType = GetString(ImageType);
                Imges.ReportID = vReportId;
                Imges.ReportNumber = vreportnumber;
                Imges.ImagePath = GetString(_sigPath);
                Imges.ImagePathStemped = GetString(_sigPath);
                Imges.ImagePathThumb = GetString(_sigPath);
                // Imges.UserId = userid;
                // Imges.Imagename = _FileName;
                //  Imges.UpdatedDate = DateTime.Now;
                // Imges.ImagePhonePath = ImagePathPhone;
                //Imges.Latitude = longitude;
                //Imges.Longitude = latitude;
                // Imges.ClientLocation = ClientLocation;
                db.SaveChanges();
                return Json(Imges, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ReportImages ImgesTemp = new ReportImages();

                ImgesTemp.ImageType = GetString(ImageType);
                ImgesTemp.ReportID = vReportId;
                ImgesTemp.ReportNumber = vreportnumber;
                ImgesTemp.ImagePath = GetString(_sigPath);
                ImgesTemp.UserId = GetString(UserId);
                ImgesTemp.Imagename = GetString(_FileName);
                ImgesTemp.UpdatedDate = DateTime.Parse(UpdatedDate);
                ImgesTemp.ImagePhonePath = GetString(_sigPath);
                ImgesTemp.Latitude = GetDecimal(Longitude);
                ImgesTemp.Longitude = GetDecimal(Latitude);
                ImgesTemp.ClientLocation = "";// ReverseGeocode(GetDecimal(Latitude), GetDecimal(Longitude));
                ImgesTemp.ImagePathStemped = GetString(_sigPath).Replace("\\", "/");
                ImgesTemp.ImagePathThumb = GetString(_sigPath).Replace("\\", "/");
                db.ReportImages.Add(ImgesTemp);
                db.SaveChanges();
                return Json(ImgesTemp, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveLocalReport(string reportnumber, int vehicletypeid, int booknumber, string companyid, string vehiclenumber, string chassinumber,string UserId,string datetime)
        {
            long Rnumber = Convert.ToInt64(reportnumber);
            ReportMasterPreview report = db.ReportMasterPreview.Where(x => x.ReportNumber == Rnumber && x.UserId==UserId).FirstOrDefault();
            if (report == null)
            {
                ReportMasterPreview vreport = new ReportMasterPreview();
                vreport.ReportNumber = Rnumber;
                vreport.VehicleCategoryID = vehicletypeid;
                vreport.VehicleNumber = vehiclenumber;
                vreport.ChassisNumber = chassinumber;
                vreport.BookNumber = booknumber;
                vreport.CompanyID = companyid;
                vreport.Status = 1;
                vreport.UserId = UserId;
                DateTime utcdate = DateTime.UtcNow;
                var istdate = DateTime.Parse(datetime);
                vreport.StartDate = istdate;
                db.ReportMasterPreview.Add(vreport);
                db.SaveChanges();
            }
            else
            {

                report.ReportID = report.ReportID;
                report.Comment = "Report number does not exist";

            }
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportByNumber(string reportnumber)
        {
            ReportMaster RMaster = new ReportMaster();
            long Rnumber = Convert.ToInt64(reportnumber);
            var report = db.ReportMaster.Where(x => x.ReportNumber == Rnumber).FirstOrDefault();
            if (report != null)
            {
                RMaster.ReportID = report.ReportID;
                RMaster.ReportNumber = report.ReportNumber;
                RMaster.UserId = report.UserId;
                RMaster.StartDate = report.StartDate;
                RMaster.FinishDate = report.FinishDate;
                RMaster.Status = report.Status;
                RMaster.IsLocked = report.IsLocked;
                RMaster.VehicleCategoryID = report.VehicleCategoryID;
                RMaster.VehicleNumber = report.VehicleNumber;
                RMaster.ChassisNumber = report.ChassisNumber;
                RMaster.BookNumber = report.BookNumber;
                RMaster.Comment = report.Comment;
                RMaster.CompanyID = report.CompanyID;
                RMaster.CompanyName = report.CompanyName;
            }
            else
            {

                RMaster.ReportID = 0;
                RMaster.Comment = "Report number does not exist";

            }
            return Json(RMaster, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCompanies()
        {
            List<CompanyMaster> CompanyList = new List<CompanyMaster>();
            var companies = db.CompanyMaster.ToList();
            foreach (var item in companies)
            {
                CompanyList.Add(item);
            }
            return Json(CompanyList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleCategories()
        {
            List<VehicleCategories> vehicleCategorieList = new List<VehicleCategories>();
            var vehicleCategorie = db.VehicleCategories.ToList();
            foreach (var item in vehicleCategorie)
            {
                vehicleCategorieList.Add(item);
            }
            return Json(vehicleCategorieList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReportImageTypes(string reportnumber)
        {
            List<ImageTypes> ImageTypesList = new List<ImageTypes>();
            var ImageTypes = db.ImageTypes.ToList();
            long vreportnumber = GetLong(reportnumber);
            var reportimages = db.ReportImages.Where(x => x.ReportNumber == vreportnumber).ToList();
            foreach (var item in ImageTypes)
            {
                var Image = reportimages.Where(x => x.ImageType == item.Code).FirstOrDefault();
                if(Image!=null)
                {
                    item.Status=Image.ImagePathThumb;
                }
                
                ImageTypesList.Add(item);
            }
            return Json(ImageTypesList, JsonRequestBehavior.AllowGet);
        }

        public void ImageStamp(string imgPath,string _stempedFile,string _thumbfile, string stemp)
        {
            try
            {
                //creating a image object
                System.Drawing.Image bitmap = (System.Drawing.Image)Bitmap.FromFile(Server.MapPath(imgPath)); // set image 
                                                                                                              //draw the image object using a Graphics object
                Graphics graphicsImage = Graphics.FromImage(bitmap);

                //Set the alignment based on the coordinates   
                StringFormat stringformat = new StringFormat();
                stringformat.Alignment = StringAlignment.Near;
                stringformat.LineAlignment = StringAlignment.Near;

                //Set the font color/format/size etc..  
                Color StringColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");

                string Str_TextOnImage = stemp;// "Happy Malik";//Your Text On Image


                graphicsImage.DrawString(Str_TextOnImage, new Font("arial", 18,
                FontStyle.Regular), new SolidBrush(StringColor), new System.Drawing.Point(0, 0),
                stringformat);

                bitmap.Save(Server.MapPath(_stempedFile), ImageFormat.Jpeg);
                bitmap.Dispose();
            }
            catch(Exception ex) { }
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(imgPath));
                System.Drawing.Image thumb = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
                thumb.Save(Path.ChangeExtension(Server.MapPath(_thumbfile), "jpg"));
            }
            catch (Exception ex) { }
        }
      
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SaveReportImage(string reportnumber, string userid, string ReportId, string ImageType, string ImagePathPhone, HttpPostedFileBase file,string longitude,string latitude,string ClientLocation,string mobile="",string datetime = "")
        {
            ReportImages RptImagesExists = db.ReportImages.Where(x => x.ImagePhonePath == ImagePathPhone).FirstOrDefault();
            if(RptImagesExists!=null)
            {
                return Json("Image already exists.", JsonRequestBehavior.AllowGet);
            }
           
            try
            {
                string _path = "";
                string _FileName = "";
                string _Stemppath = "";
                string _thumbpath = "";
                try
                {
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            _FileName = reportnumber + "-"+ userid +"-" + ImageType + ".jpg";
                            string _Dir = "/ReportImages/";
                  
                            _path = Path.Combine(Server.MapPath("~" + _Dir), _FileName);
                            
                            file.SaveAs(_path);
                            _path = Path.Combine(_Dir, _FileName);
                            _Stemppath = Path.Combine("/ReportImages/stemped/",_FileName);
                            _thumbpath = Path.Combine("/ReportImages/thumb/", _FileName);
                            string clientLocationBing = "";
                            //if (clientLocationBing != "empty")
                            //{
                                clientLocationBing =  ReverseGeocode(GetDecimal(longitude), GetDecimal(latitude));
                            //}
                            //if(clientLocationBing == "empty")
                            //{
                            //    clientLocationBing = AddressByLongLat;
                            //}
                            //else
                            //{
                            //    AddressByLongLat = clientLocationBing;
                            //    clientLocationBing = AddressByLongLat;
                            //}


                            string _Text = reportnumber + "-"+ clientLocationBing + "-:"+ mobile+"-Date:"+ datetime;
                            ImageStamp(_path, _Stemppath, _thumbpath, _Text);
                            _path = _path.Replace("\\", "/");


                        }
                    }
                }
                catch { }
                    long vReportId = GetLong(ReportId);
                    long vreportnumber = GetLong(reportnumber);
                    ReportImages Imges = db.ReportImages.Where(x => x.UserId == userid && x.ReportNumber == vreportnumber && x.ImageType == ImageType).FirstOrDefault();



                    if (Imges != null)
                    {
                        Imges.ImageType = GetString(ImageType);
                        Imges.ReportID = vReportId;
                        Imges.ReportNumber = vreportnumber;
                        Imges.ImagePath = GetString(_path);
                        Imges.ImagePathStemped = GetString(_Stemppath);
                        Imges.ImagePathThumb = GetString(_thumbpath);
                        // Imges.UserId = userid;
                        // Imges.Imagename = _FileName;
                        // Imges.UpdatedDate = DateTime.Now;
                        // Imges.ImagePhonePath = ImagePathPhone;
                        // Imges.Latitude = longitude;
                        // Imges.Longitude = latitude;
                        // Imges.ClientLocation = ClientLocation;
                        db.SaveChanges();
                            return Json(Imges, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ReportImages ImgesTemp = new ReportImages();
                        ImgesTemp.ImageType = GetString(ImageType);
                        ImgesTemp.ReportID = vReportId;
                        ImgesTemp.ReportNumber = vreportnumber;
                        ImgesTemp.ImagePath = GetString(_path);
                        ImgesTemp.UserId = GetString(userid);
                        ImgesTemp.Imagename = GetString(_FileName);
                        ImgesTemp.UpdatedDate = DateTime.Parse(datetime);
                        ImgesTemp.ImagePhonePath = GetString(ImagePathPhone);
                        ImgesTemp.Latitude = GetDecimal(longitude);
                        ImgesTemp.Longitude = GetDecimal(latitude);
                        ImgesTemp.ClientLocation = GetString(ClientLocation);
                        ImgesTemp.ImagePathStemped = GetString(_Stemppath).Replace("\\", "/");
                        ImgesTemp.ImagePathThumb = GetString(_thumbpath).Replace("\\", "/");
                        db.ReportImages.Add(ImgesTemp);
                        db.SaveChanges();
                       return Json(ImgesTemp, JsonRequestBehavior.AllowGet);
                }
               // }
            }
            catch(Exception ex)
            {
                 string ImgesClientLocation = ex.Message.ToString();
                return Json(ImgesClientLocation, JsonRequestBehavior.AllowGet);
            }
            return Json("DONE", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult SaveReportVideo(string reportnumber, string userid, string ReportId, string VideoType, string VideoPathPhone, HttpPostedFileBase file, string longitude, string latitude, string ClientLocation, string mobile = "", string datetime = "")
        {
            ReportVideos RptVideosExists = db.ReportVideos.Where(x => x.VideoPhonePath == VideoPathPhone).FirstOrDefault();
            if (RptVideosExists != null)
            {
                return Json("Video already exists.", JsonRequestBehavior.AllowGet);
            }
            try
            {
                string _path = "";
                string _FileName = "";
                string _Stemppath = "";
                string _thumbpath = "";
                try
                {
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            _FileName = reportnumber + "-"+userid+"-" + VideoType + file.FileName;
                            string _Dir = "/ReportVideos/";
                            //if (!Directory.Exists("~"+ _Dir))
                            //{
                            //    Directory.CreateDirectory("~"+_Dir);
                            //}
                            _path = Path.Combine(Server.MapPath("~" + _Dir), _FileName);

                            file.SaveAs(_path);
                            _path = Path.Combine(_Dir, _FileName);
                          //  _Stemppath = Path.Combine("/ReportImages/stemped/", _FileName);
                           // _thumbpath = Path.Combine("/ReportImages/thumb/", _FileName);
                          //  string _Text = reportnumber + "-Longitude:" + GetDecimal(longitude) + "-Latitude:" + GetDecimal(longitude) + "-Mobile:" + mobile + "-Date:" + datetime;
                          //  ImageStamp(_path, _Stemppath, _thumbpath, _Text);
                            _path = _path.Replace("\\", "/");


                        }
                    }
                }
                catch { }
                long vReportId = GetLong(ReportId);
                long vreportnumber = GetLong(reportnumber);
                ReportVideos video = db.ReportVideos.Where(x => x.UserId == userid && x.ReportNumber == vreportnumber && x.VideoType == VideoType).FirstOrDefault();



                if (video != null)
                {
                   video.VideoType = GetString(VideoType);
                   video.ReportID = vReportId;
                   video.ReportNumber = vreportnumber;
                   video.VideoPath = GetString(_path);
                    // Imges.UserId = userid;
                    // Imges.Imagename = _FileName;
                    //  Imges.UpdatedDate = DateTime.Now;
                    video.VideoPhonePath = VideoPathPhone;
                    //Imges.Latitude = longitude;
                    //Imges.Longitude = latitude;
                    // Imges.ClientLocation = ClientLocation;
                    db.SaveChanges();
                    return Json(video, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ReportVideos VideoTemp = new ReportVideos();
                    VideoTemp.VideoType = GetString(VideoType);
                    VideoTemp.ReportID = vReportId;
                    VideoTemp.ReportNumber = vreportnumber;
                    VideoTemp.VideoPath = GetString(_path);
                    VideoTemp.UserId = GetString(userid);
                    VideoTemp.Videoname = GetString(_FileName);
                    VideoTemp.UpdatedDate = DateTime.Parse(datetime);
                    VideoTemp.VideoPhonePath = GetString(VideoPathPhone);
                    VideoTemp.Latitude = GetDecimal(longitude);
                    VideoTemp.Longitude = GetDecimal(latitude);
                    VideoTemp.ClientLocation = GetString(ClientLocation);
                   
                    db.ReportVideos.Add(VideoTemp);
                    db.SaveChanges();
                    return Json(VideoTemp, JsonRequestBehavior.AllowGet);
                }
                // }
            }
            catch (Exception ex)
            {
                string ImgesClientLocation = ex.Message.ToString()
                  //+"reportnumber : " + reportnumber
                  //+ "userid : " + userid
                  //+ "ReportId : " + ReportId
                  //+ "ImageType : " + ImageType
                  //+ "ImagePathPhone : " + ImagePathPhone
                  //+ "longitude : " + longitude
                  //+ "latitude : " + latitude
                  //+ "ClientLocation : " + ClientLocation
                  //     + "Filename : " + file.FileName
                  ;
                return Json(ImgesClientLocation, JsonRequestBehavior.AllowGet);
            }
            return Json("DONE", JsonRequestBehavior.AllowGet);
        }


        //Key: AurgZmdKP-2VchN7mwKaP7Rc5gZlQzOJp3aTcZH3KtzGpokGwjs_YcG1y8bX0xcz
        //Application Url:
        //Key type: Basic / Education
        //Created date: 10/03/2017
        //Expiration date: None
        //Key Status: Enabled
        //31.026208, 75.78780
        //http://dev.virtualearth.net/REST/v1/Locations/31.026208,75.78780?key=AurgZmdKP-2VchN7mwKaP7Rc5gZlQzOJp3aTcZH3KtzGpokGwjs_YcG1y8bX0xcz
        public string ReverseGeocode( decimal longitude, decimal latitude)
        {
            try
            {

            

                using (var client = new WebClient())
                {
                
                var queryString = "http://dev.virtualearth.net/REST/v1/Locations/" + latitude + "," + longitude + "?key= AurgZmdKP-2VchN7mwKaP7Rc5gZlQzOJp3aTcZH3KtzGpokGwjs_YcG1y8bX0xcz";

                dynamic response = client.DownloadString(queryString);

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
                using (var es = new MemoryStream(Encoding.Unicode.GetBytes(response)))
                {
                    var mapResponse = (ser.ReadObject(es) as Response);
                    Location location = (Location)mapResponse.ResourceSets.First().Resources.First();
                    AddressResult addObj = new AddressResult()
                    {
                        StreetAddress = location.Address.AddressLine,
                        Locality = location.Address.Locality,
                        AdminDistrict = location.Address.Locality,
                        PostalCode = location.Address.PostalCode
                    };
                    string address = addObj.StreetAddress + "-" + addObj.Locality + "-" + addObj.PostalCode  ;
                    return address;
                }

            }
            }
            catch (Exception)
            {

                return "empty";
            }
        }
        
    }
    public class AddressResult
    {
        public string StreetAddress { get; set; }
        public string AddressLine { get; set; }
        public string Locality { get; set; } //Roughly a city or town
        public string AdminDistrict { get; set; } //Roughly a state, province, or other similar area
        public string PostalCode { get; set; }
    }
}