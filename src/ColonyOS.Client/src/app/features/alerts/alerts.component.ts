import { AfterViewInit, Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { Alert } from '../../shared/models/alert.model';
import { Modal } from 'bootstrap';
import { TaskModel } from '../../shared/models/task-item.model';

@Component({
  selector: 'app-alerts',
  templateUrl: './alerts.component.html',
  styleUrls: ['./alerts.component.scss']
})
export class AlertsComponent implements AfterViewInit {
  @Input() alerts: Alert[];
  @Output() alertAcknowledged = new EventEmitter<Alert>();
  @Output() newTaskCreated = new EventEmitter<boolean>();
  @ViewChild('createTaskModal') createTaskModalElement: ElementRef<HTMLDivElement>;

  public selectedAlert: Alert | null;
  private createTaskModal: Modal;

  public ngAfterViewInit(): void {
    this.createTaskModal = new Modal(this.createTaskModalElement.nativeElement);
  }

  public trackByAlertId(_: number, alert: Alert): string {
    return alert.id;
  }

  public acknowledgeAlert(id: string): void {
    var alert = this.alerts.find(a => a.id === id);

    if (!alert) return;

    this.alertAcknowledged.emit(alert);
  }

  public openCreateTask(alert: Alert): void {
    this.selectedAlert = alert;

    const modalElement = document.getElementById('createTaskModal');

    if (!modalElement) return;

    if (!this.createTaskModal) this.createTaskModal = new Modal(modalElement);
    this.createTaskModal.show();
  }

  public closeCreateTask(): void {
    this.selectedAlert = null;
    this.createTaskModal?.hide();
  }

  public taskCreated(isSuccessful: boolean): boolean {
    if (isSuccessful) this.closeCreateTask();

    this.newTaskCreated.emit(isSuccessful);
    return isSuccessful;
  }
}
