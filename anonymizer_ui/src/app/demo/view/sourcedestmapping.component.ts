import {Component, OnInit, Input} from '@angular/core';
import { TableConfig } from '../domain/databaseConfig/tableConfig';
import { ConstantColumnConfig } from '../domain/databaseConfig/constantColumnConfig';
import { PairedColumnsOutsideTableConfig } from '../domain/databaseConfig/pairedColumnsOutsideTableConfig';
import { SourceDestMappingStepConfig } from '../domain/databaseConfig/sourceDestMappingStepConfig';

@Component({
    selector:'sourcedest-mapping',
    templateUrl: './sourcedestmapping.component.html'
})
export class SourceDestMappingComponent implements OnInit {
    displayDialog: boolean;

    sourceDestMappingStepToSave : SourceDestMappingStepConfig = {
        destinationConnectionString: null,
        destinationFullTableName: null,
        destinationLinkedInstance: null,
        foreignKeyMapping: []
    };

    selectedSourceDestMappingStep : SourceDestMappingStepConfig;

    newSelectedSourceDestMappingStep : boolean;

    @Input()
    pairedColumnsOutsideConfig: PairedColumnsOutsideTableConfig;

    cols: any[];

    ngOnInit() {
        this.cols = [
            {field:'destinationConnectionString', header:'Destination Connection String'},
            {field:'destinationFullTableName', header:'Destination Full Table Name'},
            {field:'destinationLinkedInstance', header:'Destination Linked Instance'},
            {field:'foreignKeyMapping', header:'Foreign Key Mapping'}
        ];
    }

    showDialogToAdd() {
        this.newSelectedSourceDestMappingStep = true;
        this.sourceDestMappingStepToSave = {
            destinationConnectionString: null,
            destinationFullTableName: null,
            destinationLinkedInstance: null,
            foreignKeyMapping: []
        }
        this.displayDialog = true;
    }

    AddNewSourceDestMapping() {

        this.pairedColumnsOutsideConfig.sourceDestMapping.push({
            destinationConnectionString: null,
            destinationFullTableName: null,
            destinationLinkedInstance: null,
            foreignKeyMapping: []
        });
    }

    delete() {
        let index = this.pairedColumnsOutsideConfig.sourceDestMapping.indexOf(this.selectedSourceDestMappingStep);
        this.pairedColumnsOutsideConfig.sourceDestMapping = this.pairedColumnsOutsideConfig.sourceDestMapping.filter((val,i) => i != index);
    }

    onRowSelect(event) {
        // this.newSelectedSourceDestMappingStep = false;
        // this.sourceDestMappingStepToSave = this.clone(event.data);
        // this.displayDialog = true;
    }

    // clone(sourceDestMapping: SourceDestMappingStepConfig) : SourceDestMappingStepConfig {
    //     let sourceDestMappingClone : SourceDestMappingStepConfig = {
    //         destinationConnectionString: null,
    //         destinationFullTableName: null,
    //         destinationLinkedInstance: null,
    //         foreignKeyMapping: []
    //     };

    //     for (let prop in sourceDestMapping) {
    //         if (prop !== 'foreignKeyMapping') {
    //             sourceDestMappingClone[prop] = sourceDestMapping[prop]; 
    //         }
    //         else {
    //             sourceDestMappingClone[prop] = this.cloneStringArray(sourceDestMapping.foreignKeyMapping);
    //         }
    //     }

    //     return constantColumnConfigClone;
    // }

    // cloneStringArray(c: string[]) : string[] {
    //     let stringArrayCopy = [];

    //     for (let elem of c) {
    //         stringArrayCopy.push(elem); 
    //     }

    //     return stringArrayCopy
    // }
}
