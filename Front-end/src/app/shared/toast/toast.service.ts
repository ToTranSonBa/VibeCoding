import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

export type ToastType = "success" | "error" | "info";

export interface ToastMessage {
  id: string;
  text: string;
  type: ToastType;
  timeout?: number;
}

@Injectable({ providedIn: "root" })
export class ToastService {
  private messages$ = new BehaviorSubject<ToastMessage[]>([]);
  messages = this.messages$.asObservable();

  show(text: string, type: ToastType = "info", timeout = 4000) {
    const msg: ToastMessage = {
      id: Date.now().toString(),
      text,
      type,
      timeout,
    };
    const list = [...this.messages$.value, msg];
    this.messages$.next(list);
    if (timeout && timeout > 0) {
      setTimeout(() => this.dismiss(msg.id), timeout);
    }
  }

  dismiss(id: string) {
    this.messages$.next(this.messages$.value.filter((m) => m.id !== id));
  }
}
