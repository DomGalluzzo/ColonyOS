import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable } from "rxjs";
import { ColonyState } from "../models/colony-state.model";

@Injectable({
    providedIn: 'root'
})
export class DashboardService {
    private http = inject(HttpClient);
    private baseUrl = 'https://localhost:7001/api/dashboard';

    getState(): Observable<ColonyState> {
        return this.http.get<ColonyState>(`${this.baseUrl}/state`);
    }
}