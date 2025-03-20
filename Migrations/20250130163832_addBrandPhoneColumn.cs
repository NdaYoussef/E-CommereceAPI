using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToken.Migrations
{
    /// <inheritdoc />
    public partial class addBrandPhoneColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "phone",
                table: "Brands",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phone",
                table: "Brands");
        }
    }
}
