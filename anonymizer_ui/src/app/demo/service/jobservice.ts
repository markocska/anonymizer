import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { JobDescription } from '../domain/jobDescription';

@Injectable()
export class JobService {
    

    constructor(private httpClient : HttpClient) {}

 
}