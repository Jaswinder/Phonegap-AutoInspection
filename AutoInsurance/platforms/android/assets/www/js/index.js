/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
var APIBaseUrl = "http://autoinsurance.flashcontacts.org/";
var currentdate;
var app = {
    // Application Constructor
    initialize: function() {
        document.addEventListener('deviceready', this.onDeviceReady.bind(this), false);
    },

    // deviceready Event Handler
    //
    // Bind any cordova events here. Common events are:
    // 'pause', 'resume', etc.
    onDeviceReady: function () {

        


        var mobile = "";
        var mobile_enc = localStorage.getItem("USERSINFO.MobileNumber");
        if (typeof mobile_enc !== "undefined") {
            if (mobile_enc !== null) {
                mobile = mobile_enc;
                $("#navbarResponsive").find("ul").append("<li class='nav-item' ><a class='nav-link' href='#' id='btnUsername'>" + mobile + "</a></li >");
            }
        }
       // $("#success").html("Called Background loaded");

        var latitude = 0.0;
        var longitude = 0.0;
        var ClientLocation = "";

        var onSuccess = function (position) {
            latitude = position.coords.latitude;
            longitude = position.coords.longitude
            ClientLocation = 'Latitude: ' + position.coords.latitude + '\n' +
                  'Longitude: ' + position.coords.longitude + '\n' +
                  'Altitude: ' + position.coords.altitude + '\n' +
                  'Accuracy: ' + position.coords.accuracy + '\n' +
                  'Altitude Accuracy: ' + position.coords.altitudeAccuracy + '\n' +
                  'Heading: ' + position.coords.heading + '\n' +
                  'Speed: ' + position.coords.speed + '\n' +
                  'Timestamp: ' + position.timestamp + '\n';



            var userid = "1";
            var pass_enc = localStorage.getItem("USERSINFO.Id");
            if (typeof pass_enc !== "undefined") {
                if (pass_enc !== null) {
                    userid = pass_enc;
                }
            }

            var url = APIBaseUrl + "APPServices/SaveDriverLoaction?userid=" + userid + "&latitude=" + latitude
            + "&longitude=" + longitude;
            $.ajax({
                type: "GET",
                dataType: "json",
                url: url,
                success: function (result) {
                    //var reportid = result.ReportID;
                   // $("#success").html("Uploading");
                    //StartUploadImages(ReportNumber, reportid);
                    //StartUploadVideos(ReportNumber, reportid);
                    // window.location.href = "stepdocument.html?ReportId=" + $("#txtReportNumber").val();
                },
                error: function (err) {
                      //$("#success").html("Uploading Report Error" + err.Message);
                    //alert(err);
                }
            });



        };

        // onError Callback receives a PositionError object
        //
        function onError(error) {
            // alert('code: ' + error.code + '\n' +
            // 'message: ' + error.message + '\n');
        }
        var UpdatedDate = new Date();
        navigator.geolocation.getCurrentPosition(onSuccess, onError);



        var callbackFn = function (location) {
            //console.log('[js] BackgroundGeolocation callback:  ' + location.latitude + ',' + location.longitude);
           // $("#success").html("Background running");


            var userid = "1";
            var pass_enc = localStorage.getItem("USERSINFO.Id");
            if (typeof pass_enc !== "undefined") {
                if (pass_enc !== null) {
                    userid = pass_enc;
                }
            }

            var url = APIBaseUrl + "APPServices/SaveDriverLoaction?userid=" + userid + "&latitude=" + location.latitude
            + "&longitude=" + location.longitude;
            $.ajax({
                type: "GET",
                dataType: "json",
                url: url,
                success: function (result) {
                    //var reportid = result.ReportID;
                   // $("#success").html("Uploading");
                    //StartUploadImages(ReportNumber, reportid);
                    //StartUploadVideos(ReportNumber, reportid);
                    // window.location.href = "stepdocument.html?ReportId=" + $("#txtReportNumber").val();
                },
                error: function (err) {
                   // $("#success").html("Uploading Report Error" + err.Message);
                    //alert(err);
                }
            });

            // Do your HTTP request here to POST location to your server.
            // jQuery.post(url, JSON.stringify(location));

            /*
            IMPORTANT:  You must execute the finish method here to inform the native plugin that you're finished,
            and the background-task may be completed.  You must do this regardless if your HTTP request is successful or not.
            IF YOU DON'T, ios will CRASH YOUR APP for spending too much time in the background.
            */
            backgroundGeolocation.finish();
        };

        var failureFn = function (error) {
           // $("#success1").html('BackgroundGeolocation error' + error.Message);
        };

        // BackgroundGeolocation is highly configurable. See platform specific configuration options
        backgroundGeolocation.configure(callbackFn, failureFn, {
            desiredAccuracy: 10,
            stationaryRadius: 20,
            distanceFilter: 30,
            locationProvider: backgroundGeolocation.provider.ANDROID_ACTIVITY_PROVIDER,
            interval: 1000
        });

        // Turn ON the background-geolocation system.  The user will be tracked whenever they suspend the app.
        


        backgroundGeolocation.start();



       
        

        this.receivedEvent('deviceready');


		
    },
	
   

    // Update DOM on a Received Event
    receivedEvent: function(id) {
        var parentElement = document.getElementById(id);
        var listeningElement = parentElement.querySelector('.listening');
        var receivedElement = parentElement.querySelector('.received');

        listeningElement.setAttribute('style', 'display:none;');
        receivedElement.setAttribute('style', 'display:block;');

        console.log('Received Event: ' + id);

        //var myDB;

        //myDB = window.sqlitePlugin.openDatabase({ name: "mySQLite.db", location: 'default' });
        //$("#success").html("Created");

    }
};

app.initialize();

//Menu Bar Controls
function BindButtonClicks()
{
    $("#goToLogin").click(function () {
        var pass_enc = localStorage.getItem("USERSINFO.Id");
        if (typeof pass_enc !== "undefined") {
            if (pass_enc !== null) {
                window.location.href = "home.html";
            } else {
                window.location.href = "login.html";
            }
        }
        else {
            window.location.href = "login.html";
        }

    });

    $("#btnLogout").click(function () {
        if (confirm("Do you want to logout.")) {
            localStorage.clear();
            window.location.href = "index.html";
        }
    });
}
$(document).ready(function () {
   
    BindButtonClicks();
});

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
    results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

