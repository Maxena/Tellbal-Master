using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class MakeOneToManyDeviceAndPricing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingValues_Devices_Id",
                table: "FastPricingValues");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "FastPricingValues");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "FastPricingKeys");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "FastPricingValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "ValueType",
                table: "FastPricingKeys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "OperationType",
                table: "FastPricingDDs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingValues_DeviceId",
                table: "FastPricingValues",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPricingValues_Devices_DeviceId",
                table: "FastPricingValues",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingValues_Devices_DeviceId",
                table: "FastPricingValues");

            migrationBuilder.DropIndex(
                name: "IX_FastPricingValues_DeviceId",
                table: "FastPricingValues");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "FastPricingValues");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "FastPricingDDs");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "FastPricingValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ValueType",
                table: "FastPricingKeys",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "OperationType",
                table: "FastPricingKeys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPricingValues_Devices_Id",
                table: "FastPricingValues",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
