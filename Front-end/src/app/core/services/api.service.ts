import { Injectable } from "@angular/core";
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpParams,
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, map } from "rxjs/operators";

@Injectable({ providedIn: "root" })
export class ApiService {
  constructor(private http: HttpClient) {}

  private handleError(err: HttpErrorResponse) {
    // Centralized error handling; adapt to your error shape
    let message = "An unknown error occurred";
    if (err.error && typeof err.error === "string") {
      message = err.error;
    } else if (err.error && err.error.message) {
      message = err.error.message;
    } else if (err.message) {
      message = err.message;
    }
    return throwError(() => ({ status: err.status, message }));
  }

  private buildHeaders(headers?: Record<string, string>): HttpHeaders {
    let h = new HttpHeaders({ Accept: "application/json" });
    if (headers) {
      for (const k of Object.keys(headers)) {
        h = h.set(k, headers[k]);
      }
    }
    return h;
  }

  get<T>(
    url: string,
    params?: Record<string, string | number | boolean>,
    headers?: Record<string, string>
  ): Observable<T> {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach((k) => {
        const v = params[k];
        if (v !== undefined && v !== null)
          httpParams = httpParams.set(k, String(v));
      });
    }
    return this.http
      .get<T>(url, { headers: this.buildHeaders(headers), params: httpParams })
      .pipe(catchError(this.handleError));
  }

  post<T>(
    url: string,
    body: any,
    headers?: Record<string, string>
  ): Observable<T> {
    return this.http
      .post<T>(url, body, { headers: this.buildHeaders(headers) })
      .pipe(catchError(this.handleError));
  }

  put<T>(
    url: string,
    body: any,
    headers?: Record<string, string>
  ): Observable<T> {
    return this.http
      .put<T>(url, body, { headers: this.buildHeaders(headers) })
      .pipe(catchError(this.handleError));
  }

  delete<T>(url: string, headers?: Record<string, string>): Observable<T> {
    return this.http
      .delete<T>(url, { headers: this.buildHeaders(headers) })
      .pipe(catchError(this.handleError));
  }

  // helper for endpoints returning { data: T } or other wrappers
  unwrapData<T>(obs: Observable<any>, dataKey = "data"): Observable<T> {
    return obs.pipe(
      map((response) =>
        response && response[dataKey] !== undefined
          ? (response[dataKey] as T)
          : (response as T)
      ),
      catchError(this.handleError)
    );
  }
}
