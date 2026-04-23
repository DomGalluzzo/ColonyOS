import { ColonyResource } from "./colony-resource.model";

export interface ColonyState {
    resources: ColonyResource[];
    lastUpdatedUtc: string;
}