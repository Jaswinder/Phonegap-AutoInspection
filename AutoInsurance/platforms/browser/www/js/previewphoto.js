function SaveReportNumber(ReportNumber, ReportID, UserId, UpdatedDate, Imagename, ImageType, ImagePath, ImagePhonePath, Longitude, Latitude, ClientLocation, ImagePathStemped, ImagePathThumb) {
    databaseHandler.createDatabase();
    databaseHandler.db.transaction(
    function (tx) {
        tx.executeSql(
            "insert into ReportImages(ReportNumber,ReportID, UserId,UpdatedDate,Imagename,ImageType,ImagePath, ImagePhonePath, Longitude,Latitude,ClientLocation,ImagePathStemped,ImagePathThumb) values(?,?,?,?,?,?,?,?,?,?,?,?,?)",
            [ReportNumber, ReportID, UserId, UpdatedDate, Imagename, ImageType, ImagePath, ImagePhonePath, Longitude, Latitude, ClientLocation, ImagePathStemped, ImagePathThumb],
            function (tx, results) {
                //console.log("add image" + ImageType+"Added");
                window.location.href = "photos.html";
            },
            function (tx, error) {
                console.log("add Categories error");
            }
            );
    },
    function (error) { },
    function () { }
    );

}

(function (window) {
        var userid = "";
        var pass_enc = localStorage.getItem("USERSINFO.Id");
        var CurrentPhoto = localStorage.getItem("Report.CurrentPhoto");
        var ReportID = localStorage.getItem("Report.ReportID");
        var ReportNumber = localStorage.getItem("Report.ReportNumber");
        var ImageType = localStorage.getItem("Report.ImageType");
        var MobileNumber = localStorage.getItem("USERSINFO.MobileNumber");
        var latitude = 0.0;
        var longitude = 0.0;
        var ClientLocation = "";




        if (typeof pass_enc !== "undefined") {
            if (pass_enc !== null) {
                userid = pass_enc;
            }
        }
        var img = document.getElementById("imgPreview");
        img.src = CurrentPhoto;


        var onSuccess = function (position) {
            latitude=position.coords.latitude;
            longitude = position.coords.longitude
            ClientLocation = 'Latitude: ' + position.coords.latitude + '\n' +
                  'Longitude: ' + position.coords.longitude + '\n' +
                  'Altitude: ' + position.coords.altitude + '\n' +
                  'Accuracy: ' + position.coords.accuracy + '\n' +
                  'Altitude Accuracy: ' + position.coords.altitudeAccuracy + '\n' +
                  'Heading: ' + position.coords.heading + '\n' +
                  'Speed: ' + position.coords.speed + '\n' +
                  'Timestamp: ' + position.timestamp + '\n';
        };

    // onError Callback receives a PositionError object
    //
        function onError(error) {
           // alert('code: ' + error.code + '\n' +
           //       'message: ' + error.message + '\n');
        }
        var UpdatedDate=new Date();
        navigator.geolocation.getCurrentPosition(onSuccess, onError);
        var imageURI = document.getElementById("imgPreview").src;
        var fileName = imageURI.substr(imageURI.lastIndexOf('/') + 1);
        
        SaveReportNumber(ReportNumber, ReportID, userid, UpdatedDate, fileName, ImageType, imageURI, imageURI, longitude, latitude, ClientLocation, "", "");

        $("#btnUploadImage").click(function () {
            $("#success").html("<b>Please wait while uploading...</b>");
            SaveReportNumber(ReportNumber, ReportID, userid, UpdatedDate, fileName, ImageType, imageURI, imageURI, longitude, latitude, ClientLocation, "", "");
            //var imageURI = document.getElementById("imgPreview").src;
            //var options = new FileUploadOptions();
            //options.fileKey = "file";
            //options.fileName = imageURI.substr(imageURI.lastIndexOf('/') + 1);
            //options.mimeType = "image/jpeg";
            //console.log(options.fileName);
            //var params = new Object();
            //params.reportnumber = ReportNumber;
            //params.userid = userid;   

            //params.ReportId = ReportID;
            //params.ImageType = ImageType;
            //params.ImagePathPhone = imageURI;
            //params.longitude = longitude;

            //params.mobile = MobileNumber;
            //params.datetime = new Date();

            //params.latitude = latitude;
            //params.ClientLocation = "";// ClientLocation;
      

            //options.params = params;
            //options.chunkedMode = false;

            //var ft = new FileTransfer();
            //ft.upload(imageURI, "http://autoinsurance.flashcontacts.org/appservices/SaveReportImage", function (result) {
            //    $("#success").html("<b>Uploaded Sucessfuly.</b>");
            //    window.location.href = "photos.html";
                
            //}, function (error) {
            //    $("#success").html(JSON.stringify(error));
            //}, options);
        });
        


})(window);