export interface ColonyResource {
    colonyResourceType: ColonyResourceType;
    title: string;
    percentage: number,
    minThreshold: number;
    maxThreshold: number;
}

export enum ColonyResourceType {
    Oxygen = 1,
    Water,
    Power,
    Food,
    Morale,
    StructuralIntegrity,
    Radiation
}