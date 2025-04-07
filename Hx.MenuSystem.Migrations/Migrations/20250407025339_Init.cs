using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_MENUS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    TENANTID = table.Column<Guid>(type: "uuid", nullable: true),
                    NAME = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DISPLAY_NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ICON = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ROUTE = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    APP_NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PERMISSION_NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    APP_FORM_ID = table.Column<Guid>(type: "uuid", nullable: true),
                    PARENT_ID = table.Column<Guid>(type: "uuid", nullable: true),
                    ORDER = table.Column<double>(type: "double precision", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "boolean", nullable: false),
                    EXTRA_PROPERTIES = table.Column<string>(type: "text", nullable: false),
                    CONCURRENCY_STAMP = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MENUS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_SUBJECT_MENUS",
                columns: table => new
                {
                    SUBJECT_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    MENU_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ORDER = table.Column<double>(type: "double precision", nullable: false),
                    SUBJECTTYPE = table.Column<int>(type: "integer", nullable: false),
                    CREATIONTIME = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_SUBJECT_MENUS", x => new { x.SUBJECT_ID, x.MENU_ID });
                    table.ForeignKey(
                        name: "FK_SYS_SUBJECT_MENUS_SYS_MENUS_MENU_ID",
                        column: x => x.MENU_ID,
                        principalTable: "SYS_MENUS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_MENUS_APP_NAME",
                table: "SYS_MENUS",
                column: "APP_NAME");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_MENUS_NAME",
                table: "SYS_MENUS",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_SUBJECT_MENUS_MENU_ID",
                table: "SYS_SUBJECT_MENUS",
                column: "MENU_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_SUBJECT_MENUS_SUBJECT_ID",
                table: "SYS_SUBJECT_MENUS",
                column: "SUBJECT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_SUBJECT_MENUS");

            migrationBuilder.DropTable(
                name: "SYS_MENUS");
        }
    }
}
