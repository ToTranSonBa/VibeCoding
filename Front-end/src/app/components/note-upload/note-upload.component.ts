import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { NoteService } from "../../services/note.service";
import { ToastService } from "../../shared/toast/toast.service";

@Component({
  selector: "app-note-upload",
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="max-w-2xl mx-auto p-6">
      <h3 class="text-xl font-semibold mb-3">Upload Markdown Note</h3>

      <div
        class="card border-2 border-dashed p-6 text-center cursor-pointer"
        (click)="fileInput.click()"
      >
        <input
          #fileInput
          type="file"
          accept=".md,text/markdown"
          class="hidden"
          (change)="onFile($event)"
        />
        <div class="text-gray-600">
          Drag & drop a .md file here or click to select
        </div>
      </div>

      <div *ngIf="uploading" class="mt-4">
        <div class="text-sm text-gray-600">Uploading: {{ percent }}%</div>
        <div class="w-full bg-gray-200 rounded h-2 mt-2">
          <div
            [style.width.%]="percent"
            class="h-2 bg-indigo-600 rounded"
          ></div>
        </div>
      </div>

      <div *ngIf="success" class="mt-4 text-green-700">Upload complete</div>
    </div>
  `,
})
export class NoteUploadComponent {
  uploading = false;
  percent = 0;
  success = false;

  constructor(private svc: NoteService, private toast: ToastService) {}

  onFile(ev: Event) {
    const input = ev.target as HTMLInputElement;
    const f = input.files?.[0];
    if (!f) return;
    if (!f.name.endsWith(".md") && f.type !== "text/markdown") {
      this.toast.show("Only .md files are supported", "error");
      return;
    }
    this.uploading = true;
    this.percent = 0;
    this.success = false;

    this.svc.uploadNote(f).subscribe({
      next: (evt: any) => {
        if (evt.type === 1 && evt.total) {
          this.percent = Math.round((evt.loaded / evt.total) * 100);
        }
        if (evt.type === 4) {
          this.uploading = false;
          this.success = true;
          this.toast.show("File uploaded", "success");
        }
      },
      error: (err) => {
        this.uploading = false;
        this.toast.show(err?.message || "Upload failed", "error");
      },
    });
  }
}
