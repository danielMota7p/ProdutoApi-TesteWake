using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserSeedRuntime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, "$2a$11$RTffY.rfw3qwX5.nlVm8E.rWnHi4tD7l92cdIE9MPuZTj7UBvPhc.", "admin", "admin" });
        }
    }
}
