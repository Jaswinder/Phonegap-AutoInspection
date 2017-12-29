(function (window) {
    var TotalReports = 0;
    var UploadedReports = 0;
    var totalImages = 0;
    var uploadedImages = 0;
    function StartUploading()
    {
       // alert("Starting");
        $("#success").html("Uploading Started");
        databaseHandler.createDatabase();
        databaseHandler.db.readTransaction(
               function (tx) {
                   tx.executeSql(
                     "select * from ReportMasters",
                     [],
                     function (tx, results) {
                         // alert(results.rows.length + results(i).CompanyID);
                         TotalReports = results.rows.length;
                         for (var i = 0; i < results.rows.length; i++) {
                             $("#success").html("Uploaded(" + i + "/" + results.rows.length+")");
                             var obj = results.rows.item(i);
                             //$("#listtable").append('<tr><td>' + obj.ReportNumber + '</td><td>' + obj.BookNumber + '</td><td><a href="#" onclick="deleterecord(' + obj.ReportID + "," + obj.ReportNumber + ')" class="btn"><i class="fa fa-trash" style="color:red" aria-hidden="true"></i> </a><a class="btn" href="stepone.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-play" ></i> </a><a class="btn" href="viewvideo.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-video-camera" ></i> </a><a class="btn" href="photos.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-camera" ></i></a></td></tr>');
                             uploadReportMaster(obj.ReportNumber, obj.UserId, obj.VehicleCategoryID, obj.VehicleNumber, obj.ChassisNumber, obj.BookNumber, obj.CompanyID, obj.StartDate);
                         }
                     },
                     function (tx, error) {
                         // alert("add Categories error");
                     }
                     );
               }
               );
    }
    function uploadReportMaster(ReportNumber,UserId, VehicleCategoryID,VehicleNumber,ChassisNumber,BookNumber,CompanyID,datetime)
    {
        //$("#success").html("Uploading Report");
        var url = APIBaseUrl + "APPServices/SaveLocalReport?reportnumber=" + ReportNumber + "&vehicletypeid=" + VehicleCategoryID
        + "&booknumber=" + BookNumber +
        "&companyid=" + CompanyID +
        "&vehiclenumber=" + VehicleNumber +
        "&chassinumber=" + ChassisNumber +
            "&UserId=" + UserId +
            "&datetime=" + datetime
            ;
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            success: function (result) {
                var reportid = result.ReportID;
                //$("#success").html("Uploading Report" + reportid);
                UploadedReports++;
                if (result.Status != 6) {
                    StartUploadImages(ReportNumber, reportid);
                    StartUploadVideos(ReportNumber, reportid);
                }
                else
                {

                }
               // window.location.href = "stepdocument.html?ReportId=" + $("#txtReportNumber").val();
            },
            error: function (err) {
                $("#success").html("Please Try Again...");
                //alert(err);
            }
        });
    }
    function StartUploadImages(reportNumber,reportID)
    {
        databaseHandler.createDatabase();
        databaseHandler.db.readTransaction(
               function (tx) {
                   tx.executeSql(
                     "select * from ReportImages where ReportNumber=?",
                     [reportNumber],
                     function (tx, results) {
                         // alert(results.rows.length + results(i).CompanyID);
                         totalImages = totalImages+ results.rows.length;
                         for (var i = 0; i < results.rows.length; i++) {
                             var obj = results.rows.item(i);
                             //$("#listtable").append('<tr><td>' + obj.ReportNumber + '</td><td>' + obj.BookNumber + '</td><td><a href="#" onclick="deleterecord(' + obj.ReportID + "," + obj.ReportNumber + ')" class="btn"><i class="fa fa-trash" style="color:red" aria-hidden="true"></i> </a><a class="btn" href="stepone.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-play" ></i> </a><a class="btn" href="viewvideo.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-video-camera" ></i> </a><a class="btn" href="photos.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-camera" ></i></a></td></tr>');
                            // alert(obj.ImageType);
                             if (obj.ImageType == "SIGPHOTO") {
                                 UploadSignature(obj.ReportNumber, reportID, obj.UserId, obj.UpdatedDate, obj.Imagename, obj.ImageType, obj.ImagePath, obj.ImagePhonePath, obj.Longitude, obj.Latitude, obj.ClientLocation);
                             } else {
                                 UploadReportImages(obj.ReportNumber, reportID, obj.UserId, obj.UpdatedDate, obj.Imagename, obj.ImageType, obj.ImagePath, obj.ImagePhonePath, obj.Longitude, obj.Latitude, obj.ClientLocation);
                             }
                         }
                     },
                     function (tx, error) {
                         $("#success").html("Report not submitted yet.");
                     }
                     );
               }
               );
    }
    function UploadSignature(ReportNumber, reportID, UserId, UpdatedDate, Imagename, ImageType, ImagePath, ImagePhonePath, Longitude, Latitude, ClientLocation)
    {
        
       // $("#success").html("Uploading Signature");
        var myKeyVals = {
            ReportNumber: ReportNumber, ReportId: reportID, UserId: UserId, UpdatedDate: UpdatedDate,
            Imagename: Imagename, ImageType: ImageType,
            ImagePath: ImagePath,
            ImagePhonePath: ImagePhonePath,
            Longitude: Longitude,
            Latitude: Latitude,
            ClientLocation: ClientLocation
        }

        var url = APIBaseUrl + "APPServices/SaveSignature";
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data: myKeyVals,
            success: function (result) {
                //var reportid = result.ReportID;
               // $("#success").html("Uploaded Signature");
                uploadedImages++;
                if (totalImages <= uploadedImages) {
                    $("#success").html("Uploaded Complete.");
                }
            },
            error: function (err) {
                $("#success").html("Signature not uploaded yet.");
                //alert(err.Message);
            }
        });
    }
    function UploadReportImages(ReportNumber,ReportID, UserId,UpdatedDate,Imagename,ImageType,ImagePath, ImagePhonePath, Longitude,Latitude,ClientLocation)
    {
        var imageURI = ImagePhonePath;
        var MobileNumber = localStorage.getItem("USERSINFO.MobileNumber");
        var options = new FileUploadOptions();
        options.fileKey = "file";
        options.fileName = imageURI.substr(imageURI.lastIndexOf('/') + 1);
        options.mimeType = "image/jpeg";
        var params = new Object();
        params.reportnumber = ReportNumber;
        params.userid = UserId;

        params.ReportId = ReportID;
        params.ImageType = ImageType;
        params.ImagePathPhone = imageURI;
        params.longitude = Longitude;

        params.mobile = MobileNumber;
        params.datetime = UpdatedDate;

        params.latitude = Latitude;
        params.ClientLocation = "";// ClientLocation;


        options.params = params;
        options.chunkedMode = false;

        var ft = new FileTransfer();
        ft.upload(imageURI, "http://autoinsurance.flashcontacts.org/appservices/SaveReportImage", function (result) {
           // $("#success").html("<b>Uploaded Sucessfuly.</b>");
            //window.location.href = "photos.html";
            $("#success").html("Uploading Report" + UploadedReports + "Images");
            uploadedImages++;
            if (totalImages <= uploadedImages)
            {
                $("#success").html("Uploaded Complete.");
            }

        }, function (error) {
            $("#success").html("All images are not uploaded yet.");
        }, options);
    }
    function StartUploadVideos(reportNumber, reportID) {
        databaseHandler.createDatabase();
        databaseHandler.db.readTransaction(
               function (tx) {
                   tx.executeSql(
                     "select * from ReportVideos where ReportNumber=?",
                     [reportNumber],
                     function (tx, results) {
                         // alert(results.rows.length + results(i).CompanyID);
                         for (var i = 0; i < results.rows.length; i++) {
                             var obj = results.rows.item(i);
                             //$("#listtable").append('<tr><td>' + obj.ReportNumber + '</td><td>' + obj.BookNumber + '</td><td><a href="#" onclick="deleterecord(' + obj.ReportID + "," + obj.ReportNumber + ')" class="btn"><i class="fa fa-trash" style="color:red" aria-hidden="true"></i> </a><a class="btn" href="stepone.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-play" ></i> </a><a class="btn" href="viewvideo.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-video-camera" ></i> </a><a class="btn" href="photos.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-camera" ></i></a></td></tr>');
                             
                              UploadReportVideos(obj.ReportNumber,reportID, obj.UserId, obj.UpdatedDate, obj.Videoname, obj.VideoType, obj.VideoPath, obj.VideoPhonePath, obj.Longitude, obj.Latitude, obj.ClientLocation);
                         }
                     },
                     function (tx, error) {
                         // alert("add Categories error");
                     }
                     );
               }
               );
    }
    function UploadReportVideos(ReportNumber,ReportID, UserId,UpdatedDate,Videoname,VideoType,VideoPath, VideoPhonePath, Longitude,Latitude,ClientLocation) {
        var MobileNumber = localStorage.getItem("USERSINFO.MobileNumber");
        $("#success").html("<b>Please wait while uploading...</b>");
        var imageURI = VideoPhonePath;

        var options = new FileUploadOptions();
        options.fileKey = "file";
        options.fileName = Videoname;

        var params = new Object();
        params.reportnumber = ReportNumber;
        params.userid = UserId;

        params.ReportId = ReportID;
        params.VideoType = VideoType;
        params.VideoPathPhone = imageURI;
        params.longitude = Longitude;

        params.mobile = MobileNumber;
        params.datetime = UpdatedDate;

        params.latitude = Latitude;
        params.ClientLocation = "";// ClientLocation;


        options.params = params;
        //options.contentType = 'video/mp4';
        options.chunkedMode = false;

        var ft = new FileTransfer();
        ft.upload(imageURI, "http://autoinsurance.flashcontacts.org/appservices/SaveReportVideo", function (result) {
           // $("#success").html("<b>Uploaded Sucessfuly.</b>");
           // window.location.href = "videos.html";

        }, function (error) {
            $("#success").html("Video not uploaded yet.");
        }, options);
    }
    $("#btnReportUpload").click(function () {
        StartUploading();
    });

})(window);