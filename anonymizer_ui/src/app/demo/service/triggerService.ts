import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './configService';

@Injectable()
export class TriggerService {

    constructor(private httpClient : HttpClient, private configService : ConfigService) {}

    public deleteTrigger(triggerGroup : string, triggerKey: string) {
        return this.httpClient.delete(this.configService.getConfig('baseUrl') + '/trigger?triggerGroup=' + triggerGroup + '&triggerName=' + triggerKey)
            .toPromise();
    }

}