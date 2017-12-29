using AutoInsuranceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AutoInsuranceWeb.Context
{
    public class AutoInsuranceContext:DbContext
    {
        public AutoInsuranceContext() : base("name=DefaultConnection") { }

        public DbSet<BookMasterViewModels> BookMasters { get; set; }
        public DbSet<VehicleCategories> VehicleCategories { get; set; }

        public DbSet<IssueBooksToRO> IssueBooksToRO { get; set; }
        public DbSet<IssueBooksToDSA> IssueBooksToDSA { get; set; }

        public DbSet<DeliveryStatus> DeliveryStatus { get; set; }

        public DbSet<ReportStatus> ReportStatus { get; set; }

        public DbSet<ReportMaster> ReportMaster { get; set; }

        public DbSet<CompanyMaster> CompanyMaster { get; set; }

        public DbSet<ReportVideos> ReportVideos { get; set; }

        public DbSet<ReportImages> ReportImages { get; set; }

        public DbSet<ImageTypes> ImageTypes { get; set; }

        public DbSet<UserLocationHistory> UserLocationHistory { get; set; }

        public DbSet<ReportMasterPreview> ReportMasterPreview { get; set; }

        public DbSet<ReportAttribute> ReportAttribute { get; set; }
        

        public DbSet<ReportAttributeValue> ReportAttributeValue { get; set; }

        public DbSet<CourierCompanyMaster> CourierCompanyMaster { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Precision.ConfigureModelBuilder(modelBuilder);
        }

    }
   
}