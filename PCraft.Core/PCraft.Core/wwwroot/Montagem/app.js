// Configuração dos componentes e mapeamento com os endpoints da API
const nomesComponentes = {
    cpu: 'Processador',
    motherboard: 'Placa Mãe',
    ram: 'Memória RAM',
    gpu: 'Placa de Vídeo',
    psu: 'Fonte'
};

const componentesConfig = {
    cpu: { endpoint: 'cpus' },
    motherboard: { endpoint: 'motherboards' },
    ram: { endpoint: 'rams' },
    gpu: { endpoint: 'gpus' },
    psu: { endpoint: 'psus' }
};

// Dados carregados da API
const componentesDados = {
    cpu: [],
    motherboard: [],
    ram: [],
    gpu: [],
    psu: []
};

// Estado de seleção atual
const componentesSelecionados = {
    cpu: null,
    motherboard: null,
    ram: null,
    gpu: null,
    psu: null
};

// Estado do modal
let componenteAtualModal = null;
let toastTimeoutId = null;

// Carrega todos os componentes da API e guarda em memória
async function carregarComponentes() {
    for (const [tipo, cfg] of Object.entries(componentesConfig)) {
        try {
            const res = await fetch(`${API_BASE}/${cfg.endpoint}`);
            const data = await res.json();
            componentesDados[tipo] = Array.isArray(data) ? data : [];
        } catch (e) {
            console.error(`Erro ao carregar ${cfg.endpoint}:`, e);
            componentesDados[tipo] = [];
        }
    }
}

// Abre o modal para o tipo de componente informado
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
    subtitulo.textContent =
        'Digite para filtrar as peças disponíveis em tempo real e clique para selecionar.';

    overlay.classList.add('is-open');
    overlay.setAttribute('aria-hidden', 'false');

    if (inputBusca) {
        inputBusca.value = '';
        renderizarListaModal();
        // timeout pequeno para garantir que o input esteja visível
        setTimeout(() => inputBusca.focus(), 50);
    }
}

// Fecha o modal
function closeComponentModal() {
    const overlay = document.getElementById('component-modal');
    if (!overlay) return;

    overlay.classList.remove('is-open');
    overlay.setAttribute('aria-hidden', 'true');
    componenteAtualModal = null;
}

// Renderiza a lista de componentes dentro do modal
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
        listaEl.innerHTML =
            '<div class="modal-empty">Nenhuma peça encontrada para esse filtro.</div>';
        return;
    }

    const html = filtrados
        .map((item) => {
            const preco = item.preco != null ? `R$ ${item.preco}` : null;
            const consumo =
                item.consumoEnergia != null
                    ? `${item.consumoEnergia} W`
                    : item.consumo != null
                    ? `${item.consumo} W`
                    : null;

            const imagemUrl = item.imagemUrl || item.imagem || null;

            return `
                <button class="modal-item" type="button" data-id="${item.id}">
                    ${
                        imagemUrl
                            ? `<img src="${imagemUrl}" alt="${item.nome}" class="modal-item-image" />`
                            : '<div class="modal-item-image"></div>'
                    }
                    <div class="modal-item-main">
                        <div class="modal-item-title">${item.nome}</div>
                        <div class="modal-item-meta">
                            ${
                                preco
                                    ? `<span>Preço: ${preco}</span>`
                                    : ''
                            }
                            ${
                                consumo
                                    ? `<span>Consumo: ${consumo}</span>`
                                    : ''
                            }
                        </div>
                    </div>
                </button>
            `;
        })
        .join('');

    listaEl.innerHTML = html;
}

// Seleciona uma peça a partir do modal
function selecionarPecaNoModal(idItem) {
    if (!componenteAtualModal) return;

    const dados = componentesDados[componenteAtualModal] || [];
    const selecionado = dados.find((d) => String(d.id) === String(idItem));
    if (!selecionado) return;

    aoSelecionar(componenteAtualModal, selecionado);
    closeComponentModal();
}

// Remove a peça selecionada de um componente
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
    if (metaEl) {
        metaEl.innerHTML = '';
    }

    atualizarResumo();
}

// Mantém o nome da função para compatibilidade com o código existente
function aoSelecionar(componente, item) {
    componentesSelecionados[componente] = item;

    const statusEl = document.getElementById(`${componente}-status`);
    const metaEl = document.getElementById(`${componente}-meta`);

    if (statusEl) {
        statusEl.textContent = item.nome || 'Selecionado';
        statusEl.style.color = '#22c55e';
    }

    if (metaEl) {
        const preco = item.preco != null ? `R$ ${item.preco}` : null;
        const consumo =
            item.consumoEnergia != null
                ? `${item.consumoEnergia} W`
                : item.consumo != null
                ? `${item.consumo} W`
                : null;

        let metaHtml = '';
        if (preco) {
            metaHtml += `<span>${preco}</span>`;
        }
        if (consumo) {
            metaHtml += `<span>${consumo}</span>`;
        }

        metaEl.innerHTML = metaHtml;
    }

    atualizarResumo();
}

// Atualiza o resumo da build na lateral
function atualizarResumo() {
    const resumo = document.getElementById('selected-components');
    if (!resumo) return;

    let temSelecao = false;
    let html = '';
    let totalGasto = 0;

    Object.keys(componentesSelecionados).forEach((tipo) => {
        const item = componentesSelecionados[tipo];
        if (!item) return;

        temSelecao = true;
        const preco = item.preco != null ? item.preco : null;
        if (typeof preco === 'number') {
            totalGasto += preco;
        }

        html += `
            <div class="selected-item">
                <span>${nomesComponentes[tipo]}</span>
                <span>${item.nome || ''}</span>
            </div>
        `;
    });

    if (temSelecao && totalGasto > 0) {
        html += `
            <div class="selected-item" style="border-top: 1px solid var(--border); margin-top: 8px; padding-top: 10px;">
                <span>Total estimado</span>
                <span>R$ ${totalGasto.toFixed(2)}</span>
            </div>
        `;
    }

    resumo.innerHTML = temSelecao
        ? html
        : '<p class="empty-state">Nenhum componente selecionado</p>';
}

// Verifica compatibilidade usando o estado atual em memória
async function verificarCompatibilidade() {
    const cpu = componentesSelecionados.cpu?.id ?? null;
    const motherboard = componentesSelecionados.motherboard?.id ?? null;
    const ram = componentesSelecionados.ram?.id ?? null;
    const gpu = componentesSelecionados.gpu?.id ?? null;
    const psu = componentesSelecionados.psu?.id ?? null;

    if (!cpu && !motherboard && !ram && !gpu && !psu) {
        document.getElementById('compatibility-result').innerHTML =
            '<p class="incompatible">Selecione pelo menos um componente</p>';
        return;
    }

    const request = {
        cpuId: cpu ? parseInt(cpu) : null,
        motherboardId: motherboard ? parseInt(motherboard) : null,
        ramId: ram ? parseInt(ram) : null,
        gpuId: gpu ? parseInt(gpu) : null,
        psuId: psu ? parseInt(psu) : null
    };

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
            resultDiv.innerHTML = `<p class="incompatible">${
                data.message || 'Erro na requisição'
            }</p>`;
            return;
        }

        let html = '';

        if (data.compativel) {
            html += `<div class="compatible">Componentes Compatíveis!</div>`;
        } else {
            html += `<div class="incompatible">Incompatibilidade Detectada</div>`;
        }

        if (data.problemas && data.problemas.length > 0) {
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
            document.getElementById(
                'total-power'
            ).textContent = `${data.consumoTotalEnergia}W`;
        }
        if (data.psuRecomendada != null) {
            document.getElementById(
                'recommended-psu'
            ).textContent = `Fonte recomendada: ${data.psuRecomendada}W`;
        }
    } catch (e) {
        resultDiv.innerHTML = `<p class="incompatible">Erro: ${e.message}</p>`;
    }
}

function abrirModalSalvar() {
    const modal = document.getElementById('save-build-modal');
    const inputNome = document.getElementById('build-name-input');
    if (!modal) return;

    inputNome.value = '';
    document.getElementById('build-share-input').checked = false;
    modal.classList.add('is-open');
    modal.setAttribute('aria-hidden', 'false');
    setTimeout(() => inputNome.focus(), 40);
}

function fecharModalSalvar() {
    const modal = document.getElementById('save-build-modal');
    if (!modal) return;
    modal.classList.remove('is-open');
    modal.setAttribute('aria-hidden', 'true');
}

function obterToken() {
    const raw = localStorage.getItem('pcraft.auth');
    if (!raw) return null;

    try {
        const sessao = JSON.parse(raw);
        return sessao?.token || null;
    } catch (_) {
        return null;
    }
}

function obterBuildAtual() {
    return {
        Nome: document.getElementById('build-name-input').value.trim(),
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

// Inicialização da página de montagem
function inicializarMontagem() {
    carregarComponentes();

    const overlay = document.getElementById('component-modal');
    const backdrop = overlay?.querySelector('.modal-backdrop');
    const closeBtn = document.getElementById('modal-close-btn');
    const inputBusca = document.getElementById('modal-search');
    const listaEl = document.getElementById('modal-list');
    const saveModal = document.getElementById('save-build-modal');
    const saveBackdrop = saveModal?.querySelector('.modal-backdrop');
    const openSaveBtn = document.getElementById('btn-salvar-build');
    const closeSaveBtn = document.getElementById('save-build-close-btn');
    const cancelSaveBtn = document.getElementById('cancel-save-build');
    const confirmSaveBtn = document.getElementById('confirm-save-build');

    if (backdrop) {
        backdrop.addEventListener('click', () => closeComponentModal());
    }
    if (closeBtn) {
        closeBtn.addEventListener('click', () => closeComponentModal());
    }
    if (inputBusca) {
        inputBusca.addEventListener('input', () => renderizarListaModal());
    }
    if (listaEl) {
        listaEl.addEventListener('click', (e) => {
            const target = e.target.closest('[data-id]');
            if (!target) return;
            const id = target.getAttribute('data-id');
            selecionarPecaNoModal(id);
        });
    }
    if (openSaveBtn) {
        openSaveBtn.addEventListener('click', abrirModalSalvar);
    }
    if (closeSaveBtn) {
        closeSaveBtn.addEventListener('click', fecharModalSalvar);
    }
    if (cancelSaveBtn) {
        cancelSaveBtn.addEventListener('click', fecharModalSalvar);
    }
    if (saveBackdrop) {
        saveBackdrop.addEventListener('click', fecharModalSalvar);
    }
    if (confirmSaveBtn) {
        confirmSaveBtn.addEventListener('click', salvarBuild);
    }

    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
            closeComponentModal();
            fecharModalSalvar();
        }
    });
}

document.addEventListener('DOMContentLoaded', inicializarMontagem);
