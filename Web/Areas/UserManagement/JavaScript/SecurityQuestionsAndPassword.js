var paragraphLength = "Minimum 8 Character Required.";
var paragraphOne = "Uppercase Character Required.";
var paragraphTwo = "Lowercase Character Required.";
var paragraphThree = "Special Character Required(!@#$%?&_).";
var paragraphFour = "Number Required.";
var paragraphFive = "New Password Should Match Confirmed Password.";
var paragraphSix = "New Password Cannot Be Your Email Address.";
var correct = "&#x2713;"
var wrong = "&#10007;";
var isResetPassword = false;
var redColor = "rgb(255, 0, 0)";
var changePasswordText = "<br />Security questions and answers are a method of authentication which seeks to prove the identity<br> of someone accessing their online account. " +
    "If you forget your password or want to change password,<br> you will be asked to provide <b id='lbl-last-line'>an answer to two of your security questions</b>.<br /><br />";
var changeLabelText = "Account Setup:";
$(document).ready(function () {
    $("#txt-hidden-username").val(email);
    var questionData;
    $("#ddl-second-question").kendoDropDownList({});
    $("#ddl-third-question").kendoDropDownList({});
    $("#btn-user-save").attr("disabled", "disabled");

    //Getting all question from db
    GetAllSecurityQuestion();

    if (emailType == userAccountSetting.ChangePassword.toString() || emailType == userAccountSetting.PasswordReset.toString() || emailType == userAccountSetting.RecoverPassword.toString()) {
        $("#div-security-para").html(changePasswordText);
        $("#lbl-change").html(changeLabelText);
        $(".cms-required-fields").html("");
    }
    $("#txt-hidden-username").css('visibility', 'hidden');
    // Enabling Confirm password text box when user enters in  new password text box.
    $("#newPassword").keyup(function () {
        ValidatePasswordColors();
        data = $('#newPassword').val();
        if (data != "") {
            ValidateTextColors();
        }
    });

    // Enabling Save button when user enters in  confirm password text box.
    $("#confirmPassword").keyup(function () {
        data = $('#newPassword').val();
        if (!ValidateConfirmPassword(data)) {
            $("#btn-user-save").removeAttr('disabled');
        }
        ValidateTextColors();
    });

    function ValidateTextColors() {
        var color = $('#paragraph-length').css('color');
        var colorOne = $('#paragraph-one').css('color');
        var colorTwo = $('#paragraph-two').css('color');
        var colorThree = $('#paragraph-three').css('color');
        var colorFour = $('#paragraph-four').css('color');
        var colorFive = $('#paragraph-five').css('color');
        var colorSix = $('#paragraph-six').css('color');
        if ((color == redColor) || (colorOne == redColor) || (colorTwo == redColor) || (colorThree == redColor) || (colorFour == redColor) || (colorFive == redColor) || (colorSix == redColor)) {
            $("#btn-user-save").attr("disabled", "disabled");
        }
    }

    function ValidatePasswordColors() {
        data = $('#newPassword').val();
        if (ValidatePassword(data) || data == "") {
            $("#confirmPassword").val('');
            $("#cross-five").html(wrong);
            $("#paragraph-five").css({ "color": "red" });
            $("#tick-five").html("");
            $("#btn-user-save").attr("disabled", "disabled");
        } else {
            $("#confirmPassword").removeAttr('disabled');
            if (!ValidateConfirmPassword(data)) {
                $("#btn-user-save").removeAttr('disabled');
            }
        }
    }

    //On click on cancel button 
    $('#btn-user-cancel').off('click').on('click', function () {
        registerGridPopup("Your changes have not been saved. Do you wish to continue?", "Yes", "No", navigateToLoginPage);
    });

    function navigateToLoginPage() {
        window.location.reload();
    }

    //Save all value to database
    $("#btn-user-save").click(function () {
        var questionValidation = false;
        if ((emailType == userAccountSetting.AccountActivation.toString() || emailType == userAccountSetting.AccountReset.toString()) && ($("#ddl-answer-three").val() == "" || $("#ddl-answer-two").val() == ""
            || $("#ddl-answer-one").val() == "" || $("#newPassword").val() == "" || $("#confirmPassword").val() == "")) {
            questionValidation = true;

        } else if (($("#ddl-answer-one").val() == "" && $("#ddl-answer-two").val() == "") || ($("#ddl-answer-one").val() == "" && $("#ddl-answer-three").val() == "")
        || ($("#ddl-answer-two").val() == "" && $("#ddl-answer-three").val() == "") || $("#newPassword").val() == "" || $("#confirmPassword").val() == "") {
            questionValidation = true;
        }
        else if ($("#ddl-first-question").val() == "" || $("#ddl-second-question").val() == "" || $("#ddl-third-question").val() == "") {
            questionValidation = true;
        }

        if (questionValidation) {
            registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
        }
        else {
            var securityQuestion1 = $("#ddl-first-question").val();
            var securityQuestion2 = $("#ddl-second-question").val();
            var securityQuestion3 = $("#ddl-third-question").val();
            var answer1 = $("#ddl-answer-one").val();
            var answer2 = $("#ddl-answer-two").val();
            var answer3 = $("#ddl-answer-three").val();
            var passwordNew = $("#newPassword").val();
            var question = securityQuestion1 + "," + securityQuestion2 + "," + securityQuestion3;
            var answer = answer1 + "," + answer2 + "," + answer3;
            var questionandanswer = [];
            questionandanswer.push({ QuestionId: securityQuestion1, Answer: answer1 });
            questionandanswer.push({ QuestionId: securityQuestion2, Answer: answer2 });
            questionandanswer.push({ QuestionId: securityQuestion3, Answer: answer3 });
            var model = { UserId: userId, PasswordHash: passwordNew, userSecurityQuestion: questionandanswer, EmailType: emailType, RequestedUserName: email };
            ajaxRequestHandler({
                url: SSIUrl.saveQuestionAnswerAndPassword,
                type: "POST",
                cache: false,
                dataType: "json",
                data: JSON.stringify({ 'userView': model }),
                fnSuccess: OnSuccessOfSecurityQuestionAnswer
            });
        }
    });
});

function OnSuccessOfSecurityQuestionAnswer(result) {
    if (result != null) {
        if (result == isSavePassword.SaveSuccess) {
            AccountActivationLogin();
        } else if (result == isSavePassword.SecurityAnswerFailed) {
            registerGridPopup("Please provide correct answers for at least two security questions.", "OK", null, null);
        } else if (result == isSavePassword.PasswordExists) {
            registerGridPopup("This password was recently used. Please try another.", "OK", null, null);
            ClearPasswordAndConfirmPassword();
        } else {
            registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
        }
    }
};

//after success of Password reset or Set, then user has to login automatically.
function AccountActivationLogin() {
    var userData = { UserName: email, Password: $("#newPassword").val() }
    ajaxRequestHandler({
        url: SSIUrl.accountActivationLogin,
        type: "POST",
        dataType: "html",
        data: JSON.stringify({ model: userData, currentDateTime: getCurrentDateTime() }),
        fnSuccess: onLoginSuccess
    });
}
function onLoginSuccess() {

    window.location.href = SSIUrl.showContractManagement
}
//Getting all question from database
function GetAllSecurityQuestion() {
    ajaxRequestHandler({
        url: SSIUrl.getSecurityQuestion,
        type: "POST",
        dataType: "json",
        data: JSON.stringify({ userId: userId }),
        fnSuccess: onGetAllFirstQuestionSuccess
    });
}

//Getting all data in object
function onGetAllFirstQuestionSuccess(json) {
    questionData = json;
    GetFirstOrDefaultQuestion();
}

//Getting first and Default drop down value without filtering security question
function GetFirstOrDefaultQuestion() {
    var importFirstQuestionData = [];
    if (questionData != null && questionData.tableNames != null) {
        $.each(questionData.tableNames, function (i, item) {
            importFirstQuestionData.push({
                'Text': item.Question,
                'Value': item.QuestionId
            });
        });
    }
    if (importFirstQuestionData.length > 1) {
        $("#ddl-first-question").kendoDropDownList({
            optionLabel: "Select First Question",
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: importFirstQuestionData,
            change: onChangeFirstQuestion
        });

        $("#ddl-second-question").kendoDropDownList({
            optionLabel: "Select Second Question",
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: importFirstQuestionData
        });
        $("#ddl-third-question").kendoDropDownList({
            optionLabel: "Select Third Question",
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: importFirstQuestionData
        });

        if (emailType == userAccountSetting.PasswordReset.toString() || emailType == userAccountSetting.RecoverPassword.toString()
            || emailType == userAccountSetting.ChangePassword.toString()) {
            if (questionData.tableNames[0].QuestionIds != null) {
                isResetPassword = true;
                $("#ddl-first-question").data('kendoDropDownList').value(questionData.tableNames[0].QuestionIds[0]);
                $("#ddl-second-question").data('kendoDropDownList').value(questionData.tableNames[0].QuestionIds[1]);
                $("#ddl-third-question").data('kendoDropDownList').value(questionData.tableNames[0].QuestionIds[2]);
                $("#ddl-first-question").data("kendoDropDownList").enable(false);
                $("#ddl-second-question").data("kendoDropDownList").enable(false);
                $("#ddl-third-question").data("kendoDropDownList").enable(false);
                $("#ddl-answer-one").attr('disabled', false);
                $("#ddl-answer-two").attr('disabled', false);
                $("#ddl-answer-three").attr('disabled', false);
            }
        }
    }
}

//Onchange function for enabling answer text box
function onChangeFirstQuestion() {
    $("#ddl-answer-one").val('');
    $("#ddl-answer-one").removeAttr('disabled');
    GetAllSecondQuestion();
    GetFirstQuestion();
    GetAllThirdQuestion();
}

//Getting Second question with filtering first question and third question
function GetAllSecondQuestion() {
    var importSecondQuestionData = [];
    if (questionData != null && questionData.tableNames != null) {
        $.each(questionData.tableNames, function (i, item) {
            if (questionData.tableNames[i].QuestionId != $("#ddl-first-question").data("kendoDropDownList").value() && questionData.tableNames[i].QuestionId != $("#ddl-third-question").data("kendoDropDownList").value()) {
                importSecondQuestionData.push({
                    'Text': item.Question,
                    'Value': item.QuestionId
                });
            }
        });
    }
    if (importSecondQuestionData.length > 1) {
        $("#ddl-second-question").kendoDropDownList({
            optionLabel: "Select Second Question",
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: importSecondQuestionData,
            change: onChangeSecondQuestion
        });
    }
}

//Onchange function for enabling answer text box
function onChangeSecondQuestion() {
    $("#ddl-answer-two").val('');
    $("#ddl-answer-two").removeAttr('disabled');
    GetAllThirdQuestion();
    GetAllSecondQuestion();
    GetFirstQuestion();
}

//Getting third question with filtering second question and first question
function GetAllThirdQuestion() {
    //In success function bind data into combobox using kendo
    var importThirdQuestionData = [];
    if (questionData != null && questionData.tableNames != null) {
        $.each(questionData.tableNames, function (i, item) {
            if (questionData.tableNames[i].QuestionId != $("#ddl-first-question").data("kendoDropDownList").value() && questionData.tableNames[i].QuestionId != $("#ddl-second-question").data("kendoDropDownList").value()) {
                importThirdQuestionData.push({
                    'Text': item.Question,
                    'Value': item.QuestionId
                });
            }
        });
    }
    if (importThirdQuestionData.length > 1) {
        $("#ddl-third-question").kendoDropDownList({
            optionLabel: "Select Third Question",
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: importThirdQuestionData,
            change: onChangeThirdQuestion
        });
    }
}

//Onchange function for enabling answer text box
function onChangeThirdQuestion() {
    $("#ddl-answer-three").val('');
    $("#ddl-answer-three").removeAttr('disabled');
    GetFirstQuestion();
    GetAllThirdQuestion();
    GetAllSecondQuestion();
}

//Getting first question with filtering second question and third question
function GetFirstQuestion() {
    var importFirstQuestionData = [];
    if (questionData != null && questionData.tableNames != null) {
        $.each(questionData.tableNames, function (i, item) {
            if (questionData.tableNames[i].QuestionId != $("#ddl-second-question").data("kendoDropDownList").value() && questionData.tableNames[i].QuestionId != $("#ddl-third-question").data("kendoDropDownList").value()) {
                importFirstQuestionData.push({
                    'Text': item.Question,
                    'Value': item.QuestionId
                });
            }
        });
    }
    if (importFirstQuestionData.length > 1) {
        $("#ddl-first-question").kendoDropDownList({
            optionLabel: "Select First Question",
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: importFirstQuestionData,
            change: onChangeFirstQuestion
        });
    }
}

//Validate conditions on new password testbox
function ValidatePassword(data) {
    if (data.length < 8) {
        $("#cross-length").html(wrong);
        $("#paragraph-length").css({ "color": "red" });
        $("#tick-length").html("");
    }
    else {
        $("#cross-length").html("");
        $("#paragraph-length").css({ "color": "green" });
        $("#tick-length").html(correct);
    }
    if (!data.match(/[A-Z]/)) {
        $("#cross-one").html(wrong);
        $("#paragraph-one").css({ "color": "red" });
        $("#tick-one").html("");
    }
    else {
        $("#cross-one").html("");
        $("#paragraph-one").css({ "color": "green" });
        $("#tick-one").html(correct);
    }
    if (!data.match(/[a-z]/)) {
        $("#cross-two").html(wrong);
        $("#paragraph-two").css({ "color": "red" });
        $("#tick-two").html("");
    }
    else {
        $("#cross-two").html("");
        $("#paragraph-two").css({ "color": "green" });
        $("#tick-two").html(correct);
    }
    if (!data.match(/[!,@,#,$,%,?,_]/)) {
        $("#cross-three").html(wrong);
        $("#paragraph-three").css({ "color": "red" });
        $("#tick-three").html("");
    }
    else {
        $("#cross-three").html("");
        $("#paragraph-three").css({ "color": "green" });
        $("#tick-three").html(correct);
    }
    if (!data.match(/[0-9]/)) {
        $("#cross-four").html(wrong);
        $("#paragraph-four").css({ "color": "red" });
        $("#tick-four").html("");
    }
    else {
        $("#cross-four").html("");
        $("#paragraph-four").css({ "color": "green" });
        $("#tick-four").html(correct);
    }
    if ((data.toLowerCase() == email.toLowerCase()) || data == "") {
        $("#cross-six").html(wrong);
        $("#paragraph-six").css({ "color": "red" });
        $("#tick-six").html("");
    }
    else {
        $("#cross-six").html("");
        $("#paragraph-six").css({ "color": "green" });
        $("#tick-six").html(correct);
    }
};

//Validate conditions on confirm password testbox
function ValidateConfirmPassword(data) {
    if (data != $('#confirmPassword').val() || $('#confirmPassword').val() == "") {
        $("#cross-five").html(wrong);
        $("#paragraph-five").css({ "color": "red" });
        $("#tick-five").html("");
    }
    else {
        $("#cross-five").html("");
        $("#paragraph-five").css({ "color": "green" });
        $("#tick-five").html(correct);
    }
}

function ClearPasswordAndConfirmPassword() {
    $("#paragraph-length").css({ "color": "red" });
    $("#paragraph-one").css({ "color": "red" });
    $("#paragraph-two").css({ "color": "red" });
    $("#paragraph-three").css({ "color": "red" });
    $("#paragraph-four").css({ "color": "red" });
    $("#paragraph-five").css({ "color": "red" });
    $("#paragraph-six").css({ "color": "red" });
    $("#cross-length").html(wrong);
    $("#cross-one").html(wrong);
    $("#cross-two").html(wrong);
    $("#cross-three").html(wrong);
    $("#cross-four").html(wrong);
    $("#cross-five").html(wrong);
    $("#cross-six").html(wrong);
    $("#tick-length").html("");
    $("#tick-one").html("");
    $("#tick-two").html("");
    $("#tick-three").html("");
    $("#tick-four").html("");
    $("#tick-five").html("");
    $("#tick-six").html("");
    $('#confirmPassword').val("");
    $("#newPassword").val("");
    $("#btn-user-save").attr("disabled", "disabled");
}

function ShowPasswordTextBox() {
    if (emailType == userAccountSetting.ChangePassword.toString() || emailType == userAccountSetting.PasswordReset.toString() || emailType == userAccountSetting.RecoverPassword.toString()) {
        if (($("#ddl-answer-one").val() != "" && $("#ddl-answer-two").val() != "") || ($("#ddl-answer-two").val() != "" && $("#ddl-answer-three").val() != "") || ($("#ddl-answer-three").val() != "" && $("#ddl-answer-one").val() != "")) {
            $("#newPassword").removeAttr('disabled');
        }
    }
}