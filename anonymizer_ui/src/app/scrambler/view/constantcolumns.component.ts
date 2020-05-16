import {Component, OnInit, Input} from '@angular/core';
import { TableConfig } from '../domain/databaseConfig/tableConfig';
import { ConstantColumnConfig } from '../domain/databaseConfig/constantColumnConfig';

@Component({
    selector:'constant-columns',
    templateUrl: './constantcolumns.component.html'
})
export class ConstantColumnsComponent implements OnInit {
    displayDialog: boolean;

    constantColumnConfigToSave : ConstantColumnConfig = {name: null, value: null};

    selectedConstantColumnConfig : ConstantColumnConfig;

    newConstantColumnConfig : boolean;

    @Input()
    tableConfig: TableConfig;

    cols: any[];

    ngOnInit() {
        this.cols = [
            {field:'name', header:'Name'},
            {field:'value', header:'Value'}
        ];
    }

    showDialogToAdd() {
        this.newConstantColumnConfig = true;
        this.constantColumnConfigToSave = {name: null, value:null};
        this.displayDialog = true;
    }

    save() {

        if (this.newConstantColumnConfig){
            this.tableConfig.constantColumns.push(this.constantColumnConfigToSave);
        }
        else {
            this.tableConfig.constantColumns[this.tableConfig.constantColumns.indexOf(this.selectedConstantColumnConfig)] = this.constantColumnConfigToSave;
        }

        this.constantColumnConfigToSave = null;
        this.displayDialog = false;
    }

    delete() {
        let index = this.tableConfig.constantColumns.indexOf(this.selectedConstantColumnConfig);
        this.tableConfig.constantColumns = this.tableConfig.constantColumns.filter((val,i) => i != index);
        this.constantColumnConfigToSave = null;
        this.displayDialog = false;
    }

    onRowSelect(event) {
        this.newConstantColumnConfig = false;
        this.constantColumnConfigToSave = this.clone(event.data);
        this.displayDialog = true;
    }

    clone(c: ConstantColumnConfig) : ConstantColumnConfig {
        let constantColumnConfigClone = {name: null, value:null};

        for (let prop in c) {
            constantColumnConfigClone[prop] = c[prop]; 
        }

        return constantColumnConfigClone;
    }
}
