# SmartWallet - Architecture Documentation

## Overview

SmartWallet is a personal finance management application developed with ASP.NET Core MVC.

The main objective is to provide users with a secure and organized way to manage:

- Income
- Expenses
- Categories
- Budgets
- Financial goals
- Reports

The project is designed as a professional portfolio application, focusing on clean code, maintainability, scalability and software engineering best practices.

---

# Technology Stack

## Backend

- C#
- ASP.NET Core MVC
- .NET 10
- Entity Framework Core
- SQL Server

## Frontend

- Razor Views
- HTML
- CSS
- JavaScript
- Bootstrap

## Authentication

- ASP.NET Core Identity

## Testing

- xUnit
- Moq

---

# Architecture Overview

The application follows a traditional layered architecture.

```
SmartWallet

├── Web
│
├── Application
│
├── Domain
│
└── Infrastructure
```

---

# Layers

## SmartWallet.Web

Responsible for:

- Controllers
- Views
- ViewModels
- User interaction
- HTTP requests

The Web layer should not contain business rules.

---

## SmartWallet.Application

Responsible for:

- Application services
- Use cases
- DTOs
- Business orchestration

This layer coordinates operations between the Web and Infrastructure layers.

---

## SmartWallet.Domain

Responsible for:

- Entities
- Enums
- Domain rules
- Core business logic

This layer does not depend on external technologies.

---

## SmartWallet.Infrastructure

Responsible for:

- Database access
- Entity Framework Core
- Repositories
- External services

---

# Domain Model

## Main Entities

The first version contains:

- Category
- FinancialTransaction
- Budget
- FinancialGoal

---

# Entity Relationships

```
Category

1 -------- N

FinancialTransaction
```

A category can contain multiple financial transactions.

---

```
Category

1 -------- N

Budget
```

A category can have multiple monthly budgets.

---

```
ApplicationUser

1 -------- N

Category
FinancialTransaction
Budget
FinancialGoal
```

Each user owns their financial data.

---

# Design Decisions

## Money Representation

Financial values use:

```
decimal
```

Reason:

Avoid precision problems in financial calculations.

---

## Transaction Values

Amounts are always stored as positive values.

The transaction type defines the behavior:

```
Income
Expense
```

Example:

```
Amount = 100

Type = Expense
```

---

## Database Access

The project uses:

- Entity Framework Core
- Repository Pattern
- Service Layer

---

# Development Principles

The project follows:

- SOLID principles
- Clean Code
- DRY
- Separation of responsibilities
- Dependency Injection

---

# Future Improvements

Possible future features:

- Open Finance integration
- AI financial analysis
- Mobile application
- Public API
- Advanced reports