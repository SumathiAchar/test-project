var availablePayerList = null, selectedPayerList = null, dataSourceAvailablePayer = null, dataSourceSelectedPayer = null, contractViewModel = null, previousContractName = null, fromDateCheck = null, toDateCheck = null;
var oldContractName = "";
$(document).ready(function () {
    oldContractName = $("#txt-contract-name").val();
    dataSourceAvailablePayer = new kendo.data.DataSource({
        data: availablePayerList,
        sort: { field: "Name", dir: "asc" }
    });

    dataSourceSelectedPayer = new kendo.data.DataSource({
        data: selectedPayerList,
        sort: { field: "Name", dir: "asc" }
    });

    //Combobox for displaying available Payers
    $("#available-payers").kendoListView({
        selectable: "multiple",
        dataSource: dataSourceAvailablePayer,
        template: kendo.template($("#SeletedPayerstemplate").html())
    });

    //Listview for displaying the selected Payers
    $("#list-selected-payers").kendoListView({
        selectable: "multiple",
        dataSource: dataSourceSelectedPayer,
        template: kendo.template($("#selecttemplate").html())
    });

    //filtering Payer Names based on given search criteria
    $("#txt-search-payers").keyup(function () {
        dataSourceAvailablePayer.filter({
            field: "Name",
            operator: "contains",
            value: $("#txt-search-payers").val()
        })
    });

    var selectedps = '';
    var selectedNames = '';
    var selectedUId = '';
    var lvPayersAvailable = $("#available-payers").data("kendoListView");

    $("#contract-addtogrid").click(function () {
        $("#btnsave-contractlink").removeAttr('disabled');
        isContractChange = true;
        var txt = $("#available-payers").find('.k-state-selected');
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
                dataSourceSelectedPayer.add({ PayerId: splitedId[j], Name: splitedName[j] });
            }
            for (var k = 0; k < lvPayersAvailable.items().length; k++) {
                if (splitedName[j] != "") {
                    var dataToRemove = lvPayersAvailable.dataSource.getByUid(splittedUId[j]);
                    lvPayersAvailable.dataSource.remove(dataToRemove);
                    break;
                }
            }
        }
        selectedps = '';
        selectedNames = '';
        selectedUId = '';
    });

    //Move all the available payers to Selected payers listview 
    $("#contract-add-all-togrid").click(function () {
        $("#btnsave-contractlink").removeAttr('disabled');
        var selectedPayers = $("#available-payers").data().kendoListView.dataSource._data;
        $.merge(selectedPayers, dataSourceSelectedPayer._data);
        dataSourceSelectedPayer.data(selectedPayers);
        $("#available-payers").data('kendoListView').dataSource.data([]);
        isContractChange = true;
    });

    //Move all the selected payers to available payers listview 
    $("#btn-remove-all").click(function () {
        $("#btnsave-contractlink").removeAttr('disabled');
        var selectedPayers = $("#list-selected-payers").data().kendoListView.dataSource._data;
        $.merge(selectedPayers, dataSourceAvailablePayer._data);
        dataSourceAvailablePayer.data(selectedPayers);
        $("#list-selected-payers").data('kendoListView').dataSource.data([]);
        isContractChange = true;
    });


    //For removing the selected payers from listview
    var lvPayersSelected = $("#list-selected-payers").data("kendoListView");
    var rmvselectedps = '';
    var rmvselectedNames = '';
    var uIdToRemovePayer = '';

    $("#btn-remove").click(function () {
        $("#btnsave-contractlink").removeAttr('disabled');
        isContractChange = true;
        var txtRmv = $("#list-selected-payers").find('.k-state-selected');
        for (var i = 0; i < txtRmv.length; i++) {
            rmvselectedps += txtRmv[i].value + "##";
            rmvselectedNames += txtRmv[i].textContent + "##";
            uIdToRemovePayer += txtRmv[i].getAttribute('data-uid') + "##";
        }
        rmvselectedps = rmvselectedps.substring(0, rmvselectedps.length - 2);
        rmvselectedNames = rmvselectedNames.substring(0, rmvselectedNames.length - 2);
        uIdToRemovePayer = uIdToRemovePayer.substring(0, uIdToRemovePayer.length - 2);
        var rmvsplitedId = rmvselectedps.split('##');
        var rmvsplitedName = rmvselectedNames.split('##');
        var rmvsplitterUId = uIdToRemovePayer.split('##');
        for (var j = 0; j < rmvsplitedId.length; j++) {
            dataSourceAvailablePayer.add({ PayerId: rmvsplitedId[j], Name: rmvsplitedName[j] });
            for (var k = 0; k < lvPayersSelected.items().length; k++) {
                if (rmvsplitedName[j] != "") {
                    var removePayer = lvPayersSelected.dataSource.getByUid(rmvsplitterUId[j]);
                    lvPayersSelected.dataSource.remove(removePayer);
                    break;
                }
            }
        }
        rmvselectedps = '';
        rmvselectedNames = '';
        uIdToRemovePayer = '';
        isContractChange = true;
    });

    $(".customdate").change(function (event) {
        $("#btnsave-contractlink").removeAttr('disabled');
        isContractChange = true;
        var validateDate = $(event.target).val();
        if (validateDate != "" && validateDate != "mm/dd/yyyy") {
            var getDate = $(event.target).val();
            getDate = dateFormat(new Date(getDate), "MM/DD/YYYY");
            $(event.target).val(getDate);
            $(this).datepicker('hide');
        }
    });

    $("#ddl-udf-fields").kendoDropDownList({});
    getUdfData();
    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    $('.from_date').datepicker({
        todayBtn: 'linked',
        autoclose: true,
        weekStart: 1,
        keyboardNavigation: false
    });

    $('.to_date').datepicker({
        todayBtn: 'linked',
        weekStart: 1,
        autoclose: true,
        keyboardNavigation: false
    });

    $(".customdate").keydown(function (e) {
        $("#btnsave-contractlink").removeAttr('disabled');
        isContractChange = true;
        $(e.target).removeAttr('style');
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [191, 8, 9, 27, 13, 190, 46]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39) ||
            // Allow : Ctrl+c
            (e.keyCode == 67 && e.ctrlKey === true) ||
            // Allow : Ctrl + v
            (e.keyCode == 86 && e.ctrlKey === true)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 47 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105) && e.keyCode != 111) {
            e.preventDefault();
        }
    });

    $("#txt-search-payers").attr("style", "font-style: italic; color: grey !important;");
    $("#txt-search-payers").click(function () {
        $("#txt-search-payers").attr("style", "font-style: normal; color: #13688c !important;");
    });

});
function getUdfData() {
    ajaxRequestHandler({
        url: SSIUrl.getUdfFields,
        type: "GET",
        cache: false,
        dataType: "json",
        fnSuccess: OnGetUdfFieldsSuccess
    });
}

function OnGetUdfFieldsSuccess(json) {
    //In success function bind data into combobox using kendo 
    var udfFieldsComboBoxData = [];
    if (json != null && json.udfFields != null) {
        $.each(json.udfFields, function (i, item) {
            udfFieldsComboBoxData.push({
                'Text': item.FieldName,
                'Value': item.FieldIdentityNumber
            });
        });
    }

    $("#ddl-udf-fields").kendoDropDownList({
        optionLabel: "Select Field",
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: udfFieldsComboBoxData
    });
}

$("#contract-addtogrid").attr('disabled', 'disabled');
$("#btn-remove").attr('disabled', 'disabled');
$("#btnsave-contractlink").attr('disabled', 'disabled');

$("#alert-threshold").keydown(function (e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A
        (e.keyCode == 65 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
        (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
});

$("#alert-threshold").keyup(function () {
    $("#btnsave-contractlink").removeAttr('disabled');
    isContractChange = true;
});

//set WaterMark text
function WaterMark(txtSearchPayers, event) {
    var defaultText = "<Search Payer Name>";
    // Condition to check textbox value and event type
    if (txtSearchPayers.value == defaultText & event.type == "focus") {
        txtSearchPayers.value = "";
    }
}

$("#txt-search-payers").focusout(function () {
    if ($("#txt-search-payers").val() == '') {
        $("#txt-search-payers").val('<Search Payer Name>');
        $("#txt-search-payers").attr("style", "font-style: italic; color: grey !important;");
    }
});

function dateFormat(date, format) {
    // Calculate date parts and replace instances in format string accordingly
    format = format.replace("DD", (date.getDate() < 10 ? '0' : '') + date.getDate()); // Pad with '0' if needed
    format = format.replace("MM", (date.getMonth() < 9 ? '0' : '') + (date.getMonth() + 1)); // Months are zero-based
    format = format.replace("YYYY", date.getFullYear());
    return format;
}

function strcmp(a, b) {
    if (a != null && b != null) {
        a = a.toString(), b = b.toString();
        for (var i = 0, n = Math.max(a.length, b.length) ; i < n && a.charAt(i).trim() == b.charAt(i).trim() ; ++i);
        if (i == n)
            return true;
    }
    return false;
}

//Keyup of text box button enabled
$("#txt-contract-name").keyup(function () {
    if (oldContractName != $("#txt-contract-name").val() || isContractChange == true) {
        $("#btnsave-contractlink").removeAttr('disabled');
        isContractChange = true;
    } else {
        $("#btnsave-contractlink").attr('disabled', 'disabled');
    }
});

//Check if Payer Code change then enable save button
$("#txt-payer-code").keyup(function () {
    $("#btnsave-contractlink").removeAttr('disabled');
    isContractChange = true;
});

//Check if udf dropdown change then  enable save button
$("#ddl-udf-fields").change(function () {
    $("#btnsave-contractlink").removeAttr('disabled');
    isContractChange = true;
});

//Click of available-payers list Add button enabled
$("#available-payers").click(function () {
    $("#contract-addtogrid").removeAttr('disabled');
    $("#txt-search-payers").blur();
    $("#list-selected-payers").data("kendoListView").clearSelection();
});

//Click of list-selected-payers list Remove button enabled
$("#list-selected-payers").click(function () {
    $("#btn-remove").removeAttr('disabled');
    $("#available-payers").data("kendoListView").clearSelection();
});

// check for Check box if change enable save button 
$('.common-class').change(function () {
    if (this.checked && this.uncheked) {
        $("#btnsave-contractlink").attr('disabled', 'disabled');
    } else {
        $("#btnsave-contractlink").removeAttr('disabled');
        isContractChange = true;
    }
});

//To validate Date format
function validateDateFormat() {
    var regExpression = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
    var fromDate = $("#contractlink-startdate").val();
    var toDate = $("#contractlink-endingdate").val();

    fromDate = dateFormat(new Date(fromDate), "MM/DD/YYYY");
    toDate = dateFormat(new Date(toDate), "MM/DD/YYYY");

    if (fromDate != "" || toDate != "") {
        if (regExpression.test(fromDate) == false || regExpression.test(toDate) == false) {
            return false;
        }
        return true;
    } else {
        return true;
    }
}

function startChange() {
    var startDate = start.value(),
        endDate = end.value();
    if (startDate) {
        startDate = new Date(startDate);
        startDate.setDate(startDate.getDate());
        end.min(startDate);
    } else if (endDate) {
        start.max(new Date(endDate));
    } else {
        endDate = new Date();
        end.min(endDate);
    }
}

function endChange() {
    var endDate = end.value(),
        startDate = start.value();
    if (endDate) {
        endDate = new Date(endDate);
        endDate.setDate(endDate.getDate());
    } else if (startDate) {
        end.min(new Date(startDate));
    } else {
        endDate = new Date();
        end.min(endDate);
    }
}

function days_between(dateTo, dateFrom) {
    // The number of milliseconds in one day
    var ONE_DAY = 1000 * 60 * 60 * 24;
    date1 = new Date(dateTo);
    date2 = new Date(dateFrom);
    // Convert both dates to milliseconds
    var date1_ms = date1.getTime();
    var date2_ms = date2.getTime();
    // Calculate the difference in milliseconds
    var difference_ms = Math.abs(date1_ms - date2_ms);
    // Convert back to days and return
    return Math.round(difference_ms / ONE_DAY);
}

$("#btnsave-contractlink").click(function () {
    modifiedContractDetails();
});

//$("#contractlink-startdate").focusout(function () {
//    $("#contractlink-startdate").removeAttr('style');
//});

$(".customdate").focusout(function (event) {
    $(event.target).removeAttr('style');
});

function isDate(ExpiryDate) {
    var objDate, // date object initialized from the ExpiryDate string 
        mSeconds, // ExpiryDate in milliseconds 
        day, // day 
        month, // month 
        year; // year 
    ExpiryDate = ExpiryDate.split('/');
    if (ExpiryDate[0] <= 9 && ExpiryDate[0].length == 1)
        ExpiryDate[0] = '0' + ExpiryDate[0];
    if (ExpiryDate[1] <= 9 && ExpiryDate[1].length == 1)
        ExpiryDate[1] = '0' + ExpiryDate[1];
    ExpiryDate = ExpiryDate[0] + '/' + ExpiryDate[1] + '/' + ExpiryDate[2];
    // date length should be 10 characters (no more no less) 
    if (ExpiryDate.length !== 10) {
        return false;
    }
    // third and sixth character should be '/' 
    if (ExpiryDate.substring(2, 3) !== '/' || ExpiryDate.substring(5, 6) !== '/') {
        return false;
    }
    // extract month, day and year from the ExpiryDate (expected format is mm/dd/yyyy) 
    // subtraction will cast variables to integer implicitly (needed 
    // for !== comparing) 
    month = ExpiryDate.substring(0, 2) - 1; // because months in JS start from 0 
    day = ExpiryDate.substring(3, 5) - 0;
    year = ExpiryDate.substring(6, 10) - 0;
    // test year range 
    if (year < 1000 || year > 3000) {
        return false;
    }
    // convert ExpiryDate to milliseconds 
    mSeconds = (new Date(year, month, day)).getTime();
    // initialize Date() object from calculated milliseconds 
    objDate = new Date();
    objDate.setTime(mSeconds);
    // compare input date and parts from Date() object 
    // if difference exists then date isn't valid 
    if (objDate.getFullYear() !== year ||
        objDate.getMonth() !== month ||
        objDate.getDate() !== day) {
        return false;
    }
    // otherwise return true 
    return true;
}

// validation for date time  in save click function
function dateCompare() {
    var fromDate = $("#contractlink-startdate").val();
    var toDate = $("#contractlink-endingdate").val();
    if (fromDate == "" || fromDate == "NaN") {
        fromDate = "mm/dd/yyyy";
    }
    if (toDate == "" || toDate == "NaN") {
        toDate = "mm/dd/yyyy";
    }
    if (Date.parse(fromDate) > Date.parse(toDate)) {
        $("#contractlink-endingdate").val('');
        return false;
    }
    if ((fromDate == "mm/dd/yyyy") && (toDate == "mm/dd/yyyy")) {
        return true;
    } else if ((fromDate == "mm/dd/yyyy") && (toDate != "mm/dd/yyyy")) {
        return false;
    } else if ((toDate == "mm/dd/yyyy") && (fromDate != "mm/dd/yyyy")) {
        return false;
    } else {
        return true;
    }
}

setTimeout(function () {
    $("input:text:first").focus();
}, 1000);

function saveContractDetails() {
    ajaxRequestHandler({
        url: SSIUrl.saveContractDetails,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(contractViewModel),
        fnSuccess: OnSaveContractDetailsSuccess
    });
}

function OnSaveContractDetailsSuccess(data) {
    if ($('#hidden-basicinfo-contractid').val() == '0') {
        $('#hidden-basicinfo-contractid').val(data.addedId);
    }
    $("#btn-contract-done").attr('value', 'Done')
    $('.btn-save-close').show();

    previousContractName = contractViewModel.ContractName;
    contractViewModel.ContractId = data.addedId;
    if (isContractChange) {
        ContinueNavigate();
    }
    isContractChange = false;
    resetContractViewModel = JSON.parse(JSON.stringify(contractViewModel));
    resetContractViewModel.AvailablePayerList = availablePayerList;
}

function openModifiedReasonPopup(contractId, nodeId, facilityId) {
    if ($("#container-basic-info").attr('data-isPrimaryModel') == 'true' && contractViewModel.ContractId !== '0') {
        $("#form-add-contract").html('');
        var windowAddContractModifiedReason = $("#form-add-contract").kendoWindow({
            draggable: false,
            position: {
                top: "20%"
            },
            resizable: false,
            width: "300px",
            height: "85px",
            modal: true,
            content: { url: SSIUrl.savecontractModifiedReason + "?contractId=" + contractId + "&nodeId=" + nodeId + "&facilityId=" + facilityId }
        });
        windowAddContractModifiedReason.data('kendoWindow').center().open().title('Contract Modified Reason').wrapper.css({
            width: "300px",
            height: "85px"
        });

    } else {
        isValidationSucess = true;
        saveContractDetails();
    }
}

function modifiedContractConditions() {
    var ExpiryDateFrom = document.getElementById('contractlink-startdate').value;
    var ExpiryDateTo = document.getElementById('contractlink-endingdate').value;
    var currentDate = new Date();
    if (isDate(ExpiryDateFrom) && isDate(ExpiryDateTo)) {
        var threshold = $("#alert-threshold").val();
    } else {
        registerGridPopup("Please enter a valid date.", "OK", null, null);
        return false;
    }
    fromDateCheck = $("#contractlink-startdate").val();
    toDateCheck = $("#contractlink-endingdate").val();
    if (fromDateCheck == "" || fromDateCheck == "NaN") {
        fromDateCheck = "mm/dd/yyyy";
    }
    if (toDateCheck == "" || toDateCheck == "NaN") {
        toDateCheck = "mm/dd/yyyy";
    }
    if ($("#txt-payer-code").val() == '' && $("#list-selected-payers").html() == "") {
        registerGridPopup("Please enter payer codes or select payer(s) from the list.", "OK", null, null);
        return false;
    }
    if ($("#txt-payer-code").val() != '' && $("#ddl-udf-fields").data("kendoDropDownList").text() == 'Select Field') {
        registerGridPopup("Please select a location for payer code(s).", "OK", null, null);
        return false;
    } else if ($("#txt-payer-code").val() == '' && $("#ddl-udf-fields").data("kendoDropDownList").text() != 'Select Field') {
        registerGridPopup("Please enter payer code(s).", "OK", null, null);
        return false;
    }
    return true;
}

function ResetContractLinkInfo(data) {
    contractViewModel.set("Status", data.Status);
    contractViewModel.set("ContractId", data.ContractId);
    contractViewModel.set("AvailablePayerList", data.AvailablePayerList);
    contractViewModel.set("SelectedPayerList", data.SelectedPayerList);
    contractViewModel.set("ContractName", data.ContractName);
    contractViewModel.set("ParentId", data.ParentId);
    contractViewModel.set("IsProfessional", data.IsProfessional);
    contractViewModel.set("IsInstitutional", data.IsInstitutional);
    contractViewModel.set("IsClaimStartDate", data.IsClaimStartDate);
    contractViewModel.set("FacilityId", data.FacilityId);
    contractViewModel.set("NodeId", data.NodeId);
    contractViewModel.set("ThresholdDaysToExpireAlters", data.ThresholdDaysToExpireAlters);
    contractViewModel.set("PayerCode", data.PayerCode);
    contractViewModel.set("CustomField", data.CustomField);
    //contractViewModel.set("EffectiveStartDate", data.EffectiveStartDate);
    //contractViewModel.set("EffectiveEndDate", data.EffectiveEndDate);
    contractViewModel.set("ProviderId", data.ProviderId);
    $("#contractlink-startdate").val((data.EffectiveStartDate != "" && data.EffectiveStartDate != "NaN") ? dateFormat(new Date(data.EffectiveStartDate), "MM/DD/YYYY") : "mm/dd/yyyy");//kendo observable is not updating date picker value 
    $("#contractlink-endingdate").val((data.EffectiveEndDate != "" && data.EffectiveEndDate != "NaN") ? dateFormat(new Date(data.EffectiveEndDate), "MM/DD/YYYY") : "mm/dd/yyyy");//kendo observable is not updating date picker value
    kendo.bind($("#container-basic-info"), contractViewModel);
}

function IsContractNameExists(parentNode, modelParentId, startDate, endDate, ContractId) {
    if (parentNode && modelParentId != '') {
        data = { ContractName: contractViewModel.ContractName, NodeId: modelParentId, EffectiveStartDate: startDate, EffectiveEndDate: endDate, ContractId: ContractId }
    } else {
        data = { ContractName: contractViewModel.ContractName, NodeId: contractViewModel.NodeId, EffectiveStartDate: startDate, EffectiveEndDate: endDate, ContractId: ContractId }
    }

    ajaxRequestHandler({
        url: SSIUrl.isContractNameExist,
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: JSON.stringify(data),
        fnSuccess: OnIsContractNameExistSuccess
    });
};

function OnIsContractNameExistSuccess(data) {

    if (data) {
        openModifiedReasonPopup(contractViewModel.ContractId, contractViewModel.NodeId, contractViewModel.FacilityId);
    } else {
        registerGridPopup("A contract with the same name exists with overlapping service dates. Please try another name.", "OK", null, null);
        isValidationSucess = false;
        return false;
    }
}