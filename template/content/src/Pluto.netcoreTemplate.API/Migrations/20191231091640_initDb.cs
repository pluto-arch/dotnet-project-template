using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pluto.netcoreTemplate.API.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence(
                name: "userbookseq",
                schema: "dbo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "userseq",
                schema: "dbo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    EditTime = table.Column<DateTime>(nullable: false),
                    Tel = table.Column<string>(maxLength: 20, nullable: true),
                    UserName = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBooks",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    BookName = table.Column<string>(maxLength: 250, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(8, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBooks_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBooks_UserId",
                schema: "dbo",
                table: "UserBooks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBooks",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "userbookseq",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "userseq",
                schema: "dbo");
        }
    }
}
