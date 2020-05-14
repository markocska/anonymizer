import { ScrambledColumnConfig } from './scrambledColumnConfig';
import { ConstantColumnConfig } from './constantColumnConfig';
import { PairedColumnsOutsideTableConfig } from './pairedColumnsOutsideTableConfig';

export interface TableConfig {
    fullTableName : string;
    where : string;
    scrambledColumns : Array<ScrambledColumnConfig>;
    constantColumns : Array<ConstantColumnConfig>;
    pairedColumnsInsideTable : Array<Array<string>>;
    pairedColumnsOutsideTable : Array<PairedColumnsOutsideTableConfig>;
}