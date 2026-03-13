const formCadastro = document.getElementById("formCadastro");
if (formCadastro) {
    formCadastro.addEventListener("submit", async (e) => {
        e.preventDefault();

        const nome = document.getElementById("nome").value;
        const email = document.getElementById("email").value;
        const senha = document.getElementById("senha").value;
        const confirmarSenha = document.getElementById("confirmarSenha").value;

        if (senha !== confirmarSenha) {
            alert("As senhas não coincidem!");
            return;
        }

        const usuario = { nome, email, senha };

        try {
            const response = await fetch("http://localhost:5000/api/usuarios", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(usuario)
            });

            if (response.ok) {
                alert("Usuário cadastrado com sucesso!");
                window.location.href = "login.html";
            } else {
                const error = await response.text();
                alert("Erro: " + error);
            }
        } catch (err) {
            console.error(err);
            alert("Erro ao conectar com o servidor.");
        }
    });
}

const formLogin = document.getElementById("formLogin");
if (formLogin) {
    formLogin.addEventListener("submit", async (e) => {
        e.preventDefault();

        const email = document.getElementById("loginEmail").value.trim();
        const senha = document.getElementById("loginSenha").value;

        if (!email || !senha) {
            alert("Preencha todos os campos!");
            return;
        }

        try {
            const response = await fetch("http://localhost:5000/api/usuarios/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ email, senha }) 
            });

            if (response.ok) {
                const usuario = await response.json(); 
                alert("Login realizado com sucesso!");

            } else if (response.status === 401) {
                alert("Email ou senha inválidos.");
            } else {
                const error = await response.text();
                alert("Erro: " + error);
            }

        } catch (err) {
            console.error(err);
            alert("Erro ao conectar com o servidor.");
        }
    });
}
