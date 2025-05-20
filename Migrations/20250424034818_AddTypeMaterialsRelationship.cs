using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_server.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeMaterialsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeMaterials",
                columns: table => new
                {
                    TypeEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MaterialId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeMaterials", x => new { x.TypeEntityId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_TypeMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypeMaterials_Types_TypeEntityId",
                        column: x => x.TypeEntityId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TypeMaterials_MaterialId",
                table: "TypeMaterials",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeMaterials");
        }
    }
}
