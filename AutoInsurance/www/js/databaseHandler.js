var databaseHandler = {
    db: null,
    createDatabase: function () {
        this.db = window.openDatabase(
            "autoinsurance.db",
            "1.0",
            "Auto Insurance database",
            10000000
            );
        this.db.transaction(
            function (transaction) {
                transaction.executeSql('CREATE TABLE IF NOT EXISTS VehicleCategories (VehicleCategoryID integer primary key, VehicleCategoryName text)', [],
                function (tx, result) {
                  //  $("#success").html("Table created successfully");
                },
                function (error) {
                    $("#success").html("Error occurred while creating the table.");
                });

                transaction.executeSql('CREATE TABLE IF NOT EXISTS ReportMasters (ReportID integer primary key, ReportNumber integer,UserId text,StartDate text,FinishDate text,Status text,IsLocked text, VehicleCategoryID integer,VehicleNumber text,ChassisNumber text,BookNumber integer,Comment text,CompanyID text,CompanyName text,Longitude text,Latitude text,ClientLocation text)', [],
                    function (tx, result) {
                       // $("#success").html("Table created successfully");
                    },
                    function (error) {
                      //  $("#success").html("Error occurred while creating the table.");
                    });

                transaction.executeSql('CREATE TABLE IF NOT EXISTS ReportImages (ReportNumber integer,ReportID integer, UserId text,UpdatedDate text,Imagename text,ImageType text,ImagePath text, ImagePhonePath text, Longitude text,Latitude text,ClientLocation text,ImagePathStemped text,ImagePathThumb text)', [],
                    function (tx, result) {
                     //   $("#success").html("Table created successfully");
                    },
                    function (error) {
                      //  $("#success").html("Error occurred while creating the table.");
                    });

                transaction.executeSql('CREATE TABLE IF NOT EXISTS ReportVideos (ReportNumber integer,ReportID integer, UserId text,UpdatedDate text,Videoname text,VideoType text,VideoPath text, VideoPhonePath text, Longitude text,Latitude text,ClientLocation text)', [],
                    function (tx, result) {
                     //   $("#success").html("Table created successfully");
                    },
                    function (error) {
                       // $("#success").html("Error occurred while creating the table.");
                    });

                transaction.executeSql('CREATE TABLE IF NOT EXISTS ImageTypes (ImageTypeID integer primary key, ImageTypeName text, ImageTypeRemarks text,Code text,Status text)', [],
                    function (tx, result) {
                       // $("#success").html("Table created successfully");
                    },
                    function (error) {
                       // $("#success").html("Error occurred while creating the table.");
                    });

                transaction.executeSql('CREATE TABLE IF NOT EXISTS CompanyMasters (CompanyID integer primary key, CompanyName text, CompanyRemarks text,Status text)', [],
                    function (tx, result) {
                       // $("#success").html("Table created successfully");
                    },
                    function (error) {
                        $("#success").html("Error occurred while creating the table.");
                    });

            },
            function (error) {
                $("#success").html("Transaction Error" + error.message);
            },
            function () {
               // $("#success").html("Create DB Transaction")
            }

            );
    }



}