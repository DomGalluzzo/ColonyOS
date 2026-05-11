export interface CrewMember {
    id: string;
    name: string;
    role: CrewRoleEnum;
    fatigue: number;
    isAvailable: boolean;
    currentTaskId?: string;
    skills: CrewSkillEnum[];
}

export enum CrewRoleEnum {
    Commander = 1,
    Engineer,
    Scientist,
    Medic,
    Technician,
    Pilot,
    SecurityOfficer,
    Botanist,
    OperationsSpecialist
}

export enum CrewSkillEnum {
    ElectricalRepair = 1,
    MechanicalRepair,
    LifeSupportSystems,
    MedicalTreatment,
    EVAOperations,
    ResearchAnalysis,
    Agriculture,
    Robotics,
    StructuralEngineering,
    RadiationMitigation,
    Communications,
    Navigation,
    ResourceOptimization,
    EmergencyResponse,
    SoftwareSystems
}