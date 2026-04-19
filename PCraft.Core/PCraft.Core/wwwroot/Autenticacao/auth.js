const API_BASE = 'http://localhost:5265/api';

async function fazerLogin(email, senha) {
    try {
        const res = await fetch(`${API_BASE}/usuarios/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, senha })
        });

        if (!res.ok) {
            if (res.status === 401) {
                alert('Email ou senha inválidos');
            } else {
                alert('Erro no servidor. Tente novamente.');
            }
            return;
        }

        const data = await res.json();
        localStorage.setItem('usuario', JSON.stringify(data));
        alert('Login realizado com sucesso!');
        window.location.href = '../Montagem/index.html';
    } catch (e) {
        console.error('Erro ao fazer login:', e);
        alert('Erro de conexão. Verifique se o servidor está no ar.');
    }
}

async function fazerCadastro(nome, email, senha) {
    try {
        const res = await fetch(`${API_BASE}/usuarios`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nome, email, senha })
        });

        if (!res.ok) {
            alert('Erro ao criar conta. Verifique os dados.');
            return;
        }

        alert('Conta criada com sucesso! Faça login.');
        window.location.href = 'Login.html';
    } catch (e) {
        console.error('Erro ao cadastrar:', e);
        alert('Erro de conexão. Verifique se o servidor está no ar.');
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const formLogin = document.getElementById('formLogin');
    if (formLogin) {
        formLogin.addEventListener('submit', async (e) => {
            e.preventDefault();
            const email = document.getElementById('loginEmail').value;
            const senha = document.getElementById('loginSenha').value;
            await fazerLogin(email, senha);
        });
    }

    const formCadastro = document.getElementById('formCadastro');
    if (formCadastro) {
        formCadastro.addEventListener('submit', async (e) => {
            e.preventDefault();
            const nome = document.getElementById('cadastroNome').value;
            const email = document.getElementById('cadastroEmail').value;
            const senha = document.getElementById('cadastroSenha').value;
            const confirmarSenha = document.getElementById('cadastroConfirmarSenha').value;

            if (senha !== confirmarSenha) {
                alert('As senhas não coincidem!');
                return;
            }

            await fazerCadastro(nome, email, senha);
        });
    }
});
