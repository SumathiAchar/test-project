$(document).ready(function () {
    if (userId == 0 || isEditUser == false) { //0-means Default userId and AddUser Event Triggered
        $("#btn-password-reset").hide();
        $("#btn-accountreset").hide();
        $("#divAddUserSubmitButtons").removeClass("addedit-user-btn").addClass("add-user-btn");
    }
    else {
        $("#btn-password-reset").show();
        $("#btn-accountreset").show();
        $("#divAddUserSubmitButtons").removeClass("add-user-btn").addClass("addedit-user-btn");
        if (isPasswordResetLock == 'true') {
            $("#btn-password-reset").attr('disabled', 'disabled');
        }
    }

    $("#btn-submit-adduser").off('click').on('click', function () {
        if ($.trim($("#txt-first-name").val()) == "" || $.trim($("#txt-last-name").val()) == "" || $.trim($("#txt-email").val()) == "") {
            registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
        }
        else {
            if ($("#list-selected-facilities").html() == "") {
                registerGridPopup("Please select at least one facility.", "OK", null, null);
                return false;
            }
            else {
                saveUserDetails();
            }
        }
        isEditUser = false;
        return false;
    });
    $("#btn-cancel-adduser").off('click').on('click', function () {
        $("#divAddUserPopUp").closest(".k-window-content").data("kendoWindow").close();
        isEditUser = false;
    });

});
//Fixed-RAGINI-FEB16 Why global variables are used.Its very risky. If required use it in object oriented way
$("#facility-addtogrid").click(function () {
    var selectedps = '';
    var selectedNames = '';
    var selectedUId = '';
    var lvFacilityAvailable = $("#available-facilities").data("kendoListView");
    $("#btn-submit-adduser").removeAttr('disabled');
    var txt = $("#available-facilities").find('.k-state-selected');
    for (var i = 0; i < txt.length; i++) {
        selectedps += txt[i].value + "##";
        selectedNames += txt[i].textContent + "##";
        selectedUId += txt[i].getAttribute('data-uid') + "##";
    }
    selectedps = selectedps.substring(0, selectedps.length - 2);
    selectedNames = selectedNames.substring(0, selectedNames.length - 2);
    selectedUId = selectedUId.substring(0, selectedUId.length - 2);
    var splitedId = selectedps.split('##');
    var splitedName = selectedNames.split('##');
    var splittedUId = selectedUId.split('##');
    for (var j = 0; j < splitedId.length; j++) {
        if (splitedId != "" && selectedNames != "") {
            dataSourceSelectedFacility.add({ FacilityId: splitedId[j], FacilityName: splitedName[j] });
        }
        for (var k = 0; k < lvFacilityAvailable.items().length; k++) {
            if (splitedName[j] != "") {
                var dataToRemove = lvFacilityAvailable.dataSource.getByUid(splittedUId[j]);
                lvFacilityAvailable.dataSource.remove(dataToRemove);
                break;
            }
        }
    }
    selectedps = '';
    selectedNames = '';
    selectedUId = '';
});

// Move all the available payers to Selected payers listview
$("#facility-add-all-togrid").click(function () {
    $("#btn-submit-adduser").removeAttr('disabled');
    var selectedFacility = $("#available-facilities").data().kendoListView.dataSource._data;
    $.merge(selectedFacility, dataSourceSelectedFacility._data);
    dataSourceSelectedFacility.data(selectedFacility);
    $("#available-facilities").data('kendoListView').dataSource.data([]);
});

//Move all the selected payers to available payers listview
$("#btn-remove-all-facility").click(function () {
    $("#btn-submit-adduser").removeAttr('disabled');
    var selectedFacility = $("#list-selected-facilities").data().kendoListView.dataSource._data;
    $.merge(selectedFacility, dataSourceAvailableFacility._data);
    dataSourceAvailableFacility.data(selectedFacility);
    $("#list-selected-facilities").data('kendoListView').dataSource.data([]);
});

//Fixed-RAGINI-FEB16 Why global variables are used.Its very risky. If required use it in object oriented way
$("#btn-remove-facility").click(function () {
    var lvfacilitiesSelected = $("#list-selected-facilities").data("kendoListView");
    var rmvselectedps = '';
    var rmvselectedNames = '';
    var uIdToRemoveFacility = '';
    $("#btn-submit-adduser").removeAttr('disabled');
    var txtRmv = $("#list-selected-facilities").find('.k-state-selected');
    for (var i = 0; i < txtRmv.length; i++) {
        rmvselectedps += txtRmv[i].value + "##";
        rmvselectedNames += txtRmv[i].textContent + "##";
        uIdToRemoveFacility += txtRmv[i].getAttribute('data-uid') + "##";
    }
    rmvselectedps = rmvselectedps.substring(0, rmvselectedps.length - 2);
    rmvselectedNames = rmvselectedNames.substring(0, rmvselectedNames.length - 2);
    uIdToRemoveFacility = uIdToRemoveFacility.substring(0, uIdToRemoveFacility.length - 2);
    var rmvsplitedId = rmvselectedps.split('##');
    var rmvsplitedName = rmvselectedNames.split('##');
    var rmvsplitterUId = uIdToRemoveFacility.split('##');
    for (var j = 0; j < rmvsplitedId.length; j++) {
        dataSourceAvailableFacility.add({ FacilityId: rmvsplitedId[j], FacilityName: rmvsplitedName[j] });
        for (var k = 0; k < lvfacilitiesSelected.items().length; k++) {
            if (rmvsplitedName[j] != "") {
                var removeFacility = lvfacilitiesSelected.dataSource.getByUid(rmvsplitterUId[j]);
                lvfacilitiesSelected.dataSource.remove(removeFacility);
                break;
            }
        }
    }
    rmvselectedps = '';
    rmvselectedNames = '';
    uIdToRemoveFacility = '';
});
$("#facility-addtogrid").attr('disabled', 'disabled');
$("#btn-remove-facility").attr('disabled', 'disabled');
$("#available-facilities").click(function () {
    $("#facility-addtogrid").removeAttr('disabled');
    $("#list-selected-facilities").data("kendoListView").clearSelection();
});

$("#list-selected-facilities").click(function () {
    $("#btn-remove-facility").removeAttr('disabled');
    $("#available-facilities").data("kendoListView").clearSelection();
});

function saveUserDetails() {
    Facility = [];
    for (var i = 0; i < dataSourceSelectedFacility._data.length; i++) {
        Facility.push({ 'FacilityId': dataSourceSelectedFacility._data[i].FacilityId, 'FacilityName': dataSourceSelectedFacility._data[i].FacilityName });
    }
    userViewModel.AvailableFacilityList = []
    userViewModel.SelectedFacilityList = Facility
    ajaxRequestHandler({
        url: SSIUrl.saveUserDetails,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(userViewModel),
        fnSuccess: OnSaveUserDetailsSuccess
    });
}

//Fixed-RAGINI-FEB16 - Why magic integers are used , what does data.CmUserId==-1 and data.CmUserId==-2 means
function OnSaveUserDetailsSuccess(json) {
    if (json) {
        if (json.UserId == -1) { // -1 means email id already exists
            registerGridPopup("A user with entered email already exists. Please try another email.", "OK", null, null);
        }
        else if (json.UserId == -2) { // -2 means Invalid Domain name
            registerGridPopup("The email address is invalid for the selected facility(s). Please try another email address or contact your administrator.", "OK", null, null);
        }
        else {
            isUserSavedSuccessfully = true;
            $("#divAddUserPopUp").closest(".k-window-content").data("kendoWindow").close();
            isEditUser = false;
            if (userViewModel.UserId == "0") {
                $('#user-grid').data("kendoGrid").dataSource.page(1);// Go to first page
            } else {
                $('#user-grid').data('kendoGrid').dataSource.read();
            }
        }
    }
}

$("#btn-password-reset").off('click').on('click', function () {
    if ($.trim(userViewModel.UserName) != "") {
        userViewModel.EmailType = userAccountSetting.PasswordReset;
        registerGridPopup("Are you sure you want to reset the password for " + userViewModel.UserName + "?", "Yes", "No", AccountResetAndPasswordReset);
    } else {
        registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
    }
});
$("#btn-accountreset").off('click').on('click', function () {
    if ($.trim(userViewModel.UserName) != "") {
        userViewModel.EmailType = userAccountSetting.AccountReset;
        registerGridPopup("Are you sure you want to reset the account for " + userViewModel.UserName + "?", "Yes", "No", AccountResetAndPasswordReset);
    } else {
        registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
    }
});

// Account Reset Password Reset
function AccountResetAndPasswordReset() {
    var userAccountData = {
        UserId: userViewModel.UserId, EmailType: userViewModel.EmailType
    }
    //Ajax call to Save Account Setting
    ajaxRequestHandler({
        url: SSIUrl.saveUserAccountSetting,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(userAccountData),
        fnSuccess: OnSaveUserAccountSettingSuccess
    });
}

function OnSaveUserAccountSettingSuccess(data) {
    if (data) {
        if (userViewModel.EmailType == userAccountSetting.PasswordReset) {
            registerGridPopup("An email has been sent with a link to " + userViewModel.UserName + " for resetting the password.", "Ok");
        }
        else if (userViewModel.EmailType == userAccountSetting.AccountReset) {
            registerGridPopup("An email has been sent with a link to " + userViewModel.UserName + " for resetting the account.", "Ok");
        }
        $("#isLocked").attr('checked', 'checked');
        $('#user-grid').data('kendoGrid').dataSource.page($('#user-grid').data('kendoGrid').dataSource.page());
    }
}
