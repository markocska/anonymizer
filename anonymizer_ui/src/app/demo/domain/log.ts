export interface Log {
    id : number;
    groupKey: string;
    jobKey: string;
    jobDescription: string;
    message : string;
    severity: string;
    timeStamp : Date;

}