import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable()
export class JobSchedulingService {
    
    constructor(private httpClient : HttpClient) {}

    getJobs() {
        
    }   

}