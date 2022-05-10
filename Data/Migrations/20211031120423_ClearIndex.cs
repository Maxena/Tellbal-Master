using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ClearIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FastPricingKeys_ProductId_CategoryId",
                table: "FastPricingKeys");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FastPricingKeys_ProductId_CategoryId",
                table: "FastPricingKeys",
                columns: new[] { "ProductId", "CategoryId" },
                unique: true);
        }
    }
}
