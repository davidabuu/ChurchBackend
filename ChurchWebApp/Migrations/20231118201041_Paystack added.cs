using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchWebApp.Migrations
{
    /// <inheritdoc />
    public partial class Paystackadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TnxRef",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TnxRef",
                table: "Members");
        }
    }
}
