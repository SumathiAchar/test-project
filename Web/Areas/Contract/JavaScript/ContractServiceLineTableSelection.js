var tabledatasource = [];
var data = null;
tempList = [];
var messageTableIncorrect = "Incorrect table.";

if (tableList == null) {
    tableList = [];
}
else {
    $.each(tableList, function (i, item) {
        var claimfieldText = item.Text;
        tempList.push({
            'ClaimFieldtext': claimfieldText,
            'ClaimFieldDocId': item.ClaimFieldDocId + '-' + item.ClaimFieldId,
            'ClaimFieldId': item.ClaimFieldId,
            'TableName': item.TableName,
            'OperatorType': item.OperatorType,
            'Operator': item.Operator
        });
    });
}

var dataSourceClaimandTable = new kendo.data.DataSource({
    data: tempList
});

function loadTableData() {
    function filterRequest() {
        return {
            contractId: viewmodelServiceClaimField.ContractId,
            serviceTypeId: viewmodelServiceClaimField.ContractServiceTypeId,
            tableType: $("#combo-claim-field").data("kendoDropDownList").value(),
            userText: function () {
                if ($("#combo-tables").data("kendoComboBox").text() != "" && $("#combo-tables").data("kendoComboBox").text() != "Select Table")
                    return $("#combo-tables").data("kendoComboBox").text()
                else
                    return "";
            }
        };
    }

    //kendoComboBox for selecting tables
    $("#combo-tables").kendoComboBox({
        dataTextField: "Text",
        dataValueField: "Value",
        placeholder: "Select Table",
        dataSource: {
            serverFiltering: true,
            transport: {
                read: {
                    cache: false,
                    url: SSIUrl.getTableSelectionTables,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: filterRequest,
                    global: false
                }
            }
        },
        filter: "contains",
        autoBind: false,
        minLength: 2,
        change: function () {
            var tablesList = $('#combo-tables').data("kendoComboBox");
            tablesList.refresh();
        }
    });
}
//Showing values using grid
$("#grid-service").kendoGrid({
    dataSource: dataSourceClaimandTable,
    selectable: true,
    columns: [
        { field: "ClaimFieldId", hidden: true },
        { field: "ClaimFieldtext", title: "Claim Field" },
        { field: "OperatorType", title: "Operator" },
        { field: "TableName", title: "Table Name" },
        { field: "ClaimFieldDocId", hidden: true },
        {
            title: "Actions",
            command: {
                text: "Delete",
                click: function (e) {
                    var grid = $("#grid-service").data("kendoGrid");
                    var row = $(e.target).closest("tr");
                    var dataClaim = grid.dataItem(row);
                    grid.dataSource.remove(dataClaim);
                    tabledatasource.push({
                        value: row[0].cells[4].innerHTML,
                        text: row[0].cells[3].innerHTML
                    });
                    $("#combo-claim-field").data("kendoDropDownList").select(0);
                    $("#combo-claim-field").data("kendoDropDownList").trigger("change");
                }
            }
        }
    ],
    scrollable: true
});
$(document).ready(function () {
    //just for testing
    $("#combo-tables").kendoComboBox({})
    addTableSelectionClaimFieldOperators();
    $("#combo-tables").data("kendoComboBox").enable(false);
    if (viewmodelServiceClaimField.IsEdit == 'False') {
        //to store values in database and bind dropdown values
        ajaxRequestHandler({
            url: SSIUrl.getClaimfieldsandTables + "?contractId=" + viewmodelServiceClaimField.ContractId + "&serviceTypeId=" + viewmodelServiceClaimField.ContractServiceTypeId + "&moduleId=" + viewmodelServiceClaimField.ModuleId,
            type: "GET",
            dataType: "json",
            fnSuccess: OnGetClaimfieldsandTablesSuccess
        });
    } else {
        ajaxRequestHandler({
            url: SSIUrl.getClaimfieldsandTables + "?contractId=" + viewmodelServiceClaimField.ContractId + "&serviceTypeId=" + viewmodelServiceClaimField.ContractServiceTypeId + "&moduleId=" + viewmodelServiceClaimField.ModuleId,
            type: "GET",
            dataType: "json",
            fnSuccess: OnGetClaimfieldsandTablesIsEditedSuccess
        });
    }
    //Check Box check on label click
    CheckBoxLabelClickEvent();
    //Combo box place holder default color in ie.
    ComboBoxDefaultColor("combo-tables");
});

//To add the values in Grid
$("#btn-add-table-selection").click(function () {
    if ($("#combo-claim-field").data("kendoDropDownList").value() != '' && $("#combo-tables").data("kendoComboBox").value() != '') {
        var isExist = false;
        var isValidTable = false;
        var currentTableId = $("#combo-claim-field").val();
        var currentTableName = $("#combo-tables").val();
        if (dataSourceClaimandTable._data.length > 0) {
            var data = dataSourceClaimandTable._data;
            jQuery.each(data, function (i, val) {
                if (data[i].ClaimFieldId === currentTableId && data[i].ClaimFieldDocId === currentTableName) {
                    registerGridPopup("Claim field table selection already exists. Please try again.", "OK", null, null);
                    isExist = true;
                }
            });
        }
        if ($("#combo-tables").data("kendoComboBox")) {
            var comboboxData = $("#combo-tables").data("kendoComboBox").dataSource._data;
            $.each(comboboxData, function (index, table) {
                if (currentTableName === table.Value) {
                    isValidTable = true;
                }
            });
        }
        if (!isValidTable) {
            registerGridPopup("Incorrect table.", "OK", null, null);
        }
        if (!isExist && isValidTable) {
            var claimFieldtext = $("#combo-claim-field").data("kendoDropDownList").text();
            if ($("#chk-include-table-selection").prop("checked") && claimFieldtext=="HCPCS/RATE/HIPPS") {
                claimFieldtext = claimFieldtext + " - Include Modifiers "
            }
            dataSourceClaimandTable.add({
                ClaimFieldId: $("#combo-claim-field").val(),
                ClaimFieldtext: claimFieldtext,
                ClaimFieldDocId: $("#combo-tables").val(),
                TableName: $("#combo-tables").data("kendoComboBox").text(),
                Operator: $("#combo-operators").val(),
                OperatorType: $("#combo-operators").data("kendoDropDownList").text()
            });
            var data = dataSourceClaimandTable._data;
            for (var i = 0; i < dataSourceClaimandTable._data.length; i++) {
                if ($("#combo-tables").val() == dataSourceClaimandTable._data[i].Value) {
                    var x = dataSourceClaimandTable._data[i];
                    dataSourceClaimandTable.remove(x);
                }
            }
        }
        $("#combo-claim-field").data("kendoDropDownList").select(0);
        $("#combo-operators").data("kendoDropDownList").select(0);
        $("#combo-tables").data("kendoComboBox").select(0);
        $("#combo-claim-field").data("kendoDropDownList").trigger("change");
        $(".checkBox-include-modifier").addClass("hide");
        $(".checkBox-include-modifier").parent().addClass('hide');
    } else {
        registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
    }
});

function addTableSelectionClaimFieldOperators() {
    ajaxRequestHandler({
        url: SSIUrl.getTableSelectionClaimFieldOperators,
        type: "GET",
        dataType: "json",
        fnSuccess: OnGetTableSelectionClaimFieldOperatorsSuccess
    });
}

function OnGetTableSelectionClaimFieldOperatorsSuccess(json) {
    //In success function bind data into combobox using kendo 
    var tableSelectionClaimFeildOperatorComboBoxData = [];
    if (json != null && json.claimFeildOperatorList != null) {
        $.each(json.claimFeildOperatorList, function (i, item) {
            tableSelectionClaimFeildOperatorComboBoxData.push({ 'Text': item.Text, 'Value': item.Value });
        });

        var dropdownlist = $("#combo-operators").kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: tableSelectionClaimFeildOperatorComboBoxData
        });
        $("#combo-operators").data("kendoDropDownList").select(0);
    }
}


//Saving ClaimFields and Tables
$("#save-claim-table").click(function () {
   
    if (dataSourceClaimandTable._data.length <= 0) {
        registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
    } else {
        $("#save-claim-table").attr('disabled', 'disabled');
        $("#image-logo-table").show();
        var serviceLineClaimandTable = [];
        for (var i = 0; i < dataSourceClaimandTable._data.length; i++) {
           var id = dataSourceClaimandTable._data[i].ClaimFieldDocId;
            var claimfieldId = id.split("-")[0];
            serviceLineClaimandTable.push({
                'ClaimFieldId': dataSourceClaimandTable._data[i].ClaimFieldId,
                'ClaimFieldDocId': claimfieldId.toString(),
                'ContractId': viewmodelServiceClaimField.ContractId,
                'ContractServiceTypeId': viewmodelServiceClaimField.ContractServiceTypeId,
                'ServiceLineTypeId': viewmodelServiceClaimField.ServiceLineTypeId,
                'ContractServiceLineId': viewmodelServiceClaimField.ContractServiceLineId,
                'Operator': dataSourceClaimandTable._data[i].Operator
            });
        }
        var serviceLineClaimandTablesave = { 'listofServiceLineClaimandTable': serviceLineClaimandTable };
        ajaxRequestHandler({
            url: SSIUrl.addEditServiceLineClaimAndTables,
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(serviceLineClaimandTablesave),
            fnSuccess: OnAddEditServiceLineClaimAndTablesSuccess
        });
    }
});

function OnAddEditServiceLineClaimAndTablesSuccess(json) {
    if (json.IsTableValid === false) {
        registerGridPopup(messageTableIncorrect, "OK", null, null);
    }  else if (json.success != false) {
        $('.service-line').closest(".k-window-content").data("kendoWindow").close();
        $("#image-logo-table").hide();
        ReDrawShapes();
    } else {
        $('.service-line').closest(".k-window-content").data("kendoWindow").close();
        return false;
    }
}

function OnGetClaimfieldsandTablesSuccess(json) {
    //In success function bind data into combobox using kendo 
    var claimData = [];
    var tableData = [];
    if (json != null && json.claimFeilds != null) {
        $.each(json.claimFeilds, function (i, item) {
            claimData.push({ 'Text': item.Text, 'Value': item.Value });
        });
    }
    if (json != null && json.tables != null) {
        $.each(json.tables, function (i, item) {
            tableData.push({ 'Text': item.Text, 'Value': item.Value });
        });
    }
    //Datasource for getting table values
    tabledatasource = new kendo.data.DataSource({ data: tableData });
    //Datasource for getting cliam fields
    var claimdatasource = new kendo.data.DataSource({ data: claimData });
    var claim = $("#combo-claim-field").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: claimdatasource,
        optionLabel: "Select Claim Field",
        serverFiltering: true,
        autoBind: true,
        change: function () {
            $("#combo-tables").data("kendoComboBox").text("");
            $("#combo-tables").addClass('customDefaultColor');
            if ($("#combo-claim-field").data("kendoDropDownList").text() == 'Select Claim Field') {
                $("#combo-tables").data("kendoComboBox").input.attr("placeholder", "Select Table Name");
                $("#combo-tables").data("kendoComboBox").enable(false);               
            } else {
                $("#combo-tables").data("kendoComboBox").enable(true);
            }
            if ($("#combo-claim-field").data("kendoDropDownList").text() == "HCPCS/RATE/HIPPS") { //hcpcs code show the include modifier checkbox
                
                $(".checkBox-include-modifier").removeClass("hide");
                $(".checkBox-include-modifier").parent().removeClass('hide');
            } else {
                $(".checkBox-include-modifier").addClass("hide");
                $(".checkBox-include-modifier").parent().addClass('hide');
            }
            var value = this.value();
            $("#grid-service").data("kendoGrid").clearSelection();
            //Getting ClaimdocId
                var id = "-" + value;
                var asc = "-" + 21;
                var fee = "-" + 22;
                var drg = "-" + 23;

                //filtering values based on Id
                if (id == -4) //if Claim field selection is "HCPCS/RATE/HIPPS"
                {
                    tabledatasource.one("change", function () {
                    }).filter({
                        logic: "or",
                        filters: [
                            { field: "Value", operator: "endswith", value: id },
                            { field: "Value", operator: "endswith", value: asc },
                            { field: "Value", operator: "endswith", value: fee }
                        ]
                    });
                } else if (id == -8) //If Claim field selection is "DRG(I) 
                {
                    tabledatasource.one("change", function () {

                    }).filter({
                        logic: "or",
                        filters: [
                            { field: "Value", operator: "endswith", value: id },
                            { field: "Value", operator: "endswith", value: drg }
                        ]
                    });
                } else {
                    tabledatasource.one("change", function () {

                    }).filter({
                        field: "Value",
                        operator: "endswith",
                        value: id
                    });
                }
                loadTableData();
        }
    }).data("kendoDropDownList");
    $('#window-service-line').show();
}

function OnGetClaimfieldsandTablesIsEditedSuccess(json) {
    //In success function bind data into combobox using kendo 
    var claimData = [];
    var tableData = [];
    if (json != null && json.claimFeilds != null) {
        $.each(json.claimFeilds, function (i, item) {
            claimData.push({ 'Text': item.Text, 'Value': item.Value });
        });
    }
    if (json != null && json.tables != null) {
        $.each(json.tables, function (i, item) {
            tableData.push({ 'Text': item.Text, 'Value': item.Value });
        });
    }
    //Datasource for getting table values
    var tabledatasource = new kendo.data.DataSource({ data: tableData });
    //Datasource for getting cliam fields
    var claimdatasource = new kendo.data.DataSource({ data: claimData });
    //DropdownList for binding claimfields
    var claim = $("#combo-claim-field").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: claimdatasource,
        optionLabel: "Select Claim Field",
        serverFiltering: true,
        //To bind TableNames associated with claimfield using claimFieldId
        autoBind: true,
        change: function () {
            $("#combo-tables").data("kendoComboBox").text("");
            $("#combo-tables").addClass('customDefaultColor');
            if ($("#combo-claim-field").data("kendoDropDownList").text() == 'Select Claim Field' ) {
                $("#combo-tables").data("kendoComboBox").input.attr("placeholder", "Select Table Name");
                $("#combo-tables").data("kendoComboBox").enable(false);
            } else {
                $("#combo-tables").data("kendoComboBox").enable(true);
            }
            if ($("#combo-claim-field").data("kendoDropDownList").text() == "HCPCS/RATE/HIPPS") { //hcpcs code show the include modifier checkbox
                $(".checkBox-include-modifier").removeClass("hide");
                $(".checkBox-include-modifier").parent().removeClass('hide');
                $(".checkBox-include-modifier").prop("checked",true);
            } else {
                $(".checkBox-include-modifier").addClass("hide");
                $(".checkBox-include-modifier").parent().addClass('hide');
            }
            var value = this.value();
            $("#grid-service").data("kendoGrid").clearSelection();
            //Getting ClaimdocId
            if (value) {
                var id = "-" + value;
                var asc = "-" + 21; //AscFee Schedule Id
                var fee = "-" + 22; //Fee Schedule Id
                var drg = "-" + 23; //Drg Weight Id

                //filtering values based on Id
                if (id == -4) //if Claim field selection is "HCPCS/RATE/HIPPS"
                {
                    tabledatasource.one("change", function () {
                    }).filter({
                        logic: "or",
                        filters: [
                            { field: "Value", operator: "endswith", value: id }, //add the document uploaded for HIPPS    
                            { field: "Value", operator: "endswith", value: asc }, //add the document uploaded for AscFee schedule                               
                            { field: "Value", operator: "endswith", value: fee } //add the document uploaded for Fee schedule         
                        ]
                    });
                } else if (id == -8) //If Claim field selection is "DRG(I)" 
                {
                    tabledatasource.one("change", function () {
                    }).filter({
                        logic: "or",
                        filters: [
                            { field: "Value", operator: "endswith", value: id }, //add the document uploaded for DRG(I)    
                            { field: "Value", operator: "endswith", value: drg } //add the document uploaded for DRG Weight      
                        ]
                    });
                } else {
                    tabledatasource.one("change", function () {
                    }).filter({
                        field: "Value",
                        operator: "endswith",
                        value: id
                    });
                }
            }
            loadTableData();
        }
    }).data("kendoDropDownList");
    $('#window-service-line').show();
}
