import {Injectable} from '@angular/core';
import { HttpClient} from '@angular/common/http';
import {Car} from '../domain/car';

@Injectable()
export class CarService {

    constructor(private http: HttpClient) {}

    getCarsSmall() : Promise<Car[]> {
        return this.http.get<any>('assets/demo/data/cars-small.json')
                    .toPromise()
                    .then(res => res.data as Car[])
                    .then(data => data);
    }

    getCarsMedium() : Promise<Car[]> {
        return this.http.get<any>('assets/demo/data/cars-medium.json')
                    .toPromise()
                    .then(res => res.data as Car[])
                    .then(data => data);
    }

    getCarsLarge() : Promise<Car[]> {
        return this.http.get<any>('assets/demo/data/cars-large.json')
                    .toPromise()
                    .then(res => res.data as Car[])
                    .then(data => data);
    }
}
