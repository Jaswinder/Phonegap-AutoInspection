using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AutoInsuranceWeb.Models
{
    public class BookMasterViewModels
    {
        [Key]
        public int BookMasterID { get; set; }
        public string BookTitle { get; set; }
        public long StartSNo { get; set; }
        [Required(ErrorMessage = "Number of books required. Starting from 1")]
        public long NumberOfBooks { get; set; }
        public long EndSno { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }

    }
    public class VehicleCategories
    {
       
        [Key]
        public int VehicleCategoryID { get; set; }
        public string VehicleCategoryName { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        
        public ICollection<IssueBooksToDSA> IssueBooksToDSAs { get; set; }
        public ICollection<ReportAttribute> ReportAttribute { get; set; }
    }
    public class DeliveryStatus
    {

        [Key]
        public int DeliveryStatusID { get; set; }
        public string DeliveryStatusName { get; set; }
        public string Status { get; set; }
    }
    public class ReportStatus
    {

        [Key]
        public int ReportStatusID { get; set; }
        public string ReportStatusName { get; set; }
        public string Status { get; set; }
    }
    public class CompanyMaster
    {
        [Key]
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRemarks { get; set; }
        public string Status { get; set; }
    }
    public class CourierCompanyMaster
    {
        [Key]
        public int CourierCompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRemarks { get; set; }
        public string ContactNumber { get; set; }
        public string Status { get; set; }
        public ICollection<IssueBooksToRO> IssueBooksToRO { get; set; }
        public ICollection<IssueBooksToDSA> IssueBooksToDSA { get; set; }
    }
    public class IssueBooksToRO
    {
        [Key]
        public int IssueBookID { get; set; }
        public string IssueTo { get; set; }
        public Nullable<DateTime> IssuedDate { get; set; }
        public string SendToAddress { get; set; }
        public string Status { get; set; }
        public string IssuedBy { get; set; }
        public long StartSNo { get; set; }
       
        public long NumberOfBooks { get; set; }
        public long EndSno { get; set; }
        public string Remarks { get; set; }
        public string Location { get; set; }
        public string IssuedFrom { get; set; }

        public int CourierCompanyID { get; set; }
        [Column("CourierCompanyID")]
        public virtual CourierCompanyMaster CourierCompanyMaster { get; set; }
        public string TrackingID { get; set; }

    }
    public class IssueBooksToDSA
    {
        [Key]
        public int IssueBookID { get; set; }
        public string IssueTo { get; set; }
        public Nullable<DateTime> IssuedDate { get; set; }
        public string SendToAddress { get; set; }
        public string Status { get; set; }
        public string IssuedBy { get; set; }
        public long StartSNo { get; set; }
        
        public long NumberOfBooks { get; set; }
        public long EndSno { get; set; }
        public string Remarks { get; set; }
        public string Location { get; set; }
        public string IssuedFrom { get; set; }

        public int VehicleCategoryID { get; set; }
        [Column("VehicleCategoryID")]
        public virtual VehicleCategories VehicleCategory { get; set; }

        public int CourierCompanyID { get; set; }
        [Column("CourierCompanyID")]
        public virtual CourierCompanyMaster CourierCompanyMaster { get; set; }
        public string TrackingID { get; set; }
    }
    public class ReportMaster
    {
        [Key]
        public long ReportID { get; set; }
        public long ReportNumber { get; set; }
        public string UserId { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> FinishDate { get; set; }
        public int Status { get; set; }
        public bool IsLocked { get; set; }
        public int VehicleCategoryID { get; set; }
        public string VehicleNumber { get; set; }
        public string ChassisNumber { get; set; }
        public long BookNumber { get; set; }
        public string Comment { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        [Precision(18, 9)]
        public decimal Longitude { get; set; }
        [Precision(18, 9)]
        public decimal Latitude { get; set; }
        public string ClientLocation { get; set; }
        public string ApproverID { get; set; }
    }
    public class ReportMasterPreview
    {
        [Key]
        public long ReportID { get; set; }
        public long ReportNumber { get; set; }
        public string UserId { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> FinishDate { get; set; }
        public int Status { get; set; }
        public bool IsLocked { get; set; }
        public int VehicleCategoryID { get; set; }
        public string VehicleNumber { get; set; }
        public string ChassisNumber { get; set; }
        public long BookNumber { get; set; }
        public string Comment { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        [Precision(18, 9)]
        public decimal Longitude { get; set; }
        [Precision(18, 9)]
        public decimal Latitude { get; set; }
        public string ClientLocation { get; set; }
        public string MobileNumber { get; set; }
    }

    public class ReportAttribute
    {
        [Key]
        public long ReportAttributeID { get; set; }
        public string ReportAttributeName { get; set; }

        public string ReportAttributeType { get; set; }
        public long ReportID { get; set; }

        public int VehicleCategoryID { get; set; }
        [Column("VehicleCategoryID")]
        public virtual VehicleCategories VehicleCategory { get; set; }

        public ICollection<ReportAttributeValue> ReportAttributeValue { get; set; }

        public string ReportAttributeDataType { get; set; }
        public string ReportAttributeDefaultValue { get; set; }

    }

    public class ReportAttributeValue
    {
        [Key]
        public long ReportAttributeValueID { get; set; }
        public string ReportAttributeValueName { get; set; }
  
        public long ReportAttributeID { get; set; }
        [Column("ReportAttributeID")]
        public virtual ReportAttribute ReportAttribute { get; set; }

        public long ReportID { get; set; }


    }


    public class ReportImages
    {
        [Key]
        public long ReportImageID { get; set; }
        public long ReportNumber { get; set; }
        public long ReportID { get; set; }
        public string UserId { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        
        public string Imagename { get; set; }
        public string ImageType { get; set; }
        public string ImagePath { get; set; }
        public string ImagePathStemped { get; set; }
        public string ImagePathThumb { get; set; }
        public string ImagePhonePath { get; set; }
        [Precision(18, 9)]
        public decimal Longitude { get; set; }
        [Precision(18, 9)]
        public decimal Latitude { get; set; }
        public string ClientLocation { get; set; }

    }
    public class ReportVideos
    {
        [Key]
        public long ReportVideosID { get; set; }
        public long ReportNumber { get; set; }
        public long ReportID { get; set; }
        public string UserId { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }

        public string Videoname { get; set; }
        public string VideoType { get; set; }
        public string VideoPath { get; set; }
        public string VideoPhonePath { get; set; }
        [Precision(18, 9)]
        public decimal Longitude { get; set; }
        [Precision(18, 9)]
        public decimal Latitude { get; set; }
        public string ClientLocation { get; set; }
    }
    public class ImageTypes
    {

        [Key]
        public int ImageTypeID { get; set; }
        public string ImageTypeName { get; set; }
        public string ImageTypeRemarks { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
    }
    public class UserLocationHistory
    {

        [Key]
        public int UserLocationID { get; set; }
        [Precision(18, 9)]
        public decimal Longitude { get; set; }
        [Precision(18, 9)]
        public decimal Latitude { get; set; }
        public string UserId { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
    }
}