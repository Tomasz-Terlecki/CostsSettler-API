using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CostsSettler.Repo.Migrations
{
    /// <inheritdoc />
    public partial class CircumstanceStatus_column_Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "CircumstanceStatus",
                table: "Circumstances",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CircumstanceStatus",
                table: "Circumstances");
        }
    }
}
