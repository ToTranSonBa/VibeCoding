import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiService } from "../core/services/api.service";

export interface DeckDto {
  id: string;
  title: string;
  description?: string;
  isPublic?: boolean;
  ownerId?: string;
  sourceDeckId?: string;
  createdAt?: string;
}

export interface CardDto {
  id: string;
  front?: string;
  back?: string;
  orderIndex?: number;
}

export interface CardCreateDto {
  front: string;
  back: string;
  orderIndex?: number;
}

export interface DeckCreateDto {
  title: string;
  description?: string;
  isPublic?: boolean;
  tags?: string[];
  cards?: CardCreateDto[];
}

@Injectable({ providedIn: "root" })
export class FlashcardService {
  private base = "/api/decks";

  constructor(private api: ApiService) {}

  getDecks(query?: string): Observable<DeckDto[]> {
    return this.api.get<DeckDto[]>(this.base, query ? { q: query } : undefined);
  }

  getDeckById(id: string): Observable<DeckDto> {
    return this.api.get<DeckDto>(`${this.base}/${id}`);
  }

  getCards(deckId: string) {
    return this.api.get<CardDto[]>(`${this.base}/${deckId}/cards`);
  }

  createDeck(dto: DeckCreateDto) {
    return this.api.post<{ id: string }>(this.base, dto);
  }

  saveDeckToLibrary(deckId: string) {
    return this.api.post<{ id: string }>(`${this.base}/${deckId}/clone`, {});
  }
}
