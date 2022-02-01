using Microsoft.EntityFrameworkCore.Migrations;

namespace Pet.Services.CouponAPI.Migrations
{
    public partial class SeeCouponDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DbCoupons",
                columns: new[] { "CouponId", "CouponCode", "DiscountAmount" },
                values: new object[] { 1, "10OFF", 10.0 });

            migrationBuilder.InsertData(
                table: "DbCoupons",
                columns: new[] { "CouponId", "CouponCode", "DiscountAmount" },
                values: new object[] { 2, "20OFF", 20.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DbCoupons",
                keyColumn: "CouponId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DbCoupons",
                keyColumn: "CouponId",
                keyValue: 2);
        }
    }
}
