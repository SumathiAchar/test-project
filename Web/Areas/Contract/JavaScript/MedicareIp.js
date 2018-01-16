$(document).ready(function () {
    setTimeout(function () {
        $('#txtMCPercentage').siblings('input:visible').focus();
        if (viewmodelMcIp.Formula != null && viewmodelMcIp.Formula != "") {
            var MedicareIpAcuteOptionAllCodes = viewmodelMcIp.Formula.split(' + ');
            $.each($('#divMedicareIpAcuteOption').find('div.medicare-ip-acute-option'), function () {
                if (MedicareIpAcuteOptionAllCodes.indexOf($(this).find('input[type=checkbox]:eq(0)').val()) > -1) {
                    $(this).find('input[type=checkbox]:eq(0)').prop('checked', true);
                    $.each($(this).find('div.medicare-ip-acute-option-child'), function () {
                        $(this).find('input[type=checkbox]:eq(0)').prop('checked', true);
                    });
                } else {
                    $(this).find('input[type=checkbox]:eq(0)').prop('checked', false);
                    $.each($(this).find('div.medicare-ip-acute-option-child'), function () {
                        if (MedicareIpAcuteOptionAllCodes.indexOf($(this).find('input[type=checkbox]:eq(0)').val()) > -1) {
                            $(this).find('input[type=checkbox]:eq(0)').prop('checked', true);
                        } else {
                            $(this).find('input[type=checkbox]:eq(0)').prop('checked', false);
                        }
                    });
                }
            });
        } else {
            $.each($('#divMedicareIpAcuteOption').find('div.medicare-ip-acute-option'), function () {
                $(this).find('input[type=checkbox]:eq(0)').prop('checked', true);
                $.each($(this).find('div.medicare-ip-acute-option-child'), function () {
                    $(this).find('input[type=checkbox]:eq(0)').prop('checked', true);
                });
            });
        }

    }, 1000);
    
});

//Formatting Percentage Field
$("#txtMCPercentage").kendoNumericTextBox({
    format: "#.## \\%",
    min: 0
});

//For Set Focus Text Box
$('#txtMCPercentage').siblings('input:visible').focus();

$("#btnMCSave").click(function () {
    if ($("#txtMCPercentage").val() != '') {
        var formula = '';
        $.each($('#divMedicareIpAcuteOption').find('div.medicare-ip-acute-option'), function () {
            if ($(this).find('input[type=checkbox]:eq(0)').is(":checked")) {
                if (formula == '') {
                    formula += $(this).find('input[type=checkbox]:eq(0)').val();
                } else {
                    formula += ' + ' + $(this).find('input[type=checkbox]:eq(0)').val();
                }
            } else {
                $.each($(this).find('div.medicare-ip-acute-option-child'), function () {
                    if ($(this).find('input[type=checkbox]:eq(0)').is(":checked")) {
                        if (formula == '') {
                            formula += $(this).find('input[type=checkbox]:eq(0)').val();
                        } else {
                            formula += ' + ' + $(this).find('input[type=checkbox]:eq(0)').val();
                        }
                    }
                });
            }
        });

        if (formula != '') {
            viewmodelMcIp.Formula = formula;
            $("#btnMCSave").attr('disabled', 'disabled');
            ajaxRequestHandler({
                url: SSIUrl.addEditPaymentTypeMedicareIp,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(viewmodelMcIp),
                fnSuccess: OnAddEditPaymentTypeMedicareIpSuccess,
            });
        } else {
            registerGridPopup("Please choose at least one component.", "OK", null, null);
        }
    }
    else {
        registerGridPopup("Please provide values for mandatory fields.", "OK", null, null);
    }
});

function OnAddEditPaymentTypeMedicareIpSuccess() {
    $('.ptMedicareIp').closest(".k-window-content").data("kendoWindow").close();
    ReDrawShapes();
}

function checkUncheckAll() {
    $('input[type="checkbox"]').change(function (e) {
        var checked = $(this).prop("checked"),
            container = $(this).closest("div"), //get closest div instead of parent
            siblings = container.siblings();
        container.find('input[type="checkbox"]').prop({
            indeterminate: false,
            checked: checked
        });

        function checkSiblings(el) {
            var parent = el.parent().parent(),
                all = true,
                parentcheck = parent.children("label"); //get the label that contains the disabled checkbox
            el.siblings().each(function () {
                return all = ($(this).find('input[type="checkbox"]').prop("checked") === checked);
            });
            //use parentcheck instead of parent to get the children checkbox
            if (all && checked) {
                parentcheck.children('input[type="checkbox"]').prop({
                    indeterminate: false,
                    checked: checked
                });
                checkSiblings(parent);
            } else if (all && !checked) {
                parentcheck.children('input[type="checkbox"]').prop("checked", checked);
                parentcheck.children('input[type="checkbox"]').prop("indeterminate", (parent.find('input[type="checkbox"]:checked').length > 0));
                checkSiblings(parent);
            } else {
                parentcheck.children('input[type="checkbox"]').prop({
                    indeterminate: false,
                    checked: false
                });
            }
        }

        checkSiblings(container);
    });
}