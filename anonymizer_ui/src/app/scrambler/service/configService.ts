import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';



@Injectable()
export class ConfigService {

    private config : Object = null;
    private env: Object = null;

    baseUrl: string;

    constructor(private httpClient: HttpClient) {

    }

    public getConfig(key: any) {
        return this.config[key];
    }

    public getEnv(key: any) {
        return this.env[key];
    }

    public load() {
        return new Promise((resolve, reject) => {
            this.httpClient.get('./assets/env.json')
              .toPromise()
              .then((envResponse: any) => {
                this.env = envResponse;
                let request:any = null;

                switch (envResponse.env) {
                    case 'production': {
                        request = this.httpClient.get('./assets/config.' + envResponse.env + '.json').toPromise();
                    } break;

                    case 'development': {
                        request = this.httpClient.get('./assets/config.' + envResponse.env + '.json').toPromise();
                    } break;

                    case 'default': {
                        console.error('Environment file is not set or invalid');
                        resolve(true);
                    } break;
                }
                
                if (request) {
                    request.then(responseData => {
                        this.config = responseData;
                        resolve(true);
                    })
                    .catch(error => {
                        console.error('Error reading' + envResponse.env +'configuration file');
                        resolve(error);
                    })

                }
                else {
                    console.error('Env config file "env.json" is not valid');
                    resolve(true);
                }

              })
              .catch(error => {
                console.error('Error reading configuration file env.json');
                resolve(error);
              })
    });   

    }
}
