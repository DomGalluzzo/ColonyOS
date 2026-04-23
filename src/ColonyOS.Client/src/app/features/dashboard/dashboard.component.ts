import { Component, OnInit } from '@angular/core';

import { DashboardApiService } from '../../core/services/dashboard-api.service';
import { ColonyState } from '../../shared/models/colony-state.model';
import { AlertService } from '../../shared/services/alert.services';
import { Alert } from '../../shared/models/alert.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  public title = 'ColonyOS';
  public colonyState: ColonyState;
  public isLoading = false;
  public errorMessage: string | null = null;
  public alerts: Alert[] = [];
  public selectedAlert: Alert | null = null;

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
        console.error('Failed to load alerts', error);
        this.isLoading = false;
      }
    })
  }

  public acknowledgeAlertClicked(alert: Alert): void {
    var existingAlert = this.alerts.find(a => a.id === alert.id);
    if (!existingAlert) return;

    existingAlert.acknowledged = true;
  }
}
