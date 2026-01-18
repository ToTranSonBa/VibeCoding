# StudyApp (VibeCoding)

A simple study web application: a .NET 8 Web API backend (Identity + JWT + EF Core) and an Angular frontend (standalone components, Tailwind, ngx-markdown) for managing flashcard decks and markdown-based knowledge notes.

## Repository layout

- `Back-end/` — ASP.NET Core Web API (StudyApp.Api)
  - `StudyApp.Api/` contains the API project, EF Core migrations, controllers, and services.
- `Front-end/` — Angular app (standalone components, TailwindCSS, ngx-markdown)

## Tech stack

- Backend: .NET 8, ASP.NET Core, EF Core, ASP.NET Identity, JWT
- Frontend: Angular (standalone components), Tailwind CSS, ngx-markdown
- DB: LocalDB / SQL Server (dev), configurable via `appsettings.json` or env vars

## Quickstart

### Backend

1. Configure connection string and any secrets in `Back-end/StudyApp.Api/appsettings.json` or via environment variables.
2. From the repo root:

```powershell
cd Back-end/StudyApp.Api
dotnet build
dotnet ef database update   # if you want to apply migrations locally
dotnet run
```

The API will start on the ports shown in the console (e.g. `https://localhost:5001`). Review `appsettings.json` for exact URLs.

### Frontend

1. Install dependencies and run the dev server:

```powershell
cd Front-end
npm install
npm run start
```

2. Open http://localhost:4200 and the frontend will proxy/call the backend API URLs configured in the app.

If you see peer-dependency warnings with `ngx-markdown` and Angular, try installing with `--legacy-peer-deps`:

```powershell
npm install --legacy-peer-deps
```

## Notes

- Tailwind’s typography plugin is used for markdown rendering. If you alter `tailwind.config.js`, ensure `@tailwindcss/typography` is installed in the frontend.
- The backend contains EF Core migrations in `Back-end/StudyApp.Api/Migrations` created during development.

## Contributing

- Create a branch, work on features in `Back-end` or `Front-end`, and open a pull request.
- Include short descriptions in your commits and add simple usage notes to this README when adding features.

## License

Add a license file if you want to open-source this project. This repository currently has no license file.
