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

			body.style.backgroundColor = "#1E1E2D"; /* Deep charcoal with blue touch */

			body.style.color = "#E0E0E0"; /* Soft white text */

			navLinks.forEach(link => link.style.color = "#4A90E2"); /* Soft neon blue for links */

			if (navBrand) navBrand.style.color = "#FFEB3B"; /* Gold color for brand */

		} else {

			body.style.backgroundColor = "#FAF3F0"; /* Light ivory background */

			body.style.color = "#333333"; /* Dark gray text */

			navLinks.forEach(link => link.style.color = "#FF7043"); /* Coral color for links */

			if (navBrand) navBrand.style.color = "#FF5722"; /* Dark coral for brand */

		}

	}


	// Load mode from local storage

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

$(document).ready(function () {
const searchBar = $('#SearchInput');
const table = $('table');

searchBar.on('keyup', function (event) {
	var searchValue = searchBar.val();

	$.ajax({
		url: '/Employee/Search',
		type: 'Get',
		data: { SearchInput: searchValue },
		success: function (result) {
			table.html(result);
		},
		error: function (xhr, status, error) {
			console.log(error);
		}
	});
});
});

