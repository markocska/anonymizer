import { Component, OnInit } from '@angular/core';
import {trigger, state, style, transition, animate} from '@angular/animations'
import { JobDescription } from '../domain/jobDescription';
import { JobService } from '../service/jobservice';
import { SelectItem, LazyLoadEvent } from 'primeng/api';
import { BooleanSelectItems } from '../utilities/booleanSelectItems';




@Component({
    selector: 'jobdetails',
    templateUrl: './jobdetails.component.html',
    animations: [
        trigger('rowExpansionTrigger', [
            state('void', style({
                transform: 'translateX(-10%)',
                opacity: 0
            })),
            state('active', style({
                transform: 'translateX(0)',
                opacity: 1
            })),
            transition('* <=> *', animate('400ms cubic-bezier(0.86, 0, 0.07, 1)'))
        ])
    ]
})
export class JobDetailsComponent implements OnInit {
    
    protected jobCols: any[] = [
        {field:  'id', header:'Id'},
        {field:  'jobName', header:'Job Name'},
        {field:  'jobGroup', header:'Job Group Name'},
        {field:  'requestRecovery', header:'Request Recovery'},
        {field:  'description', header:'Description'},
        {field:  'isDurable', header:'Is Durable'}
    ];
    protected jobDescriptions: JobDescription[] = [];
    protected totalNumberOfJobs: number;

    protected triggerCols: any[] = [];
    protected triggers: any[] = [];

    protected isDurableOptions: SelectItem[] = BooleanSelectItems.booleanSelectItems;
    protected isDurableSelected: string;

    protected requestRecoveryOptions: SelectItem[] = BooleanSelectItems.booleanSelectItems;
    protected requestRecoverySelected: string;

    constructor(private jobService : JobService) {

    }

    ngOnInit() {
        this.jobService.getAllJobDescriptions()
            .then(jobDescriptionsReportResponse => {
                this.jobDescriptions = jobDescriptionsReportResponse.jobDescriptions; 
                this.totalNumberOfJobs = jobDescriptionsReportResponse.totalNumber;
            });
            
    }

    loadJobsLazy(event: LazyLoadEvent) {

        this.jobService.getAllJobDescriptions()
            .then(jobDescriptionsReportResponse => {
                this.jobDescriptions = jobDescriptionsReportResponse.jobDescriptions;
                this.totalNumberOfJobs = jobDescriptionsReportResponse.totalNumber;
            });
        
    }

    expandRow(event: any) {
        console.log(this.jobDescriptions);
    }

    filter(value, field, mode) {

        // this.dpcReportRequest[field] = value;
        // if (this.selectedStatus != null) {
        //     this.dpcReportRequest['status'] = this.selectedStatus.code;
        // }
        // if (this.selectedSession != null) {
        //     this.dpcReportRequest['sessionName'] = this.selectedSession;
        // }

        // this.reportsService.getRequests(this.dpcReportRequest);
    }
}