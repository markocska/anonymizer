import {Component, OnInit, Input} from '@angular/core';
import {MenuItem} from 'primeng/primeng';
import { ScrambledColumnConfig } from '../domain/databaseConfig/scrambledColumnConfig';
import { TableConfig } from '../domain/databaseConfig/tableConfig';

@Component({
    selector:'scrambled-columns',
    templateUrl: './scrambledcolumns.component.html'
})
export class ScrambledColumnsComponent implements OnInit {
    displayDialog: boolean;

    scrambledColumnConfigToSave : ScrambledColumnConfig = {name: null};

    selectedScrambledColumnConfig : ScrambledColumnConfig;

    newScrambledColumnConfig : boolean;

    @Input()
    tableConfig: TableConfig;

    cols: any[];

    ngOnInit() {
        this.cols = [
            {field:'name', header:'Name'}
        ];
    }

    showDialogToAdd() {
        this.newScrambledColumnConfig = true;
        this.scrambledColumnConfigToSave = {name: null};
        this.displayDialog = true;
    }

    save() {

        if (this.newScrambledColumnConfig){
            this.tableConfig.scrambledColumns.push(this.scrambledColumnConfigToSave);
        }
        else {
            this.tableConfig.scrambledColumns[this.tableConfig.scrambledColumns.indexOf(this.selectedScrambledColumnConfig)] = this.scrambledColumnConfigToSave;
        }

        this.scrambledColumnConfigToSave = null;
        this.displayDialog = false;
    }

    delete() {
        let index = this.tableConfig.scrambledColumns.indexOf(this.selectedScrambledColumnConfig);
        this.tableConfig.scrambledColumns = this.tableConfig.scrambledColumns.filter((val,i) => i != index);
        this.scrambledColumnConfigToSave = null;
        this.displayDialog = false;
    }

    onRowSelect(event) {
        this.newScrambledColumnConfig = false;
        this.scrambledColumnConfigToSave = this.clone(event.data);
        this.displayDialog = true;
    }

    clone(c: ScrambledColumnConfig) : ScrambledColumnConfig {
        let scrambledColumnConfigClone = {name: null};

        for (let prop in c) {
            scrambledColumnConfigClone[prop] = c[prop]; 
        }

        return scrambledColumnConfigClone;
    }
}
