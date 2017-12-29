(function (window) {
    $("#btnLogin").click(function () {
        if (navigator.onLine == false) {
            alert("Unable to connect to the internet.Please check your internet connection");
            return;
        }
        

        $("#loading").show();
        var url = APIBaseUrl + "APPServices/login?username=" + $("#mobileno").val() + "&password=" + $("#password").val();
       // alert(url);
            $.ajax({
                type: "GET",
                dataType: "json",
                url: url,
                success: function(result){
                    localStorage.setItem("USERSINFO.FullName", result.FullName);
                    localStorage.setItem("USERSINFO.FatherName", result.FatherName);
                    localStorage.setItem("USERSINFO.MobileNumber", result.MobileNumber);
                    localStorage.setItem("USERSINFO.ProfileImage", result.ProfileImage);
                    localStorage.setItem("USERSINFO.Gender", result.Gender);
                    localStorage.setItem("USERSINFO.UserCode", result.UserCode);
                    localStorage.setItem("USERSINFO.ParentUsername", result.ParentUsername);
                    localStorage.setItem("USERSINFO.ParentUserid", result.ParentUserid);
                    localStorage.setItem("USERSINFO.UserName", result.UserName);
                    localStorage.setItem("USERSINFO.Id", result.Id);
                    if (typeof result.Id !== "undefined") {
                        if (result.Id != null) {
                            $("#loading").hide();
                            window.location.href = "home.html";
                        } else {
                            $("#loading").hide();
                           // alert("Please enter correct mobile number or password!");
                        }
                    }
                    else
                    {
                        $("#loading").hide();
                       //alert("Please enter correct mobile number or password");
                    }
                   // var pass_enc = localStorage.getItem("USERSINFO.FullName");
                   
                },
                error: function (err) {
                    $("#loading").hide();
                    alert("Unable to connect to the internet.Please check your internet connection");
                }
            });
        
    });

})(window);