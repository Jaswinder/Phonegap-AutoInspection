

$(document).ready(function () {
    $('#EndSno,.editreadonly').attr('readonly', true);
    $('#StartSNo').attr('step', 50);
    $('#StartSNo').attr('min', 1);
    $("#StartSNo").change(function () {
        var sno = parseInt($(this).val());
        if (sno % 10 == 1) {
            $("#textMessageBox").hide();
        } else {
            $("#textMessageBox").removeClass("box-success").addClass("box-danger");
            $("#textMessage").empty().append("Please enter start number like 1, 51, 101, 151, 201 or so on. this will not accept number which are not ending with 1");
            $("#textMessageBox").show();
            return;
        }
            var value = sno + 49;
            $("#EndSno").val(value);
            $("#NumberOfBooks").val(1);
            BindCheckValidBookNumber();
       
    });
    $("#NumberOfBooks").change(function(){
         var valueare = (parseInt($(this).val()) * 50);
         valueare = valueare + parseInt($("#StartSNo").val()-1);
        $("#EndSno").val(valueare);
    });

    $("#BookTitle").change(function () {
        BindCheckValidBookTitle();
    });

});
function BindCheckValidBookNumber() {
    var stNo = $("#StartSNo").val();

    var postData = {
        startNumber: stNo,
        bookId: Resources.BookID
    };
    $.ajax({
        type: "POST",
        url: "/BookMasters/CheckValidNumber",
        dataType: "json",
        data: JSON.stringify(postData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            //here variable data is in JSON format
            if (data == "NO") {
                $("#textMessageBox").removeClass("box-success").addClass("box-danger");
                $("#textMessage").empty().append(Resources.SerialNumbarValidation);
                $("#textMessageBox").show();
                $("#StartSNo").val(0)
                $("#StartSNo").focus();
            }
        }
    });
}

function BindCheckValidBookTitle() {
    var stBookTitle = $("#BookTitle").val();
    var postData = {
        bookTitle: stBookTitle,
        bookId: Resources.BookID
    };
    $.ajax({
        type: "POST",
        url: "/BookMasters/CheckValidBookTitle",
        dataType: "json",
        data: JSON.stringify(postData),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            //here variable data is in JSON format
            if (data == "NO") {
                $("#textMessageBox").removeClass("box-success").addClass("box-danger");
                $("#textMessage").empty().append(Resources.ValidateBookTitle);
                $("#textMessageBox").show();
                $("#BookTitle").val("")
                $("#BookTitle").focus();
            }
        }
    });
}