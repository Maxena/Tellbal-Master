using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class OneToOnePricingAndCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FastPricingDefinitions_CategoryId_ProductId",
                table: "FastPricingDefinitions");

            migrationBuilder.DropColumn(
                name: "FastPricingDefinitionCategoryId",
                table: "FastPricingKeys");

            migrationBuilder.DropColumn(
                name: "FastPricingDefinitionProductId",
                table: "FastPricingKeys");

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingDefinitions_CategoryId",
                table: "FastPricingDefinitions",
                column: "CategoryId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FastPricingDefinitions_CategoryId",
                table: "FastPricingDefinitions");

            migrationBuilder.AddColumn<int>(
                name: "FastPricingDefinitionCategoryId",
                table: "FastPricingKeys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "FastPricingDefinitionProductId",
                table: "FastPricingKeys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingDefinitions_CategoryId_ProductId",
                table: "FastPricingDefinitions",
                columns: new[] { "CategoryId", "ProductId" },
                unique: true);
        }
    }
}
