import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';
import { TaskFormModel } from '../../shared/models/task-form.model';
import { TargetSystem, TaskModel, TaskPriority, TaskType } from '../../shared/models/task-item.model';
import { Alert } from '../../shared/models/alert.model';
import { TasksService } from '../../shared/services/tasks.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrl: './tasks.component.scss'
})
export class TasksComponent implements OnInit {
  @Input() alert: Alert | null = null;
  @Output() taskCreated = new EventEmitter<boolean>();

  public form!: FormGroup<TaskFormModel>;
  public tasks: TaskModel[] = [];

  readonly taskTypeOptions = [
    { label: 'Maintenance', value: TaskType.Maintenance },
    { label: 'Investigation', value: TaskType.Investigation },
    { label: 'Emergency Response', value: TaskType.EmergencyResponse }
  ];

  readonly priorityOptions = [
    { label: 'Low', value: TaskPriority.Low },
    { label: 'Medium', value: TaskPriority.Medium },
    { label: 'High', value: TaskPriority.High },
    { label: 'Critical', value: TaskPriority.Critical }
  ];

  readonly subsystemOptions = [
    { label: 'Oxygen', value: TargetSystem.Oxygen },
    { label: 'Water', value: TargetSystem.Water },
    { label: 'Power', value: TargetSystem.Power },
    { label: 'Food', value: TargetSystem.Food },
    { label: 'Habitat', value: TargetSystem.Habitat },
    { label: 'Communications', value: TargetSystem.Communications }
  ];

  constructor(private readonly fb: FormBuilder,
    private readonly taskService: TasksService
  ) {};

  ngOnInit(): void {
    this.initializeForm();
    this.registerFormBehavior();
  }

  get title(): FormControl<string> {
    return this.form.controls.title;
  }

  get description(): FormControl<string> {
    return this.form.controls.description;
  }

  get taskType(): FormControl<TaskType> {
    return this.form.controls.taskType;
  }

  get targetSubsystem(): FormControl<TargetSystem> {
    return this.form.controls.targetSubsystem;
  }

  get priority(): FormControl<TaskPriority> {
    return this.form.controls.priority;
  }

  get estimatedDurationMinutes(): FormControl<number> {
    return this.form.controls.estimatedDurationMinutes;
  }

  get sourceAlertId(): FormControl<string | null> {
    return this.form.controls.sourceAlertId;
  }

  public submit(): void {
    this.form.markAllAsTouched();

    if (this.form.invalid) {
      return;
    }

    const request = {
      title: this.title.value,
      description: this.description.value,
      taskType: this.taskType.value,
      targetSystem: this.targetSubsystem.value,
      priority: this.priority.value,
      estimatedDurationMinutes: this.estimatedDurationMinutes.value,
      sourceAlertId: this.sourceAlertId.value
    };

    this.taskService.createTask(request).subscribe({
      next: createdTask => {
        this.tasks.push(createdTask);
        this.form.reset();
        this.taskCreated.emit(true);
      },
      error: error => {
        console.error('Failed to create task', error);
        this.taskCreated.emit(false);
      }
    });
  }

  private initializeForm(): void {
    this.form = this.fb.group<TaskFormModel>({
      title: this.fb.nonNullable.control('', [Validators.required, Validators.maxLength(120)]),
      description: this.fb.nonNullable.control('', [Validators.maxLength(1000)]),
      taskType: this.fb.nonNullable.control(TaskType.Maintenance, [Validators.required]),
      targetSubsystem: this.fb.nonNullable.control<TargetSystem>(TargetSystem.Communications),
      priority: this.fb.nonNullable.control(TaskPriority.Medium, [Validators.required]),
      estimatedDurationMinutes: this.fb.nonNullable.control<number>(30, [Validators.required, Validators.min(1)]),
      sourceAlertId: this.fb.control<string | null>(null)
    },
    {
      validators: [this.maintenanceRequiresSubsystemValidator()]
    })
  };

  private registerFormBehavior(): void {
    this.taskType.valueChanges.subscribe((taskTypeValue: TaskType) => {
      if (taskTypeValue === TaskType.EmergencyResponse) {
        this.priority.setValue(TaskPriority.Critical);
      }

      if (taskTypeValue !== TaskType.Maintenance) {
        this.targetSubsystem.setValue(TargetSystem.Communications);
      }

      this.form.updateValueAndValidity();
    })
  }

  private maintenanceRequiresSubsystemValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const taskTypeValue = control.get('taskType')?.value as TaskType | null;
      const targetSubsystemValue = control.get('targetSubsystem')?.value as TargetSystem | null;

      if (taskTypeValue === TaskType.Maintenance && targetSubsystemValue == null) {
        return {
          subsystemRequiredForMaintenance: true
        };
      }
      return null;
    }
  }
}
