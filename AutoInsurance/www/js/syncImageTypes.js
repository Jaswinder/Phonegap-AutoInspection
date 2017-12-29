/// <reference path="databaseHandler.js" />
var imageTypesHandler = {
    syncImageTypes: function (ImageTypeID, ImageTypeName, ImageTypeRemarks, Code, Status) {
        databaseHandler.db.transaction(
            function (tx) {
                tx.executeSql(
                    "insert into ImageTypes(ImageTypeID, ImageTypeName, ImageTypeRemarks, Code, Status) values(?,?,?,?,?)",
                    [ImageTypeID, ImageTypeName, ImageTypeRemarks, Code, Status],
                    function (tx, results) {
                        console.log("Category Inserted");
                    },
                    function (tx, error) {
                        console.log("add Categories error");
                    }
                    );
            },
            function (error) { },
            function () { }
            );
    },

    deleteAllImageTypes: function () {
        databaseHandler.db.transaction(
            function (tx) {
                tx.executeSql(
                    "delete from ImageTypes",
                    [],
                    function (tx, results) { },
                    function (tx, error) {
                        console.log("add Categories error");
                    }
                    );
            },
            function (error) { },
            function () { }
            );
    },
    downloadAllImageTypes: function () {
        var url = APIBaseUrl + "APPServices/GetReportImageTypes";
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            success: function (result) {
                //alert(""+result);
                $.each(result, function (i, obj) {
                    //$("#ddVehicleType").append('<option value=' + obj.VehicleCategoryID + '>' + obj.VehicleCategoryName + '</option>');
                    imageTypesHandler.syncImageTypes(obj.ImageTypeID, obj.ImageTypeName, obj.ImageTypeRemarks, obj.Code, obj.Status);
                });
               // $("#success").html("imageTypesHandler Updated");
                $(".progress-bar").css("width", "60%");
            },
            error: function (err) {
                // $("#success").html("Transaction Error" + err.message);
                // $("#loading").hide();
                //alert(err);
            }
        });
    },
    LoadAllImageTypes: function () {
        databaseHandler.db.readTransaction(
            function (tx) {
                tx.executeSql(
                  "select * from ImageTypes",
                  [],
                  function (tx, results) {
                      // alert(results.rows.length);
                  },
                  function (tx, error) {
                      console.log("add Categories error");
                  }
                  );
            }
            );
    }
};