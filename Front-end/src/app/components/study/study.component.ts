import { Component, OnInit, HostBinding } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ActivatedRoute } from "@angular/router";
import {
  FlashcardService,
  CardDto,
  DeckDto,
} from "../../services/flashcard.service";
import { ToastService } from "../../shared/toast/toast.service";

@Component({
  selector: "app-study",
  standalone: true,
  imports: [CommonModule],
  styles: [
    `
      .card-scene {
        perspective: 1000px;
      }
      .card {
        width: 360px;
        height: 220px;
        cursor: pointer;
        position: relative;
        transform-style: preserve-3d;
        transition: transform 500ms;
      }
      .card.flipped {
        transform: rotateY(180deg);
      }
      .face {
        position: absolute;
        inset: 0;
        backface-visibility: hidden;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 16px;
        border-radius: 8px;
        box-shadow: 0 6px 18px rgba(0, 0, 0, 0.12);
      }
      .front {
        background: white;
      }
      .back {
        background: #f8fafc;
        transform: rotateY(180deg);
      }
    `,
  ],
  template: `
    <div class="max-w-3xl mx-auto p-6">
      <div *ngIf="deck" class="mb-4">
        <h2 class="text-2xl font-semibold">{{ deck.title }}</h2>
        <p class="text-sm text-gray-600">{{ deck.description }}</p>
      </div>

      <div
        *ngIf="cards?.length; else empty"
        class="flex flex-col items-center gap-4"
      >
        <div class="card-scene" (click)="toggleFlip()">
          <div class="card" [class.flipped]="flipped">
            <div class="face front card">
              <div class="prose text-center">{{ currentCard?.front }}</div>
            </div>
            <div class="face back card">
              <div class="prose text-center">{{ currentCard?.back }}</div>
            </div>
          </div>
        </div>

        <div class="flex items-center gap-2">
          <button (click)="prev()" class="btn btn-secondary">Previous</button>
          <div class="text-sm text-gray-600">
            {{ index + 1 }} / {{ cards.length }}
          </div>
          <button (click)="next()" class="btn btn-secondary">Next</button>
        </div>
      </div>

      <ng-template #empty>
        <div class="text-center text-gray-600">
          No cards available in this deck.
        </div>
      </ng-template>
    </div>
  `,
})
export class StudyComponent implements OnInit {
  deck?: DeckDto;
  cards: CardDto[] = [];
  index = 0;
  flipped = false;

  constructor(
    private route: ActivatedRoute,
    private svc: FlashcardService,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get("id");
    if (!id) return;
    this.svc.getDeckById(id).subscribe({
      next: (d) => (this.deck = d),
      error: () => this.toast.show("Failed to load deck", "error"),
    });
    this.svc.getCards(id).subscribe({
      next: (c) => {
        this.cards = c.sort(
          (a, b) => (a.orderIndex || 0) - (b.orderIndex || 0)
        );
        this.index = 0;
      },
      error: () => this.toast.show("Failed to load cards", "error"),
    });
  }

  get currentCard() {
    return this.cards[this.index];
  }

  toggleFlip() {
    this.flipped = !this.flipped;
  }

  next() {
    this.flipped = false;
    if (this.cards.length === 0) return;
    this.index = (this.index + 1) % this.cards.length;
  }

  prev() {
    this.flipped = false;
    if (this.cards.length === 0) return;
    this.index = (this.index - 1 + this.cards.length) % this.cards.length;
  }
}
