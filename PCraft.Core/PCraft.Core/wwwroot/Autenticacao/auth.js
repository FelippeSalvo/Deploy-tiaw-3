const AUTH_STORAGE_KEY = 'pcraft.auth';
const API_BASE = "http://localhost:5265/api";

function obterSessao() {
    const raw = localStorage.getItem(AUTH_STORAGE_KEY);
    if (!raw) return null;

    try {
        return JSON.parse(raw);
    } catch (error) {
        console.error('Sessao invalida:', error);
        localStorage.removeItem(AUTH_STORAGE_KEY);
        return null;
    }
}

function salvarSessao(token, usuario, expiresIn) {
    const expiresAt = Date.now() + (expiresIn * 1000);
    localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify({ token, usuario, expiresAt }));
}

function limparSessao() {
    localStorage.removeItem(AUTH_STORAGE_KEY);
}

function sessaoValida(sessao) {
    return !!sessao && !!sessao.token && !!sessao.usuario && Number(sessao.expiresAt) > Date.now();
}

function mostrarMensagem(mensagem, tipo) {
    const box = document.getElementById('authMessage');
    if (!box) return;

    box.className = `auth-message ${tipo || ''}`.trim();
    box.textContent = mensagem || '';
}

function validarEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

function mensagemFalhaConexao(erro, contexto) {
    console.error(contexto || 'Erro de rede:', erro);
    const abrindoPorArquivo = window.location.protocol === 'file:';
    if (abrindoPorArquivo) {
        return 'Não dá para cadastrar abrindo o HTML pelo disco. Inicie o backend (dotnet run) e acesse pelo navegador: ' +
            `${API_FALLBACK_ORIGIN}/Autenticacao/Cadastro.html`;
    }
    return 'Sem conexão com a API (' + API_BASE + '). Coloque o backend no ar nesta porta ou defina manualmente localStorage \'pcraft.apiBase\' (ex.: http://localhost:5627 sem /api no final).';
}

function escapeHtml(texto) {
    const div = document.createElement('div');
    div.textContent = texto ?? '';
    return div.innerHTML;
}

/** pathname normalizado para comparação (minúsculas + decode espacos %20 na URL). */
function pathParaMatch() {
    let path = (window.location.pathname || '').replace(/\\/g, '/');
    try {
        path = decodeURIComponent(path);
    } catch (_) { /* noop */ }
    return path.toLowerCase();
}

/**
 * Pasta atual (URLs em minúsculas no servidor, ex.: /montagem/index.html).
 * `data-pcraft-page` no body confirma quando o path é ambíguo.
 */
function detectarPagina() {
    const attr = (typeof document.body !== 'undefined'
        ? (document.body.getAttribute('data-pcraft-page') || '')
        : '').toLowerCase();
    if (['home', 'montagem', 'auth', 'adm'].includes(attr)) {
        return attr;
    }

    const p = pathParaMatch();
    if (p.includes('/autenticacao/')) return 'auth';
    if (p.includes('/montagem')) return 'montagem';
    if (p.includes('/adm')) return 'adm';
    return 'home';
}

/** Caminhos relativos para links da barra, conforme a pasta atual. */
function obterRotas() {
    const pagina = detectarPagina();

    if (pagina === 'auth') {
        return {
            home: '../Home page/index.html',
            montagem: '../Montagem/index.html',
            login: 'Login.html',
            cadastro: 'Cadastro.html'
        };
    }
    if (pagina === 'montagem') {
        return {
            home: '../Home page/index.html',
            montagem: 'index.html',
            login: '../Autenticacao/Login.html',
            cadastro: '../Autenticacao/Cadastro.html'
        };
    }
    if (pagina === 'adm') {
        return {
            home: '../Home page/index.html',
            montagem: '../Montagem/index.html',
            login: '../Autenticacao/Login.html',
            cadastro: '../Autenticacao/Cadastro.html'
        };
    }
    return {
        home: 'index.html',
        montagem: '../Montagem/index.html',
        login: '../Autenticacao/Login.html',
        cadastro: '../Autenticacao/Cadastro.html'
    };
}

function htmlBarraVisitante(rotas) {
    return `
        <a href="${rotas.login}" class="btn-login">Login</a>
        <a href="${rotas.cadastro}" class="btn-cadastro">Cadastro</a>
    `;
}

function nomeExibicaoUsuario(sessao) {
    const u = sessao?.usuario;
    if (!u) return 'Usuário';
    const n = u.nome ?? u.Nome;
    return typeof n === 'string' && n.trim() ? n.trim() : 'Usuário';
}

/** Garante camelCase no objeto salvo (resposta API pode variar). */
function normalizarUsuarioResposta(u) {
    if (!u) return null;
    return {
        id: u.id ?? u.Id,
        nome: (u.nome ?? u.Nome ?? '').trim() || 'Usuário',
        email: u.email ?? u.Email ?? ''
    };
}

function htmlBarraLogado(sessao) {
    const nome = escapeHtml(nomeExibicaoUsuario(sessao));
    return `
        <div class="nav-auth" role="group" aria-label="Conta atual">
            <span class="auth-user-name">${nome}</span>
            <button type="button" class="btn-logout" id="btnLogout">Sair</button>
        </div>
    `;
}

function textoFooterSessao(autenticado, sessao) {
    if (autenticado) {
        return nomeExibicaoUsuario(sessao);
    }
    return 'Faça login na barra superior para usar sua conta em todo o site.';
}

/** Atualiza todas as barras `.nav-actions` e textos `.footer-auth-status`. */
function atualizarInterfaceAuth() {
    const sessao = obterSessao();
    let autenticado = sessaoValida(sessao);
    if (!autenticado && sessao) {
        limparSessao();
        autenticado = false;
    }

    const rotas = obterRotas();
    const htmlNav = autenticado ? htmlBarraLogado(sessao) : htmlBarraVisitante(rotas);

    document.querySelectorAll('.nav-actions').forEach((el) => {
        el.innerHTML = htmlNav;
    });

    const textoRodape = textoFooterSessao(autenticado, sessao);
    document.querySelectorAll('.footer-auth-status').forEach((el) => {
        el.textContent = textoRodape;
    });

    document.querySelectorAll('[data-pcraft-guest-only]').forEach((el) => {
        el.hidden = autenticado;
    });
    document.querySelectorAll('[data-pcraft-user-only]').forEach((el) => {
        el.hidden = !autenticado;
    });
}

function configurarLogout() {
    document.addEventListener('click', (event) => {
        const alvo = event.target;
        if (!(alvo instanceof HTMLElement) || alvo.id !== 'btnLogout') return;

        limparSessao();

        const emAuth = detectarPagina() === 'auth';
        const rotas = obterRotas();

        if (!emAuth) {
            window.location.href = rotas.login;
            return;
        }

        atualizarInterfaceAuth();
    });
}

async function fazerLogin(email, senha) {
    const resposta = await fetch(`${API_BASE}/usuarios/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, senha })
    });

    const data = await resposta.json().catch(() => ({}));
    return { resposta, data };
}

async function fazerCadastro(nome, email, senha) {
    const resposta = await fetch(`${API_BASE}/usuarios`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ nome, email, senha })
    });

    const data = await resposta.json().catch(() => ({}));
    return { resposta, data };
}

function configurarLogin() {
    const formLogin = document.getElementById('formLogin');
    if (!formLogin) return;

    const sessao = obterSessao();
    if (sessaoValida(sessao)) {
        window.location.href = '../Montagem/index.html';
        return;
    }

    formLogin.addEventListener('submit', async (e) => {
        e.preventDefault();
        mostrarMensagem('', '');

        const email = document.getElementById('loginEmail').value.trim().toLowerCase();
        const senha = document.getElementById('loginSenha').value;

        if (!validarEmail(email)) {
            mostrarMensagem('Informe um email válido.', 'erro');
            return;
        }

        if (!senha) {
            mostrarMensagem('Informe a senha.', 'erro');
            return;
        }

        try {
            const { resposta, data } = await fazerLogin(email, senha);
            if (!resposta.ok) {
                mostrarMensagem(data.message || 'Email ou senha inválidos.', 'erro');
                return;
            }

            const usuarioNorm = normalizarUsuarioResposta(data.usuario);
            if (!data.token || !usuarioNorm) {
                mostrarMensagem('Resposta incompleta do servidor. Tente novamente.', 'erro');
                return;
            }

            salvarSessao(data.token, usuarioNorm, data.expiresIn || 3600);
            mostrarMensagem('Login realizado com sucesso.', 'sucesso');
            window.location.href = '../Montagem/index.html';
        } catch (error) {
            mostrarMensagem(mensagemFalhaConexao(error, 'Erro ao fazer login'), 'erro');
        }
    });
}

function configurarCadastro() {
    const formCadastro = document.getElementById('formCadastro');
    if (!formCadastro) return;

    formCadastro.addEventListener('submit', async (e) => {
        e.preventDefault();
        mostrarMensagem('', '');

        const nome = document.getElementById('cadastroNome').value.trim();
        const email = document.getElementById('cadastroEmail').value.trim().toLowerCase();
        const senha = document.getElementById('cadastroSenha').value;
        const confirmarSenha = document.getElementById('cadastroConfirmarSenha').value;

        if (nome.length < 3) {
            mostrarMensagem('Informe um nome válido.', 'erro');
            return;
        }

        if (!validarEmail(email)) {
            mostrarMensagem('Informe um email válido.', 'erro');
            return;
        }

        if (senha.length < 6) {
            mostrarMensagem('A senha deve ter pelo menos 6 caracteres.', 'erro');
            return;
        }

        if (senha !== confirmarSenha) {
            mostrarMensagem('As senhas não coincidem.', 'erro');
            return;
        }

        try {
            const { resposta, data } = await fazerCadastro(nome, email, senha);
            if (!resposta.ok) {
                mostrarMensagem(data.message || 'Erro ao criar conta.', 'erro');
                return;
            }

            mostrarMensagem('Cadastro realizado! Faça login.', 'sucesso');
            setTimeout(() => {
                window.location.href = 'Login.html';
            }, 800);
        } catch (error) {
            mostrarMensagem(mensagemFalhaConexao(error, 'Erro ao cadastrar'), 'erro');
        }
    });
}

document.addEventListener('DOMContentLoaded', () => {
    atualizarInterfaceAuth();
    configurarLogout();
    configurarLogin();
    configurarCadastro();
});

window.addEventListener('storage', (e) => {
    if (e.key === AUTH_STORAGE_KEY) {
        atualizarInterfaceAuth();
    }
});
