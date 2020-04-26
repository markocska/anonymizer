using Microsoft.EntityFrameworkCore.Migrations;

namespace LoggingDal.Migrations
{
    public partial class AddIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_Severity",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Timestamp",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_GroupKey_JobKey",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_GroupKey_JobKey_Severity",
                table: "Logs");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_JobDescription_Timestamp",
                table: "Logs",
                columns: new[] { "JobDescription", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Severity_Timestamp",
                table: "Logs",
                columns: new[] { "Severity", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Timestamp_Severity",
                table: "Logs",
                columns: new[] { "Timestamp", "Severity" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_JobDescription_GroupKey_Timestamp",
                table: "Logs",
                columns: new[] { "JobDescription", "GroupKey", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_JobDescription_Severity_Timestamp",
                table: "Logs",
                columns: new[] { "JobDescription", "Severity", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_JobDescription_GroupKey_Severity_Timestamp",
                table: "Logs",
                columns: new[] { "JobDescription", "GroupKey", "Severity", "Timestamp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_JobDescription_Timestamp",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Severity_Timestamp",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Timestamp_Severity",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_JobDescription_GroupKey_Timestamp",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_JobDescription_Severity_Timestamp",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_JobDescription_GroupKey_Severity_Timestamp",
                table: "Logs");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Severity",
                table: "Logs",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Timestamp",
                table: "Logs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_GroupKey_JobKey",
                table: "Logs",
                columns: new[] { "GroupKey", "JobKey" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_GroupKey_JobKey_Severity",
                table: "Logs",
                columns: new[] { "GroupKey", "JobKey", "Severity" });
        }
    }
}
