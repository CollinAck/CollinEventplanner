using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollinEventplanner.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDummy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Participants",
                columns: new[] { "ParticipantId", "Email", "Name" },
                values: new object[] { 1, "test@test.com", "Test" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "ParticipantId",
                keyValue: 1);
        }
    }
}
