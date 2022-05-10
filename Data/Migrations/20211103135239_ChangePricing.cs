using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangePricing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingKeys_Categories_CategoryId",
                table: "FastPricingKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingKeys_Products_ProductId",
                table: "FastPricingKeys");

            migrationBuilder.DropIndex(
                name: "IX_FastPricingKeys_CategoryId",
                table: "FastPricingKeys");

            migrationBuilder.DropIndex(
                name: "IX_FastPricingKeys_ProductId",
                table: "FastPricingKeys");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "FastPricingKeys",
                newName: "FastPricingDefinitionProductId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "FastPricingKeys",
                newName: "FastPricingDefinitionCategoryId");

            migrationBuilder.AddColumn<Guid>(
                name: "FastPricingDefinitionId",
                table: "FastPricingKeys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FastPricingDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastPricingDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FastPricingDefinitions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FastPricingDefinitions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingKeys_FastPricingDefinitionId",
                table: "FastPricingKeys",
                column: "FastPricingDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingDefinitions_CategoryId_ProductId",
                table: "FastPricingDefinitions",
                columns: new[] { "CategoryId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingDefinitions_ProductId",
                table: "FastPricingDefinitions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPricingKeys_FastPricingDefinitions_FastPricingDefinitionId",
                table: "FastPricingKeys",
                column: "FastPricingDefinitionId",
                principalTable: "FastPricingDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingKeys_FastPricingDefinitions_FastPricingDefinitionId",
                table: "FastPricingKeys");

            migrationBuilder.DropTable(
                name: "FastPricingDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_FastPricingKeys_FastPricingDefinitionId",
                table: "FastPricingKeys");

            migrationBuilder.DropColumn(
                name: "FastPricingDefinitionId",
                table: "FastPricingKeys");

            migrationBuilder.RenameColumn(
                name: "FastPricingDefinitionProductId",
                table: "FastPricingKeys",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "FastPricingDefinitionCategoryId",
                table: "FastPricingKeys",
                newName: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingKeys_CategoryId",
                table: "FastPricingKeys",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingKeys_ProductId",
                table: "FastPricingKeys",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPricingKeys_Categories_CategoryId",
                table: "FastPricingKeys",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FastPricingKeys_Products_ProductId",
                table: "FastPricingKeys",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
