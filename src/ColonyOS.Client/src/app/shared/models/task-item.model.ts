export enum TaskType {
  Maintenance = 1,
  Investigation = 2,
  EmergencyResponse = 3
}

export enum TaskPriority {
  Low = 1,
  Medium = 2,
  High = 3,
  Critical = 4
}

export enum TaskStatus {
  Pending = 1,
  InProgress = 2,
  Completed = 3,
  Cancelled = 4
}

export enum TargetSystem {
  Oxygen = 1,
  Water = 2,
  Power = 3,
  Food = 4,
  Habitat = 5,
  Communications = 6
}

export interface TaskModel {
  id: string;
  title: string;
  description?: string | null;
  taskType: TaskType;
  targetSystem?: TargetSystem | null;
  taskPriority: TaskPriority;
  status: TaskStatus;
  estimatedDurationMinutes: number;
  createdAtUtc: string;
  startedAtUtc?: string | null;
  completedAtUtc?: string | null;
  sourceAlertId?: string | null;
}

export interface CreateTaskRequest {
  title: string;
  description?: string | null;
  taskType: TaskType;
  targetSystem?: TargetSystem | null;
  taskPriority: TaskPriority;
  estimatedDurationMinutes: number;
  sourceAlertId?: string | null;
}

export interface UpdateTaskStatusRequest {
  status: TaskStatus;
}