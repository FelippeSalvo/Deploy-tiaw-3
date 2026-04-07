
# 3. Especificações do Projeto

📌 **Pré-requisito:** Planejamento do Projeto (Cronograma e Sprints definidos).

Nesta seção serão detalhados:

- ✅ Requisitos Funcionais  
- ✅ Histórias de Usuário  
- ✅ Requisitos Não Funcionais  
- ✅ Restrições do Projeto  

O objetivo é organizar claramente as funcionalidades, qualidades e limites da solução.

---

# 3.1 Requisitos Funcionais

Os **Requisitos Funcionais (RF)** descrevem o que o sistema deve fazer.

📌 Cada requisito deve:
- Representar uma funcionalidade única
- Ser claro e objetivo
- Orientar diretamente o desenvolvimento

---

## Tabela de Requisitos Funcionais

| ID    | Descrição do Requisito                                                                  | Prioridade |
| ----- | --------------------------------------------------------------------------------------- | ---------- |
| RF-01 | O sistema deve permitir que o usuário realize cadastro informando nome, e-mail e senha. | 🔴 ALTA    |
| RF-02 | O sistema deve permitir que o usuário realize login com e-mail e senha.                 | 🔴 ALTA    |
| RF-03 | O sistema deve exibir a página inicial do sistema ao usuário.                           | 🔴 ALTA    |
| RF-04 | O sistema deve permitir que o usuário acesse a funcionalidade de montagem de PC.        | 🔴 ALTA    |
| RF-05 | O sistema deve permitir que o usuário selecione componentes para montagem de PC.        | 🔴 ALTA    |
| RF-06 | O sistema deve validar a compatibilidade entre os componentes selecionados.             | 🟡 MÉDIA   |
| RF-07 | O sistema deve permitir que o administrador cadastre novas peças no sistema.            | 🟡 MÉDIA   |
| RF-08 | O sistema deve exibir a lista de peças cadastradas para o administrador.                | 🟡 MÉDIA   |





---

# 3.2 Histórias de Usuário

Cada história deve seguir o padrão ensinado na disciplina:

> **Como** [persona],  
> **eu quero** [funcionalidade],  
> **para que** [benefício].

⚠️ **ATENÇÃO:**  
Cada História de Usuário deve estar associada a um Requisito Funcional específico (RF-XX).

---

## Exemplos

**História 1 (relacionada ao RF-01):**  
Como usuário, quero registrar minhas tarefas para não esquecer de fazê-las.

**História 2 (relacionada ao RF-02):**  
Como administrador, quero alterar permissões para controlar o acesso ao sistema.

---

## Histórias do Projeto

---

🔷 História 1 (RF-01) – Cadastro

Como visitante
Eu quero criar uma conta
Para acessar o sistema

🔷 História 2 (RF-02) – Login

Como usuário
Eu quero fazer login
Para acessar minha conta

🔷 História 3 (RF-03) – Home

Como visitante
Eu quero visualizar a página inicial
Para entender o sistema

🔷 História 4 (RF-04) – Acesso à montagem

Como usuário
Eu quero acessar a tela de montagem
Para começar a montar meu PC

🔷 História 5 (RF-05) – Seleção de peças

Como usuário
Eu quero selecionar componentes
Para montar meu computador

🔷 História 6 (RF-06) – Compatibilidade

Como usuário
Eu quero validar a compatibilidade
Para evitar erros na montagem

🔷 História 7 (RF-07) – Admin cadastro

Como administrador
Eu quero cadastrar peças
Para disponibilizar no sistema

🔷 História 8 (RF-08) – Admin lista

Como administrador
Eu quero visualizar as peças cadastradas
Para gerenciar o sistema

---

> 💡 Dica: Agrupe as histórias por módulo (Cadastro, Relatórios, Pagamentos, etc.) para melhor organização.

---

# 3.3 Requisitos Não Funcionais

Os **Requisitos Não Funcionais (RNF)** definem características de qualidade do sistema, como:

- ⚡ Desempenho  
- 🔒 Segurança  
- 🎨 Usabilidade  
- 📈 Escalabilidade  
- 🌐 Compatibilidade  

Eles garantem a qualidade da solução.

---

## Tabela de Requisitos Não Funcionais

| ID     | Descrição do Requisito                                                              | Prioridade |
| ------ | ----------------------------------------------------------------------------------- | ---------- |
| RNF-01 | O sistema deve responder às requisições em até 2 segundos.                              | 🔴 ALTA    |
| RNF-02 | O sistema deve exigir autenticação para acesso às funcionalidades restritas.            | 🔴 ALTA    |
| RNF-03 | O sistema deve permitir que o usuário realize login ou cadastro em no máximo 3 etapas.  | 🔴 ALTA    |
| RNF-04 | O sistema deve possuir separação em arquivos distintos (HTML, CSS e JS)                 | 🟡 MÉDIA   |
| RNF-05 | O sistema deve ser compatível com navegadores modernos (Chrome, Edge, Firefox).         | 🟡 MÉDIA   |
| RNF-06 | O sistema deve manter organização do código em camadas (frontend, backend e banco).     | 🟡 MÉDIA   |



---

# 3.4 Restrições do Projeto

📌 **Restrições** são limitações externas impostas ao projeto.

Elas podem envolver:
- 📅 Prazo
- 🖥️ Tecnologia obrigatória ou proibida
- 🌐 Ambiente de execução
- 📜 Normas legais
- 🏢 Políticas institucionais

⚠️ Diferente dos RNFs, as restrições impõem **limites fixos** ao projeto.

---

## Tabela de Restrições

| ID   | Restrição                                                                           |
| ---- | ----------------------------------------------------------------------------------- |
| R-01 | O projeto deve ser desenvolvido dentro do prazo definido pela disciplina.           |
| R-02 | O sistema deve utilizar as tecnologias definidas (HTML, CSS, JavaScript e backend). |
| R-03 |O banco de dados utilizado deve ser compatível com o ambiente definido pelo projeto.  |
| R-04 | O sistema deve rodar em ambiente local durante o desenvolvimento.                   |
| R-05 | O projeto deve ser versionado utilizando GitHub.                                    |
| R-06 | Não é permitido utilizar frameworks não ensinados na disciplina (se aplicável).     |
| R-07 | O projeto deve seguir os padrões definidos pela disciplina.       |
| R-08 | O projeto deve ser entregue conforme o modelo solicitado pelo professor.            |


---

# ✅ Checklist de Validação

Antes de entregar, confirme:

- [ ] Todos os RFs estão claros e numerados corretamente  
- [ ] Todas as Histórias estão associadas a um RF  
- [ ] RNFs estão mensuráveis  
- [ ] Restrições são realmente limitações externas  
- [ ] O documento está atualizado no GitHub  

---



> **Links Úteis**:
> - [O que são Requisitos Funcionais e Requisitos Não Funcionais?](https://codificar.com.br/requisitos-funcionais-nao-funcionais/)
> - [O que são requisitos funcionais e requisitos não funcionais?](https://analisederequisitos.com.br/requisitos-funcionais-e-requisitos-nao-funcionais-o-que-sao/)
