/// <reference path="databaseHandler.js" />
var vehicleCategoryHandler = {
    syncVehicleCategories: function (VehicleCategoryID, VehicleCategoryName)
    {
        databaseHandler.db.transaction(
            function (tx) {
                tx.executeSql(
                    "insert into VehicleCategories(VehicleCategoryID,VehicleCategoryName) values(?,?)",
                    [VehicleCategoryID, VehicleCategoryName],
                    function (tx, results) {
                        console.log("Category Inserted");
                    },
                    function (tx, error) {
                        console.log("add Categories error");
                    }
                    );
            },
            function(error){},
            function(){}
            );
    },

deleteAllVehicleCategories: function ()
{
    databaseHandler.db.transaction(
        function (tx) {
            tx.executeSql(
                "delete from VehicleCategories",
                [],
                function(tx,results){},
                function (tx, error) {
                    console.log("add Categories error");
                }
                );
        },
        function(error){},
        function(){}
        );
},
downloadAllVehicleCategories: function () {
    var url = APIBaseUrl + "APPServices/GetVehicleCategories";
    $.ajax({
        type: "GET",
        dataType: "json",
        url: url,
        success: function (result) {
            //alert(""+result);
            $.each(result, function (i, obj) {
                //$("#ddVehicleType").append('<option value=' + obj.VehicleCategoryID + '>' + obj.VehicleCategoryName + '</option>');
                 vehicleCategoryHandler.syncVehicleCategories(obj.VehicleCategoryID, obj.VehicleCategoryName);
            });
          //  $("#success").html("vehicleCategoryHandler Updated");
            $(".progress-bar").css("width", "30%");
        },
        error: function (err) {
           // $("#success").html("Transaction Error" + err.message);
           // $("#loading").hide();
            //alert(err);
        }
    });
},
LoadAllVehicleCategories: function () {
    databaseHandler.db.readTransaction(
        function (tx) {
            tx.executeSql(
              "select * from VehicleCategories",
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