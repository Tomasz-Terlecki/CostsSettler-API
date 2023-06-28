using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CostsSettler.Repo.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Circumstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CircumstanceStatus = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Circumstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberCharges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CircumstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CircumstanceRole = table.Column<byte>(type: "tinyint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ChargeStatus = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberCharges_Circumstances_CircumstanceId",
                        column: x => x.CircumstanceId,
                        principalTable: "Circumstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberCharges_CircumstanceId",
                table: "MemberCharges",
                column: "CircumstanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberCharges");

            migrationBuilder.DropTable(
                name: "Circumstances");
        }
    }
}
