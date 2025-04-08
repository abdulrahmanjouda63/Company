// Create grid cells for background
document.addEventListener("DOMContentLoaded", () => {
    const gridBackground = document.querySelector(".grid-background")
    const isMobile = window.innerWidth <= 768
    const gridSize = isMobile ? 100 : 256 // Fewer cells on mobile for better performance

    // Create grid cells
    for (let i = 0; i < gridSize; i++) {
        const gridCell = document.createElement("div")
        gridCell.classList.add("grid-cell")
        gridBackground.appendChild(gridCell)
    }

    // Form validation
    const form = document.querySelector("form"),
        nameFields = form.querySelectorAll(".name-field"),
        usernameField = form.querySelector(".username-field"),
        usernameInput = usernameField.querySelector(".username"),
        emailField = form.querySelector(".email-field"),
        emailInput = emailField.querySelector(".email"),
        phoneField = form.querySelector(".phone-field"),
        phoneInput = phoneField.querySelector(".phone"),
        passField = form.querySelector(".create-password"),
        passInput = passField.querySelector(".password"),
        cPassField = form.querySelector(".confirm-password"),
        cPassInput = cPassField.querySelector(".cPassword")

    // Username Validation
    function checkUsername() {
        if (usernameInput.value.length < 3) {
            return usernameField.classList.add("invalid")
        }
        usernameField.classList.remove("invalid")
    }

    // Email Validation
    function checkEmail() {
        const emaiPattern = /^[^ ]+@[^ ]+\.[a-z]{2,3}$/
        if (!emailInput.value.match(emaiPattern)) {
            return emailField.classList.add("invalid")
        }
        emailField.classList.remove("invalid")
    }

    // Phone Validation
    function checkPhone() {
        const phonePattern = /^[\d\s+\-$$]{10,15}$/
        if (!phoneInput.value.match(phonePattern)) {
            return phoneField.classList.add("invalid")
        }
        phoneField.classList.remove("invalid")
    }

    // Password Validation
    function checkPassword() {
        if (passInput.value.length < 8) {
            return passField.classList.add("invalid")
        }
        passField.classList.remove("invalid")
    }

    // Confirm Password Validation
    function confirmPass() {
        if (passInput.value !== cPassInput.value || cPassInput.value === "") {
            return cPassField.classList.add("invalid")
        }
        cPassField.classList.remove("invalid")
    }

    // Hide and show password
    const eyeIcons = document.querySelectorAll(".show-hide")

    eyeIcons.forEach((eyeIcon) => {
        eyeIcon.addEventListener("click", () => {
            const pInput = eyeIcon.parentElement.querySelector("input")
            if (pInput.type === "password") {
                eyeIcon.classList.replace("bx-hide", "bx-show")
                return (pInput.type = "text")
            }
            eyeIcon.classList.replace("bx-show", "bx-hide")
            pInput.type = "password"
        })
    })

    // Form submission
    form.addEventListener("submit", (e) => {
        // Validate all fields
        checkUsername()
        checkEmail()
        checkPhone()
        checkPassword()
        confirmPass()

        // Check if any field is invalid
        const hasInvalidField =
            usernameField.classList.contains("invalid") ||
            emailField.classList.contains("invalid") ||
            phoneField.classList.contains("invalid") ||
            passField.classList.contains("invalid") ||
            cPassField.classList.contains("invalid")

        // If any field is invalid, prevent form submission
        if (hasInvalidField) {
            e.preventDefault()
        }

        // Add keyup validation after first submission attempt
        usernameInput.addEventListener("keyup", checkUsername)
        emailInput.addEventListener("keyup", checkEmail)
        phoneInput.addEventListener("keyup", checkPhone)
        passInput.addEventListener("keyup", checkPassword)
        passInput.addEventListener("keyup", confirmPass) // Also check confirm password when password changes
        cPassInput.addEventListener("keyup", confirmPass)
    })
})
