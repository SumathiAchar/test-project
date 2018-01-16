var allowedVariables = ['CAA', 'LOS', 'TCC', 'GREATEROF', 'LESSEROF', 'LC', 'IF'];
var customEvaluationReplace = { 'GREATEROF': 'Math.max', 'LESSEROF': 'Math.min', 'CAA': '20', 'LOS': '30', 'TCC': '50', 'LC': '10', 'IF': 'Math.min' };
var tableHeaders = {};
var variables = "<b>Variables: </b><br>Length Of Stay(LOS), Total Claim Charges(TCC), Calculated Allowed Amount(CAA), Line Charge(LC)";
var operators = "<b>Operators: </b><br>+, -, *, /, (, ), <, >, <>, <=, >=, =";
var functions = "<b>Functions: </b><br>GreaterOf, LesserOf, IF";
var viewmodelCustomPayment;
var tableHeaderPrefix = 'T_';
var defaultHeaderValue = '10';
var caretPos = 0;
var messageMandatoryField = "Please fill mandatory fields.";
var messageFormulaIncorrect = "Incorrect formula.";
var messageNotSaved = "Modelling is not saved.";
var messageLineValidation = "LC can not be applied with out Rev code or HCPCS.";
var caretPos;
var cursorClickPos = false;
var customTableDivResult = [];
var customTableDiv = ['#txtChooseFormula', '#txt-first-multiplier', '#txt-second-multiplier', '#txt-third-multiplier', '#txt-fourth-multiplier', '#txt-others-multiplier', '#input-limit'];
var messageTableIncorrect = "Incorrect table";

$(document).ready(function () {
    var $selectedNode = treeView.select();
    var nodeId = treeView.dataItem($selectedNode).NodeId;

    //Loads help text to the div
    $(".brief-formulatext").html(variables + "<br>" + operators + "<br>" + functions);

    //shows help text on click of this div
    $("#help-formula").click(function () {
        if ($("#float-formula").hasClass('cmshideonload')) {
            $("#float-formula").removeClass('cmshideonload');
        } else {
            $("#float-formula").addClass('cmshideonload');
        }
    });
    $('#txtChooseFormula').val(viewmodelCustomPayment.Expression.replace(/&lt;/g, '<').replace(/&gt;/g, '>'));

    $('html').click(function (event) {
        if (!$(event.target).closest('#help-formula').length) {
            if (!$(event.target).closest('#float-formula').length) {
                $("#float-formula").addClass('cmshideonload');
            }
        }
    });

    //fetches all tables of the facility and binds it to the 'Choose Table' combobox
    ajaxRequestHandler({
        url: SSIUrl.getPayementTypeTableNameMasterData + "?paymentTypeId=0&nodeId=" + nodeId + "&userText=" + '',
        type: "GET",
        dataType: 'json',
        fnSuccess: OnGetPayementTypeTableNameMasterDataSuccess
    });

    function OnGetPayementTypeTableNameMasterDataSuccess(json) {
        //push json data to array. This will be used to bind data to combobox
        if ($("#txtChooseTable").data("kendoComboBox").text() == "" || $("#txtChooseTable").data("kendoComboBox").text() == "Select Table") {
            $("#txtChooseTable").addClass('customDefaultColor');
        }
        var paymentTypeTableNameComboBoxData = [];
        if (json != null) {
            $.each(json, function (i, item) {
                paymentTypeTableNameComboBoxData.push({ 'text': item.Text, 'value': item.Value });
            });
        }
        //In success function bind data into combobox using kendo 
        $("#txtChooseTable").kendoComboBox({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: paymentTypeTableNameComboBoxData,
            filter: "contains",
            suggest: true,
            change: changeTableNotification
        });
    }

    //gets all claim fields for custom payment using module id
    ajaxRequestHandler({
        url: SSIUrl.getClaimFieldsByModule + "?moduleId=" + $('#hiddenModuleId').val(),
        type: "GET",
        dataType: 'json',
        fnSuccess: OnGetClaimFieldsByModuleSuccess
    });

    function OnGetClaimFieldsByModuleSuccess(json) {
        //push json data to array. This will be used to bind data to combobox
        var serviceClaimFeildComboBoxData = [];
        if (json != null && json.claimFeildList != null) {
            $.each(json.claimFeildList, function (i, item) {
                serviceClaimFeildComboBoxData.push({ 'Text': item.Text, 'Value': item.Value });
            });
        }
        //bind data into combobox using kendo 
        $("#txtCustomFields").kendoDropDownList({
            optionLabel: "Select Claims",
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: serviceClaimFeildComboBoxData,
            change: changeCustomField
        });
      }
    function changeCustomField() {
        if ($("#txtCustomFields").data("kendoDropDownList").text() == "HCPCS/RATE/HIPPS" || $("#txtCustomFields").data("kendoDropDownList").text() == "Place of Service(P)"
            || $("#txtCustomFields").data("kendoDropDownList").text() == "Revenue Code(I)") {
            $("#txt-first-multiplier").attr("disabled", false);
            $("#txt-second-multiplier").attr("disabled", false);
            $("#txt-third-multiplier").attr("disabled", false);
            $("#txt-fourth-multiplier").attr("disabled", false);
            $("#txt-others-multiplier").attr("disabled", false);
            $('#checkbox-perday-stay').attr('disabled', false);
            $('#checkbox-observed-units').attr('disabled', false);
            $('#checkbox-per-code').attr('disabled', false);
        } else {
            $("#input-limit").attr("disabled", true);
            $("#input-limit").val("");
            viewmodelCustomPayment.ObserveServiceUnitLimit = $("#input-limit").val();

            $("#txt-first-multiplier").attr("disabled", true);
            $("#txt-first-multiplier").val("");
            viewmodelCustomPayment.MultiplierFirst = $("#txt-first-multiplier").val();

            $("#txt-second-multiplier").attr("disabled", true);
            $("#txt-second-multiplier").val("");
            viewmodelCustomPayment.MultiplierSecond = $("#txt-second-multiplier").val();

            $("#txt-third-multiplier").attr("disabled", true);
            $("#txt-third-multiplier").val("");
            viewmodelCustomPayment.MultiplierThird = $("#txt-third-multiplier").val();

            $("#txt-fourth-multiplier").attr("disabled", true);
            $("#txt-fourth-multiplier").val("");
            viewmodelCustomPayment.MultiplierFourth = $("#txt-fourth-multiplier").val();

            $("#txt-others-multiplier").attr("disabled", true);
            $("#txt-others-multiplier").val("");
            viewmodelCustomPayment.MultiplierOther = $("#txt-others-multiplier").val();

            $('#checkbox-perday-stay').attr('disabled', true);
            $('#checkbox-perday-stay').attr('checked', false);
            viewmodelCustomPayment.IsPerDayOfStay = false;

            $('#checkbox-observed-units').attr('disabled', true);
            $('#checkbox-observed-units').attr('checked', false);
            viewmodelCustomPayment.IsObserveServiceUnit = false;

            $('#checkbox-per-code').attr('disabled', true);
            $('#checkbox-per-code').attr('checked', false);
            viewmodelCustomPayment.IsPerCode = false;
        }
    }
   
    //bind table fields
      changeTableNotification();

    //triggers when user clicks to "Include" button/image to include selected list box table headers
    cursorPosinitial = $("#txtChooseFormula").val().length;

    $('#fields-addtogrid').click(function () {
        var selectedItems = $("#available-fields").find('.k-state-selected');
        var selectedVariables = '';
        $.each(selectedItems, function (key, value) {
            var currentVariables = tableHeaderPrefix + selectedItems[key].textContent.trim() + ' '
            selectedVariables = selectedVariables + ' ' + currentVariables
            tableHeaders[currentVariables.trim().toUpperCase()] = defaultHeaderValue;
        });
        if (!cursorClickPos) {
            cursorPosinitial = $("#txtChooseFormula").val().length;
        }
        var cursorPos = cursorPosinitial,
    v = $('#txtChooseFormula').val(),
    textBefore = v.substring(0, cursorPos),
    textAfter = v.substring(cursorPos, v.length);
        $('#txtChooseFormula').val(textBefore + selectedVariables + textAfter);
        cursorPosinitial = cursorPosinitial + selectedVariables.length;
    });


    // To get the caret position
    $("#txtChooseFormula").bind("mouseout keyup", function (e) {
        cursorClickPos = true;
        cursorPosinitial = $('#txtChooseFormula').prop('selectionStart');
    });

    //to save the form data in database
    $("#btnFormulaSave").off('click').on('click', function () {
       var selectedClaimField = $("#txtCustomFields").data("kendoDropDownList").text();
        if ($('#txtChooseFormula').val() == '' || selectedClaimField == '' || $("#txtChooseTable").data("kendoComboBox").text() == '' || $("#txtChooseTable").data("kendoComboBox").text() == 'Select Table' || $("#txtCustomFields").val() == '') {
            registerGridPopup(messageMandatoryField, "OK", null, null);
        } else {
            if (validateFormula()) {
                viewmodelCustomPayment.Expression = $('#txtChooseFormula').val().toUpperCase();
                viewmodelCustomPayment.MultiplierFirst = $('#txt-first-multiplier').val().toUpperCase();
                viewmodelCustomPayment.MultiplierSecond = $('#txt-second-multiplier').val().toUpperCase();
                viewmodelCustomPayment.MultiplierThird = $('#txt-third-multiplier').val().toUpperCase();
                viewmodelCustomPayment.MultiplierFourth = $('#txt-fourth-multiplier').val().toUpperCase();
                viewmodelCustomPayment.MultiplierOther = $('#txt-others-multiplier').val().toUpperCase();
                viewmodelCustomPayment.ObserveServiceUnitLimit = $('#input-limit').val().toUpperCase();
                $("#btnFormulaSave").attr('disabled', 'disabled');
                var dictionaryValues = {}; // create an empty array
                for (var key in tableHeaders) {
                    dictionaryValues[key.replace('.', '#$#')] = tableHeaders[key];
                }
                viewmodelCustomPayment.TableHeaders = dictionaryValues;
                ajaxRequestHandler({
                    url: SSIUrl.addEditPaymentTypeCustomTable,
                    type: 'POST',
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(viewmodelCustomPayment),
                    fnSuccess: OnAddEditPaymentTypeCustomTableSuccess,
                    fnComplete: OnAddEditPaymentTypeCustomTableComplete
                });
            } else {
                for (var index = 0; index < customTableDiv.length; index++) {
                    $(customTableDiv[index]).attr('style', 'border-color:none');
                }
                for (var index = 0; index < customTableDivResult.length; index++) {
                    $(customTableDivResult[index].Id).attr('style', 'border-color:red !important');
                }
                registerGridPopup(customTableDivResult[0].message, "OK", null, null);
            }
        }
    });
    //Combo box place holder default color in ie.
    ComboBoxDefaultColor("txtChooseTable");
    if (viewmodelCustomPayment.ClaimFieldId === "4"
        || viewmodelCustomPayment.ClaimFieldId === "3"
        || viewmodelCustomPayment.ClaimFieldId === "9") {
        if (viewmodelCustomPayment.IsObserveServiceUnit === true) {
            $("#input-limit").attr("disabled", false);
        } else {
            $("#input-limit").attr("disabled", true);
        }
        $("#txt-first-multiplier").attr("disabled", false);
        $("#txt-second-multiplier").attr("disabled", false);
        $("#txt-third-multiplier").attr("disabled", false);
        $("#txt-fourth-multiplier").attr("disabled", false);
        $("#txt-others-multiplier").attr("disabled", false);
        $('#checkbox-perday-stay').attr('disabled', false);
        $('#checkbox-observed-units').attr('disabled', false);
        $('#checkbox-per-code').attr('disabled', false);
    } else {
        $("#input-limit").attr("disabled", true);
        $("#txt-first-multiplier").attr("disabled", true);
        $("#txt-second-multiplier").attr("disabled", true);
        $("#txt-third-multiplier").attr("disabled", true);
        $("#txt-fourth-multiplier").attr("disabled", true);
        $("#txt-others-multiplier").attr("disabled", true);
        $('#checkbox-perday-stay').attr('disabled', true);
        $('#checkbox-observed-units').attr('disabled', true);
        $('#checkbox-per-code').attr('disabled', true);
    }
});
//validating all smartbox.
function validateFormula() {
    customTableDivResult = [];
    var selectedClaimField = $("#txtCustomFields").data("kendoDropDownList").text();
    for (var index = 0; index < customTableDiv.length; index++) {
        validateFormulaExpression($(customTableDiv[index]).val(), allowedVariables, $.extend(tableHeaders, customEvaluationReplace), customTableDiv[index]) && lineChargeValidation(selectedClaimField);
    }
    return (customTableDivResult.length == 0);
}




function OnAddEditPaymentTypeCustomTableSuccess(json) {
    if (json.documentId == null) {
        registerGridPopup(messageTableIncorrect, "OK", null, null);
    }
    else if (json.success) {
        for (var index = 0; index < customTableDiv.length; index++) {
            $(customTableDiv[index]).attr('style', 'border-color:none');
        }
        for (var index = 0; index < json.resultFormula.length; index++) {
            $(json.resultFormula[index]).attr('style', 'border-color:red !important');
        }
        registerGridPopup(messageFormulaIncorrect, "OK", null, null);
    } else if (json.Id == 0) {
        registerGridPopup(messageNotSaved, "OK", null, null);
    } else {
        $('#divCustomTableFormula').closest(".k-window-content").data("kendoWindow").close();
        ReDrawShapes();
    }
}

function OnAddEditPaymentTypeCustomTableComplete(data) {
    $("#btnFormulaSave").removeAttr('disabled');
}

//This validates if line level items are properly used as per the functionality
function lineChargeValidation(selectedClaimField) {
    if ((viewmodelCustomPayment.Expression.indexOf("LC") > -1 && selectedClaimField.indexOf("Rev") == -1)
        && (viewmodelCustomPayment.Expression.indexOf("LC") > -1 && selectedClaimField.indexOf("HCPCS") == -1)
        && (viewmodelCustomPayment.Expression.indexOf("LC") > -1 && selectedClaimField.indexOf("Place of Service(P)") == -1)) {
        registerGridPopup(messageLineValidation, "OK", null, null);
        return false;
    }
    return true;
}

//This function triggers when user changes the value of combobox "Choose Table"
function changeTableNotification() {
    if ($("#txtChooseTable").data("kendoComboBox").text() == '' || $("#txtChooseTable").data("kendoComboBox").text() == 'Select Table') {
        $("#available-fields").text('');
        $("#first-column-header").text('');
    } else {
        ajaxRequestHandler({
            url: SSIUrl.getCustomTableHeader + "?documentId=" + $("#txtChooseTable").data("kendoComboBox").value(),
            type: "GET",
            dataType: 'json',
            fnSuccess: OnGetCustomTableHeaderSuccess,
        });
    }
}


function OnGetCustomTableHeaderSuccess(json) {
    //In success function bind data into combobox using kendo 
    var documentHeaders = [];
    tableHeaders = {};
    if (json != null && json.documentHeader != null) {
        var splitedDocumentHeader = json.documentHeader.split(',')
        splitedDocumentHeader = splitedDocumentHeader.filter(Boolean);
        $.each(splitedDocumentHeader, function (i, item) {
            documentHeaders.push({ 'Text': item });
        });
        splitedDocumentHeader.sort(sortHeaderFields);

        //prepares a dictionary of all table variables with default value
        $.each(splitedDocumentHeader, function (i, item) {
            allowedVariables.push(tableHeaderPrefix + item.trim().toUpperCase());
            var currentVariables = tableHeaderPrefix + item;
            tableHeaders[currentVariables.trim().toUpperCase()] = defaultHeaderValue;
        });

        if (documentHeaders.length != 0) {
            $('#first-column-header').text(documentHeaders[0].Text);
            documentHeaders.shift();
            $("#available-fields").kendoListView({
                selectable: "multiple",
                dataSource: {
                    data: documentHeaders
                },
                template: kendo.template($('#header-list-template').html())
            });
        }
    }
}

//sorts the array in descending order by length of the items in the array
function sortHeaderFields(firstHeader, secondHeader) {
    if (firstHeader.length != secondHeader.length) {
        return secondHeader.length - firstHeader.length;
    }
    return (firstHeader < secondHeader) ? -1 : (firstHeader > secondHeader) ? 1 : 0;
};
// adding the cursor position text
function insertAtCaret(element, text) {
    if (element.length >= caretPos)
        element.val(element.val().substring(0, caretPos) + text + element.val().substring(caretPos));
}

$("#div-scope-checkbox input").change(function (e) {
    if (e.target.id === "checkbox-observed-units" && $(e.target).prop("checked")) {
        $("#input-limit").attr("disabled", false);
        viewmodelCustomPayment.ObserveServiceUnitLimit = $("#input-limit").val();

    } else if ($("#checkbox-per-code").prop("checked") && $("#checkbox-perday-stay").prop("checked")) {
        if (e.target.id === "checkbox-per-code") {
            $("#checkbox-perday-stay").attr("checked", false);
            viewmodelCustomPayment.IsPerDayOfStay = false;
        } else {
            $("#checkbox-per-code").attr("checked", false);
            viewmodelCustomPayment.IsPerCode = false;
        }
    } else if ((e.target.id === "checkbox-observed-units" && $(e.target).prop("checked", false))) {
        $("#input-limit").attr("disabled", true);
        $("#input-limit").val("");
        viewmodelCustomPayment.ObserveServiceUnitLimit = $("#input-limit").val();
    }
});

//For IE textarea validation.
$("textarea").bind('keypress keyup', function (e) {
    validateOnKey(e, this);
});
//For IE textarea validation.
$("#txtChooseFormula").on('paste', function (e) {
    var value = $(this).val();
    validateOnKey(e, this);
    var textFormula = getClipboardData();
    if (value != "") {
        textFormula = textFormula + value;
    }
    if (textFormula.length > 5000) {//5000 means max length 
        e.preventDefault();
    }
});
//For IE textarea validation.
function validateOnKey(event, control) {
    var value = $(control).val();
    if (value.length > 5000) {
        event.preventDefault();
    }
}
//For IE textarea validation.
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

//This method validates expression from UI and returns appropriate message in case validation fails
function validateFormulaExpression(thresholdExpression, allowedVariables, evaluationReplacement, id) {
    var expresssion = ($(id).val() === "") ? thresholdExpression : getValidExpression(thresholdExpression);
    $(id).val(expresssion.toUpperCase());
    var inputstr = expresssion;
    var new_str = inputstr.toUpperCase();

    //Matches opening and closing brackets. Number of opening bracket and closing brackets should be same
    if ((new_str.match(/\(/g) || '').length !== (new_str.match(/\)/g) || '').length) {
        customTableDivResult.push({ "Id": id, "message": "Missing parenthesis." });
    }

  //if expression has multiple operator side by side. Ex 4+-*3/2
    if (RegExp("[+-/*=]{2,}").test(new_str)) {
        customTableDivResult.push({ "Id": id, "message": "Incorrect formula." });
    }

    new_str = new_str.trim();
    var splitedExpression = [];
    splitedExpression = new_str.split(/[()+-/<>=*]/);
    for (var index = 0; index < splitedExpression.length; index++) {
        splitedExpression[index] = splitedExpression[index].trim();
    }

    //to clean out empty element from the array
    splitedExpression = splitedExpression.filter(Boolean);
    var isValid = true;
    var message = '';
    $.each(splitedExpression, function (index, item) {
        //if the item doesnot present in allowed variables and that is not a number then the expression is not valid
        if (allowedVariables.indexOf(item) === -1 && (item === '' || isNaN(item))) {
            isValid = false;

            //Expression is invalid. If expression contains T_ then its incorrect table field
            //if expression contains any other characters from a-z then incorrect variable
            //if user has used notr allowed operators like %,^,&,! etc, then incorrect operator
            if (item.substring(0, 2).match('T_')) {
                $(".btn-save-close").find('input').prop('disabled', true);
                message = "Incorrect table field entry.";
                return false;
            }
            else if (item.match(/[a-z/]/i)) {
                $(".btn-save-close").find('input').prop('disabled', true);
                message = "Incorrect variable used.";
                return false;
            }
            else if (!item.match(/[+-/*(),\s*]/g)) {
                $(".btn-save-close").find('input').prop('disabled', true);
                message = "Incorrect operator used.";
                return false;
            } else {
                $(".btn-save-close").find('input').prop('disabled', true);
                message = "Incorrect formula.";
                return false;

            }
            return isValid
        }
    });

    if (!isValid) {
        customTableDivResult.push({ "Id": id, "message": message });
    }
    else {
        var replace_str = inputstr.toUpperCase();
        try {
            //Replaces variables with dummy values. This will then be evaluated by JS eval function
            for (var key in evaluationReplacement) {
                replace_str = replace_str.replace(new RegExp(key, "g"), evaluationReplacement[key]);
            }
        }
        catch (exception) {
            customTableDivResult.push({ "Id": id, "message": "Incorrect formula." });
        }
    }
}





