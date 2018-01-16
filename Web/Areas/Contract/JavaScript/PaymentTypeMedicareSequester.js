$(document).ready(function () {
    setTimeout(function () {
        $('#txtPercentage').siblings('input:visible').focus();
    }, 1000);
});
//Formatting Percentage Field
$("#txtPercentage").kendoNumericTextBox({
    format: "#.## \\%",
    min: 0
});
//For Set Focus Text Box
$('#txtPercentage').siblings('input:visible').focus();

//Save Button
$("#btnMedicareSequester").click(function () {
    if ($("#txtPercentage").val() != '') {
        $("#btnMedicareSequester").attr('disabled', 'disabled');
        //Below code for adding payment Medicare Sequester
        ajaxRequestHandler({
            url: SSIUrl.savePaymentTypeMedicareSequester,
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(viewmodelMedicareSequester),
            fnSuccess: OnAddEditPaymentMedicareSequesterSuccess,
        });
    }
    else {
        registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
    }
});

function OnAddEditPaymentMedicareSequesterSuccess() {
    $('.ptMedicareSequester').closest(".k-window-content").data("kendoWindow").close();
    ReDrawShapes();
}

kendo.bind($("#ms-payment"), viewmodelMedicareSequester);
setTimeout(function () {
    $("input:text:first").focus();
}, 1000);
