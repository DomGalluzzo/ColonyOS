import { Alert } from "./alert.model";
import { ColonyResource } from "./colony-resource.model";
import { CrewMember } from "./crew-member.model";
import { TaskModel } from "./task-item.model";

export interface ColonyState {
    resources: ColonyResource[];
    alerts: Alert[];
    tasks: TaskModel[];
    crewMembers: CrewMember[];
    lastUpdatedUtc: string;
}