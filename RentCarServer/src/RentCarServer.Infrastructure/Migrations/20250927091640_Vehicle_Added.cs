using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Vehicle_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Brand = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Model = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    ModelYear = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Plate = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VinNumber = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    EngineNumber = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Description = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    FuelType = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Transmission = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    EngineVolume = table.Column<decimal>(type: "money", nullable: false),
                    EnginePower = table.Column<int>(type: "int", nullable: false),
                    TractionType = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    FuelConsumption = table.Column<decimal>(type: "money", nullable: false),
                    SeatCount = table.Column<int>(type: "int", nullable: false),
                    Kilometer = table.Column<int>(type: "int", nullable: false),
                    DailyPrice = table.Column<decimal>(type: "money", nullable: false),
                    WeeklyDiscountRate = table.Column<decimal>(type: "money", nullable: false),
                    MonthlyDiscountRate = table.Column<decimal>(type: "money", nullable: false),
                    InsuranceType = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    LastMaintenanceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastMaintenanceKm = table.Column<int>(type: "int", nullable: false),
                    NextMaintenanceKm = table.Column<int>(type: "int", nullable: false),
                    InspectionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InsuranceEndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CascoEndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TireStatus = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    GeneralStatus = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "varchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => new { x.VehicleId, x.Id });
                    table.ForeignKey(
                        name: "FK_Feature_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feature");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
