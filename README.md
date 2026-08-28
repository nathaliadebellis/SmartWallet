# 💰 SmartWallet

<div align="center">

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-68217A?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
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

- ✅ Cadastro
- ✅ Associação com categorias
- ✅ Validação de dados
- ✅ Observações opcionais
- ✅ Carregamento dinâmico das categorias conforme o tipo da transação

---

## 🚧 Em desenvolvimento

- Gestão completa de transações
- Dashboard financeiro
- Metas financeiras
- Relatórios
- Autenticação de usuários
- Perfil do usuário
- Exportação de dados

---

# 🏗️ Arquitetura

O SmartWallet foi desenvolvido utilizando uma arquitetura em camadas, promovendo separação de responsabilidades, baixo acoplamento, reutilização de código e facilidade de manutenção.

```text
SmartWallet

├── SmartWallet.Web
│   ├── Controllers
│   ├── ViewModels
│   ├── Views
│   └── wwwroot
│
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
│   └── Interfaces
│
└── SmartWallet.Infrastructure
    ├── Configurations
    ├── Data
    ├── Migrations
    └── Repositories
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

---

# 📐 Boas Práticas Aplicadas

- Arquitetura em camadas
- Repository Pattern
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

# 📷 Demonstração

As imagens e GIFs das principais funcionalidades serão adicionados conforme a evolução do projeto.

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

## Configurar o banco de dados

Atualize a *Connection String* em:

```text
SmartWallet.Web/appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SmartWalletDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

## Criar o banco de dados

### Package Manager Console

```powershell
Update-Database
```

### Ou utilizando a CLI

```bash
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

- [ ] ASP.NET Core Identity
- [ ] Login
- [ ] Cadastro de usuários
- [ ] Logout
- [ ] Recuperação de senha
- [ ] Perfil do usuário

---

## 📊 Dashboard

- [ ] Indicadores financeiros
- [ ] Resumo de receitas e despesas
- [ ] Metas financeiras
- [ ] Gráficos

---

## 💸 Transações

- [x] Cadastro
- [ ] Listagem
- [ ] Edição
- [ ] Exclusão
- [ ] Pesquisa
- [ ] Paginação

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
- [ ] Testes de integração
- [ ] CI/CD
- [ ] Deploy

---

# 🧪 Testes

Atualmente o projeto possui:

- Testes de Domínio
- Testes da Camada Application
- xUnit
- FluentAssertions
- Moq

Status atual:

✅ 32 testes executados
✅ 32 testes aprovados

---

# 📄 Licença

Este projeto está licenciado sob a licença **MIT**.