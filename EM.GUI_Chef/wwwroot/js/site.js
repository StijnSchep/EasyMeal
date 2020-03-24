// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function toggleDisplay(controlId) {
    console.log("id: " + controlId);
    var control = controlId

    if (control.style.display == "block" || control.style.display == "")
        control.style.display = "none";
    else
        control.style.display = "block";
}

function saveMeal() {
    var mealName = document.getElementById("mealName").value;
    var dayIndex = document.getElementById("dayIndex").value;
    var mealIndex = document.getElementById("mealIndex").value;
    //SaveMeal?dayIndex=0&mealIndex=0&mealName=mealName

    var url = 'SaveMeal?dayIndex=' + dayIndex + '&mealIndex=' + mealIndex + '&mealname=' + mealName
    console.log(url)
    window.location.href = url
}