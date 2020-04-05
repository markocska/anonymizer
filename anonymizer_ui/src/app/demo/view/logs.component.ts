import { OnInit, Component } from '@angular/core';
import { LogService } from '../service/logservice';
import { Log } from '../domain/log';
import { SelectItem, LazyLoadEvent } from 'primeng/api';
import { DateRange } from '@fullcalendar/core';
import { LogFilterRequest } from '../domain/logFilterRequest';

@Component({
    selector: 'logs',
    templateUrl: './logs.component.html'
})
export class LogComponent implements OnInit {
    
    constructor(private logservice : LogService) {

    }

    protected logCols : any[] = [
        {field:  'groupKey', header:'Group Name'},
        {field:  'jobKey', header:'Job Name'},
        {field:  'jobDescription', header:'Job Description'},
        {field:  'message', header:'Message'},
        {field:  'severity', header:'Severity'},
        {field:  'timeStamp', header:'TimeStamp'}
    ]

    protected logs : Log[] = [];
    protected numberOfLogs : number;

    protected groupDropdownOptions : SelectItem[] = [];
    protected jobNameDropdownOptions : SelectItem[] = [];
    protected severityDropdownOptions : SelectItem[] = [];
    
    protected groupKeyFilter : string = null;
    protected jobKeyFilter : string = null;
    protected jobDescriptionFilter : string = null;
    protected severityFilter : string = null;
    protected timeStampDateRangeFilter : DateRange;

    protected logFilterRequest : LogFilterRequest =
    {   
        groupKey: null,
        jobKey: null,
        fromDate: null,
        toDate: null,
        severity: null,
        description: null,
        paginationParams: {
            pageNumber: null,
            offset: null
        }
    };
    
    ngOnInit(): void {
        this.logservice.getAllGroupNames()
            .then(groupNames => {
                this.groupDropdownOptions = 
                    [{label: "Select a group", value: null}, ... groupNames.map(x => ({label: x, value: x}))];
                
            });
        
        this.jobNameDropdownOptions.push({label:"Select a job", value: null});

        this.logservice.getAllLogSeverityLevels()
            .then(severityLevels => {
                this.severityDropdownOptions =
                    [{label: "Select severity", value: null}, ...severityLevels.map(x => ({label: x, value: x}))]
            });
    }

    protected loadLogsLazy(event : LazyLoadEvent) : void { 
        this.logFilterRequest.paginationParams = 
            {
                pageNumber:  (event.first / event.rows) + 1,
                offset: 10
            }
        console.log(this.logFilterRequest);
    }
    
}