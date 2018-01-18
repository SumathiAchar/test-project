$(document).ready(function () {
    if (userId == validateToken.InvalidToken || userId == validateToken.TokenExpired) {
        $('#div-email-validation').hide();
        $("header").hide();
        $('#div-invalid-token').show();
    } else {
        $('#div-invalid-token').hide();
    }
    changeTitle(emailType);
    $("#ddl-continue").click(function () {
        var userName = $.trim($("#ddl-email-id").val());
        var filter = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
        if (userName != '') {
            if (!filter.test(userName)) {
                registerGridPopup("Invalid email address.", "OK", null, null);
            } else {
                var modelUser = { "userName": userName };
                ajaxRequestHandler({
                    type: "POST",
                    dataType: "json",
                    url: SSIUrl.isUserEmailExist,
                    data: JSON.stringify(modelUser),
                    fnSuccess: OnIsUserNameExistSuccess
                });
            }
        } else {
            registerGridPopup("Please provide email address.", "OK", null, null);
        }
    });

    function OnIsUserNameExistSuccess(json) {
        if (json > 0) {
            email = $("#ddl-email-id").val();
            SaveAndContinue(json);
        }
        else {
            registerGridPopup("Invalid email address.", "OK", null, null);
        }
    }

    function SaveAndContinue(newUserId) {
        if (newUserId != null && newUserId == userId) {
            $('#div-email-validation').hide();
            $('#my-account').html('');
            $('#my-account').show();
            $('#my-account').load(SSIUrl.showSecurityQuestionsAndPassword);
        } else {
            registerGridPopup("Invalid email address.", "OK", null, null);
        }
    }

    function changeTitle(emailType) {
        var title;
        switch (emailType) {
            case userAccountSetting.AccountActivation.toString():
                title = "Create New Account";
                break;
            case userAccountSetting.AccountReset.toString():
                title = "Reset Account";
                break;
            case userAccountSetting.ChangePassword.toString():
            case userAccountSetting.PasswordReset.toString():
            case userAccountSetting.RecoverPassword.toString():
                title = "Account Setup";
                break;
        }
        $("#title").html(title);
    }
});
