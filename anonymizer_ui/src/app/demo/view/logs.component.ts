import { OnInit, Component } from '@angular/core';
import { LogService } from '../service/logservice';
import { LogDto } from '../domain/logDto';
import { SelectItem } from 'primeng/api';

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

    protected logs : LogDto[] = [];
    protected numberOfLogs : number;

    protected groupDropdownOptions : SelectItem[] = [];
    protected jobNameDropdownOptions : SelectItem[] = [];
    protected severityDropdownOptions : SelectItem[] = [];
    protected jobDescriptionFilter : string = null;
    
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
            })
        
    }
    
}