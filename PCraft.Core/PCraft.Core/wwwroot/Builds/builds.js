let buildsCache = [];
let buildEditandoId = null;
let toastTimer = null;

function obterSessao() {
    const raw = localStorage.getItem(AUTH_STORAGE_KEY);
    if (!raw) return null;
    try {
        return JSON.parse(raw);
    } catch (_) {
        return null;
    }
}

function obterToken() {
    const sessao = obterSessao();
    if (!sessao?.token) return null;
    if (!sessao.expiresAt || Number(sessao.expiresAt) <= Date.now()) return null;
    return sessao.token;
}

function mostrarToast(mensagem, tipo = 'success') {
    const toast = document.getElementById('toast-message');
    if (!toast) return;
    toast.textContent = mensagem;
    toast.className = `toast-message show ${tipo}`;
    if (toastTimer) clearTimeout(toastTimer);
    toastTimer = setTimeout(() => {
        toast.className = 'toast-message';
    }, 2400);
}

function setEstadoLista(tipo, texto) {
    const list = document.getElementById('builds-list');
    if (!list) return;
    list.innerHTML = `<div class="${tipo}">${texto}</div>`;
}

function formatarData(valor) {
    if (!valor) return '--';
    const data = new Date(valor);
    if (Number.isNaN(data.getTime())) return '--';
    return data.toLocaleDateString('pt-BR');
}

function linhaSpecModal(label, valor) {
    const v = valor && String(valor).trim() ? valor : '—';
    return `<li><span class="spec-label">${label}</span><span class="spec-value">${v}</span></li>`;
}

function abrirModalDetalhes(build) {
    const modal = document.getElementById('build-modal');
    const content = document.getElementById('build-modal-content');
    if (!modal || !content) return;
    const autor = build.usuario?.nome || 'Usuário';
    const statusClass = build.compartilhada ? 'publica' : '';
    const statusText = build.compartilhada ? 'Pública' : 'Privada';
    content.innerHTML = `
        <div class="build-detail">
            <div class="build-detail-head">
                <span class="build-card-icon build-detail-icon" aria-hidden="true">${inicialNomeBuild(build.nome)}</span>
                <div class="build-detail-head-text">
                    <h3 class="build-detail-title">${build.nome}</h3>
                    <p class="build-detail-meta">
                        <span class="build-meta-author">${autor}</span>
                        <span class="build-meta-sep">•</span>
                        <span class="build-meta-date">${formatarData(build.criadaEm)}</span>
                    </p>
                </div>
            </div>
            <span class="build-status build-detail-badge ${statusClass}">${statusText}</span>
            <p class="build-detail-section-title">Configuração</p>
            <ul class="build-specs build-detail-specs">
                ${linhaSpecModal('CPU', build.cpu)}
                ${linhaSpecModal('Placa-mãe', build.motherboard)}
                ${linhaSpecModal('RAM', build.ram)}
                ${linhaSpecModal('GPU', build.gpu)}
                ${linhaSpecModal('Fonte', build.psu)}
            </ul>
        </div>
    `;
    modal.classList.add('is-open');
    modal.setAttribute('aria-hidden', 'false');
}

function fecharModalDetalhes() {
    const modal = document.getElementById('build-modal');
    if (!modal) return;
    modal.classList.remove('is-open');
    modal.setAttribute('aria-hidden', 'true');
}

function abrirModalEdicao(build) {
    buildEditandoId = build.id;
    const modal = document.getElementById('edit-modal');
    const nome = document.getElementById('edit-build-name');
    const compartilhada = document.getElementById('edit-build-shared');
    if (!modal || !nome || !compartilhada) return;

    nome.value = build.nome || '';
    compartilhada.checked = Boolean(build.compartilhada);
    modal.classList.add('is-open');
    modal.setAttribute('aria-hidden', 'false');
}

function fecharModalEdicao() {
    const modal = document.getElementById('edit-modal');
    if (!modal) return;
    modal.classList.remove('is-open');
    modal.setAttribute('aria-hidden', 'true');
    buildEditandoId = null;
}

function inicialNomeBuild(nome) {
    const t = (nome || 'B').trim();
    return t.length ? t[0].toUpperCase() : 'B';
}

function renderizarCards(builds, view) {
    const list = document.getElementById('builds-list');
    if (!list) return;

    if (!builds.length) {
        setEstadoLista('empty-state', 'Nenhuma build encontrada.');
        return;
    }

    list.innerHTML = builds.map((build) => `
        <article class="build-card">
            <div class="build-card-head">
                <span class="build-card-icon" aria-hidden="true">${inicialNomeBuild(build.nome)}</span>
                <div class="build-card-head-text">
                    <h3 class="build-card-title">${build.nome}</h3>
                    <p class="build-meta">
                        ${view === 'public' ? `<span class="build-meta-author">${build.usuario?.nome || 'Usuário'}</span><span class="build-meta-sep">•</span>` : ''}
                        <span class="build-meta-date">${formatarData(build.criadaEm)}</span>
                    </p>
                </div>
            </div>
            <ul class="build-specs">
                <li><span class="spec-label">CPU</span><span class="spec-value">${build.cpu || '—'}</span></li>
                <li><span class="spec-label">GPU</span><span class="spec-value">${build.gpu || '—'}</span></li>
            </ul>
            ${
                view === 'mine'
                    ? `<span class="build-status ${build.compartilhada ? 'publica' : ''}">
                        ${build.compartilhada ? 'Pública' : 'Privada'}
                       </span>`
                    : ''
            }
            <div class="card-actions">
                ${
                    view === 'mine'
                        ? `
                            <button type="button" class="btn-action btn-action--primary" data-action="open" data-id="${build.id}">Abrir</button>
                            <button type="button" class="btn-action btn-action--ghost" data-action="edit" data-id="${build.id}">Editar</button>
                            <button type="button" class="btn-action btn-action--danger" data-action="delete" data-id="${build.id}">Excluir</button>
                          `
                        : `
                            <button type="button" class="btn-action btn-action--primary" data-action="view" data-id="${build.id}">Ver build</button>
                          `
                }
            </div>
        </article>
    `).join('');
}

async function carregarMinhasBuilds() {
    const token = obterToken();
    if (!token) {
        window.location.href = '../Autenticacao/Login.html';
        return;
    }

    setEstadoLista('loading-state', 'Carregando builds...');

    try {
        const res = await fetch(`${API_BASE}/builds/me`, {
            headers: { Authorization: `Bearer ${token}` }
        });
        const data = await res.json().catch(() => []);
        if (!res.ok) {
            setEstadoLista('error-state', data.message || 'Erro ao carregar suas builds.');
            return;
        }
        buildsCache = Array.isArray(data) ? data : [];
        renderizarCards(buildsCache, 'mine');
    } catch (e) {
        setEstadoLista('error-state', `Falha de conexão: ${e.message}`);
    }
}

async function carregarBuildsPublicas() {
    setEstadoLista('loading-state', 'Carregando builds públicas...');

    try {
        const res = await fetch(`${API_BASE}/builds/publicas`);
        const data = await res.json().catch(() => []);
        if (!res.ok) {
            setEstadoLista('error-state', data.message || 'Erro ao carregar builds públicas.');
            return;
        }
        buildsCache = Array.isArray(data) ? data : [];
        renderizarCards(buildsCache, 'public');
    } catch (e) {
        setEstadoLista('error-state', `Falha de conexão: ${e.message}`);
    }
}

async function excluirBuild(id) {
    const token = obterToken();
    if (!token) {
        window.location.href = '../Autenticacao/Login.html';
        return;
    }

    const confirmou = window.confirm('Deseja realmente excluir esta build?');
    if (!confirmou) return;

    try {
        const res = await fetch(`${API_BASE}/builds/${id}`, {
            method: 'DELETE',
            headers: { Authorization: `Bearer ${token}` }
        });
        if (!res.ok) {
            const data = await res.json().catch(() => ({}));
            mostrarToast(data.message || 'Erro ao excluir build.', 'error');
            return;
        }
        mostrarToast('Build excluída com sucesso.');
        await carregarMinhasBuilds();
    } catch (e) {
        mostrarToast(`Falha ao excluir: ${e.message}`, 'error');
    }
}

async function salvarEdicao() {
    const token = obterToken();
    if (!token || !buildEditandoId) {
        window.location.href = '../Autenticacao/Login.html';
        return;
    }

    const atual = buildsCache.find((b) => b.id === buildEditandoId);
    if (!atual) return;

    const nome = document.getElementById('edit-build-name').value.trim();
    const compartilhada = document.getElementById('edit-build-shared').checked;

    if (!nome) {
        mostrarToast('Nome da build é obrigatório.', 'error');
        return;
    }

    const payload = {
        nome,
        compartilhada,
        cpuId: atual.cpuId ?? null,
        motherboardId: atual.motherboardId ?? null,
        ramId: atual.ramId ?? null,
        gpuId: atual.gpuId ?? null,
        psuId: atual.psuId ?? null
    };

    try {
        const res = await fetch(`${API_BASE}/builds/${buildEditandoId}`, {
            method: 'PUT',
            headers: {
                Authorization: `Bearer ${token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });
        const data = await res.json().catch(() => ({}));
        if (!res.ok) {
            mostrarToast(data.message || 'Erro ao editar build.', 'error');
            return;
        }
        fecharModalEdicao();
        mostrarToast('Build atualizada com sucesso.');
        await carregarMinhasBuilds();
    } catch (e) {
        mostrarToast(`Falha ao editar: ${e.message}`, 'error');
    }
}

function conectarEventos(view) {
    const list = document.getElementById('builds-list');
    const modalClose = document.getElementById('build-modal-close');
    const modalBackdrop = document.querySelector('#build-modal .modal-backdrop');
    const editClose = document.getElementById('edit-modal-close');
    const editCancel = document.getElementById('edit-cancel');
    const editSave = document.getElementById('edit-save');
    const editBackdrop = document.querySelector('#edit-modal .modal-backdrop');

    modalClose?.addEventListener('click', fecharModalDetalhes);
    modalBackdrop?.addEventListener('click', fecharModalDetalhes);
    editClose?.addEventListener('click', fecharModalEdicao);
    editCancel?.addEventListener('click', fecharModalEdicao);
    editSave?.addEventListener('click', salvarEdicao);
    editBackdrop?.addEventListener('click', fecharModalEdicao);

    list?.addEventListener('click', async (event) => {
        const alvo = event.target.closest('button[data-action]');
        if (!alvo) return;
        const action = alvo.getAttribute('data-action');
        const id = Number(alvo.getAttribute('data-id'));
        const build = buildsCache.find((b) => b.id === id);
        if (!build) return;

        if (action === 'open' || action === 'view') {
            abrirModalDetalhes(build);
            return;
        }
        if (action === 'edit' && view === 'mine') {
            abrirModalEdicao(build);
            return;
        }
        if (action === 'delete' && view === 'mine') {
            await excluirBuild(id);
            return;
        }
    });
}

async function iniciarPaginaBuilds() {
    const view = document.body.getAttribute('data-builds-view');
    conectarEventos(view);
    if (view === 'mine') {
        await carregarMinhasBuilds();
    } else {
        await carregarBuildsPublicas();
    }
}

document.addEventListener('DOMContentLoaded', iniciarPaginaBuilds);
