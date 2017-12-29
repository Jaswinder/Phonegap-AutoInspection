/// <reference path="databaseHandler.js" />
var companyMasterHandler = {
    syncCompanyMasters: function (CompanyID, CompanyName, CompanyRemarks, Status) {
        databaseHandler.db.transaction(
            function (tx) {
                tx.executeSql(
                    "insert into CompanyMasters(CompanyID,CompanyName,CompanyRemarks,Status) values(?,?,?,?)",
                    [CompanyID, CompanyName, CompanyRemarks, Status],
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

    deleteAllCompanies: function () {
        databaseHandler.db.transaction(
            function (tx) {
                tx.executeSql(
                    "delete from CompanyMasters",
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
    downloadAllCompanies: function () {
        var url = APIBaseUrl + "APPServices/GetCompanies";
        $.ajax({
            type: "GET",
            dataType: "json",
            url: url,
            success: function (result) {
                //alert(""+result);
                $.each(result, function (i, obj) {
                    //$("#ddVehicleType").append('<option value=' + obj.VehicleCategoryID + '>' + obj.VehicleCategoryName + '</option>');
                    companyMasterHandler.syncCompanyMasters(obj.CompanyID, obj.CompanyName, obj.CompanyRemarks, obj.Status);
                });

               // $("#success").html("Updated");
                $(".progress-bar").css("width", "100%");
                $("#goToLogin").show();
                $(".progress-bar").hide();
            },
            error: function (err) {
                // $("#success").html("Transaction Error" + err.message);
                // $("#loading").hide();
                // alert(err.message);
                $(".progress-bar").hide();
                $("#goToLogin").show();
            }
        });
    },
    LoadAllCompanies: function () {
        databaseHandler.db.readTransaction(
            function (tx) {
                tx.executeSql(
                  "select * from CompanyMasters",
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