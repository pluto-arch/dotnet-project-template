using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pluto.netcoreTemplate.API.Migrations
{
    public partial class initDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EditTime",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 31, 19, 5, 48, 882, DateTimeKind.Local).AddTicks(1088),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 31, 19, 5, 48, 878, DateTimeKind.Local).AddTicks(8908),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EditTime",
                schema: "dbo",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 31, 19, 5, 48, 882, DateTimeKind.Local).AddTicks(1088));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                schema: "dbo",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 31, 19, 5, 48, 878, DateTimeKind.Local).AddTicks(8908));
        }
    }
}
