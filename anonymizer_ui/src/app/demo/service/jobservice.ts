import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { JobDescription } from '../domain/jobDescription';
import { ConfigService } from './configService';
import { JobDescriptionReportResponse } from '../domain/jobDescriptionReportResponse';

@Injectable()
export class JobService {
    

    constructor(private httpClient : HttpClient, private configService : ConfigService) {}

    public getAllJobDescriptions() : Promise<JobDescriptionReportResponse> {
        return this.httpClient.get<JobDescriptionReportResponse>(this.configService.getConfig('baseUrl') + '/job/all')
            .toPromise()
            .then(data => data as JobDescriptionReportResponse)
            .then(data => data);
    }

    public getJobDescriptionsWithFilter(groupFilter: string) : Promise<JobDescriptionReportResponse> {
        return this.httpClient.get<JobDescriptionReportResponse>(this.configService.getConfig('baseUrl') + '/job?groupName=' + groupFilter)
            .toPromise()
            .then(data => data as JobDescriptionReportResponse)
            .then(data => data);
    }
 
}