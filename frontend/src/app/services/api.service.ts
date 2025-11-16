import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';
import { Error } from '../models/error.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json'
    });
  }

  get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}${endpoint}`, { headers: this.getHeaders() })
      .pipe(this.handleResponse());
  }

  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}${endpoint}`, data, { headers: this.getHeaders() })
      .pipe(this.handleResponse());
  }

  private handleResponse<T>() {
    return (source: Observable<T>) => source.pipe(
      map(response => response),
      catchError((httpError: HttpErrorResponse): Observable<never> => {
        const error: Error = {
          code: httpError.error?.code || `HTTP_${httpError.status}`,
          description: httpError.error?.description || 'An error occurred'
        };
        
        return throwError(() => error);
      })
    );
  }
}