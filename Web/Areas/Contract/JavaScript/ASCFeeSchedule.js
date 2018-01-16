$(document).ready(function () {
    //Combo box place holder default color in ie.
    ComboBoxDefaultColor("ddlAscFeeSschedule");
    $("#btnSavePaymentTypeAscFee").click(function() {
        var isValidTable = false;
        if ($("#ddlAscFeeSschedule").data("kendoComboBox")) {
            var comboboxData = $("#ddlAscFeeSschedule").data("kendoComboBox").dataSource._data;
            $.each(comboboxData, function(index, obj) {
                if (viewmodelPaymentTypeAscFee.ClaimFieldDocID === obj.Value) {
                    isValidTable = true;
                   }
            });
        }
        if (!isValidTable) {
            registerGridPopup(messageTableIncorrect, "OK", null, null);
            return;
        }
        if ($("#ddlAscFeeSschedule").val() != '') {
            $("#btnSavePaymentTypeAscFee").attr('disabled', 'disabled');

            ajaxRequestHandler({
                url: SSIUrl.addEditPaymentTypeAscFee,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(viewmodelPaymentTypeAscFee),
                type: 'POST',
                dataType: 'json',
                fnSuccess: OnAddEditPaymentTypeAscFeeSuccess,
                fnError: OnAddEditPaymentTypeAscFeeError,
            });
        }
        else {
            registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
        }

    });

    function OnAddEditPaymentTypeAscFeeSuccess(json) {
        if (json.documentId == null) {
            registerGridPopup(messageTableIncorrect, "OK", null, null);
        }
        else if (json.success != false) {
            $('.ptASCFee').closest(".k-window-content").data("kendoWindow").close();
            ReDrawShapes();
        } else {
            $('.ptASCFee').closest(".k-window-content").data("kendoWindow").close();
            return false;
        }
    }

    function OnAddEditPaymentTypeAscFeeError(jqXHR, textStatus, errorThrown) {
        alert('Error:' + jqXHR + " Status= " + textStatus + " Req= " + errorThrown);
    }

    treeView = $("#tree-view").data('kendoTreeView');
    var $selectedNode = treeView.select();
    var nodeId = treeView.dataItem($selectedNode).NodeId;

    //For saving data into database
    if (viewmodelPaymentTypeAscFee.IsEdit == 'False') {
        ajaxRequestHandler({
            url: SSIUrl.getPayementTypeTableNameMasterData + "?paymenttypeId=" + viewmodelPaymentTypeAscFee.PaymentTypeId + "&userText=" + '' + "&nodeId=" + nodeId,
            type: "GET",
            dataType: 'json',
            fnSuccess: OnGetPayementTypeTableNameMasterDataSuccess,
            fnError: OnGetPayementTypeTableNameMasterDataError,
        });
    }
        //For editing data
    else {
        ajaxRequestHandler({
            url: SSIUrl.getPayementTypeTableNameMasterData + "?paymenttypeId=" + viewmodelPaymentTypeAscFee.PaymentTypeId + "&userText=" + '' + "&nodeId=" + nodeId,
            type: "GET",
            dataType: 'json',
            fnSuccess: OnGetPayementTypeTableNameMasterDataForEditedSuccess,
            fnError: OnGetPayementTypeTableNameMasterDataError,
        });
    }


    function OnGetPayementTypeTableNameMasterDataSuccess(json) {
        //In success function bind data into combobox using kendo 
        $("#ddlAscFeeSschedule").addClass('customDefaultColor');
        var paymentTypeTableNameComboBoxData = [];
        if (json != null) {
            $.each(json, function (i, item) {
                paymentTypeTableNameComboBoxData.push({ 'Text': item.Text, 'Value': item.Value });
            });
        }
        //In success function bind data into combobox using kendo 
        $("#ddlAscFeeSschedule").kendoComboBox({
            dataTextField: "Text",
            dataValueField: "Value",
            placeholder: "Select Table Name",
            dataSource: paymentTypeTableNameComboBoxData,
            filter: "contains",
            suggest: true
          });
    }
    
    function OnGetPayementTypeTableNameMasterDataForEditedSuccess(json) {
        //In success function bind data into combobox using kendo 
        var editpaymentTypeTableNameComboBoxData = [];
        if (json != null) {
            $.each(json, function (i, item) {
                editpaymentTypeTableNameComboBoxData.push({ 'Text': item.Text, 'Value': item.Value });
            });
        }
        $("#ddlAscFeeSschedule").kendoComboBox({
            dataTextField: "Text",
            dataValueField: "Value",
            placeholder: "Select Table Name",
            dataSource: editpaymentTypeTableNameComboBoxData,
            filter: "contains",
            suggest: true
        });
    }

    function OnGetPayementTypeTableNameMasterDataError(jqXHR, textStatus, errorThrown) {
        alert('Error:' + jqXHR + " Status= " + textStatus + " Req= " + errorThrown);
    }

    //Setting the Schedule Amount on Add page
    if (viewmodelPaymentTypeAscFee.IsEdit == 'False') {
        $("input[type='radio'][name='ASCOption'][value=1]").attr('checked', 'checked');
        selectedOption = 1;
    } else {
        //Checks the Schedule Amount radio button on Edit Page
        if (viewmodelPaymentTypeAscFee.OptionSelection == 1) {
            $("input[type='radio'][name='ASCOption'][value=1]").attr('checked', 'checked');
        } else {
            $("input[type='radio'][name='ASCOption'][value=2]").attr('checked', 'checked');
        }

        //sets the selectedOption  based on the checked radio buttons
        selectedOption = $("input[type='radio'][name='ASCOption']:checked").val();
    }

    viewmodelPaymentTypeAscFee.OptionSelection = selectedOption;

    //gets the value of the selected radio options
    $('input.asc-options').change(function () {
        selectedOption = $("input[type='radio'][name='ASCOption']:checked").val();
        viewmodelPaymentTypeAscFee.OptionSelection = selectedOption;
    });


    setTimeout(function () {
        $('#txtPrimary').siblings('input:visible').focus();
    }, 1000);
});






