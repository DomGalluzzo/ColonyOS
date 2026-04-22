export type AlertSeverity = 'Info' | 'Warning' | 'Critical';

export type AlertType = | 'OxygenCritical' | 'PowerCritical' | 'StructuralIntegrityCritical';

export interface Alert {
    id: string;
    type: AlertType;
    severity: AlertSeverity;
    message: string;
    createdUtc: string;
    acknowledged: boolean;
}