using Microsoft.EntityFrameworkCore.Migrations;

namespace BusinessDataAccess.Migrations
{
    public partial class initialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "Id", "Description", "IsActive", "Name" },
                values: new object[] { 1L, null, false, "Review_OPTIMIZER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
