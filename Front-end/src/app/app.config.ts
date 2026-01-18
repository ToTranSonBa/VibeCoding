import { importProvidersFrom } from "@angular/core";
import {
  HttpClientModule,
  HttpClient,
  HTTP_INTERCEPTORS,
} from "@angular/common/http";
import { MarkdownModule, MarkedOptions } from "ngx-markdown";
import { AuthInterceptor } from "./interceptors/auth.interceptor";

// Example providers bundle for bootstrapApplication
export const APP_PROVIDERS = [
  importProvidersFrom(
    HttpClientModule,
    MarkdownModule.forRoot({
      loader: HttpClient,
    })
  ),
  {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true,
  },
  // You can also provide a custom MarkedOptions provider here if you want
];

// Usage in main.ts (example):
// bootstrapApplication(AppComponent, { providers: [ APP_PROVIDERS, provideHttpClient(), ... ] });
