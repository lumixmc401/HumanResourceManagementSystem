using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResourceManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserClaimPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("d2b7f5e1-5c3b-4d3a-8b1e-2f3b5e1c3b4d"), new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d2b7f5e1-5c3b-4d3a-8b1e-2f3b5e1c3b4d"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UserClaims",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserClaims",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("d2b7f5e1-5c3b-4d3a-8b1e-2f3b5e1c3b4d"), "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Salt" },
                values: new object[] { new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6"), "admin@example.com", "YmMgQNAeIO2STqlO/qZpHQ==", "salt" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "Name", "Admin User", new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6") });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d2b7f5e1-5c3b-4d3a-8b1e-2f3b5e1c3b4d"), new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6") });
        }
    }
}
