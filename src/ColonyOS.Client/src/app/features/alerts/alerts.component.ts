import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { Alert, AlertSeverityEnum } from '../../shared/models/alert.model';

@Component({
  selector: 'app-alerts',
  templateUrl: './alerts.component.html',
  styleUrls: ['./alerts.component.scss']
})
export class AlertsComponent {
  @Input() alerts: Alert[];
  @Output() alertAcknowledged = new EventEmitter<Alert>();

  public selectedAlert: Alert | null;
  public alertSeverity = AlertSeverityEnum;

  public trackByAlertId(_: number, alert: Alert): string {
    return alert.id;
  }

  public acknowledgeAlert(id: string): void {
    var alert = this.alerts.find(a => a.id === id);

    if (!alert) return;

    this.alertAcknowledged.emit(alert);
  }
}
