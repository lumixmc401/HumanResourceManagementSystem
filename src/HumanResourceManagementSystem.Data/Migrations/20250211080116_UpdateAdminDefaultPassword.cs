using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResourceManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminDefaultPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6"),
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "+UDNjfwWfnTQHFRdOZ5z8g==", "c2FsdA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7a8b-9c0d-e1f2a3b4c5d6"),
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "YmMgQNAeIO2STqlO/qZpHQ==", "salt" });
        }
    }
}
