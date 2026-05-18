const API_BASE = "https://pcraft-expo.onrender.com/api";

const formForgotPassword = document.getElementById("formForgotPassword");
const formResetPassword = document.getElementById("formResetPassword");
const authMessage = document.getElementById("authMessage");

function mostrarMensagem(mensagem, tipo) {
    authMessage.textContent = mensagem || "";
    authMessage.className = `auth-message ${tipo || ""}`.trim();
}

function validarEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

function obterTokenDaUrl() {
    const params = new URLSearchParams(window.location.search);
    return params.get("token");
}

function configurarModoDaPagina() {
    const token = obterTokenDaUrl();

    if (token) {
        formForgotPassword.classList.add("hidden");
        formResetPassword.classList.remove("hidden");
        mostrarMensagem("Digite sua nova senha para finalizar a recuperação.", "");
    } else {
        formForgotPassword.classList.remove("hidden");
        formResetPassword.classList.add("hidden");
    }
}

formForgotPassword.addEventListener("submit", async (event) => {
    event.preventDefault();
    mostrarMensagem("", "");

    const email = document.getElementById("emailRecuperacao").value.trim().toLowerCase();

    if (!validarEmail(email)) {
        mostrarMensagem("Informe um email válido.", "erro");
        return;
    }

    const botao = formForgotPassword.querySelector("button");
    botao.disabled = true;
    botao.textContent = "Enviando...";

    try {
        const resposta = await fetch(`${API_BASE}/auth/forgot-password`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ email })
        });

        const data = await resposta.json().catch(() => ({}));

        if (!resposta.ok) {
            mostrarMensagem(data.message || "Erro ao solicitar recuperação de senha.", "erro");
            return;
        }

        mostrarMensagem(
            data.message || "Se o e-mail existir, enviaremos um link de recuperação.",
            "sucesso"
        );
    } catch (error) {
        console.error("Erro ao solicitar recuperação:", error);
        mostrarMensagem("Não foi possível conectar com a API.", "erro");
    } finally {
        botao.disabled = false;
        botao.textContent = "Enviar link de recuperação";
    }
});

formResetPassword.addEventListener("submit", async (event) => {
    event.preventDefault();
    mostrarMensagem("", "");

    const token = obterTokenDaUrl();
    const newPassword = document.getElementById("novaSenha").value;
    const confirmarNovaSenha = document.getElementById("confirmarNovaSenha").value;

    if (!token) {
        mostrarMensagem("Token de recuperação não encontrado.", "erro");
        return;
    }

    if (newPassword.length < 6) {
        mostrarMensagem("A nova senha deve ter pelo menos 6 caracteres.", "erro");
        return;
    }

    if (newPassword !== confirmarNovaSenha) {
        mostrarMensagem("As senhas não coincidem.", "erro");
        return;
    }

    const botao = formResetPassword.querySelector("button");
    botao.disabled = true;
    botao.textContent = "Redefinindo...";

    try {
        const resposta = await fetch(`${API_BASE}/auth/reset-password`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                token,
                newPassword
            })
        });

        const data = await resposta.json().catch(() => ({}));

        if (!resposta.ok) {
            mostrarMensagem(data.message || "Token inválido ou expirado.", "erro");
            return;
        }

        mostrarMensagem(data.message || "Senha redefinida com sucesso.", "sucesso");

        setTimeout(() => {
           window.location.href = "../Autenticacao/Login.html";
        }, 1200);
    } catch (error) {
        console.error("Erro ao redefinir senha:", error);
        mostrarMensagem("Não foi possível conectar com a API.", "erro");
    } finally {
        botao.disabled = false;
        botao.textContent = "Redefinir senha";
    }
});

document.addEventListener("DOMContentLoaded", configurarModoDaPagina);