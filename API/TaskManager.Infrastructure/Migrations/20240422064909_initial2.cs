using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52a622d2-6d6f-4a2e-bc3b-e7d19add06b5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c871a234-a6aa-4c9a-87b4-3cc71340a80b", "78764d39-c84a-49b5-958c-cb677bfac9fb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c871a234-a6aa-4c9a-87b4-3cc71340a80b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "78764d39-c84a-49b5-958c-cb677bfac9fb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8050a519-869c-48e1-8f90-d51acc3b2db9", null, "Admin", "ADMIN" },
                    { "b3e41cb3-8795-4f90-ab72-e3e438bb4a3d", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsActive", "IsFirstLogin", "JoinDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c7a8dae7-8af4-471e-bf0c-73d164213dc4", 0, "8983dd0c-c45a-4fa5-893d-ca4506763178", "kasunysoft@gmail.com", false, "Mirahampe Patisthana Gedara Kasunjith Bimal Lakshitha", true, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "KASUNYSOFT@GMAIL.COM", "KASUNYSOFT@GMAIL.COM", "AQAAAAIAAYagAAAAENg5TrC3AmwS5QA4hhBvEqC5R4R58JbX5yoi1mOTJzP5VDU71IERMYg18k1xhExfCw==", "0716063159", false, "8602a450-5c8d-474e-89d6-b2fd6eb4837a", false, "kasunysoft@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "8050a519-869c-48e1-8f90-d51acc3b2db9", "c7a8dae7-8af4-471e-bf0c-73d164213dc4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3e41cb3-8795-4f90-ab72-e3e438bb4a3d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8050a519-869c-48e1-8f90-d51acc3b2db9", "c7a8dae7-8af4-471e-bf0c-73d164213dc4" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8050a519-869c-48e1-8f90-d51acc3b2db9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c7a8dae7-8af4-471e-bf0c-73d164213dc4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52a622d2-6d6f-4a2e-bc3b-e7d19add06b5", null, "User", "USER" },
                    { "c871a234-a6aa-4c9a-87b4-3cc71340a80b", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsActive", "IsFirstLogin", "JoinDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "78764d39-c84a-49b5-958c-cb677bfac9fb", 0, "482178c7-6d72-41f3-b6cc-328c6e63ac18", "kasunysoft@gmail.com", false, "Mirahampe Patisthana Gedara Kasunjith Bimal Lakshitha", true, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "KASUNYSOFT@GMAIL.COM", "KASUNYSOFT@GMAIL.COM", "AQAAAAIAAYagAAAAEPBz4YbHywHsiz14P81AnINvdZr3iennvB08UMwTGeLHw/ZFNnxftUac1vfgZbE+0g==", "0716063159", false, "e372ca9f-beed-48b4-93db-43f3cc0c4f2d", false, "kasunysoft@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "c871a234-a6aa-4c9a-87b4-3cc71340a80b", "78764d39-c84a-49b5-958c-cb677bfac9fb" });
        }
    }
}
