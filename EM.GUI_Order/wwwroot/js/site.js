function toggleDisplay(controlId) {
    console.log("id: " + controlId);
    var control = document.getElementById(controlId)

    if (control.style.display == "block" || control.style.display == "")
        control.style.display = "none";
    else
        control.style.display = "block";
}

function setMealPrice(controlId, originalPrice, inputId) {
    var input = document.getElementById(inputId);
    var inVal = input.value;

    var control = document.getElementById(controlId);


    if (inVal != "S" && inVal != "M" && inVal != "L") {
        input.value = "M";
        return;
    }

    if (inVal == "S") {
        control.innerHTML = 0.8 * originalPrice;
    } else if (inVal == "L") {
        control.innerHTML = 1.2 * originalPrice;
    } else {
        control.innerHTML = originalPrice;
    }
}