const MONTAGEM_API = `${window.location.origin}/api`;

const nomesComponentes = {
    cpu: 'Processador',
    motherboard: 'Placa Mãe',
    ram: 'Memória RAM',
    gpu: 'Placa de Vídeo',
    psu: 'Fonte'
};

async function carregarComponentes() {
    const endpoints = ['cpus', 'motherboards', 'rams', 'gpus', 'psus'];

    for (const endpoint of endpoints) {
        try {
            const res = await fetch(`${MONTAGEM_API}/${endpoint}`);
            const data = await res.json();
            const select = document.getElementById(endpoint.slice(0, -1));

            data.forEach(item => {
                const opt = document.createElement('option');
                opt.value = item.id;
                opt.textContent = item.nome;
                select.appendChild(opt);
            });
        } catch (e) {
            console.error(`Erro ao carregar ${endpoint}:`, e);
        }
    }
}

function aoSelecionar(componente, select) {
    const statusEl = document.getElementById(`${componente}-status`);
    const opcaoSelecionada = select.options[select.selectedIndex];

    if (select.value) {
        statusEl.textContent = opcaoSelecionada.textContent;
        statusEl.style.color = '#22c55e';
    } else {
        statusEl.textContent = 'Não selecionado';
        statusEl.style.color = '#a0a0a5';
    }

    atualizarResumo();
}

function atualizarResumo() {
    const resumo = document.getElementById('selected-components');
    const componentes = ['cpu', 'motherboard', 'ram', 'gpu', 'psu'];
    let temSelecao = false;
    let html = '';

    componentes.forEach(comp => {
        const select = document.getElementById(comp);
        if (select.value) {
            temSelecao = true;
            const opcao = select.options[select.selectedIndex];
            html += `
                <div class="selected-item">
                    <span>${nomesComponentes[comp]}</span>
                    <span>${opcao.textContent}</span>
                </div>
            `;
        }
    });

    resumo.innerHTML = temSelecao ? html : '<p class="empty-state">Nenhum componente selecionado</p>';
}

async function verificarCompatibilidade() {
    const cpu = document.getElementById('cpu').value;
    const motherboard = document.getElementById('motherboard').value;
    const ram = document.getElementById('ram').value;
    const gpu = document.getElementById('gpu').value;
    const psu = document.getElementById('psu').value;

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
        const res = await fetch(`${MONTAGEM_API}/compatibility/check`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(request)
        });

        const data = await res.json();

        if (!res.ok) {
            resultDiv.innerHTML = `<p class="incompatible">${data.message || 'Erro na requisição'}</p>`;
            return;
        }

        let html = '';

        if (data.isCompatible) {
            html += `<div class="compatible">Componentes Compatíveis!</div>`;
        } else {
            html += `<div class="incompatible">Incompatibilidade Detectada</div>`;
        }

        if (data.issues && data.issues.length > 0) {
            data.issues.forEach(issue => {
                html += `
                    <div class="issue">
                        <strong>${issue.component1} + ${issue.component2}</strong>
                        ${issue.issue}
                    </div>
                `;
            });
        }

        html += `
            <div class="power-info">
                <p class="total-power">${data.totalPowerConsumption}W</p>
                <p class="recommended-psu">Fonte recomendada: ${data.recommendedPSU}W</p>
            </div>
        `;

        resultDiv.innerHTML = html;

    } catch (e) {
        resultDiv.innerHTML = `<p class="incompatible">Erro: ${e.message}</p>`;
    }
}

document.addEventListener('DOMContentLoaded', () => {
    carregarComponentes();
});
