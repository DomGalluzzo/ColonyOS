import { ColonyResourceType } from "./colony-resource.model";

export enum TaskType {
  Maintenance = 1,
  Investigation,
  EmergencyResponse
}

export enum TaskPriority {
  Low = 1,
  Medium,
  High,
  Critical
}

export enum TaskStatus {
  Pending = 1,
  InProgress,
  Completed,
  Cancelled,
  Failed,
  Assigned
}

export enum TargetSystem {
  Oxygen = 1,
  Water,
  Power,
  Food,
  Habitat,
  Communications
}

export interface TaskModel {
  id: string;
  title: string;
  description?: string | null;
  taskType: TaskType;
  targetSystem?: TargetSystem | null;
  resourceType: ColonyResourceType;
  taskPriority: TaskPriority;
  status: TaskStatus;
  estimatedDurationMinutes: number;
  createdAtUtc: string;
  startedAtUtc?: string | null;
  completedAtUtc?: string | null;
  sourceAlertId?: string | null;
  assignedCrewMemberId?: string | null;
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
  taskId: string;
  status: TaskStatus;
}

export interface AssignCrewToTaskRequest {
  taskId: string;
  crewMemberId: string;
}