import { JobDescription } from './jobDescription';

export interface JobDescriptionReportResponse {
    jobDescriptions: JobDescription[];
    totalNumber: number;
}