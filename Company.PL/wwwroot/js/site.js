// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function toggleMode() {
    let body = document.body;
    let icon = document.getElementById("mode-icon");
    let navLinks = document.querySelectorAll(".nav-link");

    body.classList.toggle("dark-mode");
    body.classList.toggle("light-mode");

    if (body.classList.contains("dark-mode")) {
        icon.classList.replace("fa-sun", "fa-moon");
        navLinks.forEach(link => link.style.color = "white");
    } else {
        icon.classList.replace("fa-moon", "fa-sun");
        navLinks.forEach(link => link.style.color = "#222");
    }
}

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