var reorderColumnUid = [];
var initialSelectedModel = [];
var selectedColumnsList;
var selectedUpList;
var selectedList = [];
var availableClaimColumnList = null, selectedClaimColumnList = null, dataSourceAvailableClaimColumn = null, dataSourceSelectedClaimColumn = null;
$("#btnAddColumn").click(function () {
    var selectedClaimColumns = '';
    var selectedNames = '';
    var selectedUId = '';
    var selectedIndex = '';
    var lvAvailableColumns = $("#listAvailableColumns").data("kendoListView");
    var txt = $("#listAvailableColumns").find('.k-state-selected');
    $.each(txt, function (index, value) {
        selectedIndex = txt[index];
        selectedClaimColumns += selectedIndex.value + "##";
        selectedNames += selectedIndex.textContent + "##";
        selectedUId += selectedIndex.getAttribute('data-uid') + "##";
    });
    selectedClaimColumns = selectedClaimColumns.substring(0, selectedClaimColumns.length - 2);
    selectedNames = selectedNames.substring(0, selectedNames.length - 2);
    selectedUId = selectedUId.substring(0, selectedUId.length - 2);
    var splitedId = selectedClaimColumns.split('##');
    var splitedName = selectedNames.split('##');
    var splittedUId = selectedUId.split('##');
    $.each(splitedId, function (indexsplitedId) {
        if (splitedId != "" && selectedNames != "") {
            dataSourceSelectedClaimColumn.add({ ClaimColumnOptionId: splitedId[indexsplitedId], ColumnName: splitedName[indexsplitedId] });
        }
        //FIXED-2016-R3-S2: If local variable has the same name as an outer scope declaration it is code issue. value here is used in outer and inner scope. It should be renamed to make code more clear.
        $.each(lvAvailableColumns, function (index) {
            if (splitedName[indexsplitedId] != "") {
                var dataToRemove = lvAvailableColumns.dataSource.getByUid(splittedUId[indexsplitedId]);
                lvAvailableColumns.dataSource.remove(dataToRemove);
                return false;
            }
        });
    });
    selectedClaimColumns = '';
    selectedNames = '';
    selectedUId = '';
});
// Move all the available Claim Columns to Selected Claim Columns listview
$("#btnAddAllColumn").click(function () {
    var selectedColumnList = $("#listAvailableColumns").data("kendoListView");
    var selectedColumns = selectedColumnList.dataSource._view;
    addAllAvailableColumn(selectedColumns);
    $("#listAvailableColumns").data('kendoListView').dataSource.data([]);
});

//Move all the selected Claim Columns to available Claim Columns listview
$("#btnRemoveAllColumn").click(function () {
    var selectedColumnList = $("#listSelectedColumns").data().kendoListView.dataSource._data;
    $.merge(selectedColumnList, dataSourceAvailableClaimColumn._data);
    dataSourceAvailableClaimColumn.data(selectedColumnList);
    $("#listSelectedColumns").data('kendoListView').dataSource.data([]);
});

$("#btnRemoveColumn").click(function () {
    var lvClaimColumnSelected = $("#listSelectedColumns").data("kendoListView");
    var rmvselectedps = '';
    var rmvselectedNames = '';
    var uIdToRemoveColumnOptionId = '';
    var txtRmv = $("#listSelectedColumns").find('.k-state-selected');
    var removeIndex = '';
    $.each(txtRmv, function (index, value) {
        removeIndex = txtRmv[index];
        rmvselectedps += removeIndex.value + "##";
        rmvselectedNames += removeIndex.textContent + "##";
        uIdToRemoveColumnOptionId += removeIndex.getAttribute('data-uid') + "##";
    });
    rmvselectedps = rmvselectedps.substring(0, rmvselectedps.length - 2);
    rmvselectedNames = rmvselectedNames.substring(0, rmvselectedNames.length - 2);
    uIdToRemoveColumnOptionId = uIdToRemoveColumnOptionId.substring(0, uIdToRemoveColumnOptionId.length - 2);
    var rmvsplitedId = rmvselectedps.split('##');
    var rmvsplitedName = rmvselectedNames.split('##');
    var rmvsplitterUId = uIdToRemoveColumnOptionId.split('##');
    $.each(rmvsplitedId, function (indexrmvsplitedId) {
        dataSourceAvailableClaimColumn.add({ ClaimColumnOptionId: rmvsplitedId[indexrmvsplitedId], ColumnName: rmvsplitedName[indexrmvsplitedId] });
        //FIXED-2016-R3-S2: If local variable has the same name as an outer scope declaration it is code issue. value here is used in outer and inner scope. It should be renamed to make code more clear.
        $.each(lvClaimColumnSelected, function (index) {
            if (rmvsplitedName[indexrmvsplitedId] != "") {
                var dataToRemove = lvClaimColumnSelected.dataSource.getByUid(rmvsplitterUId[indexrmvsplitedId]);
                lvClaimColumnSelected.dataSource.remove(dataToRemove);
                return false;
            }
        });
    });
    rmvselectedps = '';
    rmvselectedNames = '';
    uIdToRemoveColumnOptionId = '';
});
$("#btnAddColumn").attr('disabled', 'disabled');
$("#btnRemoveColumn").attr('disabled', 'disabled');
$("#listAvailableColumns").click(function () {
    $("#btnAddColumn").removeAttr('disabled');
    $("#listSelectedColumns").data("kendoListView").clearSelection();
});

$("#listSelectedColumns").click(function () {
    $("#btnRemoveColumn").removeAttr('disabled');
    $("#listAvailableColumns").data("kendoListView").clearSelection();
});
// Click on up button
$("#btnColumnOptionUp").off('click').on('click', function () {
    selectedColumnsList = $("#listSelectedColumns").data("kendoListView");
    selectedUpList = $("#listSelectedColumns").find('.k-state-selected');
    if (selectedUpList.length > 0) {
        $("#listSelectedColumns").moveSelectedUp();
        moveUpDownSelectedColumn();
        }
   });
// Click on Down button
$("#btnColumnOptionDown").off('click').on('click', function () {
    selectedColumnsList = $("#listSelectedColumns").data("kendoListView");
    selectedUpList = $("#listSelectedColumns").find('.k-state-selected');
    if (selectedUpList.length > 0) {
        $("#listSelectedColumns").moveSelectedDown();
        moveUpDownSelectedColumn();
    }
});
$("#btnColumnOptionSave").off('click').on('click', function () {
    if ($("#listSelectedColumns").html() == "") {
        registerGridPopup("Please select at least one value from available list.", "OK", null, null);
        return false;
    }
    else {
        saveClaimColumnOptions();
    }
    return false;
});
$("#btnColumnOptionCancel").off('click').on('click', function () {
    $("#availableColumns").closest(".k-window-content").data("kendoWindow").close();
});

//FIXED-2016-R3-S2: Why code comment is there? Remove not used code instead of commenting.  
function saveClaimColumnOptions() {
    reorderClaimColumns();
    selectedColumnFields = [];
    selectedColumnFields = $("#listSelectedColumns").data("kendoListView").dataSource.data();
    var selectedColumnList = '';
    //loop through selectedColumnFields
    $.each(selectedColumnFields, function (index, value) {
        selectedColumnList = selectedColumnList + "," + selectedColumnFields[index].ClaimColumnOptionId;
    });
    //removing first comma from selectedFieldsList 
    selectedColumnList = selectedColumnList.slice(1);
    //calling controller method
    ajaxRequestHandler({
        url: SSIUrl.saveClaimColumnOptions,
        type: "POST",
        dataType: "json",
        data: JSON.stringify({ claimColumnOptionIds: selectedColumnList, userId: loggedInUserId }),
        fnSuccess: saveClaimColumnOptionsSuccess
    });
}

function saveClaimColumnOptionsSuccess(json) {
    isClaimColumnSavedSuccessfully = true;
    isFirstOpenClaimRecords = true;
    $("#availableColumns").closest(".k-window-content").data("kendoWindow").close();
    $("#open-claim-gridview").empty();
    var fieldNameExists = false, fieldNameExistsForSort = false;
    // If filtered column is not in selected column List then clear filter.
    var filterFields = $("#open-claim-gridview").data("kendoGrid").dataSource.filter();
    if (filterFields != null && filterFields.filters.length > 0) {
        $.each(filterFields.filters, function (index, item) {
            var columnName = getFilterTitle(item.field);
            if (!isColumnExists(columnName)) {
                fieldNameExists = true;
                return false;
            }
        });
        if (fieldNameExists) {
            $("#open-claim-gridview").data("kendoGrid").dataSource.filter({});
        }
    }
    // If sorted column is not in selected column List then clear sorting.
    var sortFields = $("#open-claim-gridview").data("kendoGrid").dataSource.sort();
    if (sortFields != null && sortFields.length > 0) {
        $.each(sortFields, function (index, item) {
            var columnName = getFilterTitle(item.field);
            if (!isColumnExists(columnName)) {
                fieldNameExistsForSort = true;
                return false;
            }
        });
        if (fieldNameExistsForSort) {
            $("#open-claim-gridview").data("kendoGrid").dataSource.sort({});
        }
    }
    bindOpenClaimGrid(claimDataSource);
}

function getFilterTitle(filterField) {
    var returnedData = $.grep($("#open-claim-gridview").data("kendoGrid").columns, function (element, index) {
        return element.field == filterField;
    });
    return (filterField == "Reviewed") ? "Reviewed" : returnedData[0].title;
}

function isColumnExists(columnName) {
    var returnedData = $.grep(selectedColumnFields, function (element, index) {
        return element.ColumnName == columnName;
    });
    return (columnName == "Reviewed") ? true : (returnedData[0] != null);
}

function reorderClaimColumns() {
    selectedColumnFields = [];
    reorderColumnUid = [];
    selectedColumnFields = $("#listSelectedColumns").data("kendoListView").dataSource.data();
    $.each(SelectedClaimColumnOptionsViewModel, function (index, value) {
        reorderColumnUid.push(SelectedClaimColumnOptionsViewModel[index]["ColumnName"]);
    });
}

function vaildateReorderClaimColumn() {
    reorderClaimColumns();
    isModelChanged = false;
    initialSelectedModel = [];
    $.each(selectedColumnFields, function (index, value) {
        initialSelectedModel.push(selectedColumnFields[index]["ColumnName"]);
    });
    if (!(reorderColumnUid.toString() === initialSelectedModel.toString())) {
        isModelChanged = true;
    }
}

function addAllAvailableColumn(SelectedColumns) {
    var selectedId = '';
    var selectedAvailableNames = '';
    var selectedAvailableUId = '';
    var selectedColumnIndex = '';
    $.each(SelectedColumns, function (index, value) {
        selectedColumnIndex = SelectedColumns[index];
        selectedId += selectedColumnIndex.ClaimColumnOptionId + "##";
        selectedAvailableNames += selectedColumnIndex.ColumnName + "##";
        selectedAvailableUId += selectedColumnIndex.uid + "##";
    });
    selectedId = selectedId.substring(0, selectedId.length - 2);
    selectedAvailableNames = selectedAvailableNames.substring(0, selectedAvailableNames.length - 2);
    selectedAvailableUId = selectedAvailableUId.substring(0, selectedAvailableUId.length - 2);
    var splitedId = selectedId.split('##');
    var splitedName = selectedAvailableNames.split('##');
    var splittedUId = selectedAvailableUId.split('##');
    $.each(splitedId, function (indexsplitedId) {
        if (splitedId != "" && selectedAvailableNames != "") {
            dataSourceSelectedClaimColumn.add({ ClaimColumnOptionId: splitedId[indexsplitedId], ColumnName: splitedName[indexsplitedId] });
        }
    });
    selectedId = '';
    selectedAvailableNames = '';
    selectedAvailableUId = '';
}

function moveUpDownSelectedColumn() {
    reorderColumnUid = [];
    var position = '';
    var count = 0;
    $("#listSelectedColumns div").each(function () {
        reorderColumnUid.push({ 'uid': $(this).attr('data-uid') });
    });
    selectedList = [];
    selectedList = selectedUpList;
    var selectedColumns = selectedColumnsList.dataSource._view;
    $("#listSelectedColumns").data('kendoListView').dataSource.data([]);
    $.each(reorderColumnUid, function (index) {
        $.each(selectedColumns, function (indexSplitedId, value) {
            if (reorderColumnUid[index].uid == selectedColumns[indexSplitedId].uid) {
                dataSourceSelectedClaimColumn.add({ ClaimColumnOptionId: selectedColumns[indexSplitedId].ClaimColumnOptionId, ColumnName: selectedColumns[indexSplitedId].ColumnName });
            }
        });
    });
    $.each(selectedList, function (index, value) {
        $("#listSelectedColumns div").each(function () {
            if (value.value == $(this)[0].value) {
                $(this).addClass('k-state-selected');
                count++;
                if (count == 1)
                    position = $("#listSelectedColumns div").index($(this).addClass('k-state-selected'));
            }
        });
    });
    //Scrolls to the selected list
    var $target = $("#listSelectedColumns");
    $target.scrollTo('div:eq(' + position + ')', 0);

}
function bindOpenClaimColumnModel(availableClaimColumnList, selectedClaimColumnList) {
    if (availableClaimColumnList == null) {
        availableClaimColumnList = [];
    }


    if (selectedClaimColumnList == null) {
        selectedClaimColumnList = [];
    }
    dataSourceAvailableClaimColumn = new kendo.data.DataSource({
        data: availableClaimColumnList,
        sort: { field: "ColumnName", dir: "asc" }
    });

    dataSourceSelectedClaimColumn = new kendo.data.DataSource({
        data: selectedClaimColumnList,
        sort: { field: "ColumnName" }
    });
    //Combobox for displaying available Facilities
    $("#listAvailableColumns").kendoListView({
        selectable: "multiple",
        dataSource: dataSourceAvailableClaimColumn,
        template: kendo.template($("#AvailableColumnsTemplate").html())
    });

    //Listview for displaying the selected Facilities
    $("#listSelectedColumns").kendoListView({
        selectable: "multiple",
        dataSource: dataSourceSelectedClaimColumn,
        template: kendo.template($("#SelectedColumnsTemplate").html())
    });
    SelectedClaimColumnOptionsViewModel = jQuery.extend(true, [], selectedClaimColumnList);
    ClaimColumnOptionsViewModel = kendo.observable({
        AvailableColumnList: availableClaimColumnList,
        SelectedColumnList: selectedClaimColumnList
    });
    kendo.bind($("#availableColumns"), ClaimColumnOptionsViewModel);
}
