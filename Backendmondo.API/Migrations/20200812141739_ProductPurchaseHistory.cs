using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backendmondo.API.Migrations
{
    public partial class ProductPurchaseHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Subscriptions_SubscriptionId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubscriptionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Purchased",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SubscriptionId = table.Column<Guid>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: true),
                    Purchased = table.Column<DateTime>(nullable: false),
                    PriceUSDWhenPurchased = table.Column<float>(nullable: false),
                    TaxUSDWhenPurchased = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPurchases_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPurchases_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchases_ProductId",
                table: "ProductPurchases",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchases_SubscriptionId",
                table: "ProductPurchases",
                column: "SubscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPurchases");

            migrationBuilder.AddColumn<DateTime>(
                name: "Purchased",
                table: "Subscriptions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubscriptionId",
                table: "Products",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Subscriptions_SubscriptionId",
                table: "Products",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
