import { Component } from '@angular/core';
import { CreateScheduledJob } from '../domain/createScheduledJob';
import { JobSchedulingService } from '../service/jobschedulingservice';
import { SelectItem } from 'primeng/api';
import { DatabaseConfig } from '../domain/databaseConfig/databaseConfig';


@Component({
    selector:'createsqljob',
    templateUrl:'./createsqljob.component.html'
})
export class CreateSqlJobComponent {

    constructor(jobSchedulingService : JobSchedulingService) {}

    protected dbTypes : SelectItem[] = [{label:"Sql server", value: "SQLSERVER"}, {label:"MySql/Aurora", value:"MYSQL"}]
    protected chosenDbType : string = null;

    jobToCreate: CreateScheduledJob = {
        jobGroup: null,
        jobName: null,
        description: null,
        cronExpression: null,
        triggerDescription: null,
        jobConfig: {
            databases: [
                {
                    connectionString: null,
                    version: null,
                    tables: []
                },
                {
                    connectionString: null,
                    version: null,
                    tables: []
                }
            ]
        }
    }

    protected getDbName(database : DatabaseConfig, index : number) {

        if (index === (this.jobToCreate.jobConfig.databases.length - 1) && index !== 0) {
            return "+";
        }

        else {
            return `Database Config ${index}`;
        }

    }

    protected handleDatabaseTabClick(event) {
        if (event.index === (this.jobToCreate.jobConfig.databases.length - 1)) {
            this.jobToCreate.jobConfig.databases.push({
                connectionString: null,
                version: null,
                tables: []
            });
        }
    }
}