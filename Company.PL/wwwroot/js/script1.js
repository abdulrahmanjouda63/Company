document.addEventListener("DOMContentLoaded", () => {
    InitializeGridBackground();
    InitializeParticles();
    InitializeFormValidation();
    InitializePasswordToggle();
    InitializeTypingEffect();
    Initialize3DHoverEffects();
});

// 🟦 Grid Background
function InitializeGridBackground() {
    const gridBackground = document.querySelector(".grid-background");
    if (!gridBackground) return;

    const isMobile = window.innerWidth <= 768;
    const gridSize = isMobile ? 100 : 256;
    const fragment = document.createDocumentFragment();

    for (let i = 0; i < gridSize; i++) {
        const gridCell = document.createElement("div");
        gridCell.classList.add("grid-cell");
        fragment.appendChild(gridCell);
    }

    gridBackground.appendChild(fragment);
}

// 🟩 Particle Background
function InitializeParticles() {
    const canvas = document.getElementById("particles");
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;

    const particlesArray = [];
    const numberOfParticles = 80;

    class Particle {
        constructor() {
            this.x = Math.random() * canvas.width;
            this.y = Math.random() * canvas.height;
            this.size = Math.random() * 2 + 1;
            this.speedX = Math.random() * 0.6 - 0.3;
            this.speedY = Math.random() * 0.6 - 0.3;
            this.alpha = Math.random() * 0.5 + 0.3;
        }

        update() {
            this.x += this.speedX;
            this.y += this.speedY;

            if (this.x > canvas.width || this.x < 0) this.speedX *= -1;
            if (this.y > canvas.height || this.y < 0) this.speedY *= -1;
        }

        draw() {
            ctx.fillStyle = `rgba(0,255,255,${this.alpha})`;
            ctx.beginPath();
            ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2);
            ctx.fill();
        }
    }

    function initParticles() {
        for (let i = 0; i < numberOfParticles; i++) {
            particlesArray.push(new Particle());
        }
    }

    function animateParticles() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        for (const p of particlesArray) {
            p.update();
            p.draw();
        }
        requestAnimationFrame(animateParticles);
    }

    window.addEventListener("resize", () => {
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
    });

    initParticles();
    animateParticles();
}

// 🟥 Password Visibility Toggle
function InitializePasswordToggle() {
    document.addEventListener("click", (event) => {
        const clickedIcon = event.target.closest(".eye-icon");
        if (clickedIcon) {
            const passwordInput = clickedIcon.parentElement.querySelector("input");
            if (passwordInput) {
                const isPassword = passwordInput.type === "password";
                clickedIcon.classList.replace(
                    isPassword ? "bx-hide" : "bx-show",
                    isPassword ? "bx-show" : "bx-hide"
                );
                passwordInput.type = isPassword ? "text" : "password";
            }
        }
    });
}

// 🟨 Form Validation
function InitializeFormValidation() {
    const form = document.querySelector("form");
    if (!form) return;

    form.addEventListener("submit", (e) => {
        const emailInput = form.querySelector('input[type="email"]');
        const passwordInput = form.querySelector('input[type="password"]');
        let isValid = true;

        if (emailInput) {
            const isValidEmail = emailInput.value.includes("@") && emailInput.value.includes(".");
            emailInput.style.borderColor = isValidEmail ? "#00b4f6" : "#ff4d4d";
            if (!isValidEmail) isValid = false;
        }

        if (passwordInput) {
            const isValidPassword = passwordInput.value.length >= 6;
            passwordInput.style.borderColor = isValidPassword ? "#00b4f6" : "#ff4d4d";
            if (!isValidPassword) isValid = false;
        }

        if (!isValid) {
            e.preventDefault();
        }
    });
}

// 🔵 Typing Animation for Headers
function InitializeTypingEffect() {
    const headers = document.querySelectorAll(".typing-header");
    headers.forEach((header) => {
        const text = header.dataset.text || header.textContent;
        header.textContent = "";
        let index = 0;

        function type() {
            if (index < text.length) {
                header.textContent += text.charAt(index);
                index++;
                setTimeout(type, 70); // Speed of typing
            }
        }

        type();
    });
}

// 🟣 3D Hover Effects
function Initialize3DHoverEffects() {
    const cards = document.querySelectorAll(".hover-3d");
    cards.forEach((card) => {
        card.addEventListener("mousemove", (e) => {
            const rect = card.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;
            const centerX = rect.width / 2;
            const centerY = rect.height / 2;
            const rotateX = -(y - centerY) / 10;
            const rotateY = (x - centerX) / 10;

            card.style.transform = `rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
        });

        card.addEventListener("mouseleave", () => {
            card.style.transform = `rotateX(0deg) rotateY(0deg)`;
        });
    });
}
