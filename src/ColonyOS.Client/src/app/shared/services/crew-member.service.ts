import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CrewMemberService {
    private http = inject(HttpClient);
    private baseUrl = 'https://localhost:7001/api/dashboard/crew';

    public beginCrewRecovery(crewId: string): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/${crewId}/begin-recovery`, {});
    }
}