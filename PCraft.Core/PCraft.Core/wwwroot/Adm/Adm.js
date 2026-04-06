const form = document.getElementById("pecaForm");
const tipoInput = document.getElementById("tipo");
const camposDinamicos = document.getElementById("camposDinamicos");

const buscaInput = document.getElementById("busca");
const filtroTipo = document.getElementById("filtroTipo");

const listaPecas = document.getElementById("listaPecas");
const totalPecas = document.getElementById("totalPecas");

const API_BASE = "http://localhost:5265/api";

const MODELOS = {
  CPU: {
    tipoLista: "Processador",
    endpoint: "CPUs",
    campos: [
      { name: "Nome", label: "Nome", type: "text", required: true },
      { name: "Fabricante", label: "Fabricante", type: "text", required: true },
      {
        name: "Socket",
        label: "Socket",
        type: "select",
        required: true,
        options: ["LGA1700", "LGA1200", "AM5", "AM4"]
      },
      { name: "TDP", label: "TDP (Watts)", type: "number", required: true, min: 1 }
    ]
  },
  GPU: {
    tipoLista: "Placa de Vídeo",
    endpoint: "GPUs",
    campos: [
      { name: "Nome", label: "Nome", type: "text", required: true },
      { name: "TDP", label: "TDP (Watts)", type: "number", required: true, min: 1 },
      {
        name: "ConsumoRecomendado",
        label: "Consumo recomendado (PSU mínima)",
        type: "number",
        required: true,
        min: 1
      },
      { name: "Comprimento", label: "Comprimento (mm)", type: "number", required: true, min: 1 }
    ]
  },
  Motherboard: {
    tipoLista: "Placa-mãe",
    endpoint: "Motherboards",
    campos: [
      { name: "Nome", label: "Nome", type: "text", required: true },
      {
        name: "Socket",
        label: "Socket",
        type: "select",
        required: true,
        options: ["LGA1700", "LGA1200", "AM5", "AM4"]
      },
      {
        name: "TipoRamSuportado",
        label: "Tipo de RAM suportado",
        type: "select",
        required: true,
        options: ["DDR4", "DDR5"]
      },
      {
        name: "CapacidadeMaximaRam",
        label: "Capacidade máxima de RAM (GB)",
        type: "number",
        required: true,
        min: 1
      },
      { name: "SlotsRam", label: "Slots de RAM", type: "number", required: true, min: 1 }
    ]
  },
  PSU: {
    tipoLista: "Fonte",
    endpoint: "PSUs",
    campos: [
      { name: "Nome", label: "Nome", type: "text", required: true },
      { name: "Potencia", label: "Potência (Watts)", type: "number", required: true, min: 1 },
      {
        name: "Certificacao",
        label: "Certificação",
        type: "select",
        required: true,
        options: ["Bronze", "Silver", "Gold", "Platinum"]
      }
    ]
  },
  RAM: {
    tipoLista: "Memória RAM",
    endpoint: "RAMs",
    campos: [
      { name: "Nome", label: "Nome", type: "text", required: true },
      {
        name: "Tipo",
        label: "Tipo",
        type: "select",
        required: true,
        options: ["DDR4", "DDR5"]
      },
      { name: "Capacidade", label: "Capacidade (GB por módulo)", type: "number", required: true, min: 1 },
      {
        name: "QuantidadeModulos",
        label: "Quantidade de módulos",
        type: "number",
        required: true,
        min: 1
      }
    ]
  }
};

let pecas = [];

function valorEnumCpuSocket(valor) {
  const mapa = {
    LGA1700: 0,
    LGA1200: 1,
    AM5: 2,
    AM4: 3
  };
  return mapa[valor];
}

function valorEnumRamType(valor) {
  const mapa = {
    DDR4: 0,
    DDR5: 1
  };
  return mapa[valor];
}

function valorEnumPsuCertification(valor) {
  const mapa = {
    Bronze: 0,
    Silver: 1,
    Gold: 2,
    Platinum: 3
  };
  return mapa[valor];
}

function textoSeguro(valor) {
  return valor !== undefined && valor !== null && valor !== "" ? valor : "-";
}

function renderizarCamposDinamicos(tipoSelecionado) {
  camposDinamicos.innerHTML = "";

  if (!tipoSelecionado || !MODELOS[tipoSelecionado]) return;

  const configuracao = MODELOS[tipoSelecionado];

  configuracao.campos.forEach((campo) => {
    const grupo = document.createElement("div");
    grupo.className = "form-group";

    const label = document.createElement("label");
    label.setAttribute("for", campo.name);
    label.textContent = campo.label;

    let input;

    if (campo.type === "select") {
      input = document.createElement("select");
      input.innerHTML = `
        <option value="">Selecione</option>
        ${campo.options.map(opcao => `<option value="${opcao}">${opcao}</option>`).join("")}
      `;
    } else {
      input = document.createElement("input");
      input.type = campo.type;
      input.placeholder = `Digite ${campo.label.toLowerCase()}`;
      if (campo.min !== undefined) input.min = campo.min;
    }

    input.id = campo.name;
    input.name = campo.name;
    input.required = !!campo.required;

    grupo.appendChild(label);
    grupo.appendChild(input);
    camposDinamicos.appendChild(grupo);
  });
}

function obterValorCampo(nomeCampo) {
  const elemento = document.getElementById(nomeCampo);
  return elemento ? elemento.value.trim() : "";
}

function montarPayloadPorTipo(tipoSelecionado) {
  const config = MODELOS[tipoSelecionado];

  if (!config) {
    throw new Error("Tipo inválido.");
  }

  const payload = {};

  for (const campo of config.campos) {
    const valor = obterValorCampo(campo.name);

    if (campo.required && !valor) {
      throw new Error(`Preencha o campo ${campo.label}.`);
    }

    if (campo.name === "Socket") {
      payload[campo.name] = valorEnumCpuSocket(valor);
    } else if (campo.name === "Tipo" || campo.name === "TipoRamSuportado") {
      payload[campo.name] = valorEnumRamType(valor);
    } else if (campo.name === "Certificacao") {
      payload[campo.name] = valorEnumPsuCertification(valor);
    } else if (campo.type === "number") {
      payload[campo.name] = Number(valor);
    } else {
      payload[campo.name] = valor;
    }
  }

  return payload;
}

function normalizarCPU(item) {
  return {
    id: item.id ?? item.Id,
    nome: item.nome ?? item.Nome,
    tipo: "Processador",
    endpoint: "CPUs",
    detalhes: [
      `Fabricante: ${textoSeguro(item.fabricante ?? item.Fabricante)}`,
      `Socket: ${textoSeguro(item.socket ?? item.Socket)}`,
      `TDP: ${textoSeguro(item.tdp ?? item.TDP)} W`
    ]
  };
}

function normalizarGPU(item) {
  return {
    id: item.id ?? item.Id,
    nome: item.nome ?? item.Nome,
    tipo: "Placa de Vídeo",
    endpoint: "GPUs",
    detalhes: [
      `TDP: ${textoSeguro(item.tdp ?? item.TDP)} W`,
      `Consumo recomendado: ${textoSeguro(item.consumoRecomendado ?? item.ConsumoRecomendado)} W`,
      `Comprimento: ${textoSeguro(item.comprimento ?? item.Comprimento)} mm`
    ]
  };
}

function normalizarMotherboard(item) {
  return {
    id: item.id ?? item.Id,
    nome: item.nome ?? item.Nome,
    tipo: "Placa-mãe",
    endpoint: "Motherboards",
    detalhes: [
      `Socket: ${textoSeguro(item.socket ?? item.Socket)}`,
      `RAM suportada: ${textoSeguro(item.tipoRamSuportado ?? item.TipoRamSuportado)}`,
      `Capacidade máxima RAM: ${textoSeguro(item.capacidadeMaximaRam ?? item.CapacidadeMaximaRam)} GB`,
      `Slots RAM: ${textoSeguro(item.slotsRam ?? item.SlotsRam)}`
    ]
  };
}

function normalizarPSU(item) {
  return {
    id: item.id ?? item.Id,
    nome: item.nome ?? item.Nome,
    tipo: "Fonte",
    endpoint: "PSUs",
    detalhes: [
      `Potência: ${textoSeguro(item.potencia ?? item.Potencia)} W`,
      `Certificação: ${textoSeguro(item.certificacao ?? item.Certificacao)}`
    ]
  };
}

function normalizarRAM(item) {
  return {
    id: item.id ?? item.Id,
    nome: item.nome ?? item.Nome,
    tipo: "Memória RAM",
    endpoint: "RAMs",
    detalhes: [
      `Tipo: ${textoSeguro(item.tipo ?? item.Tipo)}`,
      `Capacidade: ${textoSeguro(item.capacidade ?? item.Capacidade)} GB`,
      `Qtd. módulos: ${textoSeguro(item.quantidadeModulos ?? item.QuantidadeModulos)}`
    ]
  };
}

async function carregarPecas() {
  try {
    const requisicoes = [
      fetch(`${API_BASE}/CPUs`).then(r => r.json()),
      fetch(`${API_BASE}/GPUs`).then(r => r.json()),
      fetch(`${API_BASE}/Motherboards`).then(r => r.json()),
      fetch(`${API_BASE}/PSUs`).then(r => r.json()),
      fetch(`${API_BASE}/RAMs`).then(r => r.json())
    ];

    const [cpus, gpus, motherboards, psus, rams] = await Promise.all(requisicoes);

    pecas = [
      ...cpus.map(normalizarCPU),
      ...gpus.map(normalizarGPU),
      ...motherboards.map(normalizarMotherboard),
      ...psus.map(normalizarPSU),
      ...rams.map(normalizarRAM)
    ];

    renderizarPecas();
  } catch (error) {
    console.error("Erro ao carregar peças:", error);
    listaPecas.innerHTML = `
      <div class="empty-state">
        Erro ao carregar peças da API.
      </div>
    `;
    totalPecas.textContent = "0 peça(s)";
  }
}

function renderizarPecas() {
  const termoBusca = buscaInput.value.toLowerCase().trim();
  const tipoSelecionado = filtroTipo.value;

  const pecasFiltradas = pecas.filter((peca) => {
    const nomeOk = (peca.nome || "").toLowerCase().includes(termoBusca);
    const detalhesOk = peca.detalhes.some((d) => d.toLowerCase().includes(termoBusca));
    const correspondeBusca = nomeOk || detalhesOk;

    const correspondeTipo = tipoSelecionado === "" || peca.tipo === tipoSelecionado;

    return correspondeBusca && correspondeTipo;
  });

  totalPecas.textContent = `${pecasFiltradas.length} peça(s)`;

  if (pecasFiltradas.length === 0) {
    listaPecas.innerHTML = `
      <div class="empty-state">
        Nenhuma peça encontrada.
      </div>
    `;
    return;
  }

  listaPecas.innerHTML = pecasFiltradas
    .map(
      (peca) => `
        <div class="peca-item">
          <div class="peca-info">
            <span class="peca-nome">${peca.nome}</span>
            ${peca.detalhes.map((d) => `<span class="peca-detalhe">${d}</span>`).join("")}
            <span class="badge">${peca.tipo}</span>
          </div>
          <button class="btn-remove" onclick="removerPeca('${peca.endpoint}', ${peca.id})">
            Remover
          </button>
        </div>
      `
    )
    .join("");
}

async function adicionarPeca(event) {
  event.preventDefault();

  const tipoSelecionado = tipoInput.value;
  const config = MODELOS[tipoSelecionado];

  if (!config) {
    alert("Selecione um tipo válido.");
    return;
  }

  try {
    const payload = montarPayloadPorTipo(tipoSelecionado);

    const response = await fetch(`${API_BASE}/${config.endpoint}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(payload)
    });

    if (!response.ok) {
      throw new Error(`Erro ao adicionar em ${config.endpoint}`);
    }

    form.reset();
    camposDinamicos.innerHTML = "";
    await carregarPecas();
  } catch (error) {
    console.error(error);
    alert(error.message || "Erro ao adicionar peça.");
  }
}

async function removerPeca(endpoint, id) {
  try {
    const response = await fetch(`${API_BASE}/${endpoint}/${id}`, {
      method: "DELETE"
    });

    if (!response.ok) {
      throw new Error("Erro ao remover peça.");
    }

    await carregarPecas();
  } catch (error) {
    console.error(error);
    alert("Erro ao remover peça.");
  }
}

tipoInput.addEventListener("change", () => {
  renderizarCamposDinamicos(tipoInput.value);
});

form.addEventListener("submit", adicionarPeca);
buscaInput.addEventListener("input", renderizarPecas);
filtroTipo.addEventListener("change", renderizarPecas);

carregarPecas();