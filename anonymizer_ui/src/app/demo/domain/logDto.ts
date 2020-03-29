export interface LogDto {
    id : number;
    groupKey: string;
    jobKey: string;
    jobDescription: string;
    message : string;
    severity: string;
    timeStamp : Date;

}