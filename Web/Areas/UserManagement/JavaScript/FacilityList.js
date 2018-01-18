var isProceeded = false;
var facilityId = 0;
var isFacilitySavedSuccessfully = false;
var facilityHeading = '';
var initialViewModel;
var initialFeatureControlVM;
var isSSIAdmin = true;

$(document).ready(function () {
    // Save update facility details
    $("#btn-add-facility").unbind("click").bind("click", function (e) {
        $("#create-facility-popup").empty();
        facilityId = 0;
        isFacilitySavedSuccessfully = false;
        // Add edit facility popup
        addEditFacility(facilityId);
    });
    //Bind facility grid while changing hide disabled checkbox
    $('#ckhhidedisabled').change(function () {
        bindFacilityGrid();
    });

    //Checking role is ssi admin
    if (userTypeId != userRoles.SsiAdmin) {
        isSSIAdmin = false;
    }

    // Load Facility details
    bindFacilityGrid();
    if (!isSSIAdmin) {
        $("#btn-add-facility").hide();
    }

});

//Delete facility details
function deleteSelectedFacility() {
    var facilityData = { 'facilityId': facilityId };

    var grid = $("#facility-grid").data("kendoGrid");
    var selectedRow = grid.select();
    rowIndex = selectedRow[0].rowIndex;

    //Ajax call to delete facility details
    ajaxRequestHandler({
        url: SSIUrl.deleteFacility,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify(facilityData),
        fnSuccess: OnDeleteLetterTemplateSuccess,
    });
}
//Delete facility success callback funtion
function OnDeleteLetterTemplateSuccess() {
    $("#facility-grid").find('.k-grid-content').removeClass("k-state-disabled");
    var grid = $("#facility-grid").data("kendoGrid");
    var dataFacility = grid._data[rowIndex];
    grid.dataSource.remove(dataFacility);
    $('#facility-grid').data('kendoGrid').dataSource.page($('#facility-grid').data('kendoGrid').dataSource.page());
}
function addEditFacility(facilityId) {
    var popupHeight = "385px";
    if (userTypeId == userRoles.CmAdmin) {
        popupHeight = "345px";
    }
    $("#create-facility-popup").kendoWindow({
        content: { url: SSIUrl.addEditFacility + '?facilityId=' + facilityId },
        draggable: false,
        modal: true,
        width: "800px",
        height: popupHeight,
        position: {
            top: "12%"
        },
        resizable: false,
        actions: [
            "Close"
        ],
        visible: false,
        close: onCloseWithoutSave
    });
    var newTemplateWindow = $("#create-facility-popup").data('kendoWindow');
    // Modify add/edit facility popup heading
    if (facilityId == 0) {
        facilityHeading = "Add Facility";
    }
    else {
        facilityHeading = "Edit Facility";
    }
    newTemplateWindow.center().open().title(facilityHeading).wrapper.css({
        width: "800px",
        height: popupHeight
    });
}

function onCloseWithoutSave(e) {
    if (isProceeded == false) {
        e.preventDefault();
        // Checking any changes appened in add edit facility scren. Comparing initial facility viewmodel with updated facility viewmodel
        if (isFacilitySavedSuccessfully == false && ((JSON.stringify(initialViewModel) !== JSON.stringify(viewmodelFacility))
                || (JSON.stringify(initialFeatureControlVM) !== JSON.stringify(viewmodelFacility.FacilityFeatureControl))
        )) {
            registerGridPopup("Your changes have not been saved. Do you wish to continue?", "Yes", "No", closetheFacility);
        } else {
            //Close add/edit facility popup
            closetheFacility();
        }
    } else {
        isProceeded = false;
    }

    // function to close facility popup
    function closetheFacility() {
        isProceeded = true;
        var closeContinue = $("#create-facility-popup").data("kendoWindow");
        closeContinue.close();
        isFacilitySavedSuccessfully = false;
    }
}

function bindFacilityGrid() {
    var tempLoggedInUserId = "0";
    if (userTypeId != userRoles.SsiAdmin) {
        tempLoggedInUserId = loggedInUserId;
    }
    var facilityData = { "isActive": $('#ckhhidedisabled').prop('checked'), "userId": tempLoggedInUserId }

    $('#facility-grid').kendoGrid({
        selectable: 'row',
        columns: [
            {
                field: "FacilityName",
                title: "Facility Name",
                width: "300px"
            },
            {
                field: "SsiNo",
                title: "SSI #"
            },
            {
                field: "City",
                title: "City"
            },
            {
                field: "StateId",
                title: "State"
            },
            {
                field: "NoofUsers",
                title: "Number of users"
            },
            {
                field: "IsActive",
                title: "Status",
                template: (function (dataItem) {
                    if (dataItem.IsActive) {
                        return "Enabled";
                    } else {
                        return "Disabled";
                    }
                })
            },
            {
                title: "Actions",
                width: "100px",
                template: kendo.template($("#facility-template").html())
            }
        ],
        pageable: {
            input: true,
            numeric: false
        },
        dataSource: {
            serverPaging: true,
            sort: { field: "FacilityName", dir: "asc" },
            pageSize: gridPageSize,
            schema: {
                data: function (json, operation) {
                    return json.Data;
                },
                total: "Total",
                model: {
                    id: "FacilityId",
                    fields: {
                        FacilityId: { type: "number" },
                        FacilityName: { type: "string" },
                        SsiNo: { type: "string" },
                        City: { type: "string" },
                        StateId: { type: "string" },
                        NoofUsers: { type: "number" },
                        IsActive: { type: "boolean" }
                    }
                },
            },
            transport: {
                read: {
                    url: SSIUrl.getFacilityList,
                    contentType: "application/json",
                    type: "POST", //use HTTP POST request as by default GET is not allowed by ASP.NET MVC
                    data: facilityData,
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

    //facility grid databound event
    function dataBound(e) {
        //Delete Facility
        $("#facility-grid").find('.k-grid-cancel').off('click').on('click', function () {
            var checkDisabled = $(".k-grid-content").find('.k-state-disabled');
            if (checkDisabled.length == '0') {
                $("#facility-grid").find('.k-grid-content').addClass("k-state-disabled");
                facilityId = $(this).attr('data-facilityId');
                registerGridPopup("Are you sure you want to delete the selected facility?", "Yes", "No", deleteSelectedFacility);
                $("#facility-grid").find('.k-grid-content').removeClass("k-state-disabled");
            }
        });

        //Edit Facility
        $("#facility-grid").find('.k-grid-edit').off('click').on('click', function () {
            $("#create-facility-popup").html('');
            //FIXED-RAGINI-FEB16 - addClass("k-state-disabled"); is used twice in this method.
            //addClass("k-state-disabled"); from next line can be removed
            facilityId = $(this).attr('data-facilityid');
            addEditFacility(facilityId);
        });

        //View manage user group
        $("#facility-grid").find('.k-grid-preview').off('click').on('click', function () {
            var checkDisabled = $(".k-grid-content").find('.k-state-disabled');
            if (checkDisabled.length == '0') {
                facilityId = $(this).attr('data-facilityid');
                $("#cms-facilities").removeClass('k-state-admin-selected');
                $("#cms-users").addClass('k-state-admin-selected');
                $('#tool-container-left').html('');
                $('#tool-container-left').show();
                $('#tool-container-left').load(SSIUrl.getUsers + "?facilityId=" + facilityId);
            }
        });
    }
}