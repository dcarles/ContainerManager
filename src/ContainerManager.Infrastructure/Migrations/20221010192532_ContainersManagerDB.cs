using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContainerManager.Infrastructure.Migrations
{
    public partial class ContainersManagerDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiKey = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OS = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machines_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Command = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Args = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkingDirectory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    CPULimit = table.Column<int>(type: "int", nullable: false),
                    MemoryMBLimit = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MachineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Applications_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ApiKey", "Email", "FirstName", "LastName", "Role" },
                values: new object[] { new Guid("9a484f14-3234-440d-bf99-3fcf2adeaf95"), "testConsumerApiKey3264", "danielcarles-consumer@gmail.com", "Daniel", "Carles", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ApiKey", "Email", "FirstName", "LastName", "Role" },
                values: new object[] { new Guid("a6de3afd-5c74-407b-8a19-75c37027e610"), "testOwnerApiKey3264", "danielcarles@gmail.com", "Daniel", "Carles", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_MachineId",
                table: "Applications",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Name",
                table: "Applications",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_OwnerId",
                table: "Applications",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Name",
                table: "Machines",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_OwnerId",
                table: "Machines",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApiKey",
                table: "Users",
                column: "ApiKey",
                unique: true,
                filter: "[ApiKey] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
