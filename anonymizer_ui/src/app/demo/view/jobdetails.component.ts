import { Component, OnInit } from '@angular/core';
import {trigger, state, style, transition, animate} from '@angular/animations'
import { JobDescription } from '../domain/jobDescription';
import { JobService } from '../service/jobservice';
import { SelectItem, LazyLoadEvent, ConfirmationService, Message } from 'primeng/api';
import { BooleanSelectItems } from '../utilities/booleanSelectItems';
import { TriggerDescription } from '../domain/triggerDescription';
import { HttpErrorResponse } from '@angular/common/http';
import {MessagesModule} from 'primeng/messages';
import {MessageModule} from 'primeng/message';
import { TriggerService } from '../service/triggerService';



@Component({
    selector: 'jobdetails',
    styleUrls:['./jobdetails.component.css'],
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

    constructor(private jobService : JobService, private triggerService : TriggerService, private confirmationService: ConfirmationService) {

    }

    protected messages: any[] = [];

    protected jobCols: any[] = [
        {field:  'jobName', header:'Job Name'},
        {field:  'jobGroup', header:'Job Group Name'},
        {field:  'requestRecovery', header:'Request Recovery'},
        {field:  'description', header:'Description'},
        {field:  'isDurable', header:'Is Durable'},
        {field:  'operation', header:'Operation'}
    ];
    protected jobDescriptions: JobDescription[] = [];
    protected jobDescriptionsWithoutFilter: JobDescription[] = [];
    protected totalNumberOfJobs: number;

    protected triggerCols: any[] = [
        {field:  'triggerName', header:'Trigger Name'},
        {field:  'triggerGroup', header:'Trigger Group Name'},
        {field:  'description', header:'Description'},
        {field:  'calendarName', header:'Calendar Name'},
        {field:  'cronExpression', header:'Cron Expression'},
        {field: 'operation', header: 'Operation'}
    ];
    protected triggers: TriggerDescription[] = [];

    protected isDurableOptions: SelectItem[] = BooleanSelectItems.booleanSelectItems;
    protected isDurableSelected: string;

    protected requestRecoveryOptions: SelectItem[] = BooleanSelectItems.booleanSelectItems;
    protected requestRecoverySelected: string;

    protected filterCall : (jobFilter: string ) => void = (jobFilter) => {
         this.jobService.getJobDescriptionsWithFilter(this.jobGroupFilter)
            .then(jobDescriptionsReportResponse => {
                this.jobDescriptions = jobDescriptionsReportResponse.jobDescriptions;
                this.totalNumberOfJobs = jobDescriptionsReportResponse.totalNumber;
            });
        this.filterInProgress = false;
    }
    protected filterInProgress : boolean = false;
    protected jobGroupFilter : string;


    ngOnInit() {
        this.jobService.getAllJobDescriptions()
            .then(jobDescriptionsReportResponse => {
                this.jobDescriptions = jobDescriptionsReportResponse.jobDescriptions; 
                this.jobDescriptionsWithoutFilter = jobDescriptionsReportResponse.jobDescriptions;
                this.totalNumberOfJobs = jobDescriptionsReportResponse.totalNumber;
            });
    }

    loadJobsLazy(event: LazyLoadEvent) {

        this.jobService.getAllJobDescriptions()
            .then(jobDescriptionsReportResponse => {
                this.jobDescriptions = jobDescriptionsReportResponse.jobDescriptions;
                this.jobDescriptionsWithoutFilter = jobDescriptionsReportResponse.jobDescriptions;
                this.totalNumberOfJobs = jobDescriptionsReportResponse.totalNumber;
            });
        
    }

    expandRow(event: any) {
        this.triggers = this.jobDescriptions.filter(x => x.id === event.data.id)[0].triggers;
    }

    filter(value, field, mode) {
        if (field === 'jobGroup') {
            this.jobGroupFilter = value;
            if (this.filterInProgress === false) {
                this.filterInProgress = true;
                setTimeout(() => {
                    this.filterCall(this.jobGroupFilter);
                }, 1000);
        
            }
        }
        else {
            if (this.filterInProgress === false) {
                this.filterInProgress = true;
                setTimeout(() => {
                    this.jobDescriptions = this.jobDescriptionsWithoutFilter.filter(element => element[field].toString().includes(value));
                    this.filterInProgress = false;
                }, 1000);
            }
        }
        
        // this.dpcReportRequest[field] = value;
        // if (this.selectedStatus != null) {
        //     this.dpcReportRequest['status'] = this.selectedStatus.code;
        // }
        // if (this.selectedSession != null) {
        //     this.dpcReportRequest['sessionName'] = this.selectedSession;
        // }

        // this.reportsService.getRequests(this.dpcReportRequest);
    }

   

    deleteJob(event: any, rowData: JobDescription) {
        this.confirmationService.confirm({
            message: `Are you sure you wish to delete the following job? <br/>  <strong> Group key: </strong> ${rowData.jobGroup} <br/> 
                <strong> Job key: </strong> ${rowData.jobName}.`,
            accept: () => {
                this.jobService.deleteJob(rowData.jobGroup, rowData.jobName)
                    .then(() => {
                        this.messages.push({severity:'success', summary:'Success', detail:'Job deleted successfully'});
                        this.removeDeletedJobFromList(rowData.jobGroup, rowData.jobName);
                    })
                    .catch((error: HttpErrorResponse) => this.messages.push({severity:'error', summary:'Error', 
                        detail:`Error while deleting the job. ${typeof error.error === 'string' ? 'Message: ' + error.error : ''}`}))
            }
        });
    }

    deleteTrigger(event: any, rowData: TriggerDescription, jobData: JobDescription) {
        this.confirmationService.confirm({
            message: `Are you sure you wish to delete the following trigger? <br/>  <strong> Group key: </strong> ${rowData.triggerGroup} <br/> 
                <strong> Trigger key: </strong> ${rowData.triggerName}.`,
            accept: () => {
                this.triggerService.deleteTrigger(rowData.triggerGroup, rowData.triggerName)
                    .then(() => {
                        this.messages.push({severity:'success', summary:'Success', detail:'Trigger deleted successfully'});
                        this.removeDeletedTriggerFromList(rowData.triggerGroup, rowData.triggerName, jobData);
                    })
                    .catch((error: HttpErrorResponse) => this.messages.push({severity:'error', summary:'Error', 
                        detail:`Error while deleting the trigger. ${typeof error.error === 'string' ? 'Message: ' + error.error : ''}`}))
            }
        });
    }

    removeDeletedJobFromList(jobGroup: string, jobKey: string) {
        this.jobDescriptionsWithoutFilter = this.jobDescriptionsWithoutFilter.filter(job => !(job.jobGroup === jobGroup && job.jobName === jobKey));
        this.jobDescriptions = this.jobDescriptions.filter(job => !(job.jobGroup === jobGroup && job.jobName === jobKey));
    }

    removeDeletedTriggerFromList(triggerGroup: string, triggerKey: string, jobData: JobDescription) {
        this.triggers = this.triggers.filter(trigger => !(trigger.triggerGroup === triggerGroup && trigger.triggerName === triggerKey));
        var jobToRemoveTriggerOf = this.jobDescriptionsWithoutFilter.filter(job => job.jobGroup === jobData.jobGroup && job.jobName === jobData.jobName)[0];
        jobToRemoveTriggerOf.triggers = jobToRemoveTriggerOf.triggers.filter(trigger => !(trigger.triggerGroup === triggerGroup && trigger.triggerName === triggerKey));
    }
}