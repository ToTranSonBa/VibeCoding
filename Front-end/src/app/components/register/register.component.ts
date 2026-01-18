import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import { ToastService } from "../../shared/toast/toast.service";

@Component({
  selector: "app-register",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="min-h-screen flex items-center justify-center bg-gray-50">
      <div class="w-full max-w-md card p-8">
        <h2 class="text-2xl font-semibold mb-6 text-center">
          Create your account
        </h2>

        <form [formGroup]="form" (ngSubmit)="submit()" class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700"
              >Username</label
            >
            <input
              formControlName="userName"
              type="text"
              class="mt-1 block w-full input"
            />
            <div
              *ngIf="
                form.controls['userName'].invalid &&
                (form.controls['userName'].dirty ||
                  form.controls['userName'].touched)
              "
              class="text-xs text-red-600 mt-1"
            >
              <div *ngIf="form.controls['userName'].errors?.['required']">
                Username is required.
              </div>
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700">Email</label>
            <input
              formControlName="email"
              type="email"
              class="mt-1 block w-full input"
            />
            <div
              *ngIf="
                form.controls['email'].invalid &&
                (form.controls['email'].dirty || form.controls['email'].touched)
              "
              class="text-xs text-red-600 mt-1"
            >
              <div *ngIf="form.controls['email'].errors?.['required']">
                Email is required.
              </div>
              <div *ngIf="form.controls['email'].errors?.['email']">
                Enter a valid email.
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
              <div *ngIf="form.controls['password'].errors?.['minlength']">
                Password must be at least 6 characters.
              </div>
            </div>
          </div>

          <div>
            <button
              type="submit"
              [disabled]="form.invalid || loading"
              class="btn btn-primary w-full"
            >
              <span *ngIf="!loading">Create account</span>
              <span *ngIf="loading">Creating...</span>
            </button>
          </div>
        </form>

        <p class="mt-6 text-center text-sm text-gray-600">
          Already have an account?
          <a routerLink="/login" class="text-indigo-600 font-medium">Sign in</a>
        </p>
      </div>
    </div>
  `,
})
export class RegisterComponent {
  form = this.fb.group({
    userName: ["", [Validators.required]],
    email: ["", [Validators.required, Validators.email]],
    password: ["", [Validators.required, Validators.minLength(6)]],
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
    const { userName, email, password } = this.form.value;
    this.auth.register(userName, email, password).subscribe({
      next: () => {
        this.loading = false;
        this.toast.show("Account created — please sign in", "success");
        // after registration, redirect to login
        this.router.navigate(["/login"]);
      },
      error: (err) => {
        this.loading = false;
        const msg = err?.error?.message || "Registration failed";
        this.toast.show(msg, "error");
      },
    });
  }
}
