import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CrewMember, CrewRecoveryStateEnum, CrewRoleEnum, CrewSkillEnum } from '../../shared/models/crew-member.model';
import { TaskModel } from '../../shared/models/task-item.model';

@Component({
  selector: 'app-crew',
  templateUrl: './crew.component.html',
  styleUrls: ['./crew.component.scss', '../alerts/alerts.component.scss']
})
export class CrewComponent {
  @Input() crewMembers: CrewMember[];
  @Input() selectedTaskId: string | null;
  @Input() tasks: TaskModel[];
  @Output() crewAssignedToTask = new EventEmitter<string>();
  @Output() crewRecoveryStarted = new EventEmitter<string>();
  @Output() releaseCrewMemberClicked = new EventEmitter<string>();
  @Output() crewReassignedToTask = new EventEmitter<{ taskId: string; crewMemberId: string; }>();

  public recoveryStateEnum = CrewRecoveryStateEnum;
  public selectedCrewByTaskId: Record<string, CrewMember | null> = {};

  public readonly crewSkillNames = [
    { label: 'Electrical Repair', value: CrewSkillEnum.ElectricalRepair },
    { label: 'Mechanical Repair', value: CrewSkillEnum.MechanicalRepair },
    { label: 'Life Support Systems', value: CrewSkillEnum.LifeSupportSystems },
    { label: 'Medical Treatment', value: CrewSkillEnum.MedicalTreatment },
    { label: 'EVA Operations', value: CrewSkillEnum.EVAOperations },
    { label: 'Research Analysis', value: CrewSkillEnum.ResearchAnalysis },
    { label: 'Agriculture', value: CrewSkillEnum.Agriculture },
    { label: 'Robotics', value: CrewSkillEnum.Robotics },
    { label: 'Structural Engineering', value: CrewSkillEnum.StructuralEngineering },
    { label: 'Radiation Mitigation', value: CrewSkillEnum.RadiationMitigation },
    { label: 'Communications', value: CrewSkillEnum.Communications },
    { label: 'Navigation', value: CrewSkillEnum.Navigation },
    { label: 'Resource Optimization', value: CrewSkillEnum.ResourceOptimization },
    { label: 'Emergency Response', value: CrewSkillEnum.EmergencyResponse },
    { label: 'Software Systems', value: CrewSkillEnum.SoftwareSystems }
  ];

  public readonly crewRoleNames = [
    { label: 'Commander', value: CrewRoleEnum.Commander },
    { label: 'Engineer', value: CrewRoleEnum.Engineer },
    { label: 'Scientist', value: CrewRoleEnum.Scientist },
    { label: 'Medic', value: CrewRoleEnum.Medic },
    { label: 'Technician', value: CrewRoleEnum.Technician },
    { label: 'Pilot', value: CrewRoleEnum.Pilot },
    { label: 'Security Officer', value: CrewRoleEnum.SecurityOfficer },
    { label: 'Botanist', value: CrewRoleEnum.Botanist },
    { label: 'Operations Specialist', value: CrewRoleEnum.OperationsSpecialist }
  ];

  get availableCrewMembers(): CrewMember[] {
    return this.crewMembers.filter(crew =>
      crew.isAvailable &&
      crew.fatigue < 100 &&
      crew.recoveryState !== CrewRecoveryStateEnum.Recovering
    ) ?? [];
  }

  public trackByCrewMemberId(index: number, crewMember: CrewMember): string {
    return crewMember.id;
  }

  public getFatigueClass(fatigue: number): string {
    if (fatigue >= 75) {
        return 'text-danger';
    }

    if (fatigue >= 50) {
        return 'text-warning';
    }

    return 'text-success';
  }

  public getCrewSkillName(skill: CrewSkillEnum): string {
    return this.crewSkillNames.find(x => x.value === skill)?.label ?? 'Unknown';
  }

  public getCrewRoleName(role: CrewRoleEnum): string {
    return this.crewRoleNames.find(x => x.value === role)?.label ?? 'Unknown';
  }

  public getCrewStatus(crewMember: CrewMember): string {
    if (crewMember.currentTaskId) {
        return 'Working';
    }

    if (crewMember.recoveryState == this.recoveryStateEnum.Recovering) {
      return 'Recovering';
    }

    if (!crewMember.isAvailable) {
        return 'Unavailable';
    }

    return 'Available';
  }

  public getCrewStatusClass(crewMember: CrewMember): string {
    if (crewMember.currentTaskId) {
        return 'status-chip--working';
    }

    if (crewMember.recoveryState == this.recoveryStateEnum.Recovering) {
      return 'status-chip--recovering';
    }

    if (!crewMember.isAvailable) {
        return 'status-chip--unavailable';
    }

    return 'status-chip--available';
  }

  public assignCrewToTask(crewMember: CrewMember): void {
    if (!this.selectedTaskId || !crewMember.isAvailable) return;

    this.crewAssignedToTask.emit(crewMember.id);
  }

  public beginCrewRecovery(crewId: string): void {
    return this.crewRecoveryStarted.emit(crewId);
  }

  
  public reassignCrew(currentCrewMember: CrewMember): void {
    if (!currentCrewMember.currentTaskId) return;

    const replacementCrewMember = this.selectedCrewByTaskId[currentCrewMember.id];

    if (!replacementCrewMember) return;

    this.crewReassignedToTask.emit({
      taskId: currentCrewMember.currentTaskId,
      crewMemberId: replacementCrewMember.id
    });

    this.selectedCrewByTaskId[currentCrewMember.id] = null;
  }

  public releaseCrew(taskId: string): void {
    this.releaseCrewMemberClicked.emit(taskId);
  }
}
