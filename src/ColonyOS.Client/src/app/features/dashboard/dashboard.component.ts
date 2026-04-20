import { Component, OnInit } from '@angular/core';

import { DashboardApiService } from '../../core/services/dashboard-api.service';
import { ColonyState } from '../../shared/models/colony-state.model';
import { AlertService } from '../../shared/services/alert.services';
import { Alert } from '../../shared/models/alert.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  public title = 'ColonyOS';
  public colonyState: ColonyState | null = null;
  public isLoading = false;
  public errorMessage: string | null = null;
  public alerts: Alert[] = [];

  constructor(
    private readonly dashboardApiService: DashboardApiService,
    private readonly alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.loadState();
  }

  public loadState(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.dashboardApiService.getColonyState().subscribe({
      next: (state: ColonyState) => {
        this.colonyState = state;
        this.loadAlerts();
      },
      error: () => {
        this.errorMessage = 'Failed to load colony state';
        this.isLoading = false;
      }
    })
  }

  public loadAlerts(): void {
    this.alertService.getAlerts().subscribe({
      next: (alerts) => {
        this.alerts = alerts;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to load alerts');
  console.error('status:', error.status);
  console.error('statusText:', error.statusText);
  console.error('message:', error.message);
  console.error('error payload:', error.error);
  console.error('full error:', error);
  this.isLoading = false;
      }
    })
  }

  public acknowledgeAlert(id: string): void {
    this.alertService.acknowledgeAlert(id).subscribe({
      next: () => this.loadAlerts(),
      error: (error) => console.error('Failed to acknowledge alert', error)
    });
  }

  public trackByAlertId(_: number, alert: Alert): string {
    return alert.id;
  }

  // public getAlertMessage(type: AlertType): string {
  //   switch (type) {
  //     case AlertType.OxygenCritical:
  //       return 'Oxygen levels critically low'
  //   }
  // }
}
