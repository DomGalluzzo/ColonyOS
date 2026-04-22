import { TargetSystem } from "./task-item.model";

export type AlertSeverity = 'Info' | 'Warning' | 'Critical';

export type AlertType = | 'OxygenCritical' | 'PowerCritical' | 'StructuralIntegrityCritical';

export interface Alert {
    id: string;
      title: string;
      message: string;
      severity: string;
      targetSystem?: TargetSystem | null;
      acknowledged: boolean;
      createdUtc: string;
      acknowledgedAtUtc?: string | null;
      relatedTaskId?: string | null;
}