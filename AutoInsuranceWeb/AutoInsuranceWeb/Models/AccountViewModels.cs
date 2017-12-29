﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace AutoInsuranceWeb.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class UpdateViewModel
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        // public string Email { get; set; }

        public string Id { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        public string UserName { get; set; }

        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string MobileNumber { get; set; }
        public string ALTMobileNumber { get; set; }
        public string OtherMobileNumber { get; set; }
        // public string ProfileImage { get; set; }
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
    }

    public class RegisterViewModel
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
       // public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        public string UserName { get; set; }

        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string MobileNumber { get; set; }
        public string ALTMobileNumber { get; set; }
        public string OtherMobileNumber { get; set; }
       // public string ProfileImage { get; set; }
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
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}