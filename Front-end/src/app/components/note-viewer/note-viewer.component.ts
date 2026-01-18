import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ActivatedRoute } from "@angular/router";
import { NoteService } from "../../services/note.service";
import { ToastService } from "../../shared/toast/toast.service";
import { MarkdownModule } from "ngx-markdown";

@Component({
  selector: "app-note-viewer",
  standalone: true,
  imports: [CommonModule, MarkdownModule],
  template: `
    <div class="max-w-3xl mx-auto p-6">
      <div *ngIf="content; else loading" class="card">
        <article class="prose prose-slate">
          <markdown [data]="content"></markdown>
        </article>
      </div>
      <ng-template #loading>
        <div class="text-center text-gray-600">Loading note...</div>
      </ng-template>
    </div>
  `,
})
export class NoteViewerComponent implements OnInit {
  content = "";

  constructor(
    private route: ActivatedRoute,
    private svc: NoteService,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get("id");
    if (!id) return;
    this.svc.getNoteRaw(id).subscribe({
      next: (txt) => (this.content = txt),
      error: () => this.toast.show("Failed to load note", "error"),
    });
  }
}
