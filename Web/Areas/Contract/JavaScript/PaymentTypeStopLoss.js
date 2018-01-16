var allowedVariables = ['CAA', 'LOS', 'TCC', 'GREATEROF', 'LESSEROF', 'IF'];
var evaluationReplacement = { 'GREATEROF': 'Math.max', 'LESSEROF': 'Math.min', 'CAA': '20', 'LOS': '30', 'TCC': '50', 'IF': 'Math.min' };
var variables = "<b>Variables: </b><br>Length Of Stay(LOS), Total Claim Charges(TCC), Calculated Allowed Amount(CAA)";
var operators = "<b>Operators: </b><br>+, -, *, /, (, ), <, >, <>, <=, >=, =";
var functions = "<b>Functions: </b><br>GreaterOf, LesserOf, IF"
var viewmodelSLPayment;

$(document).ready(function () {

    $("#help-icon").click(function () {
        if ($("#float-help").hasClass('cmshideonload')) {
            $("#float-help").removeClass('cmshideonload');
        } else {
            $("#float-help").addClass('cmshideonload');
        }
    });

    $('html').click(function (event) {
        if (!$(event.target).closest('#help-icon').length) {
            if (!$(event.target).closest('#float-help').length) {
                $("#float-help").addClass('cmshideonload');
            }
        }
    });
    $('#txtThresholdAmount').val(viewmodelSLPayment.Expression.replace(/&lt;/g, '<').replace(/&gt;/g, '>'));

    $("#txtSLPercentage").focus(function () {
        $("#float-help").addClass('cmshideonload');
    });
    $("#txtThresholdAmount").focus(function () {
        $("#float-help").addClass('cmshideonload');
    });
    setTimeout(function () {
        $('#txtThresholdAmount').siblings('input:visible').focus();
    }, 1000);

    $("#txtSLPercentage").kendoNumericTextBox({
        format: "#.## \\%",
        min: 0
    });

    if ($('#txtCptCode').val() != "") {
        $('#chk-include-stoploss').removeClass('k-state-disabled');
        $('#chk-include-stoploss').parent().removeClass('k-state-disabled');
    }
   //For Set Focus Text Box
    $('#txtThresholdAmount').siblings('input:visible').focus();

    $(".brief-thtext").html(variables + "<br>" + operators + "<br>" + functions);

    //On save button click of the form element
    $("#btnSLSave").click(function () {
      if ($("#txtThresholdAmount").val() == '' || $("#txtSLPercentage").val() == '') {
            registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
        }
        else if (validateExpression($('#txtThresholdAmount').val(), allowedVariables, evaluationReplacement)) {
            $("#btnSLSave").attr('disabled', 'disabled');
            viewmodelSLPayment.Expression = $('#txtThresholdAmount').val().toUpperCase();
            ajaxRequestHandler({
                url: SSIUrl.addEditPaymentTypeStopLoss,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(viewmodelSLPayment),
                fnSuccess: OnAddEditPaymentTypeStopLossSuccess,
                fnComplete: OnAddEditPaymentTypeStopLossComplete
            });
        }
    });

    function OnAddEditPaymentTypeStopLossSuccess(json) {
        if (!json.success) {
            registerGridPopup("Incorrect formula.", "OK", null, null);
        } else if (json.Id == 0) {
            registerGridPopup("Modelling is not saved.", "OK", null, null);
        }
        else {
            $('.ptStopLoss').closest(".k-window-content").data("kendoWindow").close();
            ReDrawShapes();
        }
    }

    function OnAddEditPaymentTypeStopLossComplete(data) {
        $("#btnSLSave").removeAttr('disabled');
    }
    //Check box checked on click of label
    CheckBoxLabelClickEvent();
});





