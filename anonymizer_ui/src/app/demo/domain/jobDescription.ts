import { TriggerDescription } from './TriggerDescription';

export interface JobDescription {
    jobName: string;
    jobGroup: string;
    requestRecovery: string;
    description: string;
    isDurable: boolean;
    triggers: Array<TriggerDescription>;
}