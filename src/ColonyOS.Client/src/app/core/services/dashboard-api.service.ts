import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ColonyState } from '../../shared/models/colony-state.model';

@Injectable({
    providedIn: 'root'
})
export class DashboardApiService {
    constructor(private readonly http: HttpClient) {}

    public getColonyState(): Observable<ColonyState> {
        return this.http.get<ColonyState>('/api/dashboard/state');
    }
}