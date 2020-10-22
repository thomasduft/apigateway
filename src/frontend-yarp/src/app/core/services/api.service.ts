import { Injectable } from '@angular/core';

import { CoreModule } from '../core.module';

@Injectable({
  providedIn: CoreModule
})
export class ApiService {
  private baseUrl = '';

  public get apiUrl(): string {
    return `${this.baseUrl}`;
  }

  public createRawUrl(endpoint: string): string {
    return `${endpoint}`;
  }

  public createApiUrl(endpoint: string): string {
    return `${this.baseUrl}/${endpoint}`;
  }
}
