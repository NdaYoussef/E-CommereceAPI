using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestToken.Migrations
{
    /// <inheritdoc />
    public partial class editOrderItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "OrderItems",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
