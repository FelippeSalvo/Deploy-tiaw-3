let currentBuildId = null;
let toastTimer = null;

function obterToken() {
    const sessao = obterSessao(); // defined in auth.js
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

const ICONES = {
    'Processador': '<svg viewBox="0 0 24 24"><path d="M7 4V2h2v2h2V2h2v2h2V2h2v2h2v2h2v2h-2v2h2v2h-2v2h2v2h-2v2h2v2h-2v2h-2v2h-2v-2h-2v2h-2v-2H9v2H7v-2H5v-2H3v-2h2v-2H3v-2h2v-2H3v-2h2V8H3V6h2V4h2zm2 2v12h6V6H9z"/></svg>',
    'Placa de vídeo': '<svg viewBox="0 0 24 24"><path d="M21 2H3c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h18c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm0 18H3V4h18v16zm-2-14H5v2h14V6zm0 4H5v8h14v-8z"/></svg>',
    'Placa-mãe': '<svg viewBox="0 0 24 24"><path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm0 16H5V5h14v14zm-7-2h2v-2h-2v2zm0-4h2v-2h-2v2zm0-4h2V7h-2v2z"/></svg>',
    'Memória RAM': '<svg viewBox="0 0 24 24"><path d="M2 9v6h20V9H2zm2 4v-2h2v2H4zm4 0v-2h2v2H8zm4 0v-2h2v2h-2zm4 0v-2h2v2h-2z"/></svg>',
    'Fonte': '<svg viewBox="0 0 24 24"><path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"/></svg>'
};

function renderizarComponente(tipo, nome) {
    const nomeOriginal = nome || '';
    const nomeHtml = nome ? `<span class="comp-name">${nome}</span>` : `<span class="comp-empty">Não selecionado</span>`;
    const iconeSvg = ICONES[tipo] || '<svg viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>';
    
    let actionsHtml = '';
    if (nome) {
        const queryBusca = encodeURIComponent(nomeOriginal);
        actionsHtml = `<a href="https://www.google.com/search?q=${queryBusca}+preço+comprar" target="_blank" rel="noopener noreferrer" class="btn-search-price">
            <svg viewBox="0 0 24 24" style="width:16px;height:16px;fill:currentColor;"><path d="M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/></svg>
            Buscar Preço
        </a>`;
    }

    return `
        <tr>
            <td>
                <div class="comp-type-wrapper">
                    <div class="component-icon-box">${iconeSvg}</div>
                    <span class="comp-type">${tipo}</span>
                </div>
            </td>
            <td>${nomeHtml}</td>
            <td>${actionsHtml}</td>
        </tr>
    `;
}

async function carregarDetalhes() {
    const params = new URLSearchParams(window.location.search);
    const id = params.get('id');
    
    if (!id) {
        document.getElementById('loading-detail').textContent = 'Build não encontrada (ID ausente).';
        document.getElementById('loading-detail').classList.add('error-state');
        return;
    }

    currentBuildId = id;

    try {
        const token = obterToken();
        const headers = {};
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const res = await fetch(`${API_BASE}/builds/${id}`, { headers });
        if (!res.ok) {
            const data = await res.json().catch(() => ({}));
            document.getElementById('loading-detail').textContent = data.message || 'Erro ao carregar build.';
            document.getElementById('loading-detail').classList.add('error-state');
            return;
        }

        const build = await res.json();
        
        // Populate UI
        document.getElementById('loading-detail').style.display = 'none';
        document.getElementById('build-detail-content').style.display = 'block';
        
        document.getElementById('detail-title').textContent = build.nome;
        
        const autor = build.usuario?.nome || 'Usuário';
        const dataFormatada = new Date(build.criadaEm).toLocaleDateString('pt-BR');
        document.getElementById('detail-meta').innerHTML = `Criado por <span class="build-meta-highlight">${autor}</span> em <span class="build-meta-highlight">${dataFormatada}</span>`;

        if (build.descricao && build.descricao.trim() !== '') {
            document.getElementById('detail-description-text').textContent = build.descricao;
            document.getElementById('detail-description-container').style.display = 'block';
        } else {
            document.getElementById('detail-description-container').style.display = 'none';
        }

        const tagsContainer = document.getElementById('detail-tags');
        if (tagsContainer) {
            tagsContainer.innerHTML = '';
            if (build.compativel === false) {
                const badge = document.createElement('span');
                badge.className = 'build-badge incompatible';
                badge.textContent = 'Incompatível';
                tagsContainer.appendChild(badge);
            }
        }

        const tbody = document.getElementById('detail-components');
        tbody.innerHTML = `
            ${renderizarComponente('Processador', build.cpu)}
            ${renderizarComponente('Placa de vídeo', build.gpu)}
            ${renderizarComponente('Placa-mãe', build.motherboard)}
            ${renderizarComponente('Memória RAM', build.ram)}
            ${renderizarComponente('Fonte', build.psu)}
        `;



    } catch (e) {
        document.getElementById('loading-detail').textContent = `Falha de conexão: ${e.message}`;
        document.getElementById('loading-detail').classList.add('error-state');
    }
}

document.addEventListener('DOMContentLoaded', () => {
    carregarDetalhes();

    const btnShare = document.getElementById('btn-share');
    if (btnShare) {
        btnShare.addEventListener('click', () => {
            navigator.clipboard.writeText(window.location.href).then(() => {
                mostrarToast('Link copiado para a área de transferência!');
            }).catch(() => {
                mostrarToast('Erro ao copiar link.', 'error');
            });
        });
    }
});
