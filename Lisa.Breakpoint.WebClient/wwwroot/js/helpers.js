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

function addPlatform(sourceElement) {
    var number = document.getElementsByClassName("platform").length;
    if (document.getElementsByClassName("platform")[number - 1].value != "") {
        // Gets the div where the Platform input field is located
        var container = document.getElementById("platform");
        var platformdatalist = document.getElementById("platforms-list");

        // Creates an input field in the div that is defined in container and sets the properties
        var input = document.createElement("input");
        input.type = "text";
        input.name = "platform";
        input.classList.add("platform");
        input.setAttribute("list", "platforms-list");
        input.setAttribute('onkeypress', 'if (event.keyCode == 13) { addPlatform(this); return false; }');

        // Adds the last entered platform to the datalist
        if (sourceElement.value != "")
        {
            var option = document.createElement("option");
            option.value = sourceElement.value;
            platformdatalist.appendChild(option);
        }

        // Adds a br and the element with all the properties
        container.appendChild(document.createElement("br"));
        container.appendChild(input);
        input.focus();
    }
}