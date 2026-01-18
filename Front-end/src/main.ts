import { bootstrapApplication } from "@angular/platform-browser";
import { provideRouter, Routes } from "@angular/router";
import { AppComponent } from "./app/app.component";
import { APP_PROVIDERS } from "./app/app.config";

const routes: Routes = [
  { path: "", redirectTo: "decks", pathMatch: "full" },
  {
    path: "decks",
    loadComponent: () =>
      import("./app/components/deck-list/deck-list.component").then(
        (m) => m.DeckListComponent
      ),
  },
  {
    path: "study/:id",
    loadComponent: () =>
      import("./app/components/study/study.component").then(
        (m) => m.StudyComponent
      ),
  },
  {
    path: "login",
    loadComponent: () =>
      import("./app/components/login/login.component").then(
        (m) => m.LoginComponent
      ),
  },
  {
    path: "register",
    loadComponent: () =>
      import("./app/components/register/register.component").then(
        (m) => m.RegisterComponent
      ),
  },
  {
    path: "change-password",
    loadComponent: () =>
      import("./app/components/change-password/change-password.component").then(
        (m) => m.ChangePasswordComponent
      ),
  },
  {
    path: "notes/upload",
    loadComponent: () =>
      import("./app/components/note-upload/note-upload.component").then(
        (m) => m.NoteUploadComponent
      ),
  },
  {
    path: "notes/:id",
    loadComponent: () =>
      import("./app/components/note-viewer/note-viewer.component").then(
        (m) => m.NoteViewerComponent
      ),
  },
];

bootstrapApplication(AppComponent, {
  providers: [APP_PROVIDERS, provideRouter(routes)],
}).catch((err) => console.error(err));
