using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_server.Migrations
{
    /// <inheritdoc />
    public partial class EditAndDebug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptionRadios_OptionGroups_ChildrenId",
                table: "OptionRadios");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_OptionGroups_OptionChildrenId",
                table: "Options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OptionGroups",
                table: "OptionGroups");

            migrationBuilder.RenameTable(
                name: "OptionGroups",
                newName: "OptionChildren");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OptionChildren",
                table: "OptionChildren",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionRadios_OptionChildren_ChildrenId",
                table: "OptionRadios",
                column: "ChildrenId",
                principalTable: "OptionChildren",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_OptionChildren_OptionChildrenId",
                table: "Options",
                column: "OptionChildrenId",
                principalTable: "OptionChildren",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OptionRadios_OptionChildren_ChildrenId",
                table: "OptionRadios");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_OptionChildren_OptionChildrenId",
                table: "Options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OptionChildren",
                table: "OptionChildren");

            migrationBuilder.RenameTable(
                name: "OptionChildren",
                newName: "OptionGroups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OptionGroups",
                table: "OptionGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OptionRadios_OptionGroups_ChildrenId",
                table: "OptionRadios",
                column: "ChildrenId",
                principalTable: "OptionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_OptionGroups_OptionChildrenId",
                table: "Options",
                column: "OptionChildrenId",
                principalTable: "OptionGroups",
                principalColumn: "Id");
        }
    }
}
