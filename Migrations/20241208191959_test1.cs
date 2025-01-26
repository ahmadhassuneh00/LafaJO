using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjAPI.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Registration_RegistrationId",
                schema: "FinalProjPost",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "FinalProjPost",
                table: "Items",
                newName: "NumOfItems");

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationId",
                schema: "FinalProjPost",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "buyItem",
                schema: "FinalProjPost",
                columns: table => new
                {
                    BuyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    numberOfItems = table.Column<int>(type: "int", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buyItem", x => x.BuyId);
                    table.ForeignKey(
                        name: "FK_buyItem_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "FinalProjPost",
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_buyItem_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "FinalProjUser",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_buyItem_ItemId",
                schema: "FinalProjPost",
                table: "buyItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_buyItem_UserId",
                schema: "FinalProjPost",
                table: "buyItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Registration_RegistrationId",
                schema: "FinalProjPost",
                table: "Items",
                column: "RegistrationId",
                principalSchema: "FinalProjCompanies",
                principalTable: "Registration",
                principalColumn: "RegistrationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Registration_RegistrationId",
                schema: "FinalProjPost",
                table: "Items");

            migrationBuilder.DropTable(
                name: "buyItem",
                schema: "FinalProjPost");

            migrationBuilder.RenameColumn(
                name: "NumOfItems",
                schema: "FinalProjPost",
                table: "Items",
                newName: "Quantity");

            migrationBuilder.AlterColumn<int>(
                name: "RegistrationId",
                schema: "FinalProjPost",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Registration_RegistrationId",
                schema: "FinalProjPost",
                table: "Items",
                column: "RegistrationId",
                principalSchema: "FinalProjCompanies",
                principalTable: "Registration",
                principalColumn: "RegistrationID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
