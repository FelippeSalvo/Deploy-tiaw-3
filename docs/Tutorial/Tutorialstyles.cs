/* RESET */
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
    font-family: Arial, Helvetica, sans-serif;
}

/* VARIÁVEIS */
:root {
    --bg-primary: #0B0F19;
    --bg-secondary: #0F141F;
    --bg-card: #111827;
    --bg-soft: #1F2937;
    --text-primary: #ffffff;
    --text-secondary: #9CA3AF;
    --accent: #22C55E;
    --accent-hover: #16A34A;
    --border: #374151;
    --shadow: 0 20px 45px rgba(0, 0, 0, 0.35);
}

/* BODY */
body {
    background: radial-gradient(circle at top right, rgba(34, 197, 94, 0.08), transparent 25%), var(--bg-primary);
    color: var(--text-primary);
    min-height: 100vh;
}

/* CONTAINER */
.container {
    max-width: 1100px;
    margin: 0 auto;
    padding: 0 20px;
}

/* NAVBAR */
.navbar {
    position: sticky;
    top: 0;
    z-index: 1000;
    background: rgba(15, 20, 31, 0.85);
    backdrop-filter: blur(10px);
    border-bottom: 1px solid var(--border);
    padding: 16px 0;
}

.nav-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.logo {
    color: var(--accent);
    font-weight: 800;
    font-size: 1.5rem;
    text-decoration: none;
}

/* HEADER */
.tutorial-header {
    text-align: center;
    padding: 60px 0 30px;
}

    .tutorial-header h1 {
        font-size: 2.5rem;
        margin-bottom: 10px;
    }

    .tutorial-header p {
        color: var(--text-secondary);
    }

/* PASSOS */
.tutorial-step {
    display: flex;
    gap: 30px;
    margin-bottom: 60px;
    align-items: center;
    flex-wrap: wrap;
    opacity: 0;
    transform: translateY(40px);
    transition: 0.6s ease;
}

    .tutorial-step.show {
        opacity: 1;
        transform: translateY(0);
    }

.active-step {
    border: 1px solid rgba(34, 197, 94, 0.4);
    box-shadow: 0 0 25px rgba(34, 197, 94, 0.2);
    border-radius: 20px;
    padding: 20px;
}

/* IMAGEM */
.step-image img {
    width: 100%;
    max-width: 400px;
    border-radius: 20px;
    box-shadow: var(--shadow);
    transition: 0.4s ease;
}

    /* HOVER IMAGEM */
    .step-image img:hover {
        transform: scale(1.03);
    }

/* TEXTO */
.step-text {
    flex: 1;
    min-width: 280px;
}

    .step-text h2 {
        font-size: 1.6rem;
        margin-bottom: 10px;
        color: var(--accent);
    }

    .step-text p {
        color: var(--text-secondary);
        line-height: 1.7;
        margin-bottom: 10px;
    }

/* INFO BOX */
.info-box {
    background: rgba(31, 41, 55, 0.7);
    border: 1px solid var(--border);
    padding: 15px;
    border-radius: 12px;
    margin-top: 10px;
}

    .info-box strong {
        color: var(--accent);
    }

/* BOTÃO PADRÃO */
.btn {
    display: inline-block
    padding: 12px 20px;
    background: var(--accent);
    color: #07120b;
    text-decoration: none;
    border-radius: 10px;
    font-weight: 700;
    transition: 0.3s;
}

    .btn:hover {
        background: var(--accent-hover);
        transform: translateY(-2px);
    }

.scroll-top-btn {
    position: fixed;
    bottom: 30px;
    right: 30px;
    background: #22C55E;
    color: #07120b;
    border: none;
    padding: 12px 16px;
    border-radius: 10px;
    font-size: 18px;
    cursor: pointer;
    opacity: 0;
    pointer-events: none;
    transition: 0.3s;
    z-index: 999;
}

    .scroll-top-btn:hover {
        background: #16A34A;
    }

/* FOOTER */
.footer {
    border-top: 1px solid var(--border);
    padding: 30px;
    text-align: center;
    color: var(--text-secondary);
    margin-top: 60px;
}

/* RESPONSIVO */
@media (max-width: 900px) {
    .tutorial-step {
        flex-direction: column;
        text-align: center;
    }

    .step-text h2 {
        font-size: 1.4rem;
    }

    .tutorial-header h1 {
        font-size: 2rem;
    }
}
