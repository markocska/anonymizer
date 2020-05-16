import {Component, OnInit, Input} from '@angular/core';
import { TableConfig } from '../domain/databaseConfig/tableConfig';
import { ConstantColumnConfig } from '../domain/databaseConfig/constantColumnConfig';
import { PairedColumnsOutsideTableConfig } from '../domain/databaseConfig/pairedColumnsOutsideTableConfig';
import { SourceDestMappingStepConfig } from '../domain/databaseConfig/sourceDestMappingStepConfig';

@Component({
    selector:'foreignkey-mapping',
    templateUrl: './foreignkeymapping.component.html'
})
export class ForeignKeyMappingComponent implements OnInit {
    selectedMappedColumns : string[];

    @Input()
    sourceDestMappingConfig: SourceDestMappingStepConfig;

    cols: any[];

    ngOnInit() {
        this.cols = [
            {field:'sourceColumn', header:'Source Column'},
            {field:'destColumn', header:'Destination Column'}
        ];
    }


    addNewForeignKeyMapping() {
        this.sourceDestMappingConfig.foreignKeyMapping.push([

        ]);
        
    }

    delete() {
        let index = this.sourceDestMappingConfig.foreignKeyMapping.indexOf(this.selectedMappedColumns);
        this.sourceDestMappingConfig.foreignKeyMapping = this.sourceDestMappingConfig.foreignKeyMapping.filter((val,i) => i != index);
    }

    onRowSelect(event) {
        // this.newMappedColumnsConfig = false;
        // this.mappedColumnsConfigToSave = this.clone(event.data);
        // this.displayDialog = true;     console.log(event);
   
    }

    clone(c: string[]) : string[] {
        let stringArrayCopy = [];

        for (let elem of c) {
            stringArrayCopy.push(elem); 
        }

        return stringArrayCopy
    }
}
