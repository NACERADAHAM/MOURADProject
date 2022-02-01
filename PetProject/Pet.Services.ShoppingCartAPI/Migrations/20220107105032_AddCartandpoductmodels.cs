using Microsoft.EntityFrameworkCore.Migrations;

namespace Pet.Services.ShoppingCartAPI.Migrations
{
    public partial class AddCartandpoductmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbCartHeader",
                columns: table => new
                {
                    CartHeaderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbCartHeader", x => x.CartHeaderId);
                });

            migrationBuilder.CreateTable(
                name: "DbProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbProducts", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "DbCartDetails",
                columns: table => new
                {
                    CartDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartHeaderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbCartDetails", x => x.CartDetailsId);
                    table.ForeignKey(
                        name: "FK_DbCartDetails_DbCartHeader_CartHeaderId",
                        column: x => x.CartHeaderId,
                        principalTable: "DbCartHeader",
                        principalColumn: "CartHeaderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbCartDetails_DbProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "DbProducts",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbCartDetails_CartHeaderId",
                table: "DbCartDetails",
                column: "CartHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_DbCartDetails_ProductId",
                table: "DbCartDetails",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbCartDetails");

            migrationBuilder.DropTable(
                name: "DbCartHeader");

            migrationBuilder.DropTable(
                name: "DbProducts");
        }
    }
}
