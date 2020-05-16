import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { JobSuccessfullyCreated } from '../domain/jobSuccessfullyCreated';
import { ConfigService } from './configService';
import { CreateScheduledJob } from '../domain/createScheduledJob';

@Injectable()
export class JobSchedulingService {
   
    
    constructor(private httpClient : HttpClient, private configService : ConfigService) {}

    public createSqlServerJob(createScheduledJob : CreateScheduledJob) : Promise<JobSuccessfullyCreated> {
        return this.httpClient.post<JobSuccessfullyCreated>(this.configService.getConfig('baseUrl') + '/jobscheduling/sql', createScheduledJob)
            .toPromise()
            .then(data => data as JobSuccessfullyCreated)
            .then(data => data);
    }

    public createMySqlServerJob(createdScheduledJob : CreateScheduledJob) : Promise<JobSuccessfullyCreated> {
        return this.httpClient.post<JobSuccessfullyCreated>(this.configService.getConfig('baseUrl') + '/jobscheduling/mysql', createdScheduledJob)
            .toPromise()
            .then(data => data as JobSuccessfullyCreated)
            .then(data => data);
    }

}