using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopWeb.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class Addnewroleshipper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a37d654b-9771-4d90-b9d4-e6b88509c394", "a37d654b-9771-4d90-b9d4-e6b88509c394", "Shipper", "Shipper" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b1932f74-fcf6-4c60-83a0-236c9cd76e13",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a3b8e1b2-3d3b-40b5-a8ad-85da6e3e84a1", "AQAAAAIAAYagAAAAEO407RrOjBROafYHK5Wkf9MZGo0GlqN7lmxnzLmXUOnn45yJlURF+2o2cCc+OBOD8w==", "19e696c2-286d-4f5f-ba21-c222dd78dfb6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a37d654b-9771-4d90-b9d4-e6b88509c394");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b1932f74-fcf6-4c60-83a0-236c9cd76e13",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "93957b84-2fb6-4b07-beec-5c9d7deea66f", "AQAAAAIAAYagAAAAEEq8x8Y+YOeENbklbb1RURaj7XaZ7tGN3p8/gzvYgOOVwy2dXl++RHHN6PjeiYcxqQ==", "4b56b0c2-a575-435e-93f3-10e79d91addb" });
        }
    }
}
