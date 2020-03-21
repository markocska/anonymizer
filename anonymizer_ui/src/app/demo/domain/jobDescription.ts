import { TriggerDescription } from './triggerDescription';

export interface JobDescription {
    id: string;
    jobName: string;
    jobGroup: string;
    requestRecovery: string;
    description: string;
    isDurable: boolean;
    triggers: Array<TriggerDescription>;
}