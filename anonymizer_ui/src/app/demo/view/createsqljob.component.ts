import { Component } from '@angular/core';
import { CreateScheduledJob } from '../domain/createScheduledJob';
import { JobSchedulingService } from '../service/jobschedulingservice';
import { SelectItem } from 'primeng/api';
import { DatabaseConfig } from '../domain/databaseConfig/databaseConfig';
import { TableConfig } from '../domain/databaseConfig/tableConfig';


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
                    tables: [{
                        fullTableName: null,
                        constantColumns: [],
                        pairedColumnsInsideTable: [],
                        pairedColumnsOutsideTable: [],
                        scrambledColumns: [],
                        where: null
                    },
                    {
                        fullTableName: null,
                        constantColumns: [],
                        pairedColumnsInsideTable: [],
                        pairedColumnsOutsideTable: [],
                        scrambledColumns: [],
                        where: null
                    }
                ]
                },
                {
                    connectionString: null,
                    version: null,
                    tables: [{
                        fullTableName: null,
                        constantColumns: [],
                        pairedColumnsInsideTable: [],
                        pairedColumnsOutsideTable: [],
                        scrambledColumns: [],
                        where: null
                    },
                    {
                        fullTableName: null,
                        constantColumns: [],
                        pairedColumnsInsideTable: [],
                        pairedColumnsOutsideTable: [],
                        scrambledColumns: [],
                        where: null
                    }]
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

    protected getTableName(database : DatabaseConfig, index : number) {

        if (index === (database.tables.length - 1) && index !== 0) {
            return "+";
        }

        else {
            return `Table Config ${index}`;
        }

    }


    protected handleDatabaseTabClick(event) {
        if (event.index === (this.jobToCreate.jobConfig.databases.length - 1)) {
            this.jobToCreate.jobConfig.databases.push({
                connectionString: null,
                version: null,
                tables: [{
                    fullTableName: null,
                    constantColumns: [],
                    pairedColumnsInsideTable: [],
                    pairedColumnsOutsideTable: [],
                    scrambledColumns: [],
                    where: null
                },
                {
                    fullTableName: null,
                    constantColumns: [],
                    pairedColumnsInsideTable: [],
                    pairedColumnsOutsideTable: [],
                    scrambledColumns: [],
                    where: null
                }
            ]
            });
        }
    }

    protected handleTableTabClick(database: DatabaseConfig, event) {
        console.log(database.tables);
        if (event.index === (database.tables.length - 1)) {
           database.tables.push(
            {
                fullTableName: null,
                constantColumns: [],
                pairedColumnsInsideTable: [],
                pairedColumnsOutsideTable: [],
                scrambledColumns: [],
                where: null
            });
        }
    }
}