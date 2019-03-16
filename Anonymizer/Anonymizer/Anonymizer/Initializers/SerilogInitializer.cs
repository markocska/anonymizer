using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Configuration;

namespace Anonymizer.Initializers
{
    /// <summary>
    /// Serilog initializer class
    /// </summary>
    public class SerilogInitializer
    {
        /// <summary>
        /// APPSETTINGS Serilog MSSQL ConnectionString key 
        /// </summary>
        public const string SERILOG_MSSQL_CONNECTION_STRING = "serilog:write-to:MSSqlServer.connectionString";
        /// <summary>
        /// APPSETTINGS Serilog MSSQL TableName key
        /// </summary>
        public const string SERILOG_MSSQL_TABLE_NAME = "serilog:write-to:MSSqlServer.tableName";
        /// <summary>
        /// APPSETTINGS Serilog MSSQL Logging level key
        /// </summary>
        public const string SERILOG_MSSQL_LEVEL = "serilog:write-to:MSSqlServer.restrictedToMinimumLevel";

        public const string SERILOG_EMAIL_CONNECTION_INFO = "";

        /// <summary>
        /// Serilog MSSQL connection string
        /// </summary>
        public static string MssqlConnectionString => ConfigurationManager.AppSettings[SERILOG_MSSQL_CONNECTION_STRING] ?? ConfigurationManager.ConnectionStrings[1]?.Name;

        /// <summary>
        /// Serilog MSSQL Table name
        /// </summary>
        public static string MssqlTableName => ConfigurationManager.AppSettings[SERILOG_MSSQL_TABLE_NAME] ?? "SystemLogs";
        
        /// <summary>
        /// Serilog MSSQL log level
        /// </summary>
        public static LogEventLevel MssqlLogLevel => (LogEventLevel)Enum.Parse(typeof(LogEventLevel), ConfigurationManager.AppSettings[SERILOG_MSSQL_LEVEL] ?? "Verbose", true);

        /// <summary>
        /// Process GUID
        /// </summary>
        public static readonly Guid ProcessGUID = Guid.NewGuid();

        /// <summary>
        /// Register serilog - To initialize LOGGER, you shall call SerilogInitializer.Register()
        /// </summary>
        public static void Register()
        {
            var configurations = new LoggerConfiguration()
                                      .ReadFrom.AppSettings()
                                      .Enrich.FromLogContext()
                                      .Enrich.WithThreadId()
                                      .WriteTo.Console(LogEventLevel.Verbose,
                                        outputTemplate: "{Timestamp:HH:mm:ss} {Level} [{TransactionID}] - ({UserName}): {Message}{NewLine}"
                                      )
                                      .WriteTo.MSSqlServer(MssqlConnectionString, MssqlTableName,
                                            autoCreateSqlTable: true,
                                            restrictedToMinimumLevel: MssqlLogLevel,
                                            columnOptions: GetColumnOptions(),
                                            schemaName: "dbo",
                                            batchPostingLimit: 10000
                                      )
                                      .CreateLogger();

            Serilog.Context.LogContext.PushProperty("UserName", "ServiceCore");
            Serilog.Context.LogContext.PushProperty("TransactionID", Guid.NewGuid());
            Serilog.Context.LogContext.PushProperty("InstanceID", ProcessGUID);

            Log.Logger = configurations;
        }

        private static ColumnOptions GetColumnOptions()
        {
            var columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);

            return columnOptions;

        }
    }
}