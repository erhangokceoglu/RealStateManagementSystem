using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RealStateManagementSystem.Infastructure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccesToken = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProvinceId = table.Column<int>(type: "integer", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    Address = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Neighbourhoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DistrictId = table.Column<int>(type: "integer", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neighbourhoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Neighbourhoods_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    State = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    ProcessType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: true),
                    UserIp = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AppUserId = table.Column<int>(type: "integer", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RealStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IslandNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ParcelNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Qualification = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: true),
                    Latitude = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Longitude = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AppUserId = table.Column<int>(type: "integer", nullable: false),
                    ProvinceId = table.Column<int>(type: "integer", nullable: false),
                    DistrictId = table.Column<int>(type: "integer", nullable: false),
                    NeighbourhoodId = table.Column<int>(type: "integer", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealStates_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealStates_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealStates_Neighbourhoods_NeighbourhoodId",
                        column: x => x.NeighbourhoodId,
                        principalTable: "Neighbourhoods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealStates_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "CreateDate", "IsActive", "Name", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2280), true, "Ankara", null },
                    { 2, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2283), true, "İstanbul", null },
                    { 3, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2285), true, "İzmir", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateDate", "IsActive", "Name", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2107), true, "SistemYoneticisi", null },
                    { 2, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2122), true, "Kullanici", null }
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "Address", "CreateDate", "Email", "IsActive", "Name", "Password", "PasswordHash", "PasswordSalt", "RoleId", "Surname", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "Etimesgut", new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2465), "erhangokceoglu@hotmail.com", true, "Erhan", "Ankara1.", new byte[] { 66, 245, 190, 89, 110, 158, 209, 46, 21, 82, 210, 3, 39, 59, 122, 254, 187, 44, 233, 99, 245, 34, 35, 233, 141, 167, 125, 191, 148, 151, 58, 203 }, new byte[] { 189, 179, 177, 178, 241, 195, 156, 129, 226, 214, 170, 175, 191, 27, 112, 163, 103, 205, 239, 6, 67, 78, 72, 48, 126, 123, 239, 68, 81, 246, 204, 242, 98, 126, 15, 245, 103, 106, 78, 85, 180, 51, 11, 47, 197, 147, 118, 13, 23, 172, 217, 76, 33, 152, 28, 240, 51, 125, 168, 112, 225, 203, 197, 6 }, 1, "Gökçeoğlu", null },
                    { 2, "Etimesgut", new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2468), "erhangokceoglu91@gmail.com", true, "Erhan", "Ankara1.", new byte[] { 99, 255, 4, 210, 25, 222, 108, 4, 173, 164, 25, 211, 254, 167, 98, 242, 179, 20, 183, 96, 185, 249, 125, 78, 146, 52, 118, 91, 196, 151, 68, 82 }, new byte[] { 95, 175, 103, 226, 203, 120, 241, 223, 48, 244, 221, 33, 245, 42, 28, 123, 243, 139, 105, 209, 48, 242, 244, 158, 13, 144, 163, 164, 158, 3, 38, 83, 147, 74, 161, 94, 57, 41, 77, 220, 57, 177, 64, 239, 224, 214, 221, 42, 88, 165, 158, 237, 169, 147, 16, 120, 197, 237, 3, 140, 86, 236, 99, 46 }, 2, "Gökçeoğlu", null }
                });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "Id", "CreateDate", "IsActive", "Name", "ProvinceId", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2305), true, "Çankaya", 1, null },
                    { 2, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2308), true, "Keçiören", 1, null },
                    { 3, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2311), true, "Yenimahalle", 1, null },
                    { 4, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2313), true, "Levent", 2, null },
                    { 5, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2314), true, "Kadıköy", 2, null },
                    { 6, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2316), true, "Beşiktaş", 2, null },
                    { 7, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2317), true, "Bornova", 3, null },
                    { 8, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2319), true, "Karşıyaka", 3, null },
                    { 9, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2321), true, "Konak", 3, null }
                });

            migrationBuilder.InsertData(
                table: "Neighbourhoods",
                columns: new[] { "Id", "CreateDate", "DistrictId", "IsActive", "Name", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2344), 1, true, "Dikmen", null },
                    { 2, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2346), 1, true, "Bahçelievler", null },
                    { 3, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2348), 1, true, "Kızılay", null },
                    { 4, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2350), 2, true, "Etlik", null },
                    { 5, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2351), 2, true, "Karşıyaka", null },
                    { 6, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2353), 2, true, "Güzelkent", null },
                    { 7, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2355), 3, true, "Batıkent", null },
                    { 8, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2357), 3, true, "Demetevler", null },
                    { 9, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2358), 3, true, "Ümitköy", null },
                    { 10, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2360), 4, true, "Levent Mahallesi", null },
                    { 11, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2362), 4, true, "Levent 1. Bölge", null },
                    { 12, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2364), 4, true, "Levent 2. Bölge", null },
                    { 13, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2365), 5, true, "Caddebostan", null },
                    { 14, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2367), 5, true, "Fenerbahçe", null },
                    { 15, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2369), 5, true, "Göztepe", null },
                    { 16, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2371), 6, true, "Abbasağa", null },
                    { 17, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2372), 6, true, "Akaretler", null },
                    { 18, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2374), 6, true, "Arnavutköy", null },
                    { 19, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2377), 7, true, "Kazımdirik", null },
                    { 20, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2378), 7, true, "Çamdibi", null },
                    { 21, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2380), 7, true, "Gülbahçe", null },
                    { 22, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2437), 8, true, "Bahçelievler", null },
                    { 23, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2439), 8, true, "Bostanlı", null },
                    { 24, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2441), 8, true, "Çiğli", null },
                    { 25, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2442), 9, true, "Alsancak", null },
                    { 26, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2444), 9, true, "Bahribaba", null },
                    { 27, new DateTime(2023, 5, 12, 9, 59, 15, 840, DateTimeKind.Local).AddTicks(2446), 9, true, "Basmane", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_RoleId",
                table: "AppUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_AppUserId",
                table: "Logs",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Neighbourhoods_DistrictId",
                table: "Neighbourhoods",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_RealStates_AppUserId",
                table: "RealStates",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RealStates_DistrictId",
                table: "RealStates",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_RealStates_NeighbourhoodId",
                table: "RealStates",
                column: "NeighbourhoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RealStates_ProvinceId",
                table: "RealStates",
                column: "ProvinceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "RealStates");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Neighbourhoods");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");
        }
    }
}
