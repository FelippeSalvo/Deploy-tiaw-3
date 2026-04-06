// NAV LINK ACTIVE
const links = document.querySelectorAll(".nav-links a");

links.forEach(link => {
    link.addEventListener("click", () => {
        links.forEach(l => l.classList.remove("active"));
        link.classList.add("active");
    });
});


// SCROLL SUAVE (caso queira reforçar)
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


// EFEITO NA NAVBAR AO SCROLL
const navbar = document.querySelector(".navbar");

window.addEventListener("scroll", () => {
    if (window.scrollY > 50) {
        navbar.style.borderBottom = "1px solid rgba(34,197,94,0.3)";
    } else {
        navbar.style.borderBottom = "1px solid var(--border)";
    }
});