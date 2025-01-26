using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjAPI.Migrations
{
    /// <inheritdoc />
    public partial class BookTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "FinalProjPost");

            migrationBuilder.EnsureSchema(
                name: "FinalProjCompanies");

            migrationBuilder.EnsureSchema(
                name: "FinalProjUser");

            migrationBuilder.CreateTable(
                name: "CompanyTypes",
                schema: "FinalProjCompanies",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTypes", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "FinalProjUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    passwordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    passwordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                schema: "FinalProjCompanies",
                columns: table => new
                {
                    RegistrationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    passwordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    passwordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.RegistrationID);
                    table.ForeignKey(
                        name: "FK_Registration_CompanyTypes_TypeID",
                        column: x => x.TypeID,
                        principalSchema: "FinalProjCompanies",
                        principalTable: "CompanyTypes",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                schema: "FinalProjPost",
                columns: table => new
                {
                    CarID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Make = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TransmissionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DailyRate = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ImageURL = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    RegistrationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarID);
                    table.ForeignKey(
                        name: "FK_Cars_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalSchema: "FinalProjCompanies",
                        principalTable: "Registration",
                        principalColumn: "RegistrationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "FinalProjPost",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalSchema: "FinalProjCompanies",
                        principalTable: "Registration",
                        principalColumn: "RegistrationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Users_userId",
                        column: x => x.userId,
                        principalSchema: "FinalProjUser",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                schema: "FinalProjPost",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: true),
                    CompanyRegistrationID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_Reviews_Registration_CompanyRegistrationID",
                        column: x => x.CompanyRegistrationID,
                        principalSchema: "FinalProjCompanies",
                        principalTable: "Registration",
                        principalColumn: "RegistrationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "FinalProjUser",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                schema: "FinalProjPost",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: true),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageURL = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    NumOfTourist = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_Trips_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalSchema: "FinalProjCompanies",
                        principalTable: "Registration",
                        principalColumn: "RegistrationID");
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                schema: "FinalProjPost",
                columns: table => new
                {
                    RentalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RentalStartDate = table.Column<DateTime>(type: "date", nullable: false),
                    RentalEndDate = table.Column<DateTime>(type: "date", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.RentalID);
                    table.ForeignKey(
                        name: "FK_Rentals_Cars_CarID",
                        column: x => x.CarID,
                        principalSchema: "FinalProjPost",
                        principalTable: "Cars",
                        principalColumn: "CarID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rentals_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "FinalProjUser",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookTrip",
                schema: "FinalProjPost",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    numberOfPersons = table.Column<int>(type: "int", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookTrip", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_bookTrip_Trips_TripId",
                        column: x => x.TripId,
                        principalSchema: "FinalProjPost",
                        principalTable: "Trips",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookTrip_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "FinalProjUser",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookTrip_TripId",
                schema: "FinalProjPost",
                table: "bookTrip",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_bookTrip_UserId",
                schema: "FinalProjPost",
                table: "bookTrip",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_RegistrationId",
                schema: "FinalProjPost",
                table: "Cars",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_RegistrationId",
                schema: "FinalProjPost",
                table: "Items",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_userId",
                schema: "FinalProjPost",
                table: "Items",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_TypeID",
                schema: "FinalProjCompanies",
                table: "Registration",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CarID",
                schema: "FinalProjPost",
                table: "Rentals",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_UserId",
                schema: "FinalProjPost",
                table: "Rentals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CompanyRegistrationID",
                schema: "FinalProjPost",
                table: "Reviews",
                column: "CompanyRegistrationID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                schema: "FinalProjPost",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RegistrationId",
                schema: "FinalProjPost",
                table: "Trips",
                column: "RegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookTrip",
                schema: "FinalProjPost");

            migrationBuilder.DropTable(
                name: "Items",
                schema: "FinalProjPost");

            migrationBuilder.DropTable(
                name: "Rentals",
                schema: "FinalProjPost");

            migrationBuilder.DropTable(
                name: "Reviews",
                schema: "FinalProjPost");

            migrationBuilder.DropTable(
                name: "Trips",
                schema: "FinalProjPost");

            migrationBuilder.DropTable(
                name: "Cars",
                schema: "FinalProjPost");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "FinalProjUser");

            migrationBuilder.DropTable(
                name: "Registration",
                schema: "FinalProjCompanies");

            migrationBuilder.DropTable(
                name: "CompanyTypes",
                schema: "FinalProjCompanies");
        }
    }
}
