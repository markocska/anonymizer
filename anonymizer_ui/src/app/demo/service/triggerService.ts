import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './configService';
import { CreateTrigger } from '../domain/createTrigger';
import { TriggerSuccessfullyCreated } from '../domain/triggerSuccessfullyCrested';

@Injectable()
export class TriggerService {

    constructor(private httpClient : HttpClient, private configService : ConfigService) {}

    public deleteTrigger(triggerGroup : string, triggerKey: string) : Promise<any> {
        return this.httpClient.delete(this.configService.getConfig('baseUrl') + '/trigger?triggerGroup=' + triggerGroup + '&triggerName=' + triggerKey)
            .toPromise();
    }

    public createTrigger(triggerToCreate : CreateTrigger) : Promise<TriggerSuccessfullyCreated> {
        return this.httpClient.post(this.configService.getConfig('baseUrl') + '/trigger', triggerToCreate)
            .toPromise()
            .then(data => data as TriggerSuccessfullyCreated)
            .then(data => data);
    }

}