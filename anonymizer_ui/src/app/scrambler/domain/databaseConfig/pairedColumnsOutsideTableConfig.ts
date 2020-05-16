import { SourceDestMappingStepConfig } from './sourceDestMappingStepConfig';

export interface PairedColumnsOutsideTableConfig {
    columnMapping : Array<Array<string>>;
    sourceDestMapping : Array<SourceDestMappingStepConfig>;
}