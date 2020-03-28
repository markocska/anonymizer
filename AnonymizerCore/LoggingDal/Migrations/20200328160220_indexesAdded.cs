using Microsoft.EntityFrameworkCore.Migrations;

namespace LoggingDal.Migrations
{
    public partial class indexesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Severity",
                table: "Logs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Logs_GroupKey_JobKey_Timestamp",
                table: "Logs",
                columns: new[] { "GroupKey", "JobKey", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_GroupKey_JobKey_Severity_Timestamp",
                table: "Logs",
                columns: new[] { "GroupKey", "JobKey", "Severity", "Timestamp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_Logs_GroupKey_JobKey_Timestamp",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_GroupKey_JobKey_Severity_Timestamp",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "Severity",
                table: "Logs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
