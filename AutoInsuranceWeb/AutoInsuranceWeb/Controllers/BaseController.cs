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
    public class BaseController : Controller
    {
        
        public static Int64 GetLong(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return 0;
                }
                return Convert.ToInt64(value);

            }
            catch
            {
                return 0;
            }
        }
       
        public static int GetInt(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return 0;
                }
                return Convert.ToInt32(value);

            }
            catch
            {
                return 0;
            }
        }
        public static decimal GetDecimal(string value)
        {
            try
            {
                if(string.IsNullOrEmpty(value))
                {
                    return 0;
                }
                return Convert.ToDecimal(value);

            }
            catch
            {
                return 0;
            }
        }
        public static string GetString(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return "-";
                }

                return value;
            }
            catch
            {
                return "-";
            }
        }
        public static string GetUsername(string userid)
        {
            var context = new ApplicationDbContext();
           // var role = (from r in context.Roles where r.Name.Contains("RO") select r).FirstOrDefault();
            var users = (from u in context.Users
                        where u.Id== userid
                         select u.UserName).FirstOrDefault();
            if (users != null)
            {
                return users;
            }else
            {
                return "";
            }
        }
        public static string GetFullname(string userid)
        {
            var context = new ApplicationDbContext();
            //var role = (from r in context.Roles where r.Name.Contains("RO") select r).FirstOrDefault();
            var users = (from u in context.Users
                         where u.Id == userid
                         select u.FullName).FirstOrDefault();
            if (users != null)
            {
                return users;
            }
            else
            {
                return "";
            }
        }
        public static List<string> GetMyChildUsers(string userid="")
        {
            var context = new ApplicationDbContext();
            List<string> usersids = (from u in context.Users
                                   where u.ParentUserid == userid
                                   select u.Id).ToList();
            return usersids;

        }

        public static ApplicationUser GetMyParentInfo(string userid = "")
        {
            var context = new ApplicationDbContext();
            string parentUserid = (from u in context.Users
                                     where u.Id == userid
                                     select u.ParentUserid).FirstOrDefault();

            ApplicationUser info  = (from u in context.Users
                                     where u.Id == parentUserid
                            select u).FirstOrDefault();
            return info;

        }
        public static string GetCompanyName(int id )
        {
            AutoInsuranceContext db = new AutoInsuranceContext();
            var company = db.CompanyMaster.Find(id);
            if(company!=null)
            {
                return company.CompanyName;
            }
            return "";

        }
        public static string GetCompanyName(string id)
        {
            AutoInsuranceContext db = new AutoInsuranceContext();
            int companyid = GetInt(id);
            var company = db.CompanyMaster.Find(companyid);
            if (company != null)
            {
                return company.CompanyName;
            }
            return "";

        }
        public static string GetVehicleCategory(int id)
        {
            AutoInsuranceContext db = new AutoInsuranceContext();
            var vehicleCategory = db.VehicleCategories.Find(id);
            if(vehicleCategory !=null)
            {
                return vehicleCategory.VehicleCategoryName;
            }
            return "";
        }

        public static string GetReportAttributes(int id)
        {
            AutoInsuranceContext db = new AutoInsuranceContext();
            var reportAttribute = db.ReportAttribute.Find(id);
            if (reportAttribute != null)
            {
                return reportAttribute.ReportAttributeName;
            }
            return "";
        }
        public static string GetReportAttributesType(int id)
        {
            AutoInsuranceContext db = new AutoInsuranceContext();
            var reportAttribute = db.ReportAttribute.Find(id);
            if (reportAttribute != null)
            {
                return reportAttribute.ReportAttributeType;
            }
            return "";
        }
    }
}