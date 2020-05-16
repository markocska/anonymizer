export interface TriggerSuccessfullyCreated {
    id : string;
    jobGroup: string;
    jobName: string;
    triggerGroup: string;
    triggerName: string;
    triggerDescription: string;
    cronExpression: string;
    calendar: string;
}