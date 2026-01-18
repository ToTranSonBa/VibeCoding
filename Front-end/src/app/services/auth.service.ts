import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable, tap } from "rxjs";

interface UserDto {
  id: string;
  userName: string;
  email?: string;
}

interface AuthResponse {
  accessToken: string;
  expiresIn: number;
  refreshToken?: string;
  user?: UserDto;
}

@Injectable({ providedIn: "root" })
export class AuthService {
  private readonly TOKEN_KEY = "studyapp_access_token";
  private readonly USER_KEY = "studyapp_user";

  private currentUserSubject = new BehaviorSubject<UserDto | null>(
    this.loadUser()
  );
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(userNameOrEmail: string, password: string): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>("/api/auth/login", { userNameOrEmail, password })
      .pipe(
        tap((res) => {
          if (res?.accessToken) {
            localStorage.setItem(this.TOKEN_KEY, res.accessToken);
          }
          if (res?.user) {
            localStorage.setItem(this.USER_KEY, JSON.stringify(res.user));
            this.currentUserSubject.next(res.user);
          }
        })
      );
  }

  register(userName: string, email: string, password: string) {
    return this.http.post("/api/auth/register", { userName, email, password });
  }

  changePassword(currentPassword: string, newPassword: string) {
    return this.http.post("/api/auth/change-password", {
      currentPassword,
      newPassword,
    });
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  private loadUser(): UserDto | null {
    const raw = localStorage.getItem(this.USER_KEY);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as UserDto;
    } catch {
      return null;
    }
  }
}
