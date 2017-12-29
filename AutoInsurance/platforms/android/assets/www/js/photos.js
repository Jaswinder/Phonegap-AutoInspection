(function (window) {
        
        var reportNumber = localStorage.getItem("Report.ReportNumber");
        if (reportNumber==null)
        {
            reportNumber = getParameterByName("ReportNumber");
        }

        databaseHandler.createDatabase();
        databaseHandler.db.readTransaction(
function (tx) {
    tx.executeSql(
      "select ImageTypes.*,(select ImagePhonePath from ReportImages where ReportImages.ImageType=ImageTypes.Code and ReportImages.ReportNumber=? ) as ImagePhonePath from ImageTypes",
      [reportNumber],
      function (tx, results) {
          if (results.rows.length > 0) {
               
              $("#photocenterDiv").empty();
               for (var i = 0; i < results.rows.length; i++) {
                   var obj = results.rows.item(i);
                   var Isupdate = 'data-Isupdate="NO"';
                   if (obj.ImagePhonePath != null) {
                       Isupdate = 'data-Isupdate="YES"';
                   }
                   var html = '<div class="col-xs-6"><a href="#" id="FrontPhotoClick" class="PhotoClick " ' + Isupdate + ' data-ImageType="' + obj.Code + '">';
                   if (obj.ImagePhonePath != null && obj.Code != "SIGPHOTO") {
                       html += '<img src="' + obj.ImagePhonePath + '" class="img-thumbnail" alt="Image" width="100" height="100" style="margin:10px!important"/>';
                    }
                    else {
                       html += '<span class="fa-stack fa-4x"><i class="fa fa-square fa-stack-2x text-info"></i><i class="fa fa-image fa-stack-1x fa-inverse" ></i></span>';
                    }
                    html += '</a><h5 class="service-heading">' + obj.ImageTypeName + '</h5></div>';
                    $("#photocenterDiv").append(html);
               }
               bindmethod();
          }
          else {
            //  SaveReportNumber(reportNumber, userid);
          }


      },
      function (tx, error) {
          alert("add Categories error");
      }
      );
}
);


        //var url = APIBaseUrl + "APPServices/GetReportImageTypes?reportnumber=" + ReportID;
        //$.ajax({
        //    type: "GET",
        //    dataType: "json",
        //    url: url,
        //    success: function (result) {
        //        $("#photocenterDiv").empty();
        //        $.each(result, function (i, obj) {
        //            var html = '<div class="col-xs-4"><a href="#" id="FrontPhotoClick" class="PhotoClick" data-ImageType="'+obj.Code+'"><span class="fa-stack fa-3x">';
        //            if (obj.Status!= null) {
        //                html += '<img src="' + APIBaseUrl + obj.Status + '" class="img-thumbnail" style="height:80px;width:80px" />';
        //            }
        //            else {
        //                html +='<i class="fa fa-circle fa-stack-2x text-info"></i><i class="fa fa-image fa-stack-1x fa-inverse"></i>';
        //            }
        //            html += '</span></a><h5 class="service-heading">' + obj.ImageTypeName + '</h5></div>';
        //            $("#photocenterDiv").append(html);
        //        });
        //        bindmethod();
        //    },
        //    error: function (err) {
        //        $("#loading").hide();
        //        //alert(err);
        //    }
        //});


})(window);

function bindmethod()
{
    $(".PhotoClick").click(function () {
        var ImageType = $(this).attr("data-ImageType");
        localStorage.setItem("Report.ImageType", ImageType);
        var IsUpdated = $(this).attr("data-Isupdate");
        localStorage.setItem("Report.IsImageUpdated", IsUpdated);
        if (ImageType == "SIGPHOTO") {
            window.location.href = "takesignature.html";
        } else {
            window.location.href = "takephoto.html";
        }
    });
}