using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CostsSettler.Repo.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CircumstanceStatus",
                table: "Circumstances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "CircumstanceStatus",
                table: "Circumstances",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
