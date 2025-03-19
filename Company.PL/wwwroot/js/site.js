// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.querySelector(".toggle-button");
    const body = document.body;
    const icon = document.getElementById("mode-icon");
    const navLinks = document.querySelectorAll(".nav-link");
    const navBrand = document.querySelector(".navbar-brand");

    function updateColors() {
        if (body.classList.contains("dark-mode")) {
            navLinks.forEach(link => link.style.color = "#e0e0e0");
            if (navBrand) navBrand.style.color = "#ffcc00";
        } else {
            navLinks.forEach(link => link.style.color = "#222");
            if (navBrand) navBrand.style.color = "#ff6600";
        }
    }

    // Load mode from local storage
    if (localStorage.getItem("theme") === "dark") {
        body.classList.add("dark-mode");
        body.classList.remove("light-mode");
        icon.classList.replace("fa-sun", "fa-moon");
    } else {
        body.classList.add("light-mode");
        body.classList.remove("dark-mode");
        icon.classList.replace("fa-moon", "fa-sun");
    }
    updateColors();

    toggleButton.addEventListener("click", function () {
        body.classList.toggle("dark-mode");
        body.classList.toggle("light-mode");

        if (body.classList.contains("dark-mode")) {
            icon.classList.replace("fa-sun", "fa-moon");
            localStorage.setItem("theme", "dark");
        } else {
            icon.classList.replace("fa-moon", "fa-sun");
            localStorage.setItem("theme", "light");
        }
        updateColors();
    });
});


let InputSearch = document.getElementById("SearchInput");
InputSearch.addEventListener("keyup", () => {
    let xhr = new XMLHttpRequest();
    let url = `https://localhost:44398/Employee?SearchInput=${InputSearch.value}`;
    xhr.open('GET', url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            console.log(this.responseText);
        }
    }
    xhr.send();
});