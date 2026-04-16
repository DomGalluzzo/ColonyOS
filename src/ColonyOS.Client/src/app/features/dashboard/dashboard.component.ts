import { Component, OnInit } from '@angular/core';

import { DashboardApiService } from '../../core/services/dashboard-api.service';
import { ColonyState } from '../../shared/models/colony-state.model';

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

  constructor(private readonly dashboardApiService: DashboardApiService) {}

  ngOnInit(): void {
    this.loadState();
  }

  public loadState(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.dashboardApiService.getColonyState().subscribe({
      next: (state: ColonyState) => {
        this.colonyState = state;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load colony state';
        this.isLoading = false;
      }
    })
  }
}
