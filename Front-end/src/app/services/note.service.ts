import { Injectable } from "@angular/core";
import { HttpClient, HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({ providedIn: "root" })
export class NoteService {
  private base = "/api/notes";

  constructor(private http: HttpClient) {}

  uploadNote(file: File): Observable<HttpEvent<any>> {
    const fd = new FormData();
    fd.append("file", file, file.name);
    return this.http.post<any>(`${this.base}/upload`, fd, {
      reportProgress: true,
      observe: "events",
    });
  }

  getNoteRaw(id: string | number): Observable<string> {
    return this.http.get(`${this.base}/${id}/raw`, { responseType: "text" });
  }
}
