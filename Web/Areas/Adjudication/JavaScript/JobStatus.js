//Initialize Job Status click event
var reportVarainceParameter = {};
var reportParameterJob = {};
var isOpenClaimsWindow = false;
var requestIndexValues = 1;
function InitJobStatusTabClickEvent() {
    $("#checkbox-refresh").click(function () {
        isAutoRefresh = $("#checkbox-refresh").prop('checked');
        ajaxRequestHandler({
            url: SSIUrl.updateAutoRefresh,
            data: JSON.stringify({ 'isAutoRefresh': isAutoRefresh }),
            type: 'POST',
            cache: false,
            dataType: 'json'
        });
        RefreshGrid();
    });
    $(".job-click").click(function () {
        $('.diff-saveclose-container').show()
        $('.diff-saveclose-container').removeClass('preventrefresh');
        BindJobStatusDropdown();
        $('#checkbox-refresh').prop('checked', isAutoRefresh);
        BindGrid(jobStatusVariables.ddlJobStatus);
    });

    $(".alert-click").click(function () {
        $('.diff-saveclose-container').hide();
    });
}

//TODO - From nowhere we are calling this method. KJust check and remove if its not in use
function loadingElement() {
    $("#job-image-logo").show();
}

function OpenClaimLink(taskId) {
    isOpenClaimsWindow = true;
    //When open claims window is opened, the job autorefresh is stopped
    $.each(jobStatusVariables.gridAutoRefreshed, function (i, item) {
        clearTimeout(item);
    });
    $("#open-claim-container").html('');
    $("#open-claim-container").kendoWindow();
    var jobsWindow = $("#open-claim-container").data("kendoWindow");
    jobsWindow.setOptions({
        modal: true,
        actions: [
            "Maximize", "Close"
        ],
        width: "70%",
        height: "500px",
        draggable: false,
        position: {
            top: "10%"
        },
        resizable: false,
        close: function () {
            isOpenClaimsWindow = false;
            $("#grid").val('');
            $("#grid").html('');
            BindGrid(jobStatusVariables.ddlJobStatus);
        },
        visible: false
    });
    var grid = $("#grid").data("kendoGrid");
    var selectedRow = grid.select();
    rowIndex = selectedRow[0].rowIndex;
    var modelId = grid._data[rowIndex].ModelId;
    jobsWindow.refresh({
        url: SSIUrl.openClaimGridView + "?selectionCriteria=" + taskId.toString() + "&dateFrom=" + null + "&dateTo=" + null + "&modelId=" + modelId + "&dateType=" + null + "&zoneDateTime=" + getCurrentDateTime(),
        cache: false,
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
    });
    jobsWindow.center().open();
    jobsWindow.title("Open Claims Window ").wrapper.css({
        "width": "70%",
        "height": "500px",
        "-webkit-transform": "scale(1)"
    });
}

// For downloading the adjudication report
function ReAdjudicate(taskId) {
    ajaxRequestHandler({ url: SSIUrl.reAdjudicate + "?taskId=" + taskId, type: 'POST', fnSuccess: OnReAdjudicateSuccess })
}


function OnReAdjudicateSuccess(json) {
    if (json.returnValue > 0) {
        RefreshGrid();
    } else if (json.returnValue == -1) {
        registerGridPopup("This action cannot be performed as the model associated with this job has been deleted.", "OK", null, null);
    } else {
        registerGridPopup("Refresh failed. Please contact administrator.", "OK", null, null);
    }
}

// For downloading the adjudication report
function DownloadAdjudicationReport(taskId) {
    var grid = $("#grid").data("kendoGrid");
    var selectedRow = grid.select();
    rowIndex = selectedRow[0].rowIndex;
    var modelId = grid._data[rowIndex].ModelId;
    var facilityId = grid._data[rowIndex].FacilityId;

    reportParameterJob = {
        modelId: modelId,
        criteria: "-99|1|" + taskId, // Sending the request name criteria
        facilityId: facilityId,
        currentDateTime: getCurrentDateTime()
    };
    ajaxRequestHandler({ url: SSIUrl.isModelExist + "?modelId=" + modelId, type: 'POST', fnSuccess: OnDownloadAdjudicationReportSuccess })

}

function OnDownloadAdjudicationReportSuccess(json) {
    if (json.IsSuccess) {
        showAdjudicationReport(reportParameterJob);
    } else {
        registerGridPopup("This action cannot be performed as the model associated with this job has been deleted.", "OK", null, null);
    }
}


// For downloading the variance report
function DownloadVarianceReport(taskId) {
    var grid = $("#grid").data("kendoGrid");
    var selectedRow = grid.select();
    rowIndex = selectedRow[0].rowIndex;
    var modelId = grid._data[rowIndex].ModelId;
    var facilityId = grid._data[rowIndex].FacilityId;

    reportVarainceParameter = {
        nodeId: modelId,
        criteria: "-99|1|" + taskId, // Sending the request name criteria
        reportType: "1", // At claim level
        fileType: "1", // File type is PDF
        currentDateTime: getCurrentDateTime()
    };

    ajaxRequestHandler({ url: SSIUrl.isModelExist + "?modelId=" + modelId, type: 'POST', fnSuccess: OnDownloadVarianceReportSuccess })

}

function OnDownloadVarianceReportSuccess(json) {
    if (json.IsSuccess) {
        isReportSelection = false;
        showVarianceReport(reportVarainceParameter);
    } else {
        registerGridPopup("This action cannot be performed as the model associated with this job has been deleted.", "OK", null, null);
    }
}

//Refresh job grid
function RefreshGrid() {
    if ($("#checkbox-refresh").prop('checked') == true) {
        if ($(".job-click").is(".k-state-active")) {
            if ($('#grid').data('kendoGrid').dataSource._data.length > 0) {
                $('#grid').data('kendoGrid').dataSource.page($('#grid').data('kendoGrid').dataSource.page());
                $('#grid .k-grid-content').scrollTop(0);
            }
        }
    }
}

//Bind job status dropdown
function BindJobStatusDropdown() {
    ajaxRequestHandler({ url: SSIUrl.getJobStatus, type: 'POST', fnSuccess: OnGetJobStatusSuccess })
}

function OnGetJobStatusSuccess(json) {
    var jobStatus = [];
    if (json != null && json.jobStatus != null) {
        $.each(json.jobStatus, function (i, item) {
            jobStatus.push({ 'Text': item.FieldName, 'Value': item.FieldIdentityNumber });
        });
    }
    $("#ddl-jobstatus").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: jobStatus,
        change: function (e) {
            jobStatusVariables.ddlJobStatus = $("#ddl-jobstatus").val();
            $("#grid").val('');
            $("#grid").html('');
            BindGrid(jobStatusVariables.ddlJobStatus);
        }
    });
}

//Calculate completed percentage
function CalculateCompletionPercentage(NoOfClaimsAdjudicated, NoOfClaimsSelected, Status) {
    var completedPercentage;
    if (NoOfClaimsAdjudicated != 0 || NoOfClaimsSelected != 0)
        completedPercentage = RoundToTwoDigit((parseInt(NoOfClaimsAdjudicated) / parseInt(NoOfClaimsSelected)) * 100);
    else if (NoOfClaimsAdjudicated == 0 && NoOfClaimsSelected == 0 && Status == "Completed")
        completedPercentage = 100;
    else if (NoOfClaimsAdjudicated == 0 && NoOfClaimsSelected == 0 && Status == "Requested")
        completedPercentage = 0;
    else
        completedPercentage = 0;
    return completedPercentage + '%';
}

// Wrapping the text
function wrapText(RequestName) {
    if (RequestName.length <= 14)
        return RequestName;
    else {
        return RequestName.substring(0, 14) + '...';
    }
}

//Bind job grid data
function BindGrid(ddlJobStatus) {
    $("#grid").kendoGrid({
        dataSource: {
            serverPaging: true,
            pageSize: jobStatusVariables.pageSize,
            schema: {
                data: function (json, operation) {
                    return json.data;
                },
                total: "total",
            },
            transport: {
                read: {
                    url: SSIUrl.getAllJobs + "?jobStatusId=" + ddlJobStatus, //specify the URL which should return the records. This is the Read method of the HomeController.
                    data: function () {
                        $('#grid .k-grid-pager').find('.k-pager-info').html('');

                    },
                    contentType: "application/json",
                    type: "POST", //use HTTP POST request as by default GET is not allowed by ASP.NET MVC
                    cache: false,
                    global: true,
                    complete: function () {
                        //When open claims window is opened, the job autorefresh is stopped
                        if (jobStatusVariables.IsAutoRefreshEnabled.toLowerCase() == "true" && isOpenClaimsWindow == false) {
                            if (jobStatusVariables.gridAutoRefreshed.length > 0) {
                                $.each(jobStatusVariables.gridAutoRefreshed, function (i, item) {
                                    clearTimeout(item);
                                });
                                jobStatusVariables.gridAutoRefreshed = [];
                            }
                            var timer = setTimeout(RefreshGrid, jobStatusVariables.jobRefreshTime);
                            jobStatusVariables.gridAutoRefreshed.push(timer);
                        }
                    }
                },
                parameterMap: function (data, operation) {
                    return JSON.stringify(data);
                }
            }   

        },
        scrollable: true,
        selectable: 'row',
        pageable: true,
        columns: [
            {
                field: "RequestName",
                title: "Request Name",
                width: "20px",
                template: '#=wrapText(RequestName) #',
            },
            {
                field: "UserName",
                title: "User",
                width: "22px"
            },
            {
                field: "ModelName",
                title: "Model Name",
                width: "20px"
            },
            {
                field: "Status",
                title: "Status",
                width: "15px"
            },

            {
                title: "Progress",
                template: "#= NoOfClaimsAdjudicated # of #=NoOfClaimsSelected #",
                width: "17px"
            },
            {
                title: "Completion",
                template: "#= CalculateCompletionPercentage(NoOfClaimsAdjudicated, NoOfClaimsSelected, Status) #",
                width: "13px"
            },
            {
                field: "ElapsedTime",
                title: "Elapsed Time (DD:HH:MM)",
                width: "27px"
            },
            {
                title: "Actions",
                template: kendo.template($("#command-template").html()),
                width: "25px"
            }
        ],
        pageable: {
            input: true, //To input the pagenum
            numeric: false
        },
        dataBound:
            function () {
                dataView = this.dataSource.view();
                for (var i = 0; i < dataView.length; i++) {
                    if (dataView[i].IsVerified == false && dataView[i].Status == 'Completed') {
                        var name = dataView[i].RequestName;
                        var uid = dataView[i].uid;
                        $("#grid tbody").find("tr[data-uid=" + uid + "]").addClass("customClass");
                    }
                }

                //Bind click event for the cancel buttin
                $("#grid").find('.k-grid-cancel').off('click');
                $("#grid").find('.k-grid-cancel').on('click', function (e) {
                    var grid = $("#grid").data("kendoGrid");
                    var row = $(e.target).closest("tr");
                    var dataSelectClaims = grid.dataItem(row);
                    grid.dataSource.remove(dataSelectClaims); var taskid = $(this).attr('data-taskid');
                    var isUnVerified = $(this).closest('tr').hasClass('customClass');
                    SetJobStatus(taskid, 131, isUnVerified);
                    e.stopPropagation();
                });

            }
    });

    // Hide show this line
    $("#grid tbody").off('click', 'tr');

    $("#grid").on('click', 'tr', function () {
        MarkJobAsRead();
    });

}

function MarkJobAsRead() {
    var grid = $("#grid").data("kendoGrid");
    var selectedRow = grid.select();
    rowIndex = selectedRow[0].rowIndex;
    var verifiedCount = 0;
    if (grid._data[rowIndex].IsVerified == false) {
        var updateId = grid._data[rowIndex].TaskId;
        ajaxRequestHandler({
            url: SSIUrl.updateJobsCount,
            data: JSON.stringify({ jobUpdateId: updateId }),
            type: 'POST',
            dataType: 'json',
            cache: false,
            fnSuccess: OnMarkJobAsReadSuccess
        });
    }
}


function OnMarkJobAsReadSuccess(json) {
    var grid = $("#grid").data("kendoGrid");
    if (grid._data[rowIndex].IsVerified == false && grid._data[rowIndex].Status == 'Completed') {
        grid._data[rowIndex].IsVerified = true;
        var uid = grid._data[rowIndex].uid;
        $("#grid tbody").find("tr[data-uid=" + uid + "]").removeClass("customClass");
        isRefreshUnVerifiedJob = false;
        fnRefreshUnVerifiedJobCount();
    }
}

//Setting the Job Status
function SetJobStatus(val, status, isUnVerified) {
    var modeldata = { "taskid": val, "status": status };

    ajaxRequestHandler({
        url: SSIUrl.updateJobStatus,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(modeldata),
        dataType: 'json',
        type: 'POST',
        fnSuccess: OnUpdateJobStatusSuccess,
        fnError: OnUpdateJobStatusError,
        fnComplete: OnUpdateJobStatusComplete
    });
}

function OnUpdateJobStatusSuccess(data) {
    if (data.IsSuccess == true) {
        $('#grid').data('kendoGrid').dataSource.page($('#grid').data('kendoGrid').dataSource.page());
        $("#ddl-jobstatus").data("kendoDropDownList").enable(true);
        if (status == 131 && isUnVerified != null && isUnVerified == true) {
            isRefreshUnVerifiedJob = false;
            fnRefreshUnVerifiedJobCount();
        }
    }
}

function OnUpdateJobStatusError() {
    $('#grid').data('kendoGrid').dataSource.page($('#grid').data('kendoGrid').dataSource.page());
    $("#ddl-jobstatus").data("kendoDropDownList").enable(true);
}

function OnUpdateJobStatusComplete() {
    $("#ddl-jobstatus").data("kendoDropDownList").enable(true);
    $("#jobstatus-image-logo").addClass('cmshideonload');
    isRefreshUnVerifiedJob = false;
    fnRefreshUnVerifiedJobCount();
}

//Round to two digit
function RoundToTwoDigit(num) {
    return +(Math.round(num + "e+2") + "e-2");
}

//Added tool tip for Request name
$("#grid").kendoTooltip({
    filter: "tbody tr td:first-child",
    content: function (e) {
        var dataItem = $("#grid").data("kendoGrid").dataItem(e.target.closest("tr"));
        var content = dataItem.Text;
        var contentData = dataItem.Criteria;
        var dataArray = contentData.split('@');
        tooltipText = dataItem.RequestName;
        for (requestIndex = 0; requestIndex < dataArray.length; requestIndex++) {
            if (requestIndex > requestIndexValues) {//if claim field values have more then one
                tooltipText += "</br>" + addSpaceInClaimFields(dataArray[requestIndex]);
            } else {
                tooltipText += "</br>" + dataArray[requestIndex];
            }

        }
        // set the element text as content of the tooltip
        if (tooltipText.length > 0) {
            return tooltipText;
        } else {
            this.popup.wrapper.width('0');
        }
    },
    show: function (e) {
        this.popup.wrapper.width("500px");
    },
    autoHide: true,
    position: "absolute",
    autoHide: true
}).data("kendoTooltip");

function addSpaceInClaimFields(claimValues) {
    var claimIds = [];
    var splitClaimIds = $.trim(claimValues).split(",");
    if (splitClaimIds.length > requestIndexValues) {//if claim field values have more then one
        $.each(splitClaimIds, function (index, value) {
            if (value != "") {
                claimIds.push($.trim(value));
            }
        });
    }
    return (claimIds.length > 1) ? claimIds.join(', ') : claimValues;
}