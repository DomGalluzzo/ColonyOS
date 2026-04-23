export interface ColonyResource {
    colonyResourceType: ColonyResourceType;
    title: string;
    percentage: number,
    minThreshold: number;
}

export enum ColonyResourceType {
    Oxygen,
    Water,
    Power,
    Food,
    Morale,
    StructuralIntegrity,
    Radiation
}