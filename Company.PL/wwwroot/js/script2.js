document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector("form");
    if (!form) return;

    const nameFields = form.querySelectorAll(".name-field");
    const usernameField = form.querySelector(".username-field");
    const usernameInput = usernameField?.querySelector(".username");
    const emailField = form.querySelector(".email-field");
    const emailInput = emailField?.querySelector(".email");
    const phoneField = form.querySelector(".phone-field");
    const phoneInput = phoneField?.querySelector(".phone");
    const passField = form.querySelector(".create-password");
    const passInput = passField?.querySelector(".password");
    const cPassField = form.querySelector(".confirm-password");
    const cPassInput = cPassField?.querySelector(".cPassword");

    const setInvalid = (field) => field?.classList.add("invalid");
    const setValid = (field) => field?.classList.remove("invalid");

    const checkUsername = () => {
        setInvalid(usernameField);
        if (usernameInput?.value.trim().length >= 3) {
            setValid(usernameField);
        }
    };

    const checkEmail = () => {
        setInvalid(emailField);
        const emailPattern = /^[^ ]+@[^ ]+\.[a-z]{2,3}$/;
        if (emailInput?.value.match(emailPattern)) {
            setValid(emailField);
        }
    };

    const checkPhone = () => {
        setInvalid(phoneField);
        const phonePattern = /^[\d\s+\-]{10,15}$/;
        if (phoneInput?.value.match(phonePattern)) {
            setValid(phoneField);
        }
    };

    const checkPassword = () => {
        setInvalid(passField);
        if (passInput?.value.length >= 8) {
            setValid(passField);
        }
    };

    const confirmPass = () => {
        setInvalid(cPassField);
        if (passInput?.value === cPassInput?.value && cPassInput?.value !== "") {
            setValid(cPassField);
        }
    };

    const debounce = (func, delay) => {
        let timeoutId;
        return (...args) => {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => func.apply(this, args), delay);
        };
    };

    // 🔄 Real-time validation
    usernameInput?.addEventListener("keyup", debounce(checkUsername, 300));
    emailInput?.addEventListener("keyup", debounce(checkEmail, 300));
    phoneInput?.addEventListener("keyup", debounce(checkPhone, 300));
    passInput?.addEventListener("keyup", debounce(checkPassword, 300));
    cPassInput?.addEventListener("keyup", debounce(confirmPass, 300));

    // ✅ Final Submit Validation
    form.addEventListener("submit", (e) => {
        checkUsername();
        checkEmail();
        checkPhone();
        checkPassword();
        confirmPass();

        const hasInvalidField =
            usernameField?.classList.contains("invalid") ||
            emailField?.classList.contains("invalid") ||
            phoneField?.classList.contains("invalid") ||
            passField?.classList.contains("invalid") ||
            cPassField?.classList.contains("invalid");

        if (hasInvalidField) {
            e.preventDefault();
        } else {
            const submitButton = form.querySelector('button[type="submit"], input[type="submit"]');
            if (submitButton) {
                submitButton.disabled = true;
                if (submitButton.tagName.toLowerCase() === "button") {
                    submitButton.textContent = "Submitting...";
                } else {
                    submitButton.value = "Submitting...";
                }
            }
        }
    });
});
