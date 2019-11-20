var array;
getIngredients();

function getIngredients() {
    $.ajax({
        url: '/Cocktail/getallingredients',
        type: "GET",
        success: function (result) {
            array = result;
            ingredientsautocomplete(document.getElementById("primary-1"));
            ingredientsautocomplete(document.getElementById("ingredient-1"));
        }
    });
}

function ingredientsautocomplete(inp) {
    /*the autocomplete function takes two arguments,
    the text field element and an array of possible autocompleted values:*/
    var currentFocus;
    /*execute a function when someone writes in the text field:*/
    inp.addEventListener("input", function (e) {
        var a, b, i, val = this.value;
        /*close any already open lists of autocompleted values*/
        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;
        /*create a DIV element that will contain the items (values):*/
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        /*append the DIV element as a child of the autocomplete container:*/
        this.parentNode.appendChild(a);
        /*for each item in the array...*/
        for (i = 0; i < array.length; i++) {
            /*check if the item starts with the same letters as the text field value:*/
            if (array[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                /*create a DIV element for each matching element:*/
                b = document.createElement("DIV");
                /*make the matching letters bold:*/
                b.innerHTML = "<strong>" + array[i].substr(0, val.length) + "</strong>";
                b.innerHTML += array[i].substr(val.length);
                /*insert a input field that will hold the current array item's value:*/
                b.innerHTML += "<input type='hidden' value='" + array[i] + "'>";
                /*execute a function when someone clicks on the item value (DIV element):*/
                b.addEventListener("click", function (e) {
                    /*insert the value for the autocomplete text field:*/
                    inp.value = this.getElementsByTagName("input")[0].value;
                    /*close the list of autocompleted values,
                    (or any other open lists of autocompleted values:*/
                    closeAllLists();
                });
                a.appendChild(b);
            }
        }
    });
    /*execute a function presses a key on the keyboard:*/
    inp.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
            /*If the arrow DOWN key is pressed,
            increase the currentFocus variable:*/
            currentFocus++;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 38) { //up
            /*If the arrow UP key is pressed,
            decrease the currentFocus variable:*/
            currentFocus--;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 13) {
            /*If the ENTER key is pressed, prevent the form from being submitted,*/
            e.preventDefault();
            if (currentFocus > -1) {
                /*and simulate a click on the "active" item:*/
                if (x) x[currentFocus].click();
            }
        }
    });
    function addActive(x) {
        /*a function to classify an item as "active":*/
        if (!x) return false;
        /*start by removing the "active" class on all items:*/
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        /*add class "autocomplete-active":*/
        x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {
        /*a function to remove the "active" class from all autocomplete items:*/
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {
        /*close all autocomplete lists in the document,
        except the one passed as an argument:*/
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }
    /*execute a function when someone clicks in the document:*/
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}


var pIngCount = 1;
var sIngCount = 1;
function addPrimaryIngredientField() {
    pIngCount++;
    console.log($("main-ingredients"));
    $("#main-ingredients").append(`<input class="form-control" placeholder="Primary ingredient" type="text" id="primary-${pIngCount}">`);
    ingredientsautocomplete(document.getElementById(`primary-${pIngCount}`));
}
function addIngredientField() {
    sIngCount++;
    $("#ingredients").append(`<input class="form-control" placeholder="Ingredient" type="text" id="ingredient-${sIngCount}">`);
    ingredientsautocomplete(document.getElementById(`ingredient-${sIngCount}`));
}
function removePrimaryIngredientField() {
    if (pIngCount>1) {
    $(`#primary-${pIngCount}`).remove();
    pIngCount--;       
    }
}

function removeIngredientField() {
    if (sIngCount > 1) {
        $(`#ingredient-${sIngCount}`).remove();
        sIngCount--;
    }
}
function addCocktail() {
    var formData = new FormData();
    var image = document.getElementById('rand').files[0];
    formData.append('image', image);
    let name = $("#cocktail-name").val();
    let primaryIngredients = [];
    for (var i = 1; i <= pIngCount; i++) {
        primaryIngredients[i-1] = $(`#primary-${i}`).val();
    }
    let ingredients = new Array;
    for (var i = 1; i <= sIngCount; i++) {
        ingredients[i-1] = $(`#ingredient-${i}`).val();
    }
    let description = $("#cocktail-description").val();
    formData.append("name", name);
    formData.append("primaryIngredients",primaryIngredients);
    formData.append("ingredients", ingredients);
    formData.append("description", description);
    console.log(formData);
    $.ajax({
        url: '/magician/cocktail/createcocktail',
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            var url = $("#RedirectTo").val();
            window.location.href = url;
        }
    });
}


