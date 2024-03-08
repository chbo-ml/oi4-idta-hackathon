import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  async countShellsRegistry(registryUrl: string) {
    const params = new HttpParams().set(
      'registryUrl',
      encodeURIComponent(`${registryUrl}`)
    );
    const result = await lastValueFrom(
      this.http.get<any>(`${this.baseUrl}api/proxy/registry`, { params })
    );

    if (result.result != null) {
      return result.result?.length ?? 0;
    } else {
      return result.length ?? 0;
    }
  }
  async countShellsRepository(registryUrl: string) {
    const params = new HttpParams().set(
      'registryUrl',
      encodeURIComponent(`${registryUrl}`)
    );
    const result = await lastValueFrom(
      this.http.get<any>(`${this.baseUrl}api/proxy/shells`, { params })
    );

    if (result.result != null) {
      return result.result?.length ?? 0;
    } else {
      return result.length ?? 0;
    }
  }
}
