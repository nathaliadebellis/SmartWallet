# 💰 SmartWallet

<div align="center">

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-68217A?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![CI](https://img.shields.io/github/actions/workflow/status/nathaliadebellis/SmartWallet/ci.yml?branch=master&style=for-the-badge&label=CI)
![License](https://img.shields.io/github/license/nathaliadebellis/SmartWallet?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Em_Desenvolvimento-orange?style=for-the-badge)

</div>

> Organize suas finanças com simplicidade e inteligência.

O **SmartWallet** é um sistema web de gerenciamento financeiro pessoal desenvolvido com **ASP.NET Core MVC** e **Entity Framework Core**, utilizando arquitetura em camadas e boas práticas de desenvolvimento.

Além de auxiliar no controle de receitas e despesas, o projeto foi criado como portfólio para demonstrar conhecimentos em desenvolvimento backend com o ecossistema .NET, aplicando conceitos utilizados em aplicações corporativas.

---

# 🎯 Objetivo

O SmartWallet tem como objetivo oferecer uma plataforma simples, intuitiva e organizada para o gerenciamento financeiro pessoal.

O projeto evolui de forma incremental, seguindo uma abordagem semelhante ao desenvolvimento de software em ambiente corporativo, com implementação contínua de novas funcionalidades, foco em arquitetura, escalabilidade e qualidade de código.

---

# ✨ Principais Recursos

## 🌐 Landing Page

- ✅ Hero
- ✅ Dashboard Preview
- ✅ Seção de Recursos
- ✅ Como Funciona
- ✅ Benefícios
- ✅ Tecnologias
- ✅ Call To Action
- ✅ Footer Responsivo

---

## 📂 Categorias

- ✅ Cadastro
- ✅ Listagem
- ✅ Edição
- ✅ Exclusão
- ✅ Validação de categorias duplicadas
- ✅ Definição de ícone
- ✅ Definição de cor
- ✅ Classificação por tipo (Receita ou Despesa)

---

## 💸 Transações Financeiras

- ✅ Cadastro, listagem, edição e exclusão
- ✅ Associação com categorias
- ✅ Validação de dados
- ✅ Observações opcionais
- ✅ Carregamento dinâmico das categorias conforme o tipo da transação
- ✅ Pesquisa por texto
- ✅ Filtros por tipo, categoria e período
- ✅ Paginação

---

## 🔐 Autenticação e Segurança

- ✅ Cadastro, login e logout com ASP.NET Core Identity
- ✅ Recuperação de senha por e-mail
- ✅ Política de senha e bloqueio após tentativas inválidas
- ✅ Isolamento de dados por usuário
- ✅ Categorias padrão criadas no cadastro
- ✅ Autorização e antiforgery aplicados globalmente
- ✅ Headers de segurança e HSTS

---

## 📊 Dashboard

- ✅ Saldo atual, total de receitas e total de despesas
- ✅ Últimas transações

---

## 🚧 Em desenvolvimento

- Gráficos no dashboard
- Metas financeiras
- Relatórios financeiros
- Perfil do usuário
- Exportação de dados
- Deploy

---

# 🏗️ Arquitetura

O SmartWallet foi desenvolvido utilizando uma arquitetura em camadas, promovendo separação de responsabilidades, baixo acoplamento, reutilização de código e facilidade de manutenção.

```text
SmartWallet


├── SmartWallet.Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Mappings
│   └── Services
│   
├── SmartWallet.Domain
│   ├── Common
│   ├── Entities
│   ├── Enums
│   ├── Exceptions
│   ├── Filters
│   └── Interfaces
│ 
├── SmartWallet.Infrastructure
│   ├── Configurations
│   ├── Data
│   ├── Identity
│   ├── Migrations
│   ├── Repositories
│   └── Services
│
├── SmartWallet.Web
│   ├── Binders
│   ├── Controllers
│   ├── Extensions
│   ├── Localization
│   ├── Middleware
│   ├── ViewModels
│   ├── Views
│   └── wwwroot
│
├── SmartWallet.UnitTests
└── SmartWallet.IntegrationTests
```

---

# 🔄 Fluxo da Aplicação

```text
View
   │
   ▼
Controller
   │
   ▼
Application Service
   │
   ▼
Repository
   │
   ▼
Entity Framework Core
   │
   ▼
SQL Server
```

---

# 🛠️ Tecnologias Utilizadas

## Backend

- C#
- .NET 10
- ASP.NET Core MVC
- Entity Framework Core

## Banco de Dados

- SQL Server
- SQL Server LocalDB

## Front-end

- Razor Views
- Bootstrap 5
- Bootstrap Icons
- HTML5
- CSS3
- JavaScript (ES6)
- Fetch API

## Segurança e Autenticação

- ASP.NET Core Identity
- Authentication Cookies
- Filtro global de autorização
- Validação global de antiforgery token

---

# 📐 Boas Práticas Aplicadas

- Arquitetura em camadas
- Repository Pattern
- Unit of Work
- Service Layer
- Dependency Injection
- DTO Pattern
- ViewModels
- Fluent API
- Entity Configurations
- Entity Framework Migrations
- Async/Await
- Separação de responsabilidades
- Clean Code
- Middleware global para tratamento de exceções
- Domain Exceptions
- Testes unitários com xUnit
- Mocking com Moq
- Assertions fluentes com FluentAssertions

---

# ⚙️ Como Executar o Projeto

## Pré-requisitos

- .NET SDK 10
- SQL Server LocalDB
- Visual Studio 2022

---

## Clonar o repositório

```bash
git clone https://github.com/nathaliadebellis/SmartWallet.git
```

---

## Banco de dados

A aplicação já vem configurada para o SQL Server LocalDB e aplica as migrations automaticamente ao iniciar, então nenhum passo manual é necessário.

Para usar outra instância do SQL Server, altere a *Connection String* `DefaultConnection` em:

```text
SmartWallet.Web/appsettings.json
```

Se preferir aplicar as migrations manualmente:

```bash
dotnet tool restore
dotnet ef database update --project SmartWallet.Infrastructure --startup-project SmartWallet.Web
```

---

## Executar a aplicação

### Visual Studio

```text
F5
```

### CLI

```bash
dotnet run --project SmartWallet.Web
```

Depois, crie uma conta em **Cadastrar**. As categorias padrão são criadas automaticamente.

---

## Configurações opcionais

| Chave | Efeito |
|---|---|
| `Email:Smtp` | Habilita o envio real de e-mails de recuperação de senha. Sem ela, o e-mail não é enviado e o evento é apenas registrado no log. |
| `Seed:AdminPassword` | Em ambiente de desenvolvimento, cria o usuário `admin@smartwallet.com` com a senha informada. |

Prefira `dotnet user-secrets` ou variáveis de ambiente para esses valores, em vez do `appsettings.json`.

---

# 📈 Roadmap

## 🌐 Landing Page

- [x] Hero
- [x] Dashboard Preview
- [x] Seção de Recursos
- [x] Como Funciona
- [x] Benefícios
- [x] Tecnologias
- [x] Call To Action
- [x] Footer
- [x] Responsividade

---

## 🔐 Autenticação

- [x] ASP.NET Core Identity
- [x] Login
- [x] Cadastro de usuários
- [x] Logout
- [x] Recuperação de senha
- [x] Isolamento de dados por usuário
- [ ] Perfil do usuário

---

## 📊 Dashboard

- [x] Dashboard autenticado
- [x] Boas-vindas ao usuário logado
- [x] Cards financeiros
- [x] Indicadores financeiros reais
- [x] Últimas transações
- [ ] Metas financeiras
- [ ] Gráficos

---

## 💸 Transações

- [x] Cadastro
- [x] Listagem
- [x] Edição
- [x] Exclusão
- [x] Pesquisa e filtros
- [x] Paginação

---

## 📂 Categorias

- [x] Cadastro
- [x] Listagem
- [x] Edição
- [x] Exclusão

---

## 🎯 Metas Financeiras

- [ ] Cadastro
- [ ] Acompanhamento
- [ ] Indicadores

---

## 📄 Relatórios

- [ ] Relatórios financeiros
- [ ] Exportação para PDF
- [ ] Exportação para Excel

---

## 🧪 Qualidade

- [x] Testes unitários
- [x] Testes de integração
- [x] CI com GitHub Actions
- [ ] Deploy

---

# 🧪 Testes

Atualmente o projeto possui:

- Testes de Domínio
- Testes da Camada Application
- Testes de integração com `WebApplicationFactory`, cobrindo autenticação, antiforgery e isolamento de dados entre usuários
- xUnit
- FluentAssertions
- Moq

Para executar:

```bash
dotnet test
```

Os testes de integração sobem a aplicação real contra um banco SQL Server temporário, criado pelas migrations e removido ao final. Por padrão usam o LocalDB; para outra instância, defina a variável de ambiente `SMARTWALLET_TEST_CONNECTION` com a connection string (sem o nome do banco).

A cada push e pull request na `master`, o GitHub Actions compila a solução e executa todos os testes.

Status atual:

✅ 41 testes unitários aprovados  
✅ 14 testes de integração aprovados

---

# 📄 Licença

Este projeto está licenciado sob a licença **MIT**.