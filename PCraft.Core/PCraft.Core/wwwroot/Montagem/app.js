const configComponentes = [
    { id: 'cpu', nome: 'Processador', icon: 'CPU', endpoint: 'cpus' },
    { id: 'motherboard', nome: 'Placa Mãe', icon: 'MOBO', endpoint: 'motherboards' },
    { id: 'ram', nome: 'Memória RAM', icon: 'RAM', endpoint: 'rams' },
    { id: 'gpu', nome: 'Placa de Vídeo', icon: 'GPU', endpoint: 'gpus' },
    { id: 'psu', nome: 'Fonte', icon: 'PSU', endpoint: 'psus' }
];

const nomesComponentes = Object.fromEntries(configComponentes.map(c => [c.id, c.nome]));
const componentesDados = { cpu: [], motherboard: [], ram: [], gpu: [], psu: [] };
const componentesSelecionados = { cpu: null, motherboard: null, ram: null, gpu: null, psu: null };

let componenteAtualModal = null;
let toastTimeoutId = null;

async function carregarComponentes() {
    const promessas = configComponentes.map(async (cfg) => {
        try {
            const res = await fetch(`${API_BASE}/${cfg.endpoint}`);
            const data = await res.json();
            componentesDados[cfg.id] = Array.isArray(data) ? data : [];
        } catch (e) {
            console.error(`Erro ao carregar ${cfg.endpoint}:`, e);
            componentesDados[cfg.id] = [];
        }
    });

    await Promise.all(promessas);
}

function renderizarListaPrincipal() {
    const container = document.getElementById('components-list');
    if (!container) return;

    container.innerHTML = configComponentes.map(comp => `
        <article class="component-item" onclick="openComponentModal('${comp.id}')">
            <div class="component-info">
                <span class="component-icon">${comp.icon}</span>
                <div class="component-text">
                    <h3>${comp.nome}</h3>
                    <p class="component-status" id="${comp.id}-status">Não selecionado</p>
                    <div class="component-meta" id="${comp.id}-meta"></div>
                </div>
            </div>
            <div class="component-actions">
                <button class="btn-remove" type="button" onclick="removerPeca(event, '${comp.id}')">
                    Remover peça
                </button>
                <button class="btn-add-component" type="button" onclick="openComponentModal('${comp.id}', event)">
                    +
                </button>
            </div>
        </article>
    `).join('');
}

function openComponentModal(tipo, event) {
    if (event) {
        event.stopPropagation();
        event.preventDefault();
    }

    componenteAtualModal = tipo;

    const overlay = document.getElementById('component-modal');
    const titulo = document.getElementById('modal-title');
    const subtitulo = document.getElementById('modal-subtitle');
    const inputBusca = document.getElementById('modal-search');

    titulo.textContent = `Selecionar ${nomesComponentes[tipo]}`;
    subtitulo.textContent = 'Digite para filtrar as peças disponíveis em tempo real e clique para selecionar.';

    overlay.classList.add('is-open');
    overlay.setAttribute('aria-hidden', 'false');

    if (inputBusca) {
        inputBusca.value = '';
        renderizarListaModal();
        setTimeout(() => inputBusca.focus(), 50);
    }
}

function closeComponentModal() {
    const overlay = document.getElementById('component-modal');
    if (!overlay) return;

    overlay.classList.remove('is-open');
    overlay.setAttribute('aria-hidden', 'true');
    componenteAtualModal = null;
}

function renderizarListaModal() {
    const listaEl = document.getElementById('modal-list');
    const inputBusca = document.getElementById('modal-search');

    if (!listaEl || !inputBusca || !componenteAtualModal) return;

    const termo = inputBusca.value.trim().toLowerCase();
    const dados = componentesDados[componenteAtualModal] || [];

    const filtrados = dados.filter((item) => {
        const texto = (item.nome || '').toLowerCase();
        return termo === '' || texto.includes(termo);
    });

    if (filtrados.length === 0) {
        listaEl.innerHTML = '<div class="modal-empty">Nenhuma peça encontrada para esse filtro.</div>';
        return;
    }

    listaEl.innerHTML = filtrados.map((item) => {
        const consumo = item.consumoEnergia != null ? `${item.consumoEnergia} W`
            : item.consumo != null ? `${item.consumo} W` : null;

        const metaHtml = consumo ? `<span>Consumo: ${consumo}</span>` : '';

        return `
            <button class="modal-item" type="button" data-id="${item.id}">
                <div class="modal-item-main">
                    <div class="modal-item-title">${item.nome}</div>
                    <div class="modal-item-meta">${metaHtml}</div>
                </div>
            </button>
        `;
    }).join('');
}

function selecionarPecaNoModal(idItem) {
    if (!componenteAtualModal) return;

    const dados = componentesDados[componenteAtualModal] || [];
    const selecionado = dados.find((d) => String(d.id) === String(idItem));

    if (selecionado) {
        aoSelecionar(componenteAtualModal, selecionado);
        closeComponentModal();
    }
}

function aoSelecionar(componente, item) {
    componentesSelecionados[componente] = item;

    const statusEl = document.getElementById(`${componente}-status`);
    const metaEl = document.getElementById(`${componente}-meta`);

    if (statusEl) {
        statusEl.textContent = item.nome || 'Selecionado';
        statusEl.style.color = '#22c55e'; // Verde = sucesso
    }

    if (metaEl) {
        const consumo = item.consumoEnergia != null ? `${item.consumoEnergia} W`
            : item.consumo != null ? `${item.consumo} W` : null;

        metaEl.innerHTML = consumo ? `<span>${consumo}</span>` : '';
    }

    atualizarResumo();
}

function removerPeca(event, tipo) {
    if (event) {
        event.stopPropagation();
        event.preventDefault();
    }

    componentesSelecionados[tipo] = null;

    const statusEl = document.getElementById(`${tipo}-status`);
    const metaEl = document.getElementById(`${tipo}-meta`);

    if (statusEl) {
        statusEl.textContent = 'Não selecionado';
        statusEl.style.color = '#a0a0a5';
    }
    if (metaEl) metaEl.innerHTML = '';

    atualizarResumo();
}

function atualizarResumo() {
    const resumo = document.getElementById('selected-components');
    if (!resumo) return;

    const itensHtml = [];

    Object.keys(componentesSelecionados).forEach((tipo) => {
        const item = componentesSelecionados[tipo];
        if (!item) return;

        itensHtml.push(`
            <div class="selected-item">
                <span>${nomesComponentes[tipo]}</span>
                <span>${item.nome || ''}</span>
            </div>
        `);
    });

    if (itensHtml.length > 0) {
        resumo.innerHTML = itensHtml.join('');
    } else {
        resumo.innerHTML = '<p class="empty-state">Nenhum componente selecionado</p>';
    }
}

async function verificarCompatibilidade() {
    const request = {
        cpuId: componentesSelecionados.cpu?.id ? parseInt(componentesSelecionados.cpu.id) : null,
        motherboardId: componentesSelecionados.motherboard?.id ? parseInt(componentesSelecionados.motherboard.id) : null,
        ramId: componentesSelecionados.ram?.id ? parseInt(componentesSelecionados.ram.id) : null,
        gpuId: componentesSelecionados.gpu?.id ? parseInt(componentesSelecionados.gpu.id) : null,
        psuId: componentesSelecionados.psu?.id ? parseInt(componentesSelecionados.psu.id) : null
    };

    if (!request.cpuId && !request.motherboardId && !request.ramId && !request.gpuId && !request.psuId) {
        document.getElementById('compatibility-result').innerHTML =
            '<p class="incompatible">Selecione pelo menos um componente</p>';
        return;
    }

    const resultDiv = document.getElementById('compatibility-result');
    resultDiv.innerHTML = '<p style="color: #a0a0a5;">Verificando...</p>';

    try {
        const res = await fetch(`${API_BASE}/compatibilidade/check`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(request)
        });

        const data = await res.json();

        if (!res.ok) {
            resultDiv.innerHTML = `<p class="incompatible">${data.message || 'Erro na requisição'}</p>`;
            return;
        }

        let html = data.compativel
            ? `<div class="compatible">Componentes Compatíveis!</div>`
            : `<div class="incompatible">Incompatibilidade Detectada</div>`;

        if (data.problemas?.length > 0) {
            data.problemas.forEach((problema) => {
                html += `
                    <div class="issue">
                        <strong>${problema.componente1} + ${problema.componente2}</strong>
                        ${problema.problema}
                    </div>
                `;
            });
        }

        resultDiv.innerHTML = html;

        if (data.consumoTotalEnergia != null) {
            document.getElementById('total-power').textContent = `${data.consumoTotalEnergia}W`;
        }
        if (data.psuRecomendada != null) {
            document.getElementById('recommended-psu').textContent = `Fonte recomendada: ${data.psuRecomendada}W`;
        }
    } catch (e) {
        resultDiv.innerHTML = `<p class="incompatible">Erro: ${e.message}</p>`;
    }
}

function abrirModalSalvar() {
    const modal = document.getElementById('save-build-modal');
    if (!modal) return;

    document.getElementById('build-name-input').value = '';
    document.getElementById('build-desc-input').value = '';
    document.getElementById('build-share-input').checked = false;

    modal.classList.add('is-open');
    modal.setAttribute('aria-hidden', 'false');
    setTimeout(() => document.getElementById('build-name-input').focus(), 40);
}

function fecharModalSalvar() {
    const modal = document.getElementById('save-build-modal');
    if (!modal) return;

    modal.classList.remove('is-open');
    modal.setAttribute('aria-hidden', 'true');
}

function obterToken() {
    try {
        const sessao = JSON.parse(localStorage.getItem('pcraft.auth'));
        return sessao?.token || null;
    } catch {
        return null;
    }
}

function obterBuildAtual() {
    return {
        Nome: document.getElementById('build-name-input').value.trim(),
        Descricao: document.getElementById('build-desc-input').value.trim(),
        Compartilhada: document.getElementById('build-share-input').checked,
        CpuId: componentesSelecionados.cpu?.id ?? null,
        MotherboardId: componentesSelecionados.motherboard?.id ?? null,
        RamId: componentesSelecionados.ram?.id ?? null,
        GpuId: componentesSelecionados.gpu?.id ?? null,
        PsuId: componentesSelecionados.psu?.id ?? null
    };
}

function mostrarToast(mensagem, tipo = 'success') {
    const toast = document.getElementById('toast-message');
    if (!toast) return;

    toast.textContent = mensagem;
    toast.className = `toast-message show ${tipo}`;

    if (toastTimeoutId) clearTimeout(toastTimeoutId);
    toastTimeoutId = setTimeout(() => {
        toast.className = 'toast-message';
    }, 2500);
}

async function salvarBuild() {
    const token = obterToken();
    if (!token) {
        mostrarToast('Faça login para salvar builds', 'error');
        return;
    }

    const payload = obterBuildAtual();

    if (!payload.Nome) {
        mostrarToast('Informe um nome para a build', 'error');
        return;
    }
    if (!payload.CpuId && !payload.MotherboardId && !payload.RamId && !payload.GpuId && !payload.PsuId) {
        mostrarToast('Selecione pelo menos um componente', 'error');
        return;
    }

    const btnSalvar = document.getElementById('confirm-save-build');
    const textoOriginal = btnSalvar.textContent;
    btnSalvar.disabled = true;
    btnSalvar.textContent = 'Salvando...';

    try {
        const res = await fetch(`${API_BASE}/builds`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(payload)
        });

        const data = await res.json().catch(() => ({}));

        if (!res.ok) {
            mostrarToast(data.message || 'Erro ao salvar build', 'error');
            return;
        }

        fecharModalSalvar();
        mostrarToast('Build salva com sucesso', 'success');
    } catch (e) {
        mostrarToast(`Erro: ${e.message}`, 'error');
    } finally {
        btnSalvar.disabled = false;
        btnSalvar.textContent = textoOriginal;
    }
}

function inicializarMontagem() {
    renderizarListaPrincipal();
    carregarComponentes();

    const addListener = (selector, event, callback) => {
        const element = typeof selector === 'string' ? document.getElementById(selector) : selector;
        if (element) element.addEventListener(event, callback);
    };

    const overlay = document.getElementById('component-modal');
    addListener(overlay?.querySelector('.modal-backdrop'), 'click', closeComponentModal);
    addListener('modal-close-btn', 'click', closeComponentModal);
    addListener('modal-search', 'input', renderizarListaModal);

    addListener('modal-list', 'click', (e) => {
        const target = e.target.closest('[data-id]');
        if (target) selecionarPecaNoModal(target.getAttribute('data-id'));
    });

    const saveModal = document.getElementById('save-build-modal');
    addListener(saveModal?.querySelector('.modal-backdrop'), 'click', fecharModalSalvar);
    addListener('btn-salvar-build', 'click', abrirModalSalvar);
    addListener('save-build-close-btn', 'click', fecharModalSalvar);
    addListener('cancel-save-build', 'click', fecharModalSalvar);
    addListener('confirm-save-build', 'click', salvarBuild);

    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
            closeComponentModal();
            fecharModalSalvar();
        }
    });
}

document.addEventListener('DOMContentLoaded', inicializarMontagem);
