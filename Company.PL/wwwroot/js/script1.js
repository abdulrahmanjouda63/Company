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

    // Password visibility toggle
    const eyeIcon = document.querySelector(".show-hide")
    if (eyeIcon) {
        eyeIcon.addEventListener("click", () => {
            const passwordInput = eyeIcon.parentElement.querySelector("input")
            if (passwordInput.type === "password") {
                eyeIcon.classList.replace("bx-hide", "bx-show")
                passwordInput.type = "text"
            } else {
                eyeIcon.classList.replace("bx-show", "bx-hide")
                passwordInput.type = "password"
            }
        })
    }

    // Simple form validation
    const form = document.querySelector("form")
    if (form) {
        form.addEventListener("submit", (e) => {
            const emailInput = form.querySelector('input[type="email"]')
            const passwordInput = form.querySelector('input[type="password"]')

            let isValid = true

            // Simple email validation
            if (!emailInput.value.includes("@") || !emailInput.value.includes(".")) {
                isValid = false
                emailInput.style.borderColor = "#ff4d4d"
            } else {
                emailInput.style.borderColor = "#00b4f6"
            }

            // Simple password validation
            if (passwordInput.value.length < 6) {
                isValid = false
                passwordInput.style.borderColor = "#ff4d4d"
            } else {
                passwordInput.style.borderColor = "#00b4f6"
            }

            if (!isValid) {
                e.preventDefault()
            }
        })
    }
})
