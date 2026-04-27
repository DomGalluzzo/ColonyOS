import { Alert } from "./alert.model";
import { ColonyResource } from "./colony-resource.model";
import { TaskModel } from "./task-item.model";

export interface ColonyState {
    resources: ColonyResource[];
    alerts: Alert[];
    tasks: TaskModel[];
    lastUpdatedUtc: string;
}