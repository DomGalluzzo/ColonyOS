import { Component, DestroyRef, OnInit } from '@angular/core';

import { DashboardApiService } from '../../core/services/dashboard-api.service';
import { ColonyState } from '../../shared/models/colony-state.model';
import { AlertService } from '../../shared/services/alert.services';
import { Alert } from '../../shared/models/alert.model';
import { TaskModel } from '../../shared/models/task-item.model';
import { TasksService } from '../../shared/services/tasks.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ColonyDashboardRealtimeService } from '../../shared/services/colony-dashboard-realtime.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  public title = 'ColonyOS';
  public colonyState: ColonyState;
  public isLoading = false;
  public errorMessage: HttpErrorResponse | null = null;
  public alerts: Alert[] = [];
  public selectedAlert: Alert | null = null;
  public tasks: TaskModel[];

  constructor(
    private readonly dashboardApiService: DashboardApiService,
    private readonly alertService: AlertService,
    private readonly tasksService: TasksService,
    private readonly realtimeDashboardService: ColonyDashboardRealtimeService,
    private readonly destroyRef: DestroyRef
  ) {}

  ngOnInit(): void {
    this.loadInitialDashboard();
    this.realtimeDashboardService.startConnection();
    this.realtimeDashboardService.colonyState$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(state => {
        if (!state) return;

        this.colonyState = state;
      })
  }

  public acknowledgeAlertClicked(alert: Alert): void {
    var existingAlert = this.alerts.find(a => a.id === alert.id);
    if (!existingAlert) return;

    this.alertService.acknowledgeAlert(existingAlert.id);
  }

  public newTaskCreated(isNewTaskCreated: boolean): void {
    if (isNewTaskCreated) this.loadTasks();
  }

  private loadInitialDashboard(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.dashboardApiService.getColonyState().subscribe({
      next: (state: ColonyState) => {
        this.colonyState = state;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error;
        this.isLoading = false;
      }
    })
  }

  private loadTasks(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.tasksService.getActiveTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error;
        this.isLoading = false;
      }
    })
  }
}
