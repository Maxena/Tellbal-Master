using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Miscellaneous : Migration
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

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "1");

            migrationBuilder.AlterColumn<Guid>(
                name: "FastPricingDefinitionId",
                table: "FastPricingKeys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPriced",
                table: "Devices",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "0");

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

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "FastPricingDefinitionId",
                table: "FastPricingKeys",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPriced",
                table: "Devices",
                type: "bit",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(bool),
                oldType: "bit");

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
    }
}
