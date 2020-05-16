import { TableConfig } from './tableConfig';

export interface DatabaseConfig {
    version : string;
    connectionString : string;
    tables : Array<TableConfig>;
}