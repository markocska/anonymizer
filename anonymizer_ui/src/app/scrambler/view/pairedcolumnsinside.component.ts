import {Component, OnInit, Input, OnChanges} from '@angular/core';
import { TableConfig } from '../domain/databaseConfig/tableConfig';
import { ConstantColumnConfig } from '../domain/databaseConfig/constantColumnConfig';
import { SelectItem } from 'primeng/components/common/api';
import { ThrowStmt } from '@angular/compiler';

@Component({
    selector:'pairedcolumns-inside',
    templateUrl: './pairedcolumnsinside.component.html'
})
export class PairedColumnsInsideComponent implements OnInit, OnChanges {
    displayDialog: boolean;

    scrambledColumns: SelectItem[];
    selectedScrambledColumns : string[];


    selectedPairedColumnsInside : string[];

    pairedColumnsInsideTableValues : string[] = [];


    @Input()
    tableConfig: TableConfig;

    cols: any[];

    ngOnInit() {
        this.cols = [
            {field:'name', header:'Column names'},
        ];

        
    }

    getScrambledColumns() : SelectItem[] {
        var scrambledColumnList = this.tableConfig.scrambledColumns.map((value,index) => {
            return {label:value.name, value:value.name}
        });
        return scrambledColumnList;
    }

    ngOnChanges(changes) {
        console.log(changes);
    }


    saveNewPairedColumns() {
        if ((!this.selectedScrambledColumns) || (this.selectedScrambledColumns.length === 0)) {
            return;
        }

        this.tableConfig.pairedColumnsInsideTable.push(this.selectedScrambledColumns);
        
        
        this.pairedColumnsInsideTableValues.push(this.selectedScrambledColumns.join());

        this.selectedScrambledColumns = [];
    }

    delete() {
        let indexToRemove= this.tableConfig.pairedColumnsInsideTable.indexOf(this.selectedPairedColumnsInside);
        this.tableConfig.pairedColumnsInsideTable = this.tableConfig.pairedColumnsInsideTable.filter((value, i) => i !== indexToRemove);
    }

}
