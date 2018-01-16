var viewModelUserSetting, initialViewModelUserSetting;
$(document).ready(function () {
    $("#ddlUserSetting").kendoDropDownList({});
    var userSetting = [];
    if (cmsTabs != null) {
        $.each(cmsTabs, function (text, value) {
            userSetting.push({ 'Text': text, 'Value': value });
        });
    }
    $("#ddlUserSetting").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: userSetting,
        change: onChangeUserSetting
    });
});
function onChangeUserSetting() {
    isUserSettingChanged = viewModelUserSetting.LandingPageId != initialViewModelUserSetting.LandingPageId ? true : false;
}
$("#btnUserSettingSave").off('click').on('click', function () {
    saveUserSetting();
    return false;
});
function saveUserSetting() {
    var modelData = { 'UserId': loggedInUserId, 'LandingPageId': $("#ddlUserSetting").val() };
    ajaxRequestHandler({
        url: SSIUrl.SaveUserLandingPage,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(modelData),
        fnSuccess: saveUserSettingSuccess
    });
}
function saveUserSettingSuccess(json) {
    isUserSettingSavedSuccessfully = true;
    isUserSettingChanged = false;
    $("#divUserSetting").closest(".k-window-content").data("kendoWindow").close();
}

$("#btnUserSettingCancel").off('click').on('click', function () {
    $("#divUserSetting").closest(".k-window-content").data("kendoWindow").close();
});
