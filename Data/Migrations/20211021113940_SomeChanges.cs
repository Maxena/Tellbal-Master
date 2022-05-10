using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class SomeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingDDS_FastPricingKeys_FastPricingKeyId",
                table: "FastPricingDDS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FastPricingDDS",
                table: "FastPricingDDS");

            migrationBuilder.RenameTable(
                name: "FastPricingDDS",
                newName: "FastPricingDDs");

            migrationBuilder.RenameColumn(
                name: "UpdatedPriceTime",
                table: "PriceLogs",
                newName: "DT");

            migrationBuilder.RenameIndex(
                name: "IX_FastPricingDDS_FastPricingKeyId",
                table: "FastPricingDDs",
                newName: "IX_FastPricingDDs_FastPricingKeyId");

            migrationBuilder.AddColumn<string>(
                name: "WhareHouseState",
                table: "WhareHouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellRequestStatus",
                table: "SellRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "PropertyValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ValueType",
                table: "FastPricingKeys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "RateType",
                table: "FastPricingKeys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ConsultingStatus",
                table: "Consultings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastPricingDDs",
                table: "FastPricingDDs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPricingDDs_FastPricingKeys_FastPricingKeyId",
                table: "FastPricingDDs",
                column: "FastPricingKeyId",
                principalTable: "FastPricingKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingDDs_FastPricingKeys_FastPricingKeyId",
                table: "FastPricingDDs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FastPricingDDs",
                table: "FastPricingDDs");

            migrationBuilder.DropColumn(
                name: "WhareHouseState",
                table: "WhareHouses");

            migrationBuilder.DropColumn(
                name: "SellRequestStatus",
                table: "SellRequests");

            migrationBuilder.RenameTable(
                name: "FastPricingDDs",
                newName: "FastPricingDDS");

            migrationBuilder.RenameColumn(
                name: "DT",
                table: "PriceLogs",
                newName: "UpdatedPriceTime");

            migrationBuilder.RenameIndex(
                name: "IX_FastPricingDDs_FastPricingKeyId",
                table: "FastPricingDDS",
                newName: "IX_FastPricingDDS_FastPricingKeyId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "PropertyValues",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "ValueType",
                table: "FastPricingKeys",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "RateType",
                table: "FastPricingKeys",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ConsultingStatus",
                table: "Consultings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastPricingDDS",
                table: "FastPricingDDS",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPricingDDS_FastPricingKeys_FastPricingKeyId",
                table: "FastPricingDDS",
                column: "FastPricingKeyId",
                principalTable: "FastPricingKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
