$(document).ready(function () {
    var backYear = new Date().getFullYear() - 3;
    var startDate = new Date(backYear - 14, 12, 01);
    var pastDate = new Date(backYear - 13, startDate.getMonth(), startDate.getDate(), 0, 0, 0, 0);
    var defaultDate = dateFormat(new Date(backYear + "/1/1"), "MM/DD/YYYY");

    $("#txtPasswordExpirationDays").css({ "width": "50px" });
    $("#txtPasswordAttempts").css({ "width": "50px" });
    // "0" Means Add Facility
    if (facilityId == '0') {
        $("#chkenable").prop('checked', true);;
        $("#txtPasswordExpirationDays").val(60);
        $("#txtPasswordAttempts").val(5);
        $("#txt-early-statement-date").val(defaultDate);
    } else {
        $("#ddlFacilityDataSource").attr('disabled', 'disabled');
    }

    // get active states
    getActiveStates();

    // Get Active facility bubble
    getFecilityBubble();

    lockFacilityDetails();

    $('.from_date').datepicker({
        todayBtn: 'linked',
        autoclose: true,
        weekStart: 1,
        startDate: pastDate,
        keyboardNavigation: false
    });

    $("#txt-early-statement-date").change(function () {
        validateEarlyStatement();
    });
    $("#txt-early-statement-date").focusout(function () {
        validateEarlyStatement();
    });
    $("#txt-early-statement-date").datepicker().on('show', function (ev) {
        var fromValidate = $("#txt-early-statement-date").val();
        $('#txt-early-statement-date').data({ date: fromValidate }).datepicker('update');
    });
    $("#txt-early-statement-date").datepicker().on('hide', function (ev) {
        validateEarlyStatement();
    });
    function validateEarlyStatement() {
        var fromValidate = $("#txt-early-statement-date").val();
        if (fromValidate != "" && fromValidate != "mm/dd/yyyy") {
            var getDate = $("#txt-early-statement-date").val();
            getDate = dateFormat(new Date(getDate), "MM/DD/YYYY");
            if (getDate == "" || getDate == "NaN/NaN/NaN") {
                getDate = defaultDate;
            } else if (new Date(getDate) < startDate) {
                getDate = defaultDate;
            }
            $("#txt-early-statement-date").val(getDate);
        }
    }
    if (!isSSIAdmin) {
        $("#txtPasswordExpirationDays").kendoNumericTextBox({
            min: 1,
            max: viewmodelFacility.PasswordExpirationDays,
            format: "#",
        });
        $("#txtPasswordAttempts").kendoNumericTextBox({
            min: 1,
            max: viewmodelFacility.InvalidPasswordAttempts,
            format: "#"
        });
        $('#txt-early-statement-date').attr('disabled', 'disabled');
    } else {
        $("#txtPasswordExpirationDays").kendoNumericTextBox({
            min: 1,
            max: 90,
            format: "#",
        });
        $("#txtPasswordAttempts").kendoNumericTextBox({
            min: 1,
            max: 5,
            format: "#"
        });
    }
    $("#btn-facility-cancel").off('click').on('click', function () {
        $("#container-facility-info").closest(".k-window-content").data("kendoWindow").close();
    });

    //FIXED-2016-R2-S3 : Create method and move code into method. Call method on required key events.
    //use method validateOnKey of common.js
    $("textarea").bind('keypress keyup', function (e) {
        validateOnKey(e, this);
    });

    $("#txt-domain").on('paste', function (e) {
        var value = $(this).val();
        validateOnKey(e, this);
        var domain = getClipboardData();
        if (value != "") {
            domain = domain + value;
        }
        if (domain.length > 200) {//200 means max length 
            e.preventDefault();
        }
    });

    $("#txt-phone").bind('keypress paste', function (e) {
        alphaNumericwithHypenCharacter(e);
    });

    $("#txt-fax").bind('keypress paste', function (e) {
        alphaNumericwithHypenCharacter(e);
    });

    $("#txt-ssi-numbers").keypress(function (e) {
        //if the letter is not digit then don't type anything
        if (e.which != 44 && e.which != 32 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
    $("#txt-ssi-numbers").bind('paste', function (e) {
        var ssiNumbers = $(this).val();
        validateOnKey(e, this);
        //if the letter is not digit then don't type anything
        if (e.which != 1 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        } else {
            var pastedData = getClipboardData(e);
            $.each(pastedData.split(","), function (index, value) {
                if (value != "" && !$.trim(value).match(/^\d+$/)) {
                    e.preventDefault();
                }
            });
            if (ssiNumbers != "") {
                pastedData = pastedData + ssiNumbers;
            }
            if (pastedData.length > 200) {//199 means max length 
                e.preventDefault();
            }
        }
    });

    $("#txt-zip").keypress(function (e) {
        //if the letter is not digit then don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
    $("#txt-zip").bind("paste", function (e) {
        //if the letter is not digit then don't type anything
        if (e.which != 1 && e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        } else {
            var zipValue = getClipboardData(e);
            if (!zipValue.match(/^\d+$/)) {
                return false;
            }
        }
    });

});

function alphaNumericwithHypenCharacter(e) {
    var regex = new RegExp("^[a-zA-Z0-9-]+$");
    var fax;
    if (e.type == 'paste') {
        fax = getClipboardData(e);
    } else {
        fax = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    }
    if (regex.test(fax)) {
        return true;
    }
    e.preventDefault();
    return false;
}

function getClipboardData(e) {
    var clipboardData;
    if (window.clipboardData && window.clipboardData.getData) { // IE
        clipboardData = window.clipboardData.getData('Text');
    }
    else if (e.originalEvent.clipboardData && e.originalEvent.clipboardData.getData) { // other browsers
        clipboardData = e.originalEvent.clipboardData.getData('text');
    }
    return clipboardData;
}
$("#btn-addEditFacility").off('click').on('click', function () {
    if ($.trim($("#txt-facility-name").val()) == "" || $.trim($("#txt-display-name").val()) == "" || $.trim($("#txt-ssi-numbers").val()) == ""
        || $.trim($("#txt-domain").val()) == "" || $.trim($("#ddlFacilityDataSource").val()) == "" || $.trim($("#txtPasswordExpirationDays").val()) == "0"
        || $.trim($("#txtPasswordExpirationDays").val()) == "" || $.trim($("#txtPasswordAttempts").val()) == ""
        || $.trim($("#txtPasswordAttempts").val()) == "0") {
        registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
        return false;
    }
    else {
        var validationFailed = false;
        var duplicateSSIcheck = false;
        var duplicateDomainName = false;
        var domainNames = $.trim($("#txt-domain").val()).split(",");
        //Domain Validation
        $.each(domainNames, function (index, value) {
            var reg = /[a-zA-Z0-9][a-zA-Z0-9-]{0,61}\.[a-zA-Z]{2,}/;
            if (!reg.test($.trim(value.replace(atSymbol, '')))) {
                validationFailed = true;
            }
        });
        var ssiNumbers = $.trim($("#txt-ssi-numbers").val()).split(",");
        //Checking SSI Number Duplication
        $.each(ssiNumbers, function (index, value) {
            $.each($.trim($("#txt-ssi-numbers").val()).split(","), function (index1, value1) {
                if ($.trim(value) == $.trim(value1) && index != index1 && value != '') {
                    validationFailed = true;
                    duplicateSSIcheck = true;
                }
            });
        });

        //Checking Domain name Duplication
        $.each(domainNames, function (index, value) {
            $.each($.trim($("#txt-domain").val()).split(","), function (index1, value1) {
                if ($.trim(value.replace(atSymbol, '')) == $.trim(value1.replace(atSymbol, '')) && index != index1 && value != '') {
                    validationFailed = true;
                    duplicateDomainName = true;
                }
            });
        });

        if (!validationFailed) {
            saveFacility();
        } else {
            if (duplicateSSIcheck) {
                registerGridPopup("Duplicate SSI numbers are not allowed.", "OK", null, null);

            } else if (duplicateDomainName) {
                registerGridPopup("Duplicate domain names are not allowed.", "OK", null, null);
            }
            else {
                registerGridPopup("Please provide valid domain name.", "OK", null, null);
            }
            validationFailed = true;
            return false;
        }
    }

});

//FIXED-RAGINI-FEB2016 - Why Get method is called via POST
function getActiveStates() {
    ajaxRequestHandler({
        url: SSIUrl.getActiveStates,
        dataType: 'json',
        type: "GET",
        fnSuccess: OnGetStateSuccess
    });
}
//FIXED-RAGINI-FEB2016 - Why Get method is called via POST
function getFecilityBubble() {
    ajaxRequestHandler({
        url: SSIUrl.GetBubbles,
        dataType: 'json',
        type: "GET",
        fnSuccess: OnGetFecilityBubbleSuccess
    });
}
function saveFacility() {
    viewmodelFacility.IsActive = $("#chkenable").prop('checked');
    viewmodelFacility.EarlyStatementDate = $("#txt-early-statement-date").val();
    viewmodelFacility.PasswordExpirationDays = $("#txtPasswordExpirationDays").val();
    viewmodelFacility.InvalidPasswordAttempts = $("#txtPasswordAttempts").val();

    ajaxRequestHandler({
        url: SSIUrl.saveFacility,
        dataType: 'json',
        type: "POST",
        data: JSON.stringify(viewmodelFacility),
        fnSuccess: OnSaveFacilitySuccess
    });
}

//FIXED-RAGINI-FEB16 - Do not use keywords as variable names
function OnSaveFacilitySuccess(facilityResult) {
    if (facilityResult != null) {
        if (facilityResult == "0") { // "0" means Facility Saved Successfully. //FIXED-RAGINI-FEB16 - what does == "0" means here
            isFacilitySavedSuccessfully = true;
            //$('.facilityTemp').closest(".k-window-content").data("kendoWindow").close();
            $("#container-facility-info").closest(".k-window-content").data("kendoWindow").close();
            if (viewmodelFacility.FacilityId == "0") {
                $('#facility-grid').data("kendoGrid").dataSource.page(1);// Go to first page
            } else {
                $('#facility-grid').data("kendoGrid").dataSource.read();
            }
        } else if (facilityResult == "-1") {// "-1" means facility name already exists.  //FIXED-RAGINI-FEB16 - what does == "-1" means here
            registerGridPopup("The facility name already exists. Please try another name.", "OK", null, null);
        } else if (facilityResult == "-2") { // "-2" means display name already exists.  //FIXED-RAGINI-FEB16 - what does == "-2" means here
            registerGridPopup("The display name for a facility already exists, Please try another one.", "OK", null, null);
        }
        else {//ssiNumners already exists
            var duplicateSsiNumber = $.unique(facilityResult.split(','));
            var ssiNumbers = duplicateSsiNumber.join(",");
            registerGridPopup("The SSI number " + ssiNumbers + " cannot be used as it is already allocated to a facility.", "OK", null, null);
        }
    }
}
//On success of State Success
function OnGetStateSuccess(json) {
    var stateList = [];
    //FIXED-RAGINI-FEB16 - create a css class in class file and use add or remove class method at runtime to change css
    //dont use inline styles
    $("#ddlfacilitystate").addClass("facility-state");
    stateList.push({ Text: '', Value: '' });
    $.each(json.states, function (index, value) {
        stateList.push({ Text: value, Value: value });
    });
    //Datasource for getting state values
    var statedatasource = new kendo.data.DataSource({ data: stateList });

    $("#ddlfacilitystate").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: statedatasource
    }).data("kendoDropDownList");

}

function OnGetFecilityBubbleSuccess(json) {

    var bubbleList = [];
    bubbleList.push({ Text: 'Select Data Source', Value: '' });
    $.each(json.bubbleList, function (index, value) {
        bubbleList.push({ Text: value.Description, Value: value.FacilityBubbleId });
    });

    //Datasource for getting Fecility Bubble
    var fecilityBubbleDatasource = new kendo.data.DataSource({ data: bubbleList });

    $("#ddlFacilityDataSource").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: fecilityBubbleDatasource
    }).data("kendoDropDownList");
}

//Lock facility fields if CMadmin logged in
function lockFacilityDetails() {
    if (!isSSIAdmin) {
        $("#txt-facility-name").prop('disabled', true);
        $("#txt-ssi-numbers").prop('disabled', true);
        $("#chkenable").prop('disabled', true);
        $("#divFacilityFeatures :input").attr("disabled", true);
        $(".bubble").hide();
    }
}