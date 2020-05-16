import { Component, OnInit } from '@angular/core';
import { CreateScheduledJob } from '../domain/createScheduledJob';
import { JobSchedulingService } from '../service/jobschedulingservice';
import { SelectItem } from 'primeng/api';
import { DatabaseConfig } from '../domain/databaseConfig/databaseConfig';
import { TableConfig } from '../domain/databaseConfig/tableConfig';
import { DatabaseConfigInitializerService } from '../utilities/databasesConfigInitializerService';
import { HttpErrorResponse } from '@angular/common/http';
import {ErrorResponse} from '../domain/errorResponse';


@Component({
    selector:'createsqljob',
    templateUrl:'./createsqljob.component.html'
})
export class CreateSqlJobComponent implements OnInit {

    constructor(private jobSchedulingService : JobSchedulingService, private databaseConfigIniatializer : DatabaseConfigInitializerService) {}

    protected dbTypes : SelectItem[] = [{label:"Select database type", value:""},{label:"Sql server", value: "SQLSERVER"}, {label:"MySql/Aurora", value:"MYSQL"}]
    protected chosenDbType : string = null;

    jobToCreate: CreateScheduledJob = {
        jobGroup: null,
        jobName: null,
        description: null,
        cronExpression: null,
        triggerDescription: null,
        jobConfig: null
    }

    protected showJobCreatedSuccessfullyDialog : boolean = false;
    
    protected showErrorWhileCreatingJobDialog : boolean = false;
    protected errorWhileCreatingJobErrorMessage : string = null;


    ngOnInit() {
        this.jobToCreate.jobConfig = this.databaseConfigIniatializer.initializeDatabasesConfig();
    }

    protected createJobButtonClickHandler() {
        let lastDatabaseIndex = this.jobToCreate.jobConfig.databases.length - 1;

        this.jobToCreate.jobConfig.databases = 
            this.jobToCreate.jobConfig.databases.filter((value, index) => index !== lastDatabaseIndex);

        for(let database of this.jobToCreate.jobConfig.databases) {
            let lastTableIndex = database.tables.length - 1;

            database.tables = database.tables.filter((value, index) => index !== lastTableIndex);

            for(let table of database.tables) {
                let lastPairedColumnOutsideIndex = table.pairedColumnsOutsideTable.length - 1;

                table.pairedColumnsOutsideTable = table.pairedColumnsOutsideTable.filter((value, index) => index !== lastPairedColumnOutsideIndex);
            }
        }

        if (this.chosenDbType === 'SQLSERVER') {
           console.log('sql server');
            this.jobSchedulingService.createSqlServerJob(this.jobToCreate)
                .then(result => {
                    this.showJobCreatedSuccessfullyDialog = true;
                })
                .catch((error: HttpErrorResponse) => {
                    console.log(error);
                    let errorMessage = "";
                    if (error.error instanceof Object) {
                        for(let errorProp in error.error.errors) {
                            errorMessage += error.error.errors[errorProp][0] + " ";
                        }
                    }
                    this.errorWhileCreatingJobErrorMessage = errorMessage;
                    this.showErrorWhileCreatingJobDialog = true;
                });
        }
        else if (this.chosenDbType === 'MYSQL') {
            this.jobSchedulingService.createMySqlServerJob(this.jobToCreate)
            .then(result => {
                this.showJobCreatedSuccessfullyDialog = true;
            })
            .catch((error: HttpErrorResponse) => {
                console.log(error.error);
                this.errorWhileCreatingJobErrorMessage = error.error;
                this.showErrorWhileCreatingJobDialog;
            });
        }

        this.jobToCreate = {
            jobGroup: null,
            jobName: null,
            description: null,
            cronExpression: null,
            triggerDescription: null,
            jobConfig: this.databaseConfigIniatializer.initializeDatabasesConfig()
        };
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

    protected getPairedColumnsOutsideName(table : TableConfig, index : number) {

        if (index === (table.pairedColumnsOutsideTable.length - 1) && index !== 0) {
            return "+";
        }

        else {
            return `Paired column outside ${index}`;
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
                    pairedColumnsOutsideTable: [
                        {
                            columnMapping: [],
                            sourceDestMapping: []
                        },
                        {
                            columnMapping: [],
                            sourceDestMapping: []
                        }
                    ],
                    scrambledColumns: [],
                    where: null
                },
                {
                    fullTableName: null,
                    constantColumns: [],
                    pairedColumnsInsideTable: [],
                    pairedColumnsOutsideTable: [
                        {
                            columnMapping: [],
                            sourceDestMapping: []
                        },
                        {
                            columnMapping: [],
                            sourceDestMapping: []
                        }
                    ],
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
                pairedColumnsOutsideTable: [
                    {
                        columnMapping: [],
                        sourceDestMapping: []
                    },
                    {
                        columnMapping: [],
                        sourceDestMapping: []
                    }
                ],
                scrambledColumns: [],
                where: null
            });
        }
    }

    protected handlePairedColumnsOutsideTabClick(table: TableConfig, event) {
        if (event.index === (table.pairedColumnsOutsideTable.length - 1)) {
           table.pairedColumnsOutsideTable.push(
            {
                columnMapping: [],
                sourceDestMapping: []
            });
        }
    }
}