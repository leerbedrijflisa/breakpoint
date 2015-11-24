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

function addPlatform() {
    var number = document.getElementsByClassName("platform").length - 1;
    if (document.getElementsByClassName("platform")[number].value != "") {
        //gets the ID of the last element with the classname platform and adds 1
        var unparsed = document.getElementsByClassName("platform")[number].id.replace("inputfield", "");
        var lastnumber = parseInt(unparsed) + 1;

        //Gets the div where the Platform is located
        var container = document.getElementById("platform");

        //creates an input field in the div that is defined in container and sets the properties
        var input = document.createElement("input");
        input.type = "text";
        input.name = "platform";
        input.id = "inputfield" + lastnumber;
        input.classList.add("platform");
        input.setAttribute('onkeypress', 'if (event.keyCode == 13) { addPlatform(); return false; }');
        input.setAttribute('onkeyup', 'checkDouble()');

        //create a delete button
        var button = document.createElement("button");
        button.id = "deletebutton" + lastnumber;
        button.innerHTML = "Delete";
        button.classList.add("deletebuttoncreate");
        button.setAttribute('onclick', 'deleteInputField(' + lastnumber + ')');

        //adds an br and the element with all the properties
        container.appendChild(document.createElement("br"));
        container.appendChild(input);
        container.appendChild(button);
        input.focus();
    }
}

function deleteInputField(id) {
    var inputfield = document.getElementById("inputfield" + id);
    inputfield.previousSibling.remove();
    inputfield.remove();
    document.getElementById("deletebutton" + id).remove();
    this.checkDouble();
}

function checkDouble() {
    var platformElement = document.getElementsByClassName("platform");
    for (var i = 0; i < platformElement.length; i++) {
        platformElement[i].classList.remove("platformdouble");
    }
    for (var i = 0; i < platformElement.length; i++) {
        var element = platformElement[i];
        for (var int = i + 1; int < platformElement.length; int++) {
            console.log("platform " + platformElement[int].value + " en element " + element.value);
            if (element.value == platformElement[int].value) {
                document.getElementById(platformElement[int].id).classList.add("platformdouble");
                element.classList.add("platformdouble");
            }
        }
    }
}