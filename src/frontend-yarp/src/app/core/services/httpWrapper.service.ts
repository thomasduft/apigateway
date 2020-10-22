import { Observable, throwError } from 'rxjs';

import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpParams,
  HttpErrorResponse
} from '@angular/common/http';

import { CoreModule } from '../core.module';

import { ApiService } from './api.service';

@Injectable({
  providedIn: CoreModule
})
export class HttpWrapperService {
  public constructor(
    private http: HttpClient,
    private api: ApiService
  ) { }

  public get<T>(endpoint: string, params?: HttpParams): Observable<T> {
    const url = this.api.createApiUrl(endpoint);

    return this.http.get<T>(url, { params });
  }

  public post<T>(endpoint: string, body: any): Observable<T> {
    const url = this.api.createApiUrl(endpoint);

    return this.http.post<T>(url, body);
  }

  public put<T>(endpoint: string, body: any): Observable<T> {
    const url = this.api.createApiUrl(endpoint);

    return this.http.put<T>(url, body);
  }

  public delete<T>(endpoint: string): Observable<T> {
    const url = this.api.createApiUrl(endpoint);

    return this.http.delete<T>(url);
  }

  public handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('An error occured:', error.error.message);
    } else {
      console.error(`Backend returned code ${error.status}: ${error.error.title}`);
    }

    return throwError('Somehting went wrong. Please try again later.');
  }
}
