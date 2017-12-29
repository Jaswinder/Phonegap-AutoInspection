(function (window) {
    var userid = "";
    var pass_enc = localStorage.getItem("USERSINFO.Id");
    if (typeof pass_enc !== "undefined") {
        if (pass_enc !== null) {
            userid = pass_enc;
        }
    }
    $("#listtable").empty();
    databaseHandler.createDatabase();
    databaseHandler.db.readTransaction(
           function (tx) {
               tx.executeSql(
                 "select * from ReportMasters",
                 [],
                 function (tx, results) {
                     // alert(results.rows.length + results(i).CompanyID);
                     for (var i = 0; i < results.rows.length; i++) {
                         var obj = results.rows.item(i);
                         //if (obj.IsLocked == true) {
                         //       $("#listtable").append('<tr><td>' + obj.ReportNumber + '</td><td>' + obj.BookNumber + '</td><td><i class="fa fa-lock danger" style="color:#d60606" aria-hidden="true"></i>  </td></tr>');
                         //   }
                         //   else {
                         $("#listtable").append('<tr><td>' + obj.ReportNumber + '</td><td>' + obj.BookNumber + '</td><td><a href="#" data-reportid="' + obj.ReportID + '" data-reportnumber="' + obj.ReportNumber + '" class="btn deleteReport"><i class="fa fa-trash" style="color:red" aria-hidden="true"></i> </a><a class="btn" href="stepone.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-play" ></i> </a><a class="btn" href="viewvideo.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-video-camera" ></i> </a><a class="btn" href="photos.html?ReportNumber=' + obj.ReportNumber + '" > <i class="fa fa-camera" ></i></a></td></tr>');
                         //   }

                     }
                     $(".deleteReport").click(function () {
                         var reportnumber = $(this).attr("data-reportnumber");
                         var reportid = $(this).attr("data-reportid");
                         deleterecord(reportid, reportnumber);
                     });
                 },
                 function (tx, error) {
                     // alert("add Categories error");
                 }
                 );
           }
           );

    //var url = APIBaseUrl + "APPServices/GetAllMyReports?userid="+userid;
    //$.ajax({
    //    type: "GET",
    //    dataType: "json",
    //    url: url,
    //    success: function (result) {
    //        $("#listtable").empty();
    //        $.each(result, function (i, obj) {
    //            if (obj.IsLocked == true) {
    //                $("#listtable").append('<tr><td>' + obj.ReportNumber + '</td><td>' + obj.BookNumber + '</td><td><i class="fa fa-lock danger" style="color:#d60606" aria-hidden="true"></i>  </td></tr>');
    //            }
    //            else {
    //                $("#listtable").append('<tr><td>' + obj.ReportNumber + '</td><td>' + obj.BookNumber + '</td><td><a href="#" class="btn"><i class="fa fa-unlock" aria-hidden="true"></i> </a><a class="btn" href="stepone.html?ReportId=' + obj.ReportNumber + '" > <i class="fa fa-play" ></i> </a></td></tr>');
    //            }
    //        });
    //    },
    //    error: function (err) {
    //        $("#loading").hide();
    //        //alert(err);
    //    }
    //});
    $(".deleteReport").click(function () {
        var reportnumber = $(this).attr("data-reportnumber");
        var reportid = $(this).attr("data-reportid");
        deleterecord(reportid, reportnumber);
    });

})(window);
function deleterecord(id,reportno)
{
    
    if (confirm("Do you want to delete this report with images and videos.")) {
        databaseHandler.createDatabase();
        databaseHandler.db.transaction(
                function (tx) {
                    tx.executeSql(
                      "DELETE from ReportMasters where ReportID=?",
                      [id],
                      function (tx, results) {
                          deleteVideos(reportno);
                      },
                      function (tx, error) {
                          // alert("add Categories error");
                      }
                      );
                }
                );
    }
}
function deleteVideos(rnumber)
{
    databaseHandler.db.transaction(
              function (tx) {
                  tx.executeSql(
                    "delete from ReportVideos where ReportNumber=?",
                    [rnumber],
                    function (tx, results) {
                        deleteImages(rnumber);
                    },
                    function (tx, error) {
                        // alert("add Categories error");
                    }
                    );
              }
              );
}
function deleteImages(rnumber) {
    databaseHandler.db.transaction(
              function (tx) {
                  tx.executeSql(
                    "delete from ReportImages where ReportNumber=?",
                    [rnumber],
                    function (tx, results) {
                        location.reload();
                    },
                    function (tx, error) {
                        // alert("add Categories error");
                    }
                    );
              }
              );
}