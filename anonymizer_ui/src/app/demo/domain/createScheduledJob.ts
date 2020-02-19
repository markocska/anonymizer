export interface CreateScheduledJob {
    jobName : string;
    jobGroup : string;
    triggerDescription : string;
    cronExpression : string;
    description : string;
    
}