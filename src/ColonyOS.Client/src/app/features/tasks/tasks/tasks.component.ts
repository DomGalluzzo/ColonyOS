import { Component, Input } from '@angular/core';
import { TaskModel, TaskPriority, TaskStatus } from '../../../shared/models/task-item.model';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss']
})
export class TasksComponent {
  @Input() tasks: TaskModel[];

  public taskPriority = TaskPriority;
  public taskStatus = TaskStatus;

  public readonly taskStatusMeta: Record<TaskStatus, { label: string; class: string }> = {
    [TaskStatus.Pending]: { label: 'Pending', class: 'text-light' },
    [TaskStatus.InProgress]: { label: 'In Progress', class: 'text-info' },
    [TaskStatus.Completed]: { label: 'Completed', class: 'text-success' },
    [TaskStatus.Cancelled]: { label: 'Cancelled', class: 'text-danger' }
  };

  trackByTaskId(index: number, task: TaskModel) {
    return task.id;
  }

  public getTaskStatus(status: TaskStatus): string {
    switch (status) {
      case TaskStatus.Cancelled:
        return 'Cancelled';
      case TaskStatus.Completed:
        return 'Completed';
      case TaskStatus.InProgress:
        return 'In Progress';
      case TaskStatus.Pending:
        return 'Pending';
    }
  }
}
