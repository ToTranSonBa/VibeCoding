import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AsyncPipe } from "@angular/common";
import { ToastService } from "./toast.service";

@Component({
  selector: "app-toasts",
  standalone: true,
  imports: [CommonModule, AsyncPipe],
  template: `
    <div class="fixed top-4 right-4 z-50 flex flex-col gap-2 w-80">
      <ng-container *ngFor="let m of toast.messages | async">
        <div
          [ngClass]="getClass(m.type)"
          class="rounded shadow p-3 text-sm flex justify-between items-start"
        >
          <div class="mr-3">{{ m.text }}</div>
          <button
            class="ml-2 text-xs opacity-60 hover:opacity-100"
            (click)="toast.dismiss(m.id)"
          >
            ✕
          </button>
        </div>
      </ng-container>
    </div>
  `,
})
export class ToastComponent {
  constructor(public toast: ToastService) {}

  getClass(type: string) {
    switch (type) {
      case "success":
        return "bg-green-50 text-green-800 border border-green-100";
      case "error":
        return "bg-red-50 text-red-800 border border-red-100";
      default:
        return "bg-gray-50 text-gray-800 border border-gray-100";
    }
  }
}
