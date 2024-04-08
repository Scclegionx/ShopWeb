using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopWeb.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class q : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b1932f74-fcf6-4c60-83a0-236c9cd76e13",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1081cf14-aed9-4fe4-a904-6825508b9572", "AQAAAAIAAYagAAAAEBn5RMiy3/euKRfWJ/yF3nKkbtAo4kwgJER1MyJLYpfsWElxM2+SmW6E0z1mAV+4wA==", "d71f5f59-b77c-475e-b35b-2b7f396906f1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b1932f74-fcf6-4c60-83a0-236c9cd76e13",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a3b8e1b2-3d3b-40b5-a8ad-85da6e3e84a1", "AQAAAAIAAYagAAAAEO407RrOjBROafYHK5Wkf9MZGo0GlqN7lmxnzLmXUOnn45yJlURF+2o2cCc+OBOD8w==", "19e696c2-286d-4f5f-ba21-c222dd78dfb6" });
        }
    }
}
