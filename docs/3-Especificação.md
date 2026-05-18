
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
| RF-03 | O sistema deve permitir que o usuário realize logout da plataforma.                     | 🔴 ALTA    |
| RF-04 | O sistema deve exibir a página inicial do sistema ao usuário.                           | 🔴 ALTA    |
| RF-05 | O sistema deve permitir que o usuário acesse a funcionalidade de montagem de PC.        | 🔴 ALTA    |
| RF-06 | O sistema deve permitir que o usuário selecione componentes para montagem de PC.        | 🔴 ALTA    |
| RF-07 | O sistema deve validar a compatibilidade entre os componentes selecionados.             | 🔴 ALTA    |
| RF-08 | O sistema deve permitir que o usuário salve builds personalizadas.                      | 🔴 ALTA    |
| RF-09 | O sistema deve permitir que o administrador cadastre novas peças no sistema.            | 🔴 ALTA    |
| RF-10 | O sistema deve permitir a persistência de dados utilizando PostgreSQL/Supabase.         | 🔴 ALTA    |
| RF-11 | O sistema deve permitir que o usuário visualize suas builds salvas.                     | 🟡 MÉDIA   |
| RF-12 | O sistema deve permitir que o usuário edite builds salvas.                              | 🟡 MÉDIA   |
| RF-13 | O sistema deve permitir que o usuário exclua builds salvas.                             | 🟡 MÉDIA   |
| RF-14 | O sistema deve permitir que o usuário compartilhe builds publicamente.                  | 🟡 MÉDIA   |
| RF-15 | O sistema deve permitir que o administrador visualize as peças cadastradas.             | 🟡 MÉDIA   |
| RF-16 | O sistema deve permitir que o administrador edite peças cadastradas.                    | 🟡 MÉDIA   |
| RF-17 | O sistema deve permitir que o administrador exclua peças cadastradas.                   | 🟡 MÉDIA   |
| RF-18 | O sistema deve disponibilizar integração entre frontend e backend por meio de API REST. | 🟡 MÉDIA   |







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

🔷 História 3 (RF-03) – Logout

Como usuário
Eu quero realizar logout
Para encerrar minha sessão com segurança

🔷 História 4 (RF-04) – Home

Como visitante
Eu quero visualizar a página inicial
Para entender o sistema

🔷 História 5 (RF-05) – Acesso à montagem

Como usuário
Eu quero acessar a tela de montagem
Para começar a montar meu PC

🔷 História 6 (RF-06) – Seleção de peças

Como usuário
Eu quero selecionar componentes
Para montar meu computador

🔷 História 7 (RF-07) – Compatibilidade

Como usuário
Eu quero validar a compatibilidade
Para evitar erros na montagem

🔷 História 8 (RF-08) – Salvar builds

Como usuário
Eu quero salvar minhas builds
Para acessá-las futuramente

🔷 História 9 (RF-09) – Admin cadastro

Como administrador
Eu quero cadastrar peças
Para disponibilizar no sistema

🔷 História 10 (RF-10) – Banco de dados

Como sistema
Eu quero persistir os dados no PostgreSQL/Supabase
Para garantir armazenamento das informações

🔷 História 11 (RF-11) – Visualizar builds

Como usuário
Eu quero visualizar minhas builds salvas
Para gerenciar minhas montagens

🔷 História 12 (RF-12) – Editar builds

Como usuário
Eu quero editar builds salvas
Para atualizar configurações

🔷 História 13 (RF-13) – Excluir builds

Como usuário
Eu quero excluir builds
Para remover montagens antigas

🔷 História 14 (RF-14) – Compartilhar builds

Como usuário
Eu quero compartilhar builds
Para mostrar minha configuração para outras pessoas

🔷 História 15 (RF-15) – Admin visualização

Como administrador
Eu quero visualizar as peças cadastradas
Para gerenciar o sistema

🔷 História 16 (RF-16) – Admin edição

Como administrador
Eu quero editar peças cadastradas
Para corrigir informações

🔷 História 17 (RF-17) – Admin exclusão

Como administrador
Eu quero excluir peças cadastradas
Para remover itens inválidos

🔷 História 18 (RF-18) – Integração API

Como sistema
Eu quero integrar frontend e backend por API REST
Para permitir comunicação entre as camadas

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

| ID     | Descrição do Requisito                                                                 | Prioridade |
| ------ | -------------------------------------------------------------------------------------- | ---------- |
| RNF-01 | O sistema deve responder às requisições em até 2 segundos.                             | 🔴 ALTA    |
| RNF-02 | O sistema deve exigir autenticação para acesso às funcionalidades restritas.           | 🔴 ALTA    |
| RNF-03 | O sistema deve permitir que o usuário realize login ou cadastro em no máximo 3 etapas. | 🔴 ALTA    |
| RNF-04 | O sistema deve utilizar PostgreSQL/Supabase para persistência de dados.                | 🔴 ALTA    |
| RNF-05 | O sistema deve garantir comunicação segura entre frontend e backend.                   | 🔴 ALTA    |
| RNF-06 | O sistema deve possuir separação em arquivos distintos (HTML, CSS e JS).               | 🟡 MÉDIA   |
| RNF-07 | O sistema deve ser compatível com navegadores modernos (Chrome, Edge e Firefox).       | 🟡 MÉDIA   |
| RNF-08 | O sistema deve manter organização do código em camadas (frontend, backend e banco).    | 🟡 MÉDIA   |
| RNF-09 | O sistema deve utilizar ASP.NET Core no backend.                                       | 🟡 MÉDIA   |
| RNF-10 | O sistema deve possuir interface responsiva para diferentes tamanhos de tela.          | 🟡 MÉDIA   |
| RNF-11 | O sistema deve possuir validações de compatibilidade no backend.                       | 🟡 MÉDIA   |
| RNF-12 | O sistema deve permitir escalabilidade para inclusão de novos componentes futuramente. | 🟡 MÉDIA   |




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
