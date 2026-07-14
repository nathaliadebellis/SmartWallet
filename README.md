# 💰 SmartWallet

Sistema web de gerenciamento financeiro pessoal desenvolvido em **ASP.NET Core MVC**, com foco em organização financeira, boas práticas de desenvolvimento e arquitetura em camadas.

O projeto tem como objetivo auxiliar usuários no controle de receitas, despesas, categorias e planejamento financeiro, aplicando conceitos utilizados no desenvolvimento de aplicações .NET profissionais.

---

## 🚀 Status do Projeto

🟡 Em desenvolvimento

### Versão atual: v0.1

Funcionalidades concluídas:

- ✅ Estrutura inicial da aplicação
- ✅ Arquitetura em camadas
- ✅ Persistência com Entity Framework Core
- ✅ Banco de dados SQL Server
- ✅ CRUD completo de categorias
- ✅ Validação de categorias duplicadas
- ✅ Seed inicial de categorias padrão

Próximas funcionalidades:

- 🔜 Gerenciamento de transações financeiras
- 🔜 Dashboard com indicadores financeiros
- 🔜 Autenticação de usuários
- 🔜 Relatórios financeiros
- 🔜 Testes automatizados

---

# 🏗️ Arquitetura

O SmartWallet utiliza uma arquitetura em camadas, separando responsabilidades para facilitar manutenção, testes e evolução da aplicação.

```
SmartWallet

├── SmartWallet.Web
│   └── Interface MVC
│
├── SmartWallet.Application
│   └── Serviços e regras de aplicação
│
├── SmartWallet.Domain
│   └── Entidades e contratos
│
└── SmartWallet.Infrastructure
    └── Banco de dados e persistência
```

Fluxo da aplicação:

```
Controller
    ↓
Service
    ↓
Repository
    ↓
Entity Framework Core
    ↓
SQL Server
```

---

# 🛠️ Tecnologias utilizadas

## Backend

- C#
- .NET 10
- ASP.NET Core MVC
- Entity Framework Core

## Banco de Dados

- SQL Server
- LocalDB

## Front-end

- Razor Views
- Bootstrap
- Bootstrap Icons

## Boas práticas aplicadas

- Repository Pattern
- Service Layer
- Dependency Injection
- DTOs
- ViewModels
- Entity Configuration
- Async/Await
- Validação de dados

---

# 📂 Estrutura do Projeto

```
SmartWallet

├── SmartWallet.Domain
│   ├── Entities
│   └── Interfaces
│
├── SmartWallet.Application
│   ├── DTOs
│   ├── Interfaces
│   └── Services
│
├── SmartWallet.Infrastructure
│   ├── Data
│   ├── Configurations
│   ├── Migrations
│   └── Repositories
│
└── SmartWallet.Web
    ├── Controllers
    ├── Views
    └── ViewModels
```

---

# 📌 Funcionalidades

## Categorias

Atualmente o sistema permite:

- Criar categorias
- Listar categorias
- Editar categorias
- Excluir categorias
- Definir ícone e cor
- Impedir categorias duplicadas

Categorias iniciais:

- Alimentação
- Transporte
- Moradia
- Saúde
- Educação
- Lazer
- Salário
- Investimentos
- Mercado
- Pets
- Assinaturas
- Outros

---

# ⚙️ Como executar o projeto

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

## Configurar banco de dados

Atualize a connection string em:

```
SmartWallet.Web/appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SmartWalletDb;Trusted_Connection=True"
  }
}
```

---

## Criar banco

No Package Manager Console:

```powershell
Update-Database -Project SmartWallet.Infrastructure -StartupProject SmartWallet.Web
```

---

## Executar aplicação

No Visual Studio:

```
F5
```

---

# 🧪 Testes

Testes automatizados serão adicionados nas próximas versões utilizando:

- xUnit
- Moq

Objetivo:

- Validar regras de negócio.
- Garantir estabilidade das funcionalidades.
- Melhorar qualidade do código.

---

# 📈 Roadmap

## Versão 0.1
- [x] Arquitetura inicial
- [x] CRUD de categorias

## Versão 0.2
- [ ] CRUD de transações
- [ ] Filtros
- [ ] Dashboard financeiro

## Versão 0.3
- [ ] Autenticação
- [ ] Perfil de usuário
- [ ] Configurações

## Versão 1.0
- [ ] Relatórios
- [ ] Testes automatizados
- [ ] Deploy em ambiente cloud

---

# 👩‍💻 Desenvolvimento

Projeto desenvolvido como estudo prático de desenvolvimento backend com **C# e ASP.NET Core**, aplicando conceitos de arquitetura, persistência de dados e boas práticas de engenharia de software.

---

# 📄 Licença

Este projeto está sob a licença MIT.