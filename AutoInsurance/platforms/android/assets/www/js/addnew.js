
function SaveReportNumber(reportnumber, userid) {
	

    databaseHandler.db.transaction(
                               function (tx) {
                                   tx.executeSql(
                                       "insert into ReportMasters(ReportNumber,UserId,StartDate) values(?,?,DateTime('now'))",
                                       [reportnumber, userid],
                                       function (tx, results) {
                                           localStorage.setItem("Report.ReportNumber", $("#txtReportNumber").val());
                                           localStorage.setItem("Report.ReportID", results.insertId);
                                           window.location.href = "stepone.html?ReportNumber=" + $("#txtReportNumber").val();
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
    var reportNumber = 0;
    try {
        reportNumber = getParameterByName("ReportNumber");
        $("#txtReportNumber").val(reportNumber);
    } catch (ex) { }
    databaseHandler.createDatabase();
    $("#btnStartReport").click(function () {
        //$("#loading").show();
        var userid = "";
        var pass_enc = localStorage.getItem("USERSINFO.Id");
        if (typeof pass_enc !== "undefined") {
            if (pass_enc !== null) {
                userid = pass_enc;
            }
        }
        reportNumber = $("#txtReportNumber").val();

        databaseHandler.db.readTransaction(
    function (tx) {
        tx.executeSql(
          "select * from ReportMasters where ReportNumber=? and UserId=?",
          [reportNumber, userid],
          function (tx, results) {
              if (results.rows.length > 0) {
                  var ReportID = results.rows.item(0).ReportID;
                  localStorage.setItem("Report.ReportNumber", $("#txtReportNumber").val());
                  localStorage.setItem("Report.ReportID", ReportID);
                  window.location.href = "stepone.html?ReportNumber=" + $("#txtReportNumber").val();
              }
              else {
                  SaveReportNumber(reportNumber, userid);
              }
             

          },
          function (tx, error) {
              alert("add Categories error");
          }
          );
    }
    );



    });


})(window);