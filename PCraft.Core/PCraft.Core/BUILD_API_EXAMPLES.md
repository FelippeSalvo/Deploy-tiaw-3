# API de Builds — exemplos JSON

Base: `http://localhost:5265/api`  
Header de autenticação (onde indicado): `Authorization: Bearer <token_jwt>`

---

## POST /api/builds

Cria uma build (usuário autenticado).

**Request**

```json
{
  "nome": "Meu PC Gamer",
  "compartilhada": true,
  "cpuId": 1,
  "motherboardId": 2,
  "ramId": 3,
  "gpuId": 4,
  "psuId": 5
}
```

**Response 201** — corpo igual a GET (nomes das peças resolvidos).

---

## GET /api/builds/me

Lista builds do usuário logado.

**Headers:** `Authorization: Bearer …`

**Response 200**

```json
[
  {
    "id": 10,
    "nome": "Meu PC Gamer",
    "compartilhada": true,
    "criadaEm": "2026-02-10T15:30:00Z",
    "usuario": { "id": 1, "nome": "João" },
    "cpu": "Ryzen 5 5600",
    "motherboard": "B550M",
    "ram": "16GB DDR4",
    "gpu": "RTX 4060",
    "psu": "650W 80 Plus Bronze"
  }
]
```

---

## GET /api/builds/publicas

Lista builds compartilhadas (público, sem token).

---

## GET /api/builds/{id}

Detalhe de uma build.

- Build **compartilhada**: qualquer um.
- Build **privada**: apenas o dono (com JWT); outros recebem 404.

---

## PUT /api/builds/{id}

Atualiza build (somente dono).

**Request**

```json
{
  "nome": "Meu PC Atualizado",
  "compartilhada": false,
  "cpuId": 1,
  "motherboardId": 2,
  "ramId": null,
  "gpuId": 4,
  "psuId": 5
}
```

**Nota:** pelo menos um entre `cpuId`, `motherboardId`, `ramId`, `gpuId`, `psuId` deve permanecer preenchido.

---

## DELETE /api/builds/{id}

Remove a build (somente dono). **Resposta 204** sem corpo.

---

## Banco de dados

Após puxar o código, aplique a migration:

```bash
dotnet ef database update --project PCraft.Core/PCraft.Core/PCraft.Core.csproj
```

Migration: `20260210120000_SistemaBuildsSalvas` (tabela `BuildsSalvas`).
