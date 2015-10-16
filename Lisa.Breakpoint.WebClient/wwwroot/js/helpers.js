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



// recieves an object containing the values of a multiple select box
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


function toSlug(value) {
    return value.replace(/\s+/g, '-').toLowerCase()
}