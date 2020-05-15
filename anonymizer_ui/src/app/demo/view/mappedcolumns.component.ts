import {Component, OnInit, Input} from '@angular/core';
import { TableConfig } from '../domain/databaseConfig/tableConfig';
import { ConstantColumnConfig } from '../domain/databaseConfig/constantColumnConfig';
import { PairedColumnsOutsideTableConfig } from '../domain/databaseConfig/pairedColumnsOutsideTableConfig';

@Component({
    selector:'mapped-columns',
    templateUrl: './mappedcolumns.component.html'
})
export class MappedColumnsComponent implements OnInit {
    displayDialog: boolean;

    mappedColumnsConfigToSave : string[] = [];

    selectedMappedColumns : string[];

    newMappedColumnsConfig : boolean;

    @Input()
    pairedColumnsOutsideConfig: PairedColumnsOutsideTableConfig;

    cols: any[];

    ngOnInit() {
        this.cols = [
            {field:'sourceColumn', header:'Source Column'},
            {field:'destColumn', header:'Destination Column'}
        ];
    }

    showDialogToAdd() {
        this.newMappedColumnsConfig = true;
        this.mappedColumnsConfigToSave = [];
        this.displayDialog = true;
    }

    save() {

        if (this.newMappedColumnsConfig){
            this.pairedColumnsOutsideConfig.columnMapping.push(this.mappedColumnsConfigToSave);
        }
        else {
            this.pairedColumnsOutsideConfig.columnMapping[this.pairedColumnsOutsideConfig.columnMapping.indexOf(this.selectedMappedColumns)] = this.mappedColumnsConfigToSave;
        }

        this.mappedColumnsConfigToSave = null;
        this.displayDialog = false;
    }

    delete() {
        let index = this.pairedColumnsOutsideConfig.columnMapping.indexOf(this.selectedMappedColumns);
        this.pairedColumnsOutsideConfig.columnMapping = this.pairedColumnsOutsideConfig.columnMapping.filter((val,i) => i != index);
        this.mappedColumnsConfigToSave = null;
        this.displayDialog = false;
    }

    onRowSelect(event) {
        this.newMappedColumnsConfig = false;
        // this.mappedColumnsConfigToSave = this.clone(event.data);
        this.displayDialog = true;
    }

    // clone(c: string[]) : string[] {
    //     let constantColumnConfigClone = {name: null, value:null};

    //     for (let prop in c) {
    //         constantColumnConfigClone[prop] = c[prop]; 
    //     }

    //     return constantColumnConfigClone;
    // }
}
