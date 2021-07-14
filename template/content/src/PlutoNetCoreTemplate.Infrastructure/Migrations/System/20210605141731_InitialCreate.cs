using Microsoft.EntityFrameworkCore.Migrations;

using System;

namespace PlutoNetCoreTemplate.Infrastructure.Migrations.System
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermissionGroupDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroupDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AllowedProviders = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionDefinition_PermissionGroupDefinition_GroupId",
                        column: x => x.GroupId,
                        principalTable: "PermissionGroupDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantConnectionStrings",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantConnectionStrings", x => new { x.TenantId, x.Name });
                    table.ForeignKey(
                        name: "FK_TenantConnectionStrings_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionDefinition_GroupId",
                table: "PermissionDefinition",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionDefinition_Name",
                table: "PermissionDefinition",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupDefinition_Name",
                table: "PermissionGroupDefinition",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionDefinition");

            migrationBuilder.DropTable(
                name: "TenantConnectionStrings");

            migrationBuilder.DropTable(
                name: "PermissionGroupDefinition");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
