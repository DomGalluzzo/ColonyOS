import { FormControl } from "@angular/forms";
import { TargetSystem, TaskPriority, TaskType } from "./task-item.model";

export interface TaskFormModel {
  title: FormControl<string>;
  description: FormControl<string>;
  taskType: FormControl<TaskType>;
  targetSubsystem: FormControl<TargetSystem | null>;
  priority: FormControl<TaskPriority>;
  estimatedDurationMinutes: FormControl<number | null>;
  sourceAlertId: FormControl<string | null>;
}