var dateRegExp = /^\/Date\((.*?)\)\/$/;
var seconddateRegExp = /^((0?[1-9]|1[012])[- /.](0?[1-9]|[12][0-9]|3[01])[- /.](19|20)?[0-9]{2})*$/;
//Format date
$(document).ready(function() {
    $("#txt-claimnotes").keyup(function() {
        if ($("#txt-claimnotes").val() != '') {
            $("#btn-addnewclaimnote").removeAttr('disabled');
        } else {
            $("#btn-addnewclaimnote").attr('disabled', 'disabled');
        }
    });
    var claimNotesDataSource = new kendo.data.DataSource({
        data: gridData,
        sort: { field: "ShortDateTime", dir: "desc" }
    });

    $("#grid-claim-notes").kendoGrid({
        dataSource: claimNotesDataSource,
        selectable: "row",
        sortable: true,
        columns: [
            { field: "ShortDateTime", title: "Date - Time(Stamp)", width: 130 },
            { field: "ClaimNoteId", hidden: true, width: 80 },
            { field: "UserName", title: "User Name", width: 100 },
            { field: "ClaimNoteText", title: "Claim Notes", width: 250 },
            {
                title: "Actions",
                width: 60,
                attributes: { style: "text-align:center;" },
                template: kendo.template($("#command-template-note-delete").html()),
            }
        ],
        dataBound: function(e) {
            if (window.location.pathname.indexOf('DenialManagement') > -1) {
                $('.k-grid-Delete').attr('disabled', 'disabled');
            }
            $("#grid-claim-notes").find('.k-grid-cancel').off('click');
            $("#grid-claim-notes").find('.k-grid-cancel').on('click', function(e) {
                if (window.location.pathname.indexOf('DenialManagement') > -1) {
                    return false;
                }
                var claimNoteID = $(this).attr('data-ClaimNoteId');
                var modeldata = { 'id': claimNoteID };

                var grid = $("#grid-claim-notes").data("kendoGrid");
                var selectedRow = grid.select();
                rowIndex = selectedRow[0].rowIndex;
                if (window.location.pathname.indexOf('DenialManagement') != 1) {
                    ajaxRequestHandler({
                        url: SSIUrl.deleteNote,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: JSON.stringify(modeldata),
                        fnSuccess: OnDeleteContractNotesSuccess
                    });
                }
                e.stopPropagation();
            });
        }
    });
    function OnDeleteContractNotesSuccess() {
        var grid = $("#grid-claim-notes").data("kendoGrid");
        var dataContactNotes = grid._data[rowIndex];
        grid.dataSource.remove(dataContactNotes);
    }

    var model = kendo.observable({
        ClaimNotes: claimNotesDataSource
    });

    //For addding the new contract notes
    $("#btn-addnewclaimnote").click(function () {
        $("#btn-addnewclaimnote").attr('disabled', 'disabled');
        var claimNote = { 'ClaimId': $('#hidden-basicinfo-claimid').val(), 'ClaimNoteText': $("#txt-claimnotes").val(), 'ClaimNoteId': $('#hidden-claim-notesid').val(), 'CurrentDateTime': getCurrentDateTime() };
        
        ajaxRequestHandler({
            url: SSIUrl.saveClaimNote,
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(claimNote),
            fnSuccess: OnSaveClaimNoteSuccess
        });
    });

    function OnSaveClaimNoteSuccess(data) {
        claimNotesDataSource.add({
            ClaimNoteId: data.Id,
            UserName: data.userName,
            ClaimNoteText: $("#txt-claimnotes").val(),
            ShortDateTime: data.insertedTime
        });
        $("#txt-claimnotes").val('');
        $('#hidden-claim-notesid').val('');
        $("#btn-addnewclaimnote").attr('disabled', 'disabled');
    }
    kendo.bind($("#container-claim-notes"), model);
    $("#btn-addnewclaimnote").attr('disabled');
});


function toDate(value) {
    if (value != null) {
        var date = dateRegExp.exec(value);
        if (dateRegExp.exec(value) != null) //Edit Mode
        {
            return dateTimeFormat(new Date(parseInt(date[1])), "MM/DD/YYYY HH:MM:SS");
        }

        if (seconddateRegExp.exec(value) != null)   //Add Mode
        {
            var today = new Date();
            var todayUtc = new Date(today.getUTCFullYear(), today.getUTCMonth(), today.getUTCDate(), today.getUTCHours(), today.getUTCMinutes(), today.getUTCSeconds());
            return dateTimeFormat(todayUtc, "MM/DD/YYYY HH:MM:SS");
        }
    }
    return '';
}

function dateTimeFormat(date, format) {
    // Calculate date parts and replace instances in format string accordingly
    format = format.replace("DD", (date.getDate() < 10 ? '0' : '') + date.getDate()); // Pad with '0' if needed
    format = format.replace("MM", (date.getMonth() < 9 ? '0' : '') + (date.getMonth() + 1)); // Months are zero-based
    format = format.replace("YYYY", date.getFullYear());
    format = format.replace("HH", date.getHours());
    format = format.replace("MM", date.getMinutes());
    format = format.replace("SS", date.getSeconds());
    return format;
}

function Limittext(e) {
    var maxLength = 100;
    if (e.value.length > maxLength) {
        e.value = e.value.substring(0, maxLength);
    }
}