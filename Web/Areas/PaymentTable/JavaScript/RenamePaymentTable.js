var isProceeded = false;
$(document).ready(function () {
    setTimeout(function () {
        $("#txt-rename-paymenttable").focus();
        var selectedTableName = $("#ddlTable").data("kendoComboBox").text();
        $("#txt-rename-paymenttable").val(selectedTableName);
    }, 100);
   
    $("#txt-rename-paymenttable").val("");
    $("#btn-rename-paymenttable").off('click').on('click', function () {
        if ($("#txt-rename-paymenttable").val() != "") {
            renameValue = $("#txt-rename-paymenttable").val();
            SaveRenamePaymentTable();
        }
        else {
            registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
        }
        return false;
    });
});


function SaveRenamePaymentTable() {
        ajaxRequestHandler({
            type: "POST",
            dataType: "json",
            url: SSIUrl.renamePaymentTable,
            data: JSON.stringify({ 'claimFieldDocId': claimFieldDocId, 'tableName': renameValue }),
            fnSuccess: OnRenamePaymentTableSuccess
        });
}

function OnRenamePaymentTableSuccess(json) {
    if (json) {
        if (json.ClaimFieldDocId == -1) { // -1 means Table name already exists
            $("#txt-rename-paymenttable").val('');
            isFromRenamePaymentTable = false;
            registerGridPopup("Table name already exists. Please enter another name.", "OK", null, null);
        }
        else {
            $("#txt-rename-paymenttable").val("");
            $("#rename-paymenttable-container").closest(".k-window-content").data("kendoWindow").close();
            GetTableNames(claimPaymentTypeId);
            $("#ddlTable").data("kendoComboBox").text(renameValue);
            isFromRenamePaymentTable = true;
            $("#ddlTable").data("kendoComboBox").input.attr("style", "font-style: normal; color: #13688c !important;");
        }
    }
}

$('input').keypress(function (event) {
    if (event.keyCode == 13) {
        return false;
    }
});
