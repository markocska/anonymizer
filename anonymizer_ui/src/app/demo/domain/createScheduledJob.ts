import { DatabasesConfig } from './databaseConfig/databasesConfig';

export interface CreateScheduledJob {
    jobName : string;
    jobGroup : string;
    triggerDescription : string;
    cronExpression : string;
    description : string;
    jobConfig : DatabasesConfig;
}