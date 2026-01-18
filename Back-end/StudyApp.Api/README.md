# StudyApp.Api

This folder contains the EF Core Code-First entities and `ApplicationDbContext` for the Study web application.

Quick start - create database and initial migration:

1. Ensure `dotnet-ef` is available:

```bash
dotnet tool install --global dotnet-ef
```

2. From the workspace root run (Windows PowerShell example):

```powershell
cd "D:/MyLearning/Back-end/StudyApp.Api"
dotnet restore
dotnet ef migrations add InitialCreate --output-dir Migrations
dotnet ef database update
```

Notes:

- The project targets .NET 8 and includes packages: `Microsoft.AspNetCore.Identity.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, and `Microsoft.EntityFrameworkCore.Design`.
- The `ApplicationDbContext` extends `IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>`.
- The migration step will generate the SQL schema from the models in `Models/` and the DbContext configuration.

If you want, I can scaffold a working `Program.cs` with Identity/EF configuration and a sample `appsettings.json` connection string next.
