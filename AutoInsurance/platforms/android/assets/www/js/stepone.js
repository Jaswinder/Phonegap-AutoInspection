/// <reference path="databaseHandler.js" />
var reportID = 0;
function GetReportNo(report) {
    var bookSize = 50;

    var bookSlot = Math.floor(((report - 1) / bookSize));

    var BookNo = (bookSlot * bookSize) + 1;

    return BookNo;
}
function IfRowExist(reportnumber) {
    
   databaseHandler.db.readTransaction(
   function (tx) {
       tx.executeSql(
         "select * from ReportMasters where ReportNumber=?",
         [reportnumber],
         function (tx, results) {

             for (var i = 0; i < results.rows.length; i++) {
                 reportID = results.rows.item(i).ReportID;
                 if (results.rows.item(i).VehicleCategoryID > 0) {
                     $("#ddVehicleType").val(results.rows.item(i).VehicleCategoryID);
                 }
                 if (results.rows.item(i).CompanyID > 0) {
                     $("#ddCompanyName").val(results.rows.item(i).CompanyID);
                 }
                // $("#txtBookno").val(results.rows.item(i).BookNumber);
                 var bookNoByReport = GetReportNo(reportnumber);
                 $("#txtBookno").val(bookNoByReport);
                 $("#txtVehicleno").val(results.rows.item(i).VehicleNumber);
                 $("#txtChessino").val(results.rows.item(i).ChassisNumber);
             }
         },
         function (tx, error) {
             alert("add Categories error");
         }
         );
   }
   );
}

(function (window) {
    var reportNumber = getParameterByName("ReportNumber");
    $("#ReportNumber").text(reportNumber);
    if (reportNumber > 0)
	{
        localStorage.setItem("Report.ReportNumber", reportNumber);
    }
    $("#txtBookno").prop("disabled", true);
    var bookNoByReport = GetReportNo(reportNumber);
    //alert(bookNoByReport);
    $("#txtBookno").val(bookNoByReport);
	databaseHandler.createDatabase();
	databaseHandler.db.readTransaction(
           function (tx) {
               tx.executeSql(
                 "select * from CompanyMasters",
                 [],
                 function (tx, results) {
                    // alert(results.rows.length + results(i).CompanyID);
                     for (var i = 0; i < results.rows.length; i++) {

                         $("#ddCompanyName").append('<option value=' + results.rows.item(i).CompanyID + '>' + results.rows.item(i).CompanyName + '</option>');
                     }
                 },
                 function (tx, error) {
                    // alert("add Categories error");
                 }
                 );
           }
           );

	databaseHandler.db.readTransaction(
       function (tx) {
           tx.executeSql(
             "select * from VehicleCategories",
             [],
             function (tx, results) {
                 for (var i = 0; i < results.rows.length; i++) {

                     $("#ddVehicleType").append('<option value=' + results.rows.item(i).VehicleCategoryID + '>' + results.rows.item(i).VehicleCategoryName + '</option>');
                 }
                 IfRowExist(reportNumber);
             },
             function (tx, error) {
                // alert("add Categories error");
             }
             );
       }
       );

	
	

    $("#btnSteponeSave").click(function () {

        //var url = APIBaseUrl + "APPServices/SaveStepOne?reportnumber=" + reportid + "&vehicletypeid=" + $("#ddVehicleType").val()
        //+ "&booknumber="+ $("#txtBookno").val() +
        //"&companyid="+ $("#ddCompanyName").val() +
        //"&vehiclenumber="+$("#txtVehicleno").val()+
        //"&chassinumber=" + $("#txtChessino").val();
        var ReportNumber = reportNumber;
        var VehicleCategoryID=$("#ddVehicleType").val();
        var BookNumber= $("#txtBookno").val();
        var CompanyID= $("#ddCompanyName").val();
        var VehicleNumber= $("#txtVehicleno").val(); 
        var ChassisNumber = $("#txtChessino").val();
        if (reportID > 0) {
            databaseHandler.db.transaction(
             function (tx) {
                 tx.executeSql(
                     "update ReportMasters set ReportNumber=?,VehicleCategoryID=?,BookNumber=?,CompanyID=?,VehicleNumber=?,ChassisNumber=? where ReportID=?",
                     [ReportNumber, VehicleCategoryID, BookNumber, CompanyID, VehicleNumber, ChassisNumber, reportID],
                     function (tx, results) {
                         window.location.href = "stepdocument.html?ReportNumber=" + ReportNumber;
                     },
                     function (tx, error) {
                         
                     }
                     );
             },
             function (error) {
                 
             },
             function () {
                
             }
             );

        }else
        {
            databaseHandler.db.transaction(
             function (tx) {
                 tx.executeSql(
                     "insert into ReportMasters(ReportNumber,VehicleCategoryID,BookNumber,CompanyID,VehicleNumber,ChassisNumber) values(?,?,?,?,?,?)",
                     [ReportNumber, VehicleCategoryID, BookNumber, CompanyID, VehicleNumber, ChassisNumber],
                     function (tx, results) {
                         window.location.href = "stepdocument.html?ReportNumber=" + ReportNumber;
                     },
                     function (tx, error) {
                         
                     }
                     );
             },
             function (error) { },
             function () { }
             );
        }
        
        //$.ajax({
        //    type: "GET",
        //    dataType: "json",
        //    url: url,
        //    success: function (result) {

        //        window.location.href = "stepdocument.html?ReportId=" + $("#txtReportNumber").val();
        //    },
        //    error: function (err) {
        //        $("#loading").hide();
        //        //alert(err);
        //    }
        //});

    });

})(window);

