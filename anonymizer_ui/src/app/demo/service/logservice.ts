import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './configService';
import { LogFilter } from '../domain/logFilter';
import { LogDto } from '../domain/logDto';

@Injectable()
export class LogService {

    constructor(private httpClient : HttpClient, private configService: ConfigService) {

    }

    public getLogs(logFilter: LogFilter) : Promise<LogDto[]> {
        return this.httpClient.post(this.configService.getConfig('baseUrl') + '/log/filter', logFilter)
            .toPromise()
            .then(data => data as LogDto[])
            .then(data => data);
    }

    public getAllGroupNames() : Promise<string[]> {
        return this.httpClient.get(this.configService.getConfig('baseUrl') + '/group')
            .toPromise()
            .then(data => data as string[])
            .then(data => data);
    }

    public getAllJobKeysForJobGroup(groupName : string) : Promise<string[]> {
        return this.httpClient.get(this.configService.getConfig('baseUrl') + '/group/' + groupName  + '/jobnames')
            .toPromise()
            .then(data => data as string[])
            .then(data => data);
    }

    public getAllLogSeverityLevels() : Promise<string[]> {
        return this.httpClient.get(this.configService.getConfig('baseUrl') + '/log/severity')
            .toPromise()
            .then(data => data as string[])
            .then(data => data);
    }

}