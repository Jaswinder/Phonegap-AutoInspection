(function (window) {
    // Called when capture operation is finished
    //
    function captureSuccess(mediaFiles) {
        var i, len;
        for (i = 0, len = mediaFiles.length; i < len; i += 1) {
            var filename = "File-" + i;

           // setTimeout(uploadFile(mediaFiles[i], filename);
            setTimeout(uploadFile, 1000, mediaFiles[i], filename);
            //alert(mediaFiles[i].fullPath);
        }
    }

    // Called if something bad happens.
    //
    function captureError(error) {
        var msg = 'An error occurred during capture: ' + error.code;
        navigator.notification.alert(msg, null, 'Uh oh!');
    }

    // A button will call this function
    //
    function captureVideo() {
        // Launch device video recording application,
        // allowing user to capture up to 2 video clips
        navigator.device.capture.captureVideo(captureSuccess, captureError, {
        duration: 120, limit: 1,
        height: 200,
        width: 200,
        quality: 0 });
    }

    var userid = "";
    var pass_enc = localStorage.getItem("USERSINFO.Id");
    if (typeof pass_enc !== "undefined") {
        if (pass_enc !== null) {
            userid = pass_enc;
        }
    }

    var CurrentPhoto = localStorage.getItem("Report.CurrentPhoto");
    var ReportID = localStorage.getItem("Report.ReportID");
    var ReportNumber = localStorage.getItem("Report.ReportNumber");
  
    var MobileNumber = localStorage.getItem("USERSINFO.MobileNumber");
    var latitude = 0.0;
    var longitude = 0.0;
    var ClientLocation = "";

    var onSuccess = function (position) {
        latitude = position.coords.latitude;
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
              // 'message: ' + error.message + '\n');
    }
    var UpdatedDate = new Date();
    navigator.geolocation.getCurrentPosition(onSuccess, onError);
    // Upload files to server
    function uploadFile(mediaFile, filetype) {
        var ImageType = filetype;
        var imageURI = mediaFile.fullPath;
        var fileName = mediaFile.name;

        databaseHandler.createDatabase();

        databaseHandler.db.transaction(
        function (tx) {
            tx.executeSql(
                "insert into ReportVideos(ReportNumber,ReportID, UserId,UpdatedDate,Videoname,VideoType,VideoPath, VideoPhonePath, Longitude,Latitude,ClientLocation) values(?,?,?,DateTime('now'),?,?,?,?,?,?,?)",
                [ReportNumber, ReportID, userid, fileName, ImageType, imageURI, imageURI, longitude, latitude, ClientLocation],
                function (tx, results) {
                    //console.log("add image" + ImageType+"Added");
                    window.location.href = "stepdocument.html";
                },
                function (tx, error) {
                    console.log("add Categories error");
                }
                );
        },
        function (error) { },
        function () { }
        );


        



        //$("#success").html("<b>Please wait while uploading...</b>");
        //var imageURI = mediaFile.fullPath;

        //var options = new FileUploadOptions();
        //options.fileKey = "file";
        //options.fileName = mediaFile.name;

        //var params = new Object();
        //params.reportnumber = ReportNumber;
        //params.userid = userid;

        //params.ReportId = ReportID;
        //params.VideoType = ImageType;
        //params.VideoPathPhone = imageURI;
        //params.longitude = longitude;

        //params.mobile = MobileNumber;
        //params.datetime = new Date();

        //params.latitude = latitude;
        //params.ClientLocation = "";// ClientLocation;


        //options.params = params;
        ////options.contentType = 'video/mp4';
        //options.chunkedMode = false;

        //var ft = new FileTransfer();
        //ft.upload(imageURI, "http://autoinsurance.flashcontacts.org/appservices/SaveReportVideo", function (result) {
        //    $("#success").html("<b>Uploaded Sucessfuly.</b>");
        //    window.location.href = "videos.html";

        //}, function (error) {
        //    $("#success").html(JSON.stringify(error));
        //}, options);
    }

    $("#takevideo").click(function () {
        captureVideo();
        });
    //$("#uploadPhoto").click(function () {

    //    navigator.camera.getPicture(function (result) {
    //        localStorage.setItem("Report.CurrentPhoto", result);
    //        window.location.href = "previewphoto.html";
    //    }, function (error) {
    //        console.log(error);
    //    }, {
    //        sourceType: Camera.PictureSourceType.SAVEDPHOTOALBUM
    //    });
    //});


})(window);