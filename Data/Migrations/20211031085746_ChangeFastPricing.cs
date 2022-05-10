using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangeFastPricing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExactPricingValues_CustomerProducts_Id",
                table: "ExactPricingValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRequests_CustomerProducts_Id",
                table: "ExchangeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingValues_CustomerProducts_Id",
                table: "FastPricingValues");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyValues_CustomerProducts_CustomerProductId",
                table: "PropertyValues");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_CustomerProducts_Id",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SellRequests_CustomerProducts_Id",
                table: "SellRequests");

            migrationBuilder.DropTable(
                name: "CustomerProducts");

            migrationBuilder.DropIndex(
                name: "IX_PropertyValues_CustomerProductId",
                table: "PropertyValues");

            migrationBuilder.DropIndex(
                name: "IX_FastPricingKeys_ProductId",
                table: "FastPricingKeys");

            migrationBuilder.DropColumn(
                name: "CustomerProductId",
                table: "PropertyValues");

            migrationBuilder.DropColumn(
                name: "IsAccessory",
                table: "FastPricingKeys");

            migrationBuilder.RenameColumn(
                name: "RateType",
                table: "FastPricingKeys",
                newName: "OperationType");

            migrationBuilder.AddColumn<Guid>(
                name: "FastPricingDDId",
                table: "FastPricingValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "ValueType",
                table: "FastPricingKeys",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "MinRate",
                table: "FastPricingDDs",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "MaxRate",
                table: "FastPricingDDs",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "ErrorDiscription",
                table: "FastPricingDDs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorTitle",
                table: "FastPricingDDs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsPriced = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Devices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingValues_FastPricingDDId",
                table: "FastPricingValues",
                column: "FastPricingDDId");

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingKeys_ProductId",
                table: "FastPricingKeys",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingKeys_ProductId_CategoryId",
                table: "FastPricingKeys",
                columns: new[] { "ProductId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CategoryId",
                table: "Devices",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId");

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
                name: "FK_FastPricingValues_Devices_Id",
                table: "FastPricingValues",
                column: "Id",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FastPricingValues_FastPricingDDs_FastPricingDDId",
                table: "FastPricingValues",
                column: "FastPricingDDId",
                principalTable: "FastPricingDDs",
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
                name: "FK_ExactPricingValues_Devices_Id",
                table: "ExactPricingValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRequests_Devices_Id",
                table: "ExchangeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingValues_Devices_Id",
                table: "FastPricingValues");

            migrationBuilder.DropForeignKey(
                name: "FK_FastPricingValues_FastPricingDDs_FastPricingDDId",
                table: "FastPricingValues");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairRequests_Devices_Id",
                table: "RepairRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SellRequests_Devices_Id",
                table: "SellRequests");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_FastPricingValues_FastPricingDDId",
                table: "FastPricingValues");

            migrationBuilder.DropIndex(
                name: "IX_FastPricingKeys_ProductId",
                table: "FastPricingKeys");

            migrationBuilder.DropIndex(
                name: "IX_FastPricingKeys_ProductId_CategoryId",
                table: "FastPricingKeys");

            migrationBuilder.DropColumn(
                name: "FastPricingDDId",
                table: "FastPricingValues");

            migrationBuilder.DropColumn(
                name: "ErrorDiscription",
                table: "FastPricingDDs");

            migrationBuilder.DropColumn(
                name: "ErrorTitle",
                table: "FastPricingDDs");

            migrationBuilder.RenameColumn(
                name: "OperationType",
                table: "FastPricingKeys",
                newName: "RateType");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerProductId",
                table: "PropertyValues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ValueType",
                table: "FastPricingKeys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccessory",
                table: "FastPricingKeys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "MinRate",
                table: "FastPricingDDs",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "MaxRate",
                table: "FastPricingDDs",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerProducts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyValues_CustomerProductId",
                table: "PropertyValues",
                column: "CustomerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FastPricingKeys_ProductId",
                table: "FastPricingKeys",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_CategoryId",
                table: "CustomerProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProducts_UserId",
                table: "CustomerProducts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExactPricingValues_CustomerProducts_Id",
                table: "ExactPricingValues",
                column: "Id",
                principalTable: "CustomerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRequests_CustomerProducts_Id",
                table: "ExchangeRequests",
                column: "Id",
                principalTable: "CustomerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FastPricingValues_CustomerProducts_Id",
                table: "FastPricingValues",
                column: "Id",
                principalTable: "CustomerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyValues_CustomerProducts_CustomerProductId",
                table: "PropertyValues",
                column: "CustomerProductId",
                principalTable: "CustomerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairRequests_CustomerProducts_Id",
                table: "RepairRequests",
                column: "Id",
                principalTable: "CustomerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SellRequests_CustomerProducts_Id",
                table: "SellRequests",
                column: "Id",
                principalTable: "CustomerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
