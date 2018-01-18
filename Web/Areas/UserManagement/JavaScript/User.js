var facilityId = 0;
var isProceeded = false;
var userId = 0;
var isUserSavedSuccessfully = false;
var editTemplateName = '';
var initialViewModel;
var SelectedFacilityViewModel;
var url;
var newTemplateWindow;
$(document).ready(function () {
    isEditUser = false;
    $("#btn-add-user").off('click').on('click', function () {
        $("#create-user-popup").html('');
        $("#create-user-popup").empty();
        userId = 0;
        addEditUser(userId);
    });
    getAllFacilities();
    onHideAddUserUI();
});
function addEditUser(userId) {
    $("#create-user-popup").kendoWindow({
        content: { url: SSIUrl.addUserUI + '?UserId=' + userId + '&LoggedInUserId=' + loggedInUserId },
        draggable: false,
        modal: true,
        width: "520px",
        height: "364px",
        actions: [
        "Close"
        ],
        position: {
            top: "12%"
        },
        resizable: false,
        visible: false,
        close: onCloseWithoutSave
    });
    newTemplateWindow = $("#create-user-popup").data('kendoWindow');
    //Setting size parmeters for Create Template window
    if (userId == 0) {
        editTemplateName = "Add User";
    }
    else {
        isEditUser = true;
        editTemplateName = "Edit User";
    }
    newTemplateWindow.center().open().title(editTemplateName).wrapper.css({
        width: "520px",
        height: "364px"
    });
}
function onCloseWithoutSave(e) {
    if (isProceeded == false) {
        e.preventDefault();
        // Checking any changes appened in add edit user scren. Comparing initial user viewmodel with updated user viewmodel
        if (isUserSavedSuccessfully == false && validateSaveChanges()) {
            registerGridPopup("Your changes have not been saved. Do you wish to continue?", "Yes", "No", closetheUser);
        } else {
            //Close add/edit user popup
            closetheUser();
        }
    } else {
        isProceeded = false;
    }
    // function to close user popup
    function closetheUser() {
        isProceeded = true;
        isEditUser = false;
        isUserSavedSuccessfully = false;
        var closeContinue = $("#create-user-popup").data("kendoWindow");
        closeContinue.close();
    }
}
function validateSaveChanges() {
    var returnValue = false;
    if ((JSON.stringify(initialViewModel.UserName) !== JSON.stringify(userViewModel.UserName)) || (JSON.stringify(initialViewModel.FirstName) !== JSON.stringify(userViewModel.FirstName))
        || (JSON.stringify(initialViewModel.LastName) !== JSON.stringify(userViewModel.LastName)) || (JSON.stringify(initialViewModel.MiddleName) !== JSON.stringify(userViewModel.MiddleName))
        || (JSON.stringify(initialViewModel.IsLocked) !== JSON.stringify(userViewModel.IsLocked)) || (JSON.stringify(initialViewModel.IsAdmin) !== JSON.stringify(userViewModel.IsAdmin))) {
        return returnValue = true;
    }
    if (JSON.stringify(SelectedFacilityViewModel) !== JSON.stringify(userViewModel.SelectedFacilityList)) {
        return returnValue = true;
    }
    return returnValue;
}

function deleteSelectedTemp() {
    var modelData = { 'id': userId };
    ajaxRequestHandler({
        url: SSIUrl.deleteUser,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify(modelData),
        fnSuccess: OnDeleteUserSuccess,
    });
}

function OnDeleteUserSuccess() {
    $("#user-grid").find('.k-grid-content').removeClass("k-state-disabled");
    var grid = $("#user-grid").data("kendoGrid");
    var dataUserId = grid._data[rowIndex];
    grid.dataSource.remove(dataUserId);
    $('#user-grid').data('kendoGrid').dataSource.page($('#user-grid').data('kendoGrid').dataSource.page());
}
function dataBound(e) {
    $("#user-grid").find('.k-grid-cancel').off('click').on('click', function (e) {
        $("#user-grid").find('.k-grid-content').addClass("k-state-disabled");
        var grid = $("#user-grid").data("kendoGrid");
        var selectedRow = grid.select();
        rowIndex = selectedRow[0].rowIndex;
        var SelectedUserName = grid._data[rowIndex].UserName;
        userId = $(this).attr('data-userid');
        registerGridPopup("Are you sure you want to delete the user " + SelectedUserName + "?", "Yes", "No", deleteSelectedTemp);
        $("#user-grid").find('.k-grid-content').removeClass("k-state-disabled");
    });

    $("#user-grid").find('.k-grid-edit').off('click').on('click', function (e) {
        $("#create-user-popup").html('');
        $("#user-grid").find('.k-grid-content').addClass("k-state-disabled");
        userId = $(this).attr('data-userid');
        addEditUser(userId);
        $("#user-grid").find('.k-grid-content').removeClass("k-state-disabled");

    });
}
function getAllUser() {
    if ($("#listof-facility-field").data("kendoDropDownList").value() == "") {
        facilityId = 0;//0 - Means Default FacilityId when 
    } else {
        facilityId = $("#listof-facility-field").data("kendoDropDownList").value();
    }

    param = { "facilityId": facilityId, "userId": loggedInUserId };
    $('#user-grid').kendoGrid({
        selectable: 'row',
        columns: [
            {
                field: "FirstName",
                width: "150px",
                title: "First Name ",
            },
            {
                field: "LastName",
                width: "150px",
                title: "Last Name",
            },
            {
                field: "MiddleName",
                width: "150px",
                title: "Middle Name",
            },
             {
                 field: "UserName",
                 width: "230px",
                 title: "Email",
             },
             {
                 field: "IsAdmin",
                 width: "80px",
                 title: "Is Admin?",
                 template: (function (dataItem) {
                     if (dataItem.IsAdmin) {
                         return "Yes";
                     }
                     else {
                         return "No";
                     }
                 })
             },
            {
                field: "IsLocked",
                width: "80px",
                title: "Locked",
                template: (function (dataItem) {
                    if (dataItem.IsLocked) {
                        return "Yes";
                    }
                    else {
                        return "No";
                    }
                })
            },
            {
                title: "Actions",
                width: "70px",
                template: kendo.template($("#users-template").html())
            }
        ],
        pageable: {
            input: true,
            numeric: false
        },
        dataSource: {
            serverPaging: true,
            sort: { field: "UserName", dir: "asc" },
            pageSize: defaultPageSize,
            schema: {
                data: function (json, operation) {
                    return json.Data;
                },
                total: "Total",
                model: {
                    id: "UserId",
                    fields: {
                        UserId: { type: "number" },
                        FirstName: { type: "string" },
                        LastName: { type: "string" },
                        MiddleName: { type: "string" },
                        UserName: { type: "string" },
                        IsAdmin: { type: "bool" },
                        IsLocked: { type: "bool" }
                    }
                },
            },
            transport: {
                read: {
                    url: SSIUrl.getUserList,
                    contentType: "application/json",
                    data: param,
                    type: "POST",//use HTTP POST request as by default GET is not allowed by ASP.NET MVC
                    cache: false
                },
                parameterMap: function (data, operation) {
                    return JSON.stringify(data);
                },
            }
        },
        dataBinding: function () {
        },
        dataBound: dataBound
    });
}

function getAllFacilities() {
    param = { "UserId": loggedInUserId };
    ajaxRequestHandler({
        url: SSIUrl.getAllFacilityList,
        isAsync: false,
        dataType: "json",
        type: "POST",
        data: JSON.stringify(param),
        fnSuccess: OnGetFacilitySuccess
    });
}

function OnGetFacilitySuccess(json) {
    var facilityFeildDropDownListData = [];
    if (json != null) {
        $.each(json, function (i, item) {
            facilityFeildDropDownListData.push({ 'Text': json[i].FacilityName, 'Value': json[i].FacilityId });
        });
    }

    $("#listof-facility-field").kendoDropDownList({
        optionLabel: "Select Facility",
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: facilityFeildDropDownListData,
        change: onSelectedFaciltyUser
    });
    $("#listof-facility-field").data("kendoDropDownList").value(selectedFacilityId);
    function onSelectedFaciltyUser() {
        onHideAddUserUI();
    }
    if (selectedFacilityId != null) {
        $("#menu-list-facilities").parent().removeClass('k-state-admin-selected');
        $("#menu-list-users").parent().addClass('k-state-admin-selected');
        $("#menu-list-facilities").removeClass('k-state-admin-selected');
        $("#menu-list-users").addClass('k-state-admin-selected');
        $("#tool-container").css({ "background-image": "none" });
    }
}

function onHideAddUserUI() {
    $("#btn-add-user").show();
    $("#user-templates").show();
    $("#choosefacility").addClass("choose-facility");
    $("#user-grid").html('');
    getAllUser();
}
