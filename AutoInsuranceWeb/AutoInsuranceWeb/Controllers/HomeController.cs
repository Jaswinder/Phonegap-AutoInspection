using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoInsuranceWeb.Context;
using AutoInsuranceWeb.Models;

namespace AutoInsuranceWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AutoInsuranceContext db = new AutoInsuranceContext();
            ViewBag.BookMastersCountViewBag = db.BookMasters.Count();
            ViewBag.ReportMasterCountViewBag = db.ReportMaster.Count();
            ViewBag.ReportCountViewBag = db.ReportMasterPreview.Count();

            var context = new ApplicationDbContext();

            var roleRo = (from r in context.Roles where r.Name.Contains("RO") select r).FirstOrDefault();
            var usersRo = from u in context.Users
                        where u.Roles.Any(r => r.RoleId == roleRo.Id) && u.ParentUserid !=null
                        select u;
            var roCount  =  usersRo.Count();

            ViewBag.roCountViewBag = roCount;//

            var roleDSA = (from r in context.Roles where r.Name.Contains("DSA") select r).FirstOrDefault();
            var usersDSA = from u in context.Users
                          where u.Roles.Any(r => r.RoleId == roleDSA.Id) && u.ParentUserid != null
                           select u;
            var DSACount = usersDSA.Count();
            ViewBag.DSACountViewBag = DSACount;//

            var roleSURVEYOR = (from r in context.Roles where r.Name.Contains("SURVEYOR") select r).FirstOrDefault();
            var usersSURVEYOR = from u in context.Users
                           where u.Roles.Any(r => r.RoleId == roleSURVEYOR.Id) && u.ParentUserid != null
                                select u;
            var SURVEYORCount = usersSURVEYOR.Count();
            ViewBag.SURVEYORCountViewBag = SURVEYORCount;//


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}