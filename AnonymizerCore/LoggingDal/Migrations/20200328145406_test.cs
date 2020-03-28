using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LoggingDal.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Msg = table.Column<string>(nullable: true),
                    Template = table.Column<string>(nullable: true),
                    Severity = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: true),
                    Ex = table.Column<string>(nullable: true),
                    LogEvent = table.Column<string>(nullable: true),
                    JobKey = table.Column<string>(maxLength: 500, nullable: true),
                    GroupKey = table.Column<string>(maxLength: 500, nullable: true),
                    JobDescription = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
