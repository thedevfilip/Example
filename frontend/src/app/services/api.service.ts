import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Result } from '../models/result.model';

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

  private handleResult<T>(result: Result<T>): Result<T> {
    return result;
  }

  get<T>(endpoint: string): Observable<Result<T>> {
    return this.http.get<Result<T>>(`${this.apiUrl}${endpoint}`, { headers: this.getHeaders() })
      .pipe(
        map(apiResult => this.handleResult(apiResult))
      );
  }

  post<T>(endpoint: string, data: any): Observable<Result<T>> {
    return this.http.post<Result<T>>(`${this.apiUrl}${endpoint}`, data, { headers: this.getHeaders() })
      .pipe(
        map(apiResult => this.handleResult(apiResult))
      );
  }
}