import { OnInit, Component } from '@angular/core';
import { LogService } from '../service/logservice';
import { Log } from '../domain/log';
import { SelectItem, LazyLoadEvent, SortEvent } from 'primeng/api';
import { DateRange, DateMarker } from '@fullcalendar/core';
import { LogFilterRequest } from '../domain/logFilterRequest';
import { DATE_PROPS } from '@fullcalendar/core/structs/event';

@Component({
    selector: 'logs',
    templateUrl: './logs.component.html',
    styleUrls:['./logs.component.css']
})
export class LogComponent implements OnInit {
    
    constructor(private logService : LogService) {

    }

    public loading: boolean;

    public logCols : any[] = [
        {field:  'groupKey', header:'Group Name'},
        {field:  'jobKey', header:'Job Name'},
        {field:  'jobDescription', header:'Job Description'},
        {field:  'message', header:'Message'},
        {field:  'severity', header:'Severity'},
        {field:  'timeStamp', header:'TimeStamp'}
    ]

    public logs : Log[] = [];
    public numberOfLogs : number;

    public groupDropdownOptions : SelectItem[] = [];
    public jobNameDropdownOptions : SelectItem[] = [];
    public readonly jobNameDefault : SelectItem =  {label: "Select a job", value: null};
    public severityDropdownOptions : SelectItem[] = [];
    
    public groupKeyFilter : string = null;
    public jobKeyFilter : string = null;
    public jobDescriptionFilter : string = null;
    public severityFilter : string = null;
    public timeStampDateRangeFilter : Date[];

    public timezoneOffset : string = '+0200';


    public logFilterRequest : LogFilterRequest =
    {   
        groupKey: null,
        jobKey: null,
        fromDate: null,
        toDate: null,
        severity: null,
        isAscending: false,
        description: null,
        paginationParams: {
            pageNumber: null,
            offset: null
        }
    };
    
    ngOnInit(): void {
        this.loading = true;
        this.logService.getAllGroupNames()
            .then(groupNames => {
                this.groupDropdownOptions = 
                    [{label: "Select a group", value: null}, ... groupNames.map(x => ({label: x, value: x}))];
                
            });
        
        this.jobNameDropdownOptions.push({label:"Select a job", value: null});

        this.logService.getAllLogSeverityLevels()
            .then(severityLevels => {
                this.severityDropdownOptions =
                    [{label: "Select severity", value: null}, ...severityLevels.map(x => ({label: x, value: x}))]
            });
        this.loading = false;
    }

    public loadLogsLazy(event : LazyLoadEvent) : void { 
        this.logFilterRequest.paginationParams = 
            {
                pageNumber:  (event.first / event.rows) + 1,
                offset: 10
            }

        if (event.sortField === 'timeStamp' && event.sortOrder === 1) {
            this.logFilterRequest.isAscending = true;
        }
        else {
            this.logFilterRequest.isAscending = false;
        }

        this.loading = true;
        this.logService.getLogs(this.logFilterRequest)
            .then(logs => {
                this.logs = logs.logs;
                this.numberOfLogs = logs.totalNumber;
            });
        this.loading = false;
    }

    public filter(value, field, mode) : void {
        switch(field) {
            case 'groupKey':
                if (value) {
                    this.logService.getAllJobKeysForJobGroup(value)
                    .then(jobKeys => {
                        this.jobNameDropdownOptions = 
                            [this.jobNameDefault, ... jobKeys.map(x => ({label: x, value: x}))];
                    });
                }
                else {
                    this.jobNameDropdownOptions = [this.jobNameDefault]
                }
                break;
            case 'timestamp':
                if (this.timeStampDateRangeFilter) {
                    this.logFilterRequest.fromDate = new Date(this.timeStampDateRangeFilter[0]);
                    this.logFilterRequest.toDate = this.timeStampDateRangeFilter[1];
                }
            case 'jobDescription': 
                if (value) {
                    this.logFilterRequest.jobKey = this.jobNameDefault.value;
                }
                break;
        }

        this.loading = true;
        this.logService.getLogs(this.logFilterRequest)
            .then(logs => {
                this.logs = logs.logs;
                this.numberOfLogs = logs.totalNumber;
            });
        this.loading = false;
    }

    public sortByTimeStamp(event: SortEvent) : void {
        console.log(event);
    }
    
}