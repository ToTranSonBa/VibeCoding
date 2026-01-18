import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router, RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import {
  FlashcardService,
  DeckDto,
  DeckCreateDto,
  CardCreateDto,
} from "../../services/flashcard.service";
import { ToastService } from "../../shared/toast/toast.service";

@Component({
  selector: "app-deck-list",
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  template: `
    <div class="max-w-6xl mx-auto p-6">
      <div class="flex items-center justify-between mb-6">
        <h2 class="text-2xl font-semibold">Decks</h2>
        <button (click)="refresh()" class="btn btn-secondary">Refresh</button>
      </div>

      <div class="mb-6 card p-4">
        <h3 class="font-medium mb-2">Create new deck</h3>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-3">
          <input [(ngModel)]="newTitle" placeholder="Title" class="input" />
          <input
            [(ngModel)]="newDescription"
            placeholder="Description"
            class="input"
          />
          <input
            [(ngModel)]="newTags"
            placeholder="Tags (comma)"
            class="input"
          />
        </div>
        <textarea
          [(ngModel)]="newCardsRaw"
          rows="3"
          class="w-full mt-3 input"
          placeholder="Cards: one per line, front|back"
        ></textarea>
        <div class="mt-3 flex gap-2">
          <button
            (click)="createDeck()"
            [disabled]="creating"
            class="btn btn-primary"
          >
            Create Deck
          </button>
          <div *ngIf="creating" class="text-sm text-gray-600">Creating...</div>
        </div>
      </div>

      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        <ng-container *ngFor="let d of decks">
          <div class="card flex flex-col">
            <div class="flex-1">
              <h4 class="font-semibold text-lg">{{ d.title }}</h4>
              <p class="text-sm text-gray-600 mt-2">{{ d.description }}</p>
            </div>
            <div class="mt-4 flex gap-2">
              <button (click)="study(d)" class="btn btn-primary flex-1">
                Study
              </button>
              <button (click)="saveToLibrary(d)" class="btn btn-secondary">
                Save to Library
              </button>
            </div>
          </div>
        </ng-container>
      </div>
    </div>
  `,
})
export class DeckListComponent implements OnInit {
  decks: DeckDto[] = [];
  loading = false;

  newTitle = "";
  newDescription = "";
  newTags = "";
  newCardsRaw = "";
  creating = false;

  constructor(
    private svc: FlashcardService,
    private router: Router,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.loading = true;
    this.svc.getDecks().subscribe({
      next: (d) => {
        this.decks = d;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.toast.show("Failed to load decks", "error");
      },
    });
  }

  study(d: DeckDto) {
    this.router.navigateByUrl(`/study/${d.id}`);
  }

  saveToLibrary(d: DeckDto) {
    this.svc.saveDeckToLibrary(d.id).subscribe({
      next: (res) => {
        this.toast.show("Deck saved to your library", "success");
      },
      error: (err) =>
        this.toast.show(err?.message || "Failed to save deck", "error"),
    });
  }

  createDeck() {
    if (!this.newTitle.trim()) {
      this.toast.show("Title required", "error");
      return;
    }
    const dto: DeckCreateDto = {
      title: this.newTitle.trim(),
      description: this.newDescription.trim(),
      tags: this.newTags
        .split(",")
        .map((t) => t.trim())
        .filter(Boolean),
      cards: [],
    };
    if (this.newCardsRaw.trim()) {
      const lines = this.newCardsRaw
        .split("\n")
        .map((l) => l.trim())
        .filter(Boolean);
      let idx = 0;
      for (const ln of lines) {
        const parts = ln.split("|");
        const front = parts[0]?.trim() || "";
        const back = parts[1]?.trim() || "";
        dto.cards!.push({ front, back, orderIndex: idx++ });
      }
    }
    this.creating = true;
    this.svc.createDeck(dto).subscribe({
      next: (res) => {
        this.creating = false;
        this.toast.show("Deck created", "success");
        this.newTitle = "";
        this.newDescription = "";
        this.newTags = "";
        this.newCardsRaw = "";
        this.refresh();
      },
      error: (e) => {
        this.creating = false;
        this.toast.show(e?.message || "Create failed", "error");
      },
    });
  }
}
