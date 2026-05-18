// JavaScript source code
// ==========================
// ANIMAÇÃO AO SCROLL
// ==========================

const steps = document.querySelectorAll(".tutorial-step");

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add("show");
        }
    });
}, {
    threshold: 0.2
});

steps.forEach(step => {
    observer.observe(step);
});


// ==========================
// EFEITO HOVER DINÂMICO NAS IMAGENS
// ==========================

const images = document.querySelectorAll(".step-image img");

images.forEach(img => {
    img.addEventListener("mousemove", (e) => {
        const rect = img.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        const centerX = rect.width / 2;
        const centerY = rect.height / 2;

        const rotateX = ((y - centerY) / centerY) * 5;
        const rotateY = ((x - centerX) / centerX) * -5;

        img.style.transform = `scale(1.05) rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
    });

    img.addEventListener("mouseleave", () => {
        img.style.transform = "scale(1) rotateX(0) rotateY(0)";
    });
});


// ==========================
// DESTACAR PASSO ATIVO
// ==========================

const highlightObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            document.querySelectorAll(".tutorial-step").forEach(step => {
                step.classList.remove("active-step");
            });

            entry.target.classList.add("active-step");
        }
    });
}, {
    threshold: 0.5
});

steps.forEach(step => {
    highlightObserver.observe(step);
});


// ==========================
// BOTÃO "VOLTAR AO TOPO"
// ==========================

const scrollBtn = document.createElement("button");
scrollBtn.innerText = "↑";
scrollBtn.classList.add("scroll-top-btn");
document.body.appendChild(scrollBtn);

window.addEventListener("scroll", () => {
    if (window.scrollY > 400) {
        scrollBtn.style.opacity = "1";
        scrollBtn.style.pointerEvents = "auto";
    } else {
        scrollBtn.style.opacity = "0";
        scrollBtn.style.pointerEvents = "none";
    }
});

scrollBtn.addEventListener("click", () => {
    window.scrollTo({
        top: 0,
        behavior: "smooth"
    });
});


// ==========================
// SCROLL SUAVE (links internos)
// ==========================

document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener("click", function (e) {
        e.preventDefault();

        const target = document.querySelector(this.getAttribute("href"));
        if (target) {
            target.scrollIntoView({
                behavior: "smooth"
            });
        }
    });
});