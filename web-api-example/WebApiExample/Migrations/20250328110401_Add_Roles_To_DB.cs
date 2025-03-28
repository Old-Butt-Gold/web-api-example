using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiExample.Migrations
{
    /// <inheritdoc />
    public partial class Add_Roles_To_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4a69f173-4276-4225-b8e4-db3fd53d50e5", null, "Manager", "MANAGER" },
                    { "c88a51d2-ab69-4204-bd63-73e0f2eb77da", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a69f173-4276-4225-b8e4-db3fd53d50e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c88a51d2-ab69-4204-bd63-73e0f2eb77da");
        }
    }
}
