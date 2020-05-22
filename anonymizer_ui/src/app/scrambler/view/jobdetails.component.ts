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
import { CreateTrigger } from '../domain/createTrigger';
import { TriggerSuccessfullyCreated } from '../domain/triggerSuccessfullyCrested';



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

    public messages: any[] = [];

    public jobCols: any[] = [
        {field:  'jobName', header:'Job Name'},
        {field:  'jobGroup', header:'Job Group Name'},
        {field:  'requestRecovery', header:'Request Recovery'},
        {field:  'description', header:'Description'},
        {field:  'isDurable', header:'Is Durable'},
        {field:  'operation', header:'Operation'}
    ];
    public jobDescriptions: JobDescription[] = [];
    public jobDescriptionsWithoutFilter: JobDescription[] = [];
    public totalNumberOfJobs: number;

    public triggerCols: any[] = [
        {field:  'triggerName', header:'Trigger Name'},
        {field:  'triggerGroup', header:'Trigger Group Name'},
        {field:  'description', header:'Description'},
        {field:  'calendarName', header:'Calendar Name'},
        {field:  'cronExpression', header:'Cron Expression'},
        {field: 'operation', header: 'Operation'}
    ];
    public triggers: TriggerDescription[] = [];

    public isDurableOptions: SelectItem[] = BooleanSelectItems.booleanSelectItems;
    public isDurableSelected: string;

    public requestRecoveryOptions: SelectItem[] = BooleanSelectItems.booleanSelectItems;
    public requestRecoverySelected: string;

    public filterCall : (jobFilter: string ) => void = (jobFilter) => {
         this.jobService.getJobDescriptionsWithFilter(this.jobGroupFilter)
            .then(jobDescriptionsReportResponse => {
                this.jobDescriptions = jobDescriptionsReportResponse.jobDescriptions;
                this.totalNumberOfJobs = jobDescriptionsReportResponse.totalNumber;
            });
        this.filterInProgress = false;
    }
    public filterInProgress : boolean = false;
    public jobGroupFilter : string;
    

    public displayCreateTriggerDialog : boolean = false;
    public triggerToCreate : CreateTrigger = {jobName :"", jobGroup: "", triggerDescription: "", cronExpression: ""};
    public expandedJob : JobDescription;

    ngOnInit() {
        this.jobService.getAllJobDescriptions()
            .then(jobDescriptionsReportResponse => {
                this.jobDescriptions = jobDescriptionsReportResponse.jobDescriptions; 
                this.jobDescriptionsWithoutFilter = jobDescriptionsReportResponse.jobDescriptions;
                this.totalNumberOfJobs = jobDescriptionsReportResponse.totalNumber;
            });
    }

    public loadJobsLazy(event: LazyLoadEvent) : void {

        this.jobService.getAllJobDescriptions()
            .then(jobDescriptionsReportResponse => {
                this.jobDescriptions = jobDescriptionsReportResponse.jobDescriptions;
                this.jobDescriptionsWithoutFilter = jobDescriptionsReportResponse.jobDescriptions;
                this.totalNumberOfJobs = jobDescriptionsReportResponse.totalNumber;
            });
        
    }

    public expandRow(event: any) : void {
        this.expandedJob = this.jobDescriptions.filter(x => x.id === event.data.id)[0];
        this.triggers = this.expandedJob.triggers;
    }

    public filter(value, field, mode) : void {
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

   

    public deleteJob(event: any, rowData: JobDescription) : void {
        this.confirmationService.confirm({
            message: `Are you sure you wish to delete the following job? <br/>  <strong> Group key: </strong> ${rowData.jobGroup} <br/> 
                <strong> Job key: </strong> ${rowData.jobName}.`,
            accept: () => {
this.jobService.deleteJob(rowData.jobGroup, rowData.jobName)
    .then(() => {
        this.messages.push({
            severity:'success', 
            summary:'Success', 
            detail:'Job deleted successfully'});
        this.removeDeletedJobFromList(rowData.jobGroup, rowData.jobName);
    })
    .catch((error: HttpErrorResponse) => 
        this.messages.push({
            severity:'error',
            summary:'Error', 
            detail:`Error while deleting the job. 
            ${typeof error.error === 'string' ? 
                'Message: ' + error.error : ''}`}))
            }
        });
    }

    public deleteTrigger(event: any, rowData: TriggerDescription, jobData: JobDescription) : void {
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

    public removeDeletedJobFromList(jobGroup: string, jobKey: string) : void {
        this.jobDescriptionsWithoutFilter = this.jobDescriptionsWithoutFilter.filter(job => !(job.jobGroup === jobGroup && job.jobName === jobKey));
        this.jobDescriptions = this.jobDescriptions.filter(job => !(job.jobGroup === jobGroup && job.jobName === jobKey));
    }

    public removeDeletedTriggerFromList(triggerGroup: string, triggerKey: string, jobData: JobDescription) : void {
        this.triggers = this.triggers.filter(trigger => !(trigger.triggerGroup === triggerGroup && trigger.triggerName === triggerKey));
        var jobToRemoveTriggerOf = this.jobDescriptionsWithoutFilter.filter(job => job.jobGroup === jobData.jobGroup && job.jobName === jobData.jobName)[0];
        jobToRemoveTriggerOf.triggers = jobToRemoveTriggerOf.triggers.filter(trigger => !(trigger.triggerGroup === triggerGroup && trigger.triggerName === triggerKey));
        if (jobToRemoveTriggerOf.triggers.length === 0) {
            this.removeDeletedJobFromList(jobToRemoveTriggerOf.jobGroup, jobToRemoveTriggerOf.jobName);
        }
    }

    public showDialogToAddTrigger() : void {
        this.triggerToCreate.jobGroup = this.expandedJob.jobGroup;
        this.triggerToCreate.jobName = this.expandedJob.jobName;
        this.displayCreateTriggerDialog = true;
    }

    public saveTrigger() : void {
        this.triggerService.createTrigger(this.triggerToCreate)
            .then((successResponse : TriggerSuccessfullyCreated) => {
                this.messages.push({severity: 'success', summary:'Success', detail: 'Trigger created successfully'});
                this.addCreatedTriggerToList(successResponse);
            })
            .catch((error: HttpErrorResponse) => this.messages.push({severity:'error', summary:'Error', 
                detail:`Error while creating the trigger. ${typeof error.error === 'string' ? 'Message ' + error.error : '' }`}))
    }

    public addCreatedTriggerToList(createdTrigger: TriggerSuccessfullyCreated) : void {
        let newTrigger = {id: createdTrigger.id, triggerGroup : createdTrigger.triggerGroup, triggerName: createdTrigger.triggerName,
            description: createdTrigger.triggerDescription, cronExpression: createdTrigger.cronExpression, calendarName: createdTrigger.calendar};

        this.triggers.push(newTrigger);

        // this.jobDescriptionsWithoutFilter.filter(job => job.jobGroup === createdTrigger.jobGroup && job.jobName === createdTrigger.jobName)[0]
        //     .triggers.push(newTrigger);

        this.cancelCreateTriggerDialog();
    }

    public cancelCreateTriggerDialog() : void {
        this.displayCreateTriggerDialog = false;
    }
}