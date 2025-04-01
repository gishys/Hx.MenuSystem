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
                name: "SYS_USERMENUS",
                columns: table => new
                {
                    USER_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    MENU_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ORDER = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USERMENUS", x => new { x.USER_ID, x.MENU_ID });
                    table.ForeignKey(
                        name: "FK_SYS_USERMENUS_SYS_MENUS_MENU_ID",
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
                name: "IX_SYS_USERMENUS_MENU_ID",
                table: "SYS_USERMENUS",
                column: "MENU_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_USERMENUS_USER_ID",
                table: "SYS_USERMENUS",
                column: "USER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_USERMENUS");

            migrationBuilder.DropTable(
                name: "SYS_MENUS");
        }
    }
}
