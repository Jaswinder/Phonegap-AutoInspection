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
using Microsoft.Owin.Security;

namespace AutoInsuranceWeb.Controllers
{
    public class UserLocationsController : BaseController
    {
        // GET: UserLocations
        private AutoInsuranceContext db = new AutoInsuranceContext();

        public ActionResult Index()
        {
            return View(db.UserLocationHistory.ToList());
        }


        [HttpPost]
        public JsonResult GetMap()
        {
            var data1 = Map();
            return Json(data1, JsonRequestBehavior.AllowGet);
        }
        public List<userlocationdata> Map()
        {
            string userid = "";
            if (Request.IsAuthenticated)
            {
                userid = User.Identity.GetUserId();
            }
            var context = new ApplicationDbContext();
            List<string> usersSURVEYORids = new List<string>();
            if (User.IsInRole("MIS")) {
                var role = (from r in context.Roles where r.Name.Contains("SURVEYOR") select r).FirstOrDefault();
                usersSURVEYORids = (from u in context.Users
                            where u.Roles.Any(r => r.RoleId == role.Id) 
                            select u.Id).ToList();
         
            }
            else
            {
                usersSURVEYORids = (from u in context.Users
                                                 where u.ParentUserid == userid
                                                 select u.Id).ToList();
            }
            List<UserLocationHistory> locations =  db.UserLocationHistory.Where(x=>usersSURVEYORids.Contains(x.UserId)).ToList();

            locations.ForEach(x => {
                x.UserId = GetFullname(x.UserId);
                x.Latitude = x.Latitude;
                x.Longitude = x.Longitude;
                x.UserLocationID = x.UserLocationID;
            });
          List<userlocationdata>  dataobj = locations.Select(p => new userlocationdata() {
                Name = p.UserId,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Location = "",
                Description = p.UserId,
                Id = p.UserLocationID
            }).ToList();

            return dataobj;
        }
    }
    public class userlocationdata
    {
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
    }
}