using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerceClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class updateIsUniqueForProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name_BrandId",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 11, 27, 9, 31, 20, 194, DateTimeKind.Local).AddTicks(6972));

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name_BrandId_SizeId",
                table: "Products",
                columns: new[] { "Name", "BrandId", "SizeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name_BrandId_SizeId",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2024, 10, 27, 19, 51, 45, 727, DateTimeKind.Local).AddTicks(7762));

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name_BrandId",
                table: "Products",
                columns: new[] { "Name", "BrandId" },
                unique: true);
        }
    }
}
