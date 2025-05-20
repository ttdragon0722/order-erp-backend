using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Materials_MaterialId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Products_ProductId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Types_TypeId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Types_TypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Options_MaterialId",
                table: "Options");

            migrationBuilder.DropIndex(
                name: "IX_Options_ProductId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Options");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Options",
                newName: "Depend");

            migrationBuilder.RenameIndex(
                name: "IX_Options_TypeId",
                table: "Options",
                newName: "IX_Options_Depend");

            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Enable",
                table: "Tags",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "TypeId",
                table: "Products",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Options",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.CreateTable(
                name: "ProductExcludedOptions",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductExcludedOptions", x => new { x.ProductId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_ProductExcludedOptions_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductExcludedOptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductOptions",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptions", x => new { x.ProductId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_ProductOptions_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TagId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => new { x.ProductId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TypeOptions",
                columns: table => new
                {
                    TypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOptions", x => new { x.TypeId, x.OptionId });
                    table.ForeignKey(
                        name: "FK_TypeOptions_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypeOptions_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductExcludedOptions_OptionId",
                table: "ProductExcludedOptions",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptions_OptionId",
                table: "ProductOptions",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagId",
                table: "ProductTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeOptions_OptionId",
                table: "TypeOptions",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Materials_Depend",
                table: "Options",
                column: "Depend",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Types_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Materials_Depend",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Types_TypeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductExcludedOptions");

            migrationBuilder.DropTable(
                name: "ProductOptions");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "TypeOptions");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Enable",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "Depend",
                table: "Options",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Options_Depend",
                table: "Options",
                newName: "IX_Options_TypeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TypeId",
                table: "Products",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Options",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                table: "Options",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Options",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Options_MaterialId",
                table: "Options",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_ProductId",
                table: "Options",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Materials_MaterialId",
                table: "Options",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Products_ProductId",
                table: "Options",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Types_TypeId",
                table: "Options",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Types_TypeId",
                table: "Products",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id");
        }
    }
}
