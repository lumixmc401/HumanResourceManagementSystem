using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResourceManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUserAndAdminRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("d2b7f5e1-5c3b-4d3a-8b1e-2f3b5e1c3b4d"), "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Salt" },
                values: new object[] { new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6"), "admin@example.com", "iUMZ1joaHTQhSWV3MuDXJw==", "salt" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d8"), "Name", "Admin User", new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6") });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("d2b7f5e1-5c3b-4d3a-8b1e-2f3b5e1c3b4d"), new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d8"));

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
        }
    }
}
