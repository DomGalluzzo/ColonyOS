import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { ColonyState } from '../models/colony-state.model';

@Injectable({
  providedIn: 'root'
})
export class ColonyDashboardRealtimeService {
    private readonly colonyStateSubject = new BehaviorSubject<ColonyState | null>(null);

    readonly colonyState$ = this.colonyStateSubject.asObservable();

    private hubConnection?: signalR.HubConnection;

    startConnection(): void {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7001/hubs/colony-dashboard')
            .withAutomaticReconnect()
            .build();

        this.hubConnection.on('ColonyStateUpdated', (state: ColonyState) => {
            this.colonyStateSubject.next(state);
        });

        this.hubConnection
            .start()
            .catch(error => console.error('SignalR connection failed', error));
    }

    stopConnection(): void {
        this.hubConnection?.stop();
    }
}