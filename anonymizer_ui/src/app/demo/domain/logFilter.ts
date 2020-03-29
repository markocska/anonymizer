export interface LogFilter {
    groupKey: string;
    jobKey: string;
    description: string;
    severity: string;
    fromDate: Date;
    toDate: Date;
}