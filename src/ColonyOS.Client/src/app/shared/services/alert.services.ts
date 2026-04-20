import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Alert } from '../models/alert.model';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
    private http = inject(HttpClient);
    private baseUrl = 'https://localhost:7001/api/dashboard/alerts';

    getAlerts(): Observable<Alert[]> {
        return this.http.get<Alert[]>(this.baseUrl);
    }

    acknowledgeAlert(id: string): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/${id}/acknowledge`, {});
    }
}