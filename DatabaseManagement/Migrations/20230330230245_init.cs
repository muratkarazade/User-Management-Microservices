using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseManagement.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthenticationCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthenticationCredentials_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthenticationCredentials_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Surname" },
                values: new object[] { 1, "user1@example.com", "User", "One" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Surname" },
                values: new object[] { 2, "user2@example.com", "User", "Two" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Surname" },
                values: new object[] { 3, "user3@example.com", "User", "Three" });

            migrationBuilder.InsertData(
                table: "AuthenticationCredentials",
                columns: new[] { "Id", "Password", "UserId", "UserId1", "Username" },
                values: new object[] { 1, "password1", 1, null, "user1" });

            migrationBuilder.InsertData(
                table: "AuthenticationCredentials",
                columns: new[] { "Id", "Password", "UserId", "UserId1", "Username" },
                values: new object[] { 2, "password2", 2, null, "user2" });

            migrationBuilder.InsertData(
                table: "AuthenticationCredentials",
                columns: new[] { "Id", "Password", "UserId", "UserId1", "Username" },
                values: new object[] { 3, "password3", 3, null, "user3" });

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationCredentials_UserId",
                table: "AuthenticationCredentials",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationCredentials_UserId1",
                table: "AuthenticationCredentials",
                column: "UserId1",
                unique: true,
                filter: "[UserId1] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticationCredentials");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
