//This method validates expression from UI and returns appropriate message in case validation fails
function validateExpression(thresholdExpression, allowedVariables, evaluationReplacement) {

    var expresssion = ($('#txtThresholdAmount').val() === "") ? thresholdExpression : getValidExpression(thresholdExpression);
    $('#txtThresholdAmount').val(expresssion.toUpperCase());
    var inputstr = expresssion;
    var new_str = inputstr.toUpperCase();

    //Matches opening and closing brackets. Number of opening bracket and closing brackets should be same
    if ((new_str.match(/\(/g) || '').length != (new_str.match(/\)/g) || '').length) {
        registerGridPopup("Missing parenthesis.", "OK", null, null);
        return false;
    }
    
    //if expression has multiple operator side by side. Ex 4+-*3/2
    if (RegExp("[+-/*=]{2,}").test(new_str)) {
        registerGridPopup("Incorrect formula.", "OK", null, null);
        return false;
    }

    new_str = new_str.trim();
    var splitedExpression = [];
    splitedExpression = new_str.split(/[()+-/<>=*]/);
    for (var index = 0; index < splitedExpression.length; index++) {
        splitedExpression[index] = splitedExpression[index].trim();
    }
    
    //to clean out empty element from the array
    splitedExpression = splitedExpression.filter(Boolean);
    var isValid = true;
    var message = '';
    $.each(splitedExpression, function (index, item) {
        //if the item doesnot present in allowed variables and that is not a number then the expression is not valid
        if (allowedVariables.indexOf(item) == -1 && (item == '' || isNaN(item))) {
            isValid = false;
            
            //Expression is invalid. If expression contains T_ then its incorrect table field
            //if expression contains any other characters from a-z then incorrect variable
            //if user has used notr allowed operators like %,^,&,! etc, then incorrect operator
            if (item.substring(0, 2).match('T_')) {
                $(".btn-save-close").find('input').prop('disabled', true);
                message = "Incorrect table field entry.";
                return false;
            }
            else if (item.match(/[a-z/]/i)) {
                $(".btn-save-close").find('input').prop('disabled', true);
                message = "Incorrect variable used.";
                return false;
            }
            else if (!item.match(/[+-/*(),\s*]/g)) {
                $(".btn-save-close").find('input').prop('disabled', true);
                message = "Incorrect operator used.";
                return false;
            } else {
                $(".btn-save-close").find('input').prop('disabled', true);
                message = "Incorrect formula.";
                return false;

            }
            return isValid
        }
   });

    if (!isValid) {
        registerGridPopup(message, "OK", null, null);
        return false;
    }
    else {
        var replace_str = inputstr.toUpperCase();
        try {
            //Replaces variables with dummy values. This will then be evaluated by JS eval function
            for (var key in evaluationReplacement) {
                replace_str = replace_str.replace(new RegExp(key, "g"), evaluationReplacement[key]);
            }
            return true;
        }
        catch (exception) {
                registerGridPopup("Incorrect formula.", "OK", null, null);
                return false;
        }
    }
}