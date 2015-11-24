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
        //Gets the ID of the last element with the classname platform and adds 1
        var unparsed = document.getElementsByClassName("platform")[number].id.replace("inputfield", "");
        var lastnumber = parseInt(unparsed) + 1;

        //Gets the div where the Platform is located
        var container = document.getElementById("platform");

        //Creates an input field in the div that is defined in container and sets the properties
        var input = document.createElement("input");
        input.type = "text";
        input.name = "platform";
        input.id = "inputfield" + lastnumber;
        input.classList.add("platform");
        input.setAttribute('onkeypress', 'if (event.keyCode == 13) { addPlatform(); return false; }');
        input.setAttribute('onkeyup', 'checkDouble()');

        //Create a delete button to delete the input field created above
        var button = document.createElement("button");
        button.id = "deletebutton" + lastnumber;
        button.innerHTML = "Delete";
        button.classList.add("deletebuttoncreate");
        button.setAttribute('onclick', 'deleteInputField(' + lastnumber + ')');

        //Adds an br and the elements with all the properties also sets the focus on the input field
        container.appendChild(document.createElement("br"));
        container.appendChild(input);
        container.appendChild(button);
        input.focus();
    }
}

function deleteInputField(id) {
    //Deletes the inputfield, delete button and br we used to make the input field
    var inputfield = document.getElementById("inputfield" + id);
    inputfield.previousSibling.remove();
    inputfield.remove();
    document.getElementById("deletebutton" + id).remove();
    //does the function to check if you had a double value which you just deleted
    this.checkDouble();
}

function checkDouble() {
    //Gets the elements with the class Platform
    var platformElement = document.getElementsByClassName("platform");

    //Removes all the classes that makes it so that the text is red
    for (var i = 0; i < platformElement.length; i++) {
        platformElement[i].classList.remove("platformdouble");
    }

    //Checks every input after itself to see if something has the same value
    for (var i = 0; i < platformElement.length; i++) {
        var element = platformElement[i];
        for (var int = i + 1; int < platformElement.length; int++) {
            if (element.value == platformElement[int].value) {
                document.getElementById(platformElement[int].id).classList.add("platformdouble");
                element.classList.add("platformdouble");
            }
        }
    }
}