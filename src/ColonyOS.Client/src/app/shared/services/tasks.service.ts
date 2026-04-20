import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CreateTaskRequest, TaskModel, UpdateTaskStatusRequest } from "../models/task-item.model";

@Injectable({
    providedIn: 'root'
})
export class TasksService {
    constructor(private readonly http: HttpClient) {}

    public getActiveTasks(): Observable<TaskModel[]> {
        return this.http.get<TaskModel[]>('/api/tasks/active');
    }

    public createTask(request: CreateTaskRequest): Observable<TaskModel> {
        return this.http.post<TaskModel>('/api/tasks', request);
    }

    public updateTaskStatus(taskId: string, request: UpdateTaskStatusRequest): Observable<TaskModel> {
        return this.http.patch<TaskModel>(`/api/tasks/${taskId}/status`, request);
    }
}