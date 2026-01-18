import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import { ToastService } from "../../shared/toast/toast.service";

@Component({
  selector: "app-login",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="min-h-screen flex items-center justify-center bg-gray-100">
      <div class="w-full max-w-md card p-8">
        <h2 class="text-2xl font-semibold mb-6 text-center">
          Sign in to StudyApp
        </h2>

        <form [formGroup]="form" (ngSubmit)="submit()" class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700"
              >Username or Email</label
            >
            <input
              formControlName="user"
              type="text"
              class="mt-1 block w-full input"
              placeholder="you@example.com"
            />
            <div
              *ngIf="
                form.controls['user'].invalid &&
                (form.controls['user'].dirty || form.controls['user'].touched)
              "
              class="text-xs text-red-600 mt-1"
            >
              <div *ngIf="form.controls['user'].errors?.['required']">
                Username or email is required.
              </div>
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700"
              >Password</label
            >
            <input
              formControlName="password"
              type="password"
              class="mt-1 block w-full input"
              placeholder="••••••••"
            />
            <div
              *ngIf="
                form.controls['password'].invalid &&
                (form.controls['password'].dirty ||
                  form.controls['password'].touched)
              "
              class="text-xs text-red-600 mt-1"
            >
              <div *ngIf="form.controls['password'].errors?.['required']">
                Password is required.
              </div>
            </div>
          </div>

          <div class="flex items-center justify-between">
            <div class="text-sm">
              <a
                class="font-medium text-indigo-600 hover:text-indigo-500"
                href="#"
                >Forgot your password?</a
              >
            </div>
          </div>

          <div>
            <button
              type="submit"
              [disabled]="form.invalid || loading"
              class="btn btn-primary w-full"
            >
              <span *ngIf="!loading">Sign in</span>
              <span *ngIf="loading">Signing in...</span>
            </button>
          </div>
        </form>

        <p class="mt-6 text-center text-sm text-gray-600">
          Don't have an account?
          <a routerLink="/register" class="text-indigo-600 font-medium"
            >Sign up</a
          >
        </p>
      </div>
    </div>
  `,
})
export class LoginComponent {
  form = this.fb.group({
    user: ["", [Validators.required]],
    password: ["", [Validators.required]],
  });

  loading = false;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private toast: ToastService
  ) {}

  submit() {
    if (this.form.invalid) return;
    this.loading = true;
    const { user, password } = this.form.value;
    this.auth.login(user, password).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigateByUrl("/");
      },
      error: (err) => {
        this.loading = false;
        const msg = err?.error?.message || "Login failed";
        this.toast.show(msg, "error");
      },
    });
  }
}
