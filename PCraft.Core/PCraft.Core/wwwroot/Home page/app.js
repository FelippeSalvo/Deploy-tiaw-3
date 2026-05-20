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

if (navbar) {
    window.addEventListener("scroll", () => {
        if (window.scrollY > 50) {
            navbar.style.borderBottom = "1px solid rgba(34,197,94,0.3)";
        } else {
            navbar.style.borderBottom = "1px solid var(--border)";
        }
    });
}

// BUILDS HOME — usa API_BASE de ../Autenticacao/auth.js (carregar antes deste script)

async function carregarBuildsHome() {

    const list = document.getElementById('builds-list');

    if (!list) return;

    list.innerHTML = `
        <div class="loading-state">
            Carregando builds...
        </div>
    `;

    try {

        const res = await fetch(`${API_BASE}/builds/publicas`);

        const data = await res.json().catch(() => []);

        if (!res.ok) {

            list.innerHTML = `
                <div class="error-state">
                    Erro ao carregar builds.
                </div>
            `;

            return;
        }

        const builds = Array.isArray(data)
            ? data
                .sort((a, b) => new Date(b.criadaEm) - new Date(a.criadaEm))
                .slice(0, 3)
            : [];

        if (!builds.length) {

            list.innerHTML = `
                <div class="empty-state">
                    Nenhuma build encontrada.
                </div>
            `;

            return;
        }

        list.innerHTML = builds.map((build) => {

            const placeholderIndex =
                ((build.id || build.nome.length) % 4) + 1;

            const imgPath =
                `../Builds/img/placeholder_${placeholderIndex}.png`;

            return `
                <article class="build-card">

                    <div class="build-card-image">
                        <img 
                            src="${imgPath}" 
                            alt="Build placeholder"
                            loading="lazy"
                        >
                    </div>

                    <div class="build-card-content">

                        <h3 class="build-card-title">
                            ${build.nome}
                        </h3>

                        <div class="build-specs-box">

                            <p class="build-desc">
                                ${build.descricao || 'Nenhuma descrição fornecida para esta configuração.'}
                            </p>

                            <ul class="build-specs">

                                <li>
                                    <span class="spec-value">
                                        • ${build.cpu || 'CPU não informada'}
                                    </span>
                                </li>

                                <li>
                                    <span class="spec-value">
                                        • ${build.gpu || 'GPU não informada'}
                                    </span>
                                </li>

                                ${build.motherboard
                                    ? `
                                        <li>
                                            <span class="spec-value">
                                                • ${build.motherboard}
                                            </span>
                                        </li>
                                    `
                                    : ''
                                }

                            </ul>

                        </div>

                        <div class="build-badges">
                            <span class="build-badge">
                                Gaming
                            </span>

                            ${build.compativel === false
                                ? `
                                    <span class="build-badge incompatible">
                                        Incompatível
                                    </span>
                                `
                                : ''
                            }
                        </div>

                        <div class="build-footer">

                            <div class="build-author-info">

                                <span class="build-meta-author">
                                    Por <strong>
                                        ${build.usuario?.nome || 'Usuário'}
                                    </strong>
                                </span>

                                <span class="build-meta-sep">•</span>

                                <span class="build-meta-date">
                                    ${new Date(build.criadaEm).toLocaleDateString('pt-BR')}
                                </span>

                            </div>

                        </div>

                        <div class="card-actions">

                            <button
                                type="button"
                                class="btn-action btn-action--primary"
                                onclick="window.location.href='../Builds/BuildsUsuarios.html'"
                            >
                                Ver build
                            </button>

                        </div>

                    </div>

                </article>
            `;

        }).join('');

    } catch (e) {

        list.innerHTML = `
            <div class="error-state">
                Falha de conexão.
            </div>
        `;

        console.error(e);
    }
}

carregarBuildsHome();

// INDICADORES DA COMUNIDADE

async function carregarIndicadoresComunidade() {

    try {

        const response = await fetch(`${API_BASE}/builds/publicas`);

        const builds = await response.json();

        if (!Array.isArray(builds)) return;

        const cpuCount = {};
        const gpuCount = {};
        const userCount = {};

        builds.forEach(build => {

            // CPU
            if (build.cpu) {

                cpuCount[build.cpu] =
                    (cpuCount[build.cpu] || 0) + 1;
            }

            // GPU
            if (build.gpu) {

                gpuCount[build.gpu] =
                    (gpuCount[build.gpu] || 0) + 1;
            }

            // USER
            const user =
                build.usuario?.nome || "Usuário";

            userCount[user] =
                (userCount[user] || 0) + 1;

        });

        const topCpu =
            Object.entries(cpuCount)
                .sort((a, b) => b[1] - a[1])[0];

        const topGpu =
            Object.entries(gpuCount)
                .sort((a, b) => b[1] - a[1])[0];

        const topUser =
            Object.entries(userCount)
                .sort((a, b) => b[1] - a[1])[0];

        // RENDER

        const elCpu = document.getElementById("topCpu");
        const elGpu = document.getElementById("topGpu");
        const elUser = document.getElementById("topUser");

        if (elCpu) {
            elCpu.textContent = topCpu ? `${topCpu[0]} (${topCpu[1]})` : "Sem dados";
        }
        if (elGpu) {
            elGpu.textContent = topGpu ? `${topGpu[0]} (${topGpu[1]})` : "Sem dados";
        }
        if (elUser) {
            elUser.textContent = topUser ? `${topUser[0]} (${topUser[1]})` : "Sem dados";
        }

    } catch (error) {

        console.error(
            "Erro ao carregar indicadores:",
            error
        );

    }
}

carregarIndicadoresComunidade();

