/// <reference path="databaseHandler.js" />
/// <reference path="syncVehicleCategories.js" />
/// <reference path="syncImageTypes.js" />
/// <reference path="syncCompanyMasters.js" />

(function (window) {
   
   
    if (navigator.onLine == false) {
        $(".progress-bar").hide();
        //alert("No Internet");
        $("#success").html("Offline");
    } else
    {
        $("#success").html("Downloading Data");
        $(".progress-bar").show();
        $("#goToLogin").hide();

        databaseHandler.createDatabase();
        vehicleCategoryHandler.deleteAllVehicleCategories();
        vehicleCategoryHandler.downloadAllVehicleCategories();

        imageTypesHandler.deleteAllImageTypes();
        imageTypesHandler.downloadAllImageTypes();

        companyMasterHandler.deleteAllCompanies();
        companyMasterHandler.downloadAllCompanies();
        
    }
    

    //var myDB;
   
    //    myDB = window.sqlitePlugin.openDatabase({ name: "mySQLite.db", location: 'default' });
    //    $("#success").html("Created");
    
    //$("#success").html("Laded1");

    //myDB.transaction(function (transaction) {
    //    transaction.executeSql('CREATE TABLE IF NOT EXISTS VehicleCategories (VehicleCategoryID integer primary key, VehicleCategoryName text)', [],
    //        function (tx, result) {
    //            $("#success").html("Table created successfully");
    //        },
    //        function (error) {
    //            $("#success").html("Error occurred while creating the table.");
    //        });

    //    transaction.executeSql('CREATE TABLE IF NOT EXISTS ReportMasters (ReportID integer primary key, ReportNumber integer,UserId text,StartDate text,FinishDate text,Status text,IsLocked text, VehicleCategoryID integer,VehicleNumber text,ChassisNumber text,BookNumber integer,Comment text,CompanyID text,CompanyName text,Longitude text,Latitude text,ClientLocation text)', [],
    //        function (tx, result) {
    //            $("#success").html("Table created successfully");
    //        },
    //        function (error) {
    //            $("#success").html("Error occurred while creating the table.");
    //        });

    //    transaction.executeSql('CREATE TABLE IF NOT EXISTS ReportImages (ReportNumber integer,ReportID integer, UserId text,UpdatedDate text,Imagename text,ImageType text,ImagePath text, ImagePhonePath text, Longitude text,Latitude text,ClientLocation text,ImagePathStemped text,ImagePathThumb text)', [],
    //        function (tx, result) {
    //            $("#success").html("Table created successfully");
    //        },
    //        function (error) {
    //            $("#success").html("Error occurred while creating the table.");
    //        });

    //    transaction.executeSql('CREATE TABLE IF NOT EXISTS ReportVideos (ReportNumber integer,ReportID integer, UserId text,UpdatedDate text,Videoname text,VideoType text,VideoPath text, VideoPhonePath text, Longitude text,Latitude text,ClientLocation text)', [],
    //       function (tx, result) {
    //           $("#success").html("Table created successfully");
    //       },
    //       function (error) {
    //           $("#success").html("Error occurred while creating the table.");
    //       });

    //    transaction.executeSql('CREATE TABLE IF NOT EXISTS ImageTypes (ImageTypeID integer primary key, ImageTypeName text, ImageTypeRemarks text,Code text,Status text)', [],
    //      function (tx, result) {
    //          $("#success").html("Table created successfully");
    //      },
    //      function (error) {
    //          $("#success").html("Error occurred while creating the table.");
    //      });
    //});
    
    //function createTable()
    //{
    //    myDB.transaction(function (transaction) {
    //        var executeQuery = "INSERT INTO phonegap_pro (title, desc) VALUES (?,?)";
    //        transaction.executeSql(executeQuery, [title, desc]
    //            , function (tx, result) {
    //                alert('Inserted');
    //            },
    //            function (error) {
    //                alert('Error occurred');
    //            });
    //    });
    //}

    //function Select()
    //{
    //    myDB.transaction(function (transaction) {
    //        transaction.executeSql('SELECT * FROM phonegap_pro', [], function (tx, results) {
    //            var len = results.rows.length, i;
    //            $("#rowCount").html(len);
    //            for (i = 0; i < len; i++) {
    //                $("#TableData").append("<tr><td>" + results.rows.item(i).id + "</td><td>" + results.rows.item(i).title + "</td><td>" + results.rows.item(i).desc + "</td><td><a href='edit.html?id=" + results.rows.item(i).id + "&title=" + results.rows.item(i).title + "&desc=" + results.rows.item(i).desc + "'>Edit</a> &nbsp;&nbsp; <a class='delete' href='#' id='" + results.rows.item(i).id + "'>Delete</a></td></tr>");
    //            }
    //        }, null);
    //    });
        //}
    

})(window);