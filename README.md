# Database Scrambler

A database scrambler software that can dynamically generate SQL code to scramble table rows of MySQL and MSSQL databases, with a UI for creating, configuring, scheduling and monitoring scrambling jobs.

The easiest way to run the project is by using the allInBundle.zip compressed folder, which already contains the compiled .NET Core backend and also the compiled, bundled Angular UI.

#### Prerequisites for running the application:

* .NET Core 2.2 runtime
* .NET Core CLI
* SQL Server 2012 or newer version


## Steps to run and try out the application using the allInBundle.zip comporessed folder

1. Download the allInBundle.zip file, either by cloning the project, or by downloading [directly](https://github.com/markocska/anonymizer/blob/master/allInBundle.zip).

2. After extracting the folder, run the **1_tables_Quartz_sqlServer.sql** and **2_tables_Serilog_sqlServer.sql** scripts located inside the sqlToRun folder, on the SQL Server instance, which you intend to use for the application.

3. If you wish to try the software using predefined test data, run the **3_createTestDataSqlServer.sql** on your SQL Server instance and/or the **4_createTestDataMySql.sql** scripts on your MySQL instance.

4. In the prodbuild/appsettings.json file, under the Serilog configs, change the **data source** value of the "connectionString" config parameter, so that the data source points to your desired SQL Server instance.  

5. In the prodbuild/quartz.config.json file, change the **Server** value of the "quartz.dataSource.ds.connectionString" config parameter, so that it points to your desired SQL Server instance

6. Run "**dotnet Scrambler.Api.dll**" for the Scrambler.Api.dll file, which is located inside prodbuild folder. 

7. You should be able to reach the UI now at the URL: \<application root url\>/index.html which is usually https://localhost:5001/index.html

## Notes
* On the UI, the full table name for SQL Server tables should be in format "Database name.Schema name.Table name" , while "Schema name.Table name" for MySQL tables
* When writing column or database names on the UI, you don't have to use brackets or apostrophes.
