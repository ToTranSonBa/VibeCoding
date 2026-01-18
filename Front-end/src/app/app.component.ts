import { Component } from "@angular/core";
import { ToastComponent } from "./shared/toast/toast.component";
import { RouterModule, Router } from "@angular/router";
import { FormsModule } from "@angular/forms";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [ToastComponent, RouterModule, FormsModule],
  template: `
    <app-toasts></app-toasts>
    <header class="bg-white shadow">
      <div
        class="max-w-6xl mx-auto px-4 py-3 flex items-center justify-between"
      >
        <div class="text-lg font-semibold">StudyApp</div>
        <div class="flex items-center gap-4">
          <nav class="space-x-4">
            <a routerLink="/decks" class="text-indigo-600">Decks</a>
            <a routerLink="/notes/upload" class="text-indigo-600"
              >Upload Note</a
            >
            <a routerLink="/login" class="text-indigo-600">Login</a>
            <a routerLink="/register" class="text-indigo-600">Register</a>
          </nav>

          <form (ngSubmit)="openNote()" class="flex items-center gap-2">
            <input
              [(ngModel)]="noteId"
              name="noteId"
              placeholder="Note id"
              class="border rounded px-2 py-1 w-24"
            />
            <button class="px-3 py-1 bg-gray-100 rounded">Open</button>
          </form>
        </div>
      </div>
    </header>

    <main>
      <router-outlet></router-outlet>
    </main>
  `,
})
export class AppComponent {
  noteId = "";

  constructor(private router: Router) {}

  openNote() {
    const id = (this.noteId || "").trim();
    if (!id) return;
    this.router.navigateByUrl(`/notes/${id}`);
    this.noteId = "";
  }
}
