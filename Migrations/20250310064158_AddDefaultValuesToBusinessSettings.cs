using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_server.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValuesToBusinessSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "EnableTakeout",
                table: "BusinessSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                comment: "啟用外帶",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "啟用外帶");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableOrdering",
                table: "BusinessSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                comment: "點餐系統啟用",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "點餐系統啟用");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableDineIn",
                table: "BusinessSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                comment: "啟用內用",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "啟用內用");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableDelivery",
                table: "BusinessSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                comment: "啟用外送",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "啟用外送");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "EnableTakeout",
                table: "BusinessSettings",
                type: "tinyint(1)",
                nullable: false,
                comment: "啟用外帶",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true,
                oldComment: "啟用外帶");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableOrdering",
                table: "BusinessSettings",
                type: "tinyint(1)",
                nullable: false,
                comment: "點餐系統啟用",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true,
                oldComment: "點餐系統啟用");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableDineIn",
                table: "BusinessSettings",
                type: "tinyint(1)",
                nullable: false,
                comment: "啟用內用",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true,
                oldComment: "啟用內用");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableDelivery",
                table: "BusinessSettings",
                type: "tinyint(1)",
                nullable: false,
                comment: "啟用外送",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true,
                oldComment: "啟用外送");
        }
    }
}
