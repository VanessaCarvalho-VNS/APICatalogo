# 📦 API Catálogo — Web API REST com ASP.NET Core

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=flat&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-7F5AF0?style=flat&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=flat&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-Docs-85EA2D?style=flat&logo=swagger&logoColor=black)

Web API REST desenvolvida com **ASP.NET Core**, aplicando os principais conceitos de criação, segurança e documentação de APIs modernas na plataforma .NET.
API REST para gerenciamento de produtos e categorias, desenvolvida com ASP.NET Core seguindo boas práticas de arquitetura em camadas.

---

## 📋 Funcionalidades

- 🔐 Autenticação e autorização com **JWT** (Login e Registro)
- 📄 Paginação de dados
- ⚡ Programação assíncrona
- 🗂️ Documentação com **Swagger / OpenAPI**
- 🛡️ Rate Limiting e CORS configurados
- 💾 Cache com MemoryCache
- 🔍 Suporte a **GraphQL**
- 📝 Logging e tratamento de erros global
- 🔢 Versionamento de API

---

## 🛠️ Tecnologias

- ASP.NET Core Web API
- Entity Framework Core (Code-First + Migrations)
- SQL Server
- JWT (JSON Web Token)
- AutoMapper
- Swagger / OpenAPI
- GraphQL
- Minimal APIs

---

## 📐 Padrões Utilizados

- Padrão **Repository**
- Padrão **Unit of Work**
- **DTOs** com Data Annotations
- **Filtros** e Middlewares customizados

---

## 🚀 Como Executar

### Pré-requisitos
- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Passos

```bash
# Clone o repositório
git clone https://github.com/VanessaCarvalho-VNS/APICatalogo.git

# Acesse a pasta do projeto
cd APICatalogo
```

No `appsettings.json`, configure a string de conexão e o JWT:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=CatalogoDB;Trusted_Connection=True;"
},
"JWT": {
  "SecretKey": "SUA_SECRET_KEY",
  "ValidIssuer": "seu-issuer",
  "ValidAudience": "sua-audience"
}
```

```bash
# Aplique as migrations
dotnet ef database update

# Execute o projeto
dotnet run
```

Acesse a documentação Swagger: `https://localhost:7000/swagger`

---

## 🔗 Endpoints principais

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/auth/login` | Autenticação do usuário |
| POST | `/api/auth/register` | Registro de novo usuário |
| GET | `/api/produtos` | Lista produtos (paginado) |
| GET | `/api/categorias` | Lista categorias |

---

## 📸 Demonstração

> Em breve

---

## 👩‍💻 Autora

**Vanessa Erika de Carvalho**  
[![LinkedIn](https://img.shields.io/badge/LinkedIn-0A66C2?style=flat&logo=linkedin&logoColor=white)](https://linkedin.com/in/vanessacarvalho20)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=flat&logo=github&logoColor=white)](https://github.com/VanessaCarvalho-VNS)
