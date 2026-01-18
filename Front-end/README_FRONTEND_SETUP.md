Angular Front-end - Structure, Tailwind, ngx-markdown, ApiService

1. Recommended folder structure

- src/
  - app/
    - app.config.ts # optional providers bundle for bootstrap
    - app.routes.ts
    - app.component.ts (shell)
    - core/ # singletons, providers, interceptors
      - services/
        - api.service.ts # generic API wrapper
        - auth.service.ts
      - guards/
        - auth.guard.ts
      - interceptors/
        - auth.interceptor.ts
    - features/ # feature areas (lazy loadable)
      - auth/
        - login.component.ts
        - register.component.ts
      - decks/
      - notes/
    - shared/ # shared UI components, pipes
      - components/
      - ui/
  - assets/
  - styles.css # tailwind entry

Notes:

- Keep features directory focused and lazy-load feature routes.
- Core contains app-wide singletons and providers (Http, Auth, ApiService, interceptors).
- Shared contains presentational/dumb components and pipes used across the app.

2. Tailwind CSS configuration (Angular + Tailwind)

Install:

```bash
cd Front-end
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init
```

tailwind.config.cjs (example):

```js
module.exports = {
  content: ["./src/**/*.{html,ts,css}"],
  theme: { extend: {} },
  plugins: [],
};
```

postcss.config.cjs:

```js
module.exports = {
  plugins: {
    tailwindcss: {},
    autoprefixer: {},
  },
};
```

src/styles.css (entry):

```css
@tailwind base;
@tailwind components;
@tailwind utilities;

/* global styles */
html,
body {
  height: 100%;
}
```

Then ensure `angular.json` includes `src/styles.css` in the build styles array (or your equivalent Vite/NG CLI config).

3. ngx-markdown (render Markdown)

Install:

```bash
npm install ngx-markdown marked
```

Example for standalone bootstrap: use `importProvidersFrom` to register `MarkdownModule.forRoot({ loader: HttpClient })`.

See `src/app/app.config.ts` for a sample providers bundle you can pass to `bootstrapApplication`.

4. ApiService (generic HTTP wrapper)

- Location: `src/app/core/services/api.service.ts`
- Uses `HttpClient`, returns typed Observables, centralizes error handling and optional headers.

Use the provided `ApiService` (core/services/api.service.ts) as the single HTTP client for features.
