import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class ApiService<T> {
  
    private headers = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient) {}

  get(url: string): Observable<T[]> {
    return this.http.get<T[]>(url, { headers: this.headers });
  }

  getById(url: string, id: number): Observable<T> {
    return this.http.get<T>(`${url}/${id}`, { headers: this.headers });
  }

  create(url: string, item: T): Observable<T> {
    return this.http.post<T>(url, item, { headers: this.headers, responseType: 'text' as 'json' });
  }

  update(url: string, id: number, item: T): Observable<T> {
    return this.http.put<T>(`${url}/${id}`, item, { headers: this.headers });
  }

  delete(url: string, id: number): Observable<void> {
    return this.http.delete<void>(`${url}/${id}`, { headers: this.headers });
  }
}
