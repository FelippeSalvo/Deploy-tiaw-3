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

function renderizarComponente(tipo, nome) {
    const nomeHtml = nome ? `<span class="comp-name">${nome}</span>` : `<span class="comp-empty">Não selecionado</span>`;
    return `
        <tr>
            <td class="comp-type">${tipo}</td>
            <td>${nomeHtml}</td>
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

        const tbody = document.getElementById('detail-components');
        tbody.innerHTML = `
            ${renderizarComponente('Processador', build.cpu)}
            ${renderizarComponente('Placa de vídeo', build.gpu)}
            ${renderizarComponente('Placa-mãe', build.motherboard)}
            ${renderizarComponente('Memória RAM', build.ram)}
            ${renderizarComponente('Fonte', build.psu)}
        `;

        // Check ownership
        const sessao = obterSessao();
        if (sessao && build.usuarioId === sessao.id) {
            const btnEdit = document.getElementById('btn-edit');
            btnEdit.style.display = 'inline-flex';
            btnEdit.addEventListener('click', () => {
                // For editing, redirect to montagem with edit mode, or keep the old edit logic. 
                // Since old logic used a modal, for now we can just show a toast or implement a basic prompt.
                // Wait, previously editing was just a modal for Nome and Compartilhada.
                mostrarToast('Edição redirecionará para montagem em breve.', 'success');
            });
        }

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
