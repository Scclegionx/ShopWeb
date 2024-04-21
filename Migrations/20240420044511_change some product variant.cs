using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopWeb.Migrations
{
    /// <inheritdoc />
    public partial class changesomeproductvariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariantAttribute_ProductVariants_ProductVariantId",
                table: "VariantAttribute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantAttribute",
                table: "VariantAttribute");

            migrationBuilder.RenameTable(
                name: "VariantAttribute",
                newName: "VariantAttributes");

            migrationBuilder.RenameIndex(
                name: "IX_VariantAttribute_ProductVariantId",
                table: "VariantAttributes",
                newName: "IX_VariantAttributes_ProductVariantId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductVariants",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductVariantId",
                table: "VariantAttributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantAttributes",
                table: "VariantAttributes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariantAttributes_ProductVariants_ProductVariantId",
                table: "VariantAttributes",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariantAttributes_ProductVariants_ProductVariantId",
                table: "VariantAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VariantAttributes",
                table: "VariantAttributes");

            migrationBuilder.RenameTable(
                name: "VariantAttributes",
                newName: "VariantAttribute");

            migrationBuilder.RenameIndex(
                name: "IX_VariantAttributes_ProductVariantId",
                table: "VariantAttribute",
                newName: "IX_VariantAttribute_ProductVariantId");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "ProductVariants",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductVariantId",
                table: "VariantAttribute",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VariantAttribute",
                table: "VariantAttribute",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariantAttribute_ProductVariants_ProductVariantId",
                table: "VariantAttribute",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id");
        }
    }
}
