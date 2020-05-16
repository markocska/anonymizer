import { DatabasesConfig } from '../domain/databaseConfig/databasesConfig';

export class DatabaseConfigInitializerService {
    
    
    public initializeDatabasesConfig() : DatabasesConfig {
        return {
            databases: [
                {
                    connectionString: null,
                    version: null,
                    tables: [{
                        fullTableName: null,
                        constantColumns: [],
                        pairedColumnsInsideTable: [],
                        pairedColumnsOutsideTable: [
                            {
                                columnMapping: [],
                                sourceDestMapping: [],
                            },
                            {
                                columnMapping: [],
                                sourceDestMapping: []
                            }
                        ],
                        scrambledColumns: [],
                        where: null
                    },
                    {
                        fullTableName: null,
                        constantColumns: [],
                        pairedColumnsInsideTable: [],
                        pairedColumnsOutsideTable: [
                            {
                                columnMapping: [],
                                sourceDestMapping: []
                            },
                            {
                                columnMapping: [],
                                sourceDestMapping: []
                            }
                        ],
                        scrambledColumns: [],
                        where: null
                    }
                ]
                },
                {
                    connectionString: null,
                    version: null,
                    tables: [{
                        fullTableName: null,
                        constantColumns: [],
                        pairedColumnsInsideTable: [],
                        pairedColumnsOutsideTable: [
                            {
                                columnMapping: [],
                                sourceDestMapping: []
                            },
                            {
                                columnMapping: [],
                                sourceDestMapping: []
                            }
                        ],
                        scrambledColumns: [],
                        where: null
                    },
                    {
                        fullTableName: null,
                        constantColumns: [],
                        pairedColumnsInsideTable: [],
                        pairedColumnsOutsideTable: [
                            {
                                columnMapping: [],
                                sourceDestMapping: []
                            },
                            {
                                columnMapping: [],
                                sourceDestMapping: []
                            }
                        ],
                        scrambledColumns: [],
                        where: null
                    }]
                }
            ]
        }
    }
}