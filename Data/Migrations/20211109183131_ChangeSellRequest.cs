using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangeSellRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultings_Users_UserId",
                table: "Consultings");

            migrationBuilder.DropForeignKey(
                name: "FK_ExactPricingValues_Devices_Id",
                table: "ExactPricingValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRequests_Devices_Id",
                table: "ExchangeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_Devices_Id",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SellRequests_Devices_Id",
                table: "SellRequests");

            migrationBuilder.AddColumn<string>(
                name: "StatusDescription",
                table: "SellRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultings_Users_UserId",
                table: "Consultings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExactPricingValues_Devices_Id",
                table: "ExactPricingValues",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRequests_Devices_Id",
                table: "ExchangeRequests",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_Devices_Id",
                table: "RepairRequests",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SellRequests_Devices_Id",
                table: "SellRequests",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultings_Users_UserId",
                table: "Consultings");

            migrationBuilder.DropForeignKey(
                name: "FK_ExactPricingValues_Devices_Id",
                table: "ExactPricingValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRequests_Devices_Id",
                table: "ExchangeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_Devices_Id",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SellRequests_Devices_Id",
                table: "SellRequests");

            migrationBuilder.DropColumn(
                name: "StatusDescription",
                table: "SellRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultings_Users_UserId",
                table: "Consultings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExactPricingValues_Devices_Id",
                table: "ExactPricingValues",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRequests_Devices_Id",
                table: "ExchangeRequests",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_Devices_Id",
                table: "RepairRequests",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellRequests_Devices_Id",
                table: "SellRequests",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
