using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutoInsuranceWeb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string MobileNumber { get; set; }
        public string ALTMobileNumber { get; set; }
        public string OtherMobileNumber { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> LastModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string UserCode { get; set; }
        public string ParentUsername { get; set; }
        public string ParentUserid { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        
    }
}