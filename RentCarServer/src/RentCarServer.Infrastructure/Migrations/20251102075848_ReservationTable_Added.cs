using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReservationTable_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationNumber = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickUpLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickUpDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PickUpTime = table.Column<TimeOnly>(type: "time(7)", nullable: false),
                    PickUpDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DeliveryTime = table.Column<TimeOnly>(type: "time(7)", nullable: false),
                    DeliveryDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalDay = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleDailyPrice = table.Column<decimal>(type: "money", nullable: false),
                    ProtectionPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProtectionPackagePrice = table.Column<decimal>(type: "money", nullable: false),
                    Note = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    PaymentInformation_CartNumber = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    PaymentInformation_Owner = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Status_Value = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Total = table.Column<decimal>(type: "money", nullable: false),
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
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
