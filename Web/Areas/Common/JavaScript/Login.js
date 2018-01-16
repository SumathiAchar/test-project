var recoverPassword = 4, changePassword = 5;
var emailType;

function sendChangePassword() {
    if (userId != '') {
        emailType = changePassword;
        registerGridPopup("The password for this account is expired. An email will be sent to reset the password.", "OK", null, closeRecoverPasswordPopup);
    }
}
$(".field-validation-error").show();
function GotoRecoverPasswordLink() {
    var userName = $.trim($("#email-username").val());
    if (userName != "") {
        var modelUser = { "userName": userName };
        ajaxRequestHandler({
            type: "POST",
            dataType: "json",
            url: SSIUrl.isUserEmailExistLockedOrNot,
            data: JSON.stringify(modelUser),
            fnSuccess: OnUserNameExistSuccess
        });
    } else {
        $(".field-validation-error").hide();
        $("#lbl-error-message").show();
        $("#lbl-error-message").html('The email field is required.');
    }
}
//Ajax call returns UserId.
function OnUserNameExistSuccess(json) {
    userId = json;
    if (userId > 0) {
        if (userId != userRole.SsiAdmin) {
            emailType = recoverPassword;
            $("#lbl-error-message").hide();
            $(".field-validation-error").hide();
            registerGridPopup("An email will be sent for resetting the password. Do you wish to continue?", "Yes", "No", closeRecoverPasswordPopup);
        }
    } else if (userId == -1) {
        $(".field-validation-error").hide();
        $("#lbl-error-message").show();
        $("#lbl-error-message").html('Invalid email ID.');
    }
    else if (userId == -2) {//-2 means account not complete
        $(".field-validation-error").hide();
        $("#lbl-error-message").hide();
        registerGridPopup("The account setup is not complete. Please contact the adminstrator.", "OK");
    } else {
        $(".field-validation-error").hide();
        $("#lbl-error-message").hide();
        registerGridPopup("The user account has been locked. Please contact your administrator.", "OK");
    }
}

function closeRecoverPasswordPopup() {
    var userAccountData = {
        UserId: userId, EmailType: emailType, RequestedUserName: $.trim($("#email-username").val())
    }
    //Ajax call to Save Account Setting
    ajaxRequestHandler({
        url: SSIUrl.saveRecoverPassword,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(userAccountData)
    });
}

//gets the current date time in the string format
function getCurrentDateTime() {
    var currentDate = new Date();
    var date = addZero(currentDate.getDate());
    var month = addZero(currentDate.getMonth() + 1);
    var year = currentDate.getFullYear().toString();
    var hours = addZero(currentDate.getHours());
    var minutes = addZero(currentDate.getMinutes());
    var seconds = addZero(currentDate.getSeconds());
    var entireDate = year + month + date + hours + minutes + seconds;
    return entireDate;
}

//appends zero to the date time values if it ends with a single digit
function addZero(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i.toString();
}
