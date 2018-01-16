$(document).ready(function () {

    setTimeout(function () {
            $('#txtLOPercentage').siblings('input:visible').focus().val($("#txtLOPercentage").val());
        }, 1000);

    //Formatting Percentage Field
    $("#txtLOPercentage").kendoNumericTextBox({
        format: "#.## \\%",
        min: 0
    });

    $('#txtLOPercentage').siblings('input:visible').focus().val($("#txtLOPercentage").val());

    $("#btnLOSave").click(function () {
        var isLesserOf = $('input:radio[name=lesserGreaterOf]:checked').val();
        if (isLesserOf == 'LesserOf') {
            viewModelLO.IsLesserOf = true;
        } else {
            viewModelLO.IsLesserOf = false;
        }
        if ($("#txtLOPercentage").val() != '') {
            $("#btnLOSave").attr('disabled', 'disabled');

            ajaxRequestHandler({
                url: SSIUrl.addEditPaymentTypeLesserOf,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(viewModelLO),
                fnSuccess: OnAddEditPaymentTypeLesserOfSuccess,
                fnError: OnAddEditPaymentTypeLesserOfError
            });          
        }
        else {
            registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
        }
    });
});


function OnAddEditPaymentTypeLesserOfSuccess(json) {
    if (json.success != false) {
        $('#btnLOSave').closest(".k-window-content").data("kendoWindow").close();
        ReDrawShapes();
    } else {
        $('#btnLOSave').closest(".k-window-content").data("kendoWindow").close();
        return false;
    }
}

function OnAddEditPaymentTypeLesserOfError(jqXHR, textStatus, errorThrown) {
    alert('Error:' + jqXHR + " Status= " + textStatus + " Req= " + errorThrown);
}

setTimeout(function () {
    $("input:text:first").focus().val($("#txtLOPercentage").val());
}, 1000);