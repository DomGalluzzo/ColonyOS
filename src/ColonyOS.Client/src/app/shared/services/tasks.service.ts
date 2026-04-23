import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CreateTaskRequest, TaskModel, UpdateTaskStatusRequest } from "../models/task-item.model";

@Injectable({
    providedIn: 'root'
})
export class TasksService {
    private http = inject(HttpClient);
    private baseUrl = 'https://localhost:7001/api/dashboard/tasks';

    public getActiveTasks(): Observable<TaskModel[]> {
        return this.http.get<TaskModel[]>(`${this.baseUrl}`);
    }

    public createTask(request: CreateTaskRequest): Observable<TaskModel> {
        return this.http.post<TaskModel>(`${this.baseUrl}`, request);
    }

    public updateTaskStatus(taskId: string, request: UpdateTaskStatusRequest): Observable<TaskModel> {
        return this.http.patch<TaskModel>(`${this.baseUrl}/${taskId}/status`, request);
    }
}