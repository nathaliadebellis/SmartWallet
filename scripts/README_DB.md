# Scripts de banco de dados

A aplicação aplica as migrations automaticamente ao iniciar, então estes scripts só são necessários em casos específicos. Execute-os a partir da raiz da solução.

| Arquivo | Uso |
|---|---|
| `update-db.ps1` | Aplica as migrations no banco configurado em `DefaultConnection`, sem subir a aplicação. Cria o banco se ele não existir. |
| `generate-migration-script.ps1` | Gera `migrations.sql`, um script SQL idempotente com todas as migrations. |
| `migrations.sql` | Script gerado. Use quando a aplicação não puder aplicar as migrations sozinha, por exemplo em um servidor onde o usuário da aplicação não tem permissão de DDL. |

## Criar uma nova migration

```powershell
dotnet ef migrations add <Nome> -p SmartWallet.Infrastructure -s SmartWallet.Web
```

Depois de criar a migration, rode `generate-migration-script.ps1` para atualizar o `migrations.sql`.

## Observações

- Em desenvolvimento o projeto usa SQL Server LocalDB (`(localdb)\MSSQLLocalDB`).
- Não use LocalDB sob IIS ou em produção: altere `DefaultConnection` para uma instância SQL Server ou SQL Express.
