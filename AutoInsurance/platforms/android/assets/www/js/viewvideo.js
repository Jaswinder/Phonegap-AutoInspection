(function (window) {
    var userid = "";
    var pass_enc = localStorage.getItem("USERSINFO.Id");
    if (typeof pass_enc !== "undefined") {
        if (pass_enc !== null) {
            userid = pass_enc;
        }
    }
    var reportNumber = 0;
    if (reportNumber == 0) {
        reportNumber = getParameterByName("ReportNumber");
    }
    $("#videoArea").empty();
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
                         //if (obj.IsLocked == true) {
                         //       $("#listtable").append('<tr><td>' + obj.ReportNumber + '</td><td>' + obj.BookNumber + '</td><td><i class="fa fa-lock danger" style="color:#d60606" aria-hidden="true"></i>  </td></tr>');
                         //   }
                         //   else {
                         var v = "<video controls='controls'>";
                         v += "<source src='" + obj.VideoPhonePath + "' type='video/mp4'>";
                         v += "</video>";
                         $("#videoArea").append('<div class="col-md-12">' + v + '</div>');

                     
                         //   }

                     }
                     if(results.rows.length==0)
                     {
                         $("#videoArea").append('<div class="col-md-12"><h3>There is now video recorded yet. </h3></div>');
                     }
                 },
                 function (tx, error) {
                     // alert("add Categories error");
                 }
                 );
           }
           );

})(window);