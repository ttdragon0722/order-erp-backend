using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_server.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionGroupAndFormSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OptionRadioId",
                table: "Options",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "OptionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionGroups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Options_OptionRadioId",
                table: "Options",
                column: "OptionRadioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_OptionGroups_OptionRadioId",
                table: "Options",
                column: "OptionRadioId",
                principalTable: "OptionGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_OptionGroups_OptionRadioId",
                table: "Options");

            migrationBuilder.DropTable(
                name: "OptionGroups");

            migrationBuilder.DropIndex(
                name: "IX_Options_OptionRadioId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "OptionRadioId",
                table: "Options");
        }
    }
}
