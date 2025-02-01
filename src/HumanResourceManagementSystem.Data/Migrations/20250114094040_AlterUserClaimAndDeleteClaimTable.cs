using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResourceManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserClaimAndDeleteClaimTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Claims_ClaimId",
                table: "UserClaims");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserClaims_ClaimId",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "ClaimId",
                table: "UserClaims");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserClaims",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ClaimType",
                table: "UserClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClaimValue",
                table: "UserClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "ClaimType",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "ClaimValue",
                table: "UserClaims");

            migrationBuilder.AddColumn<Guid>(
                name: "ClaimId",
                table: "UserClaims",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims",
                columns: new[] { "UserId", "ClaimId" });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_ClaimId",
                table: "UserClaims",
                column: "ClaimId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Claims_ClaimId",
                table: "UserClaims",
                column: "ClaimId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
