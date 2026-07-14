# 💰 SmartWallet

Sistema web de gerenciamento financeiro pessoal desenvolvido com **ASP.NET Core MVC** e **Entity Framework Core**, utilizando arquitetura em camadas, princípios de Clean Code e boas práticas de desenvolvimento .NET.

O SmartWallet tem como objetivo auxiliar no controle de receitas e despesas por meio de uma aplicação organizada, escalável e de fácil manutenção, servindo também como projeto de portfólio para demonstrar conhecimentos em desenvolvimento backend com o ecossistema .NET.

---

# 🚀 Status do Projeto

🟡 **Em desenvolvimento**

**Versão atual:** `v0.2`

---

# ✨ Funcionalidades

## 📂 Categorias

- ✅ Cadastro de categorias
- ✅ Listagem de categorias
- ✅ Edição de categorias
- ✅ Exclusão de categorias
- ✅ Validação de categorias duplicadas
- ✅ Definição de ícone
- ✅ Definição de cor
- ✅ Classificação por tipo (Receita ou Despesa)

---

## 💸 Transações Financeiras

- ✅ Cadastro de transações
- ✅ Associação com categorias
- ✅ Validação de dados
- ✅ Observações opcionais
- ✅ Carregamento dinâmico das categorias conforme o tipo da transação (Receita/Despesa)

### Em desenvolvimento

- 🚧 Listagem de transações
- 🚧 Edição de transações
- 🚧 Exclusão de transações

---

## 📈 Próximas funcionalidades

- Dashboard financeiro
- Metas financeiras
- Relatórios
- Autenticação de usuários
- Perfil do usuário
- Exportação de dados
- Testes automatizados

---

# 🏗️ Arquitetura

O SmartWallet foi desenvolvido utilizando uma arquitetura em camadas para promover baixo acoplamento, separação de responsabilidades, reutilização de código e facilidade de manutenção.

```text
SmartWallet

├── SmartWallet.Web
│   ├── Controllers
│   ├── Views
│   ├── ViewModels
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
- JavaScript (ES6)
- Fetch API

---

# 📐 Boas Práticas Aplicadas

- Repository Pattern
- Service Layer
- Dependency Injection
- DTO Pattern
- ViewModels
- Entity Configurations
- Fluent API
- Entity Framework Migrations
- Async/Await
- Validação de domínio
- Separação de responsabilidades
- Arquitetura em camadas

---

# 📂 Estrutura do Projeto

```text
SmartWallet

├── SmartWallet.Domain
│   ├── Common
│   ├── Entities
│   ├── Enums
│   └── Interfaces
│
├── SmartWallet.Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Mappings
│   └── Services
│
├── SmartWallet.Infrastructure
│   ├── Configurations
│   ├── Data
│   ├── Migrations
│   └── Repositories
│
└── SmartWallet.Web
    ├── Controllers
    ├── ViewModels
    ├── Views
    └── wwwroot
```

---

# 📸 Demonstração

> Em breve serão adicionadas imagens das principais funcionalidades do sistema.

---

# ⚙️ Como Executar o Projeto

## Pré-requisitos

Certifique-se de possuir instalado:

- .NET SDK 10
- SQL Server LocalDB
- Visual Studio 2022

---

## Clonar o repositório

```bash
git clone https://github.com/SEU-USUARIO/SmartWallet.git
```

---

## Configurar o banco de dados

Atualize a Connection String em:

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

No **Package Manager Console**:

```powershell
Update-Database
```

Ou utilizando a CLI do .NET:

```bash
dotnet ef database update --project SmartWallet.Infrastructure --startup-project SmartWallet.Web
```

---

## Executar a aplicação

No Visual Studio:

```text
F5
```

Ou utilizando a CLI:

```bash
dotnet run --project SmartWallet.Web
```

---

# 🧪 Testes

Os testes automatizados serão implementados nas próximas versões utilizando:

- xUnit
- Moq

Objetivos:

- Validar regras de negócio
- Garantir estabilidade das funcionalidades
- Facilitar futuras refatorações
- Melhorar a qualidade do código

---

# 📈 Roadmap

## ✅ Versão 0.2

- [x] Arquitetura em camadas
- [x] Configuração do Entity Framework Core
- [x] SQL Server
- [x] CRUD de Categorias
- [x] Cadastro de Transações
- [x] Filtro dinâmico de categorias por tipo de transação

---

## 🚧 Versão 0.3

- [ ] Listagem de transações
- [ ] Edição de transações
- [ ] Exclusão de transações
- [ ] Dashboard financeiro

---

## 🚧 Versão 0.4

- [ ] Metas financeiras
- [ ] Relatórios
- [ ] Filtros de pesquisa
- [ ] Ordenação de dados

---

## 🚧 Versão 0.5

- [ ] Autenticação de usuários
- [ ] Perfil do usuário
- [ ] Configurações da aplicação

---

## 🎯 Versão 1.0

- [ ] Testes automatizados
- [ ] Deploy em ambiente cloud
- [ ] Pipeline de CI/CD
- [ ] Documentação completa

---

# 🎯 Objetivo do Projeto

O SmartWallet foi desenvolvido como projeto de portfólio para consolidar conhecimentos em desenvolvimento web com **ASP.NET Core MVC**, aplicando conceitos utilizados em aplicações corporativas, como arquitetura em camadas, injeção de dependência, persistência de dados, separação de responsabilidades e boas práticas de engenharia de software.

Além de servir como ferramenta para gerenciamento financeiro pessoal, o projeto demonstra a aplicação prática de tecnologias e padrões amplamente utilizados no ecossistema .NET.

---

# 📄 Licença

Este projeto está licenciado sob a licença **MIT**.