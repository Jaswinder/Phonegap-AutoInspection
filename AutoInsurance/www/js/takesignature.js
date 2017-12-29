function SaveReportNumber(ReportNumber, ReportID, UserId, UpdatedDate, Imagename, ImageType, ImagePath, ImagePhonePath, Longitude, Latitude, ClientLocation, ImagePathStemped, ImagePathThumb) {
    databaseHandler.createDatabase();
    var isImageUpdate = localStorage.getItem("Report.IsImageUpdated");
    if (isImageUpdate == "NO") {
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
    else {
        databaseHandler.db.transaction(
           function (tx) {
               tx.executeSql(
                   "Update ReportImages SET UpdatedDate=?,Imagename=?,ImagePath=?, ImagePhonePath=?, Longitude=?,Latitude=?,ClientLocation=?,ImagePathStemped=?,ImagePathThumb=? where ReportNumber=? and ReportID=? and UserId=? and ImageType=?",
                   [UpdatedDate, Imagename, ImagePath, ImagePhonePath, Longitude, Latitude, ClientLocation, ImagePathStemped, ImagePathThumb, ReportNumber, ReportID, UserId, ImageType],
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

}
(function (window) {

    var ImageType = "SIGPHOTO";
    var userid = "";
    var pass_enc = localStorage.getItem("USERSINFO.Id");
    var ReportID = localStorage.getItem("Report.ReportID");
    var ReportNumber = localStorage.getItem("Report.ReportNumber");
    var MobileNumber = localStorage.getItem("USERSINFO.MobileNumber");
    var latitude = 0.0;
    var longitude = 0.0;
    var ClientLocation = "";




    if (typeof pass_enc !== "undefined") {
        if (pass_enc !== null) {
            userid = pass_enc;
        }
    }


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
        //       'message: ' + error.message + '\n');
    }
    var UpdatedDate = new Date();
    navigator.geolocation.getCurrentPosition(onSuccess, onError);

    $("#btnSaveSignature").click(function () {

        var myBaseString = document.getElementById("signaturecanvas").toDataURL();
        var fileName = "Signature";
        SaveReportNumber(ReportNumber, ReportID, userid, UpdatedDate, fileName, ImageType, "BASE64", myBaseString, longitude, latitude, ClientLocation, "", "");

    });


})(window);