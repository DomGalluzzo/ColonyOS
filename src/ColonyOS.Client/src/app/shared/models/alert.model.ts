import { TargetSystem } from "./task-item.model";
export interface Alert {
  id: string;
  title: string;
  message: string;
  severity: AlertSeverityEnum;
  targetSystem?: TargetSystem | null;
  acknowledged: boolean;
  createdUtc: string;
  acknowledgedAtUtc?: string | null;
  relatedTaskId?: string | null;
}

export enum AlertSeverityEnum {
  Info = 1,
  Warning,
  Critical
}