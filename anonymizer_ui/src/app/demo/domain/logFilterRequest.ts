import { PaginationParams } from './paginationParams';

export interface LogFilterRequest {
    groupKey: string;
    jobKey: string;
    description: string;
    severity: string;
    fromDate: Date;
    toDate: Date;
    paginationParams: PaginationParams;
}