function readCookie(key) {
    var keyq = key + "=";
    var ca = document.cookie.split(';');
    for(var i=0;i < ca.length;i++) {
        var c = ca[i];
        while (c.charAt(0)==' ') c = c.substring(1,c.length);
        if (c.indexOf(keyq) == 0) {
            return c.substring(keyq.length,c.length);
        }   
    }
    return null;
}

function setCookie(key, val, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime()+(days*24*60*60*1000));
        var expires = "; expires="+date.toGMTString();
    } else {
        var expires = "";
    }
    document.cookie = key + "=" + val + expires + "; path=/";
}

function deleteCookie(key) {
    setCookie(key, "", -1);
}



// returns an object containing the values of a multiple select box
function getSelectValues(select) {
    var result = [];
    var options = select && select.options;
    var opt;

    for (var i = 0, iLen = options.length; i < iLen; i++) {
        opt = options[i];

        if (opt.selected) {
            result.push(opt.value || opt.text);
        }
    }
    return result;
}

// returns the type of the selected value
function getAssignedToType(select) {
    if (select.options[select.selectedIndex].parentNode.label == "Groups") {
        return "group";
    } else if (select.options[select.selectedIndex].parentNode.label == "Members") {
        return "person";
    }
}

function getSelectValue(id) {
    var sel = document.getElementById(id);
    return sel.options[sel.selectedIndex].value;
}


function count(object) {
    var count = 0;
    for (var o in object) {
        if (object.hasOwnProperty(o)) {
            ++count;
        }
    }

    return count;
}


function toSlug(value) {
    return value.replace(/\s+/g, '-').toLowerCase()
}