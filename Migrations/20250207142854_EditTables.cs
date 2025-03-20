using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToken.Migrations
{
    /// <inheritdoc />
    public partial class EditTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");

            migrationBuilder.AddColumn<double>(
                name: "ShippingCost",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TrackingCode",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
