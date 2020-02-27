using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Revature.Identity.DataAccess.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoordinatorAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrainingCenterName = table.Column<string>(nullable: true),
                    TrainingCenterAddress = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoordinatorAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpdateAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UpdateType = table.Column<string>(nullable: true),
                    SerializedTarget = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProviderAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    StatusText = table.Column<string>(nullable: true),
                    AccountCreatedAt = table.Column<DateTime>(nullable: false),
                    AccountExpiresAt = table.Column<DateTime>(nullable: false),
                    CoordinatorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderAccount_CoordinatorAccount_CoordinatorId",
                        column: x => x.CoordinatorId,
                        principalTable: "CoordinatorAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StatusText = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CoordinatorId = table.Column<Guid>(nullable: true),
                    ProviderId = table.Column<Guid>(nullable: true),
                    UpdateActionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_CoordinatorAccount_CoordinatorId",
                        column: x => x.CoordinatorId,
                        principalTable: "CoordinatorAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_ProviderAccount_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "ProviderAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_UpdateAction_UpdateActionId",
                        column: x => x.UpdateActionId,
                        principalTable: "UpdateAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_CoordinatorId",
                table: "Notification",
                column: "CoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ProviderId",
                table: "Notification",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UpdateActionId",
                table: "Notification",
                column: "UpdateActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderAccount_CoordinatorId",
                table: "ProviderAccount",
                column: "CoordinatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "TenantAccount");

            migrationBuilder.DropTable(
                name: "ProviderAccount");

            migrationBuilder.DropTable(
                name: "UpdateAction");

            migrationBuilder.DropTable(
                name: "CoordinatorAccount");
        }
    }
}
