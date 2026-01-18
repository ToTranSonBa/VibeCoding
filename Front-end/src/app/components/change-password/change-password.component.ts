import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormBuilder, Validators } from "@angular/forms";
import { AuthService } from "../../services/auth.service";
import { ToastService } from "../../shared/toast/toast.service";

@Component({
  selector: "app-change-password",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="min-h-screen flex items-center justify-center bg-gray-50">
      <div class="w-full max-w-md card p-8">
        <h2 class="text-2xl font-semibold mb-6 text-center">Change Password</h2>

        <form [formGroup]="form" (ngSubmit)="submit()" class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700"
              >Current Password</label
            >
            <input
              formControlName="currentPassword"
              type="password"
              class="mt-1 block w-full input"
            />
            <div
              *ngIf="
                form.controls['currentPassword'].invalid &&
                (form.controls['currentPassword'].dirty ||
                  form.controls['currentPassword'].touched)
              "
              class="text-xs text-red-600 mt-1"
            >
              <div
                *ngIf="form.controls['currentPassword'].errors?.['required']"
              >
                Current password is required.
              </div>
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700"
              >New Password</label
            >
            <input
              formControlName="newPassword"
              type="password"
              class="mt-1 block w-full input"
            />
            <div
              *ngIf="
                form.controls['newPassword'].invalid &&
                (form.controls['newPassword'].dirty ||
                  form.controls['newPassword'].touched)
              "
              class="text-xs text-red-600 mt-1"
            >
              <div *ngIf="form.controls['newPassword'].errors?.['required']">
                New password is required.
              </div>
              <div *ngIf="form.controls['newPassword'].errors?.['minlength']">
                New password must be at least 6 characters.
              </div>
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700"
              >Confirm New Password</label
            >
            <input
              formControlName="confirmPassword"
              type="password"
              class="mt-1 block w-full input"
            />
            <div
              *ngIf="
                form.controls['confirmPassword'].invalid &&
                (form.controls['confirmPassword'].dirty ||
                  form.controls['confirmPassword'].touched)
              "
              class="text-xs text-red-600 mt-1"
            >
              <div
                *ngIf="form.controls['confirmPassword'].errors?.['required']"
              >
                Please confirm the new password.
              </div>
            </div>
          </div>

          <div>
            <button
              type="submit"
              [disabled]="form.invalid || loading"
              class="btn btn-primary w-full"
            >
              <span *ngIf="!loading">Change Password</span>
              <span *ngIf="loading">Saving...</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  `,
})
export class ChangePasswordComponent {
  form = this.fb.group({
    currentPassword: ["", [Validators.required]],
    newPassword: ["", [Validators.required, Validators.minLength(6)]],
    confirmPassword: ["", [Validators.required]],
  });

  loading = false;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private toast: ToastService
  ) {}

  submit() {
    if (this.form.invalid) return;
    const { currentPassword, newPassword, confirmPassword } = this.form.value;
    if (newPassword !== confirmPassword) {
      this.toast.show("New passwords do not match", "error");
      return;
    }
    this.loading = true;
    this.auth.changePassword(currentPassword, newPassword).subscribe({
      next: () => {
        this.loading = false;
        this.toast.show("Password changed successfully", "success");
        this.form.reset();
      },
      error: (err) => {
        this.loading = false;
        const msg = err?.error?.message || "Failed to change password";
        this.toast.show(msg, "error");
      },
    });
  }
}
