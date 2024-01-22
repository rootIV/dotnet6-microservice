using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekShopping.Coupon.Api.Migrations
{
    public partial class SeedCouponDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "coupon",
                columns: new[] { "id", "coupon_code", "discount_amount" },
                values: new object[] { 1L, "VITOR_2024_01", 10m });

            migrationBuilder.InsertData(
                table: "coupon",
                columns: new[] { "id", "coupon_code", "discount_amount" },
                values: new object[] { 2L, "VITOR_2024_02", 15m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "coupon",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "coupon",
                keyColumn: "id",
                keyValue: 2L);
        }
    }
}
