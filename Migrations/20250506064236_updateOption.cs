using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_server.Migrations
{
    /// <inheritdoc />
    public partial class updateOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_OptionGroups_OptionRadioId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "OptionGroups");

            migrationBuilder.RenameColumn(
                name: "OptionRadioId",
                table: "Options",
                newName: "OptionChildrenId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_OptionRadioId",
                table: "Options",
                newName: "IX_Options_OptionChildrenId");

            migrationBuilder.AddColumn<bool>(
                name: "Require",
                table: "Options",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OptionRadios",
                columns: table => new
                {
                    OptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChildrenId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionRadios", x => new { x.OptionId, x.ChildrenId });
                    table.ForeignKey(
                        name: "FK_OptionRadios_OptionGroups_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "OptionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionRadios_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OptionRadios_ChildrenId",
                table: "OptionRadios",
                column: "ChildrenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_OptionGroups_OptionChildrenId",
                table: "Options",
                column: "OptionChildrenId",
                principalTable: "OptionGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_OptionGroups_OptionChildrenId",
                table: "Options");

            migrationBuilder.DropTable(
                name: "OptionRadios");

            migrationBuilder.DropColumn(
                name: "Require",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Options");

            migrationBuilder.RenameColumn(
                name: "OptionChildrenId",
                table: "Options",
                newName: "OptionRadioId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_OptionChildrenId",
                table: "Options",
                newName: "IX_Options_OptionRadioId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "OptionGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_OptionGroups_OptionRadioId",
                table: "Options",
                column: "OptionRadioId",
                principalTable: "OptionGroups",
                principalColumn: "Id");
        }
    }
}
