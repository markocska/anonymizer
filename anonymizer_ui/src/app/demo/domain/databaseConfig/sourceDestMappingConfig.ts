export interface SourceDestMappingStepConfig {
    destinationConnectionString : string;
    destinationFullTableName : string;
    destinationLinkedInstance : string;
    foreignKeyMapping : Array<Array<string>>;
}