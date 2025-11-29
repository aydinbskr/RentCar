using RentCarServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarServer.Domain.Reservations
{
    public sealed class Reservation : Entity, IAggregate
    {
        private readonly List<ReservationExtra> _reservationExtras = new();
        private readonly List<ReservationHistory> _histories = new();
        private Reservation() { }
        private Reservation(
            Guid customerId,
            Guid pickUpLocationId,
            DateOnly pickUpDate,
            TimeOnly pickUpTime,
            DateOnly deliveryDate,
            TimeOnly deliveryTime,
            Guid vehicleId,
            decimal vehicleDailyPrice,
            Guid protectionPackageId,
            decimal protectionPackagePrice,
            IEnumerable<ReservationExtra> reservationExtras,
            string note,
            PaymentInformation paymentInformation,
            Status status,
            decimal total,
            int totalDay,
            ReservationHistory history)
        {
            SetCustomerId(customerId);
            SetPickUpLocationId(pickUpLocationId);
            SetPickUpDate(pickUpDate);
            SetPickUpTime(pickUpTime);
            SetDeliveryDate(deliveryDate);
            SetDeliveryTime(deliveryTime);
            SetVehicleId(vehicleId);
            SetVehicleDailyPrice(vehicleDailyPrice);
            SetProtectionPackageId(protectionPackageId);
            SetProtectionPackagePrice(protectionPackagePrice);
            SetReservationExtras(reservationExtras);
            SetNote(note);
            SetPaymentInformation(paymentInformation);
            SetReservationStatus(status);
            SetTotalDay(totalDay);
            SetTotal(total);
            SetPickupDateTime();
            SetDeliveryDateTime();
            SetReservationNumber();
            SetHistory(history);
        }
        public string ReservationNumber { get; private set; } = default!;
        public Guid CustomerId { get; private set; } = default!;
        public Guid PickUpLocationId { get; private set; } = default!;
        public DateOnly PickUpDate { get; private set; } = default!;
        public TimeOnly PickUpTime { get; private set; } = default!;
        public DateTime PickUpDatetime { get; private set; } = default!;
        public DateOnly DeliveryDate { get; private set; } = default!;
        public TimeOnly DeliveryTime { get; private set; } = default!;
        public DateTime DeliveryDatetime { get; private set; } = default!;
        public int TotalDay { get; private set; } = default!;
        public Guid VehicleId { get; private set; } = default!;
        public decimal VehicleDailyPrice { get; private set; } = default!;
        public Guid ProtectionPackageId { get; private set; } = default!;
        public decimal ProtectionPackagePrice { get; private set; } = default!;
        public IReadOnlyCollection<ReservationExtra> ReservationExtras => _reservationExtras;
        public string Note { get; private set; } = default!;
        public PaymentInformation PaymentInformation { get; private set; } = default!;
        public Status Status { get; private set; } = default!;
        public decimal Total { get; private set; } = default!;
        public IReadOnlyCollection<ReservationHistory> Histories => _histories;

        public static Reservation Create(
            Guid customerId,
            Guid pickUpLocationId,
            DateOnly pickUpDate,
            TimeOnly pickUpTime,
            DateOnly deliveryDate,
            TimeOnly deliveryTime,
            Guid vehicleId,
            decimal vehicleDailyPrice,
            Guid protectionPackageId,
            decimal protectionPackagePrice,
            IEnumerable<ReservationExtra> reservationExtras,
            string note,
            PaymentInformation paymentInformation,
            Status status,
            decimal total,
            int totalDay,
            ReservationHistory history)
        {
            var reservation = new Reservation(
                customerId,
                pickUpLocationId,
                pickUpDate,
                pickUpTime,
                deliveryDate,
                deliveryTime,
                vehicleId,
                vehicleDailyPrice,
                protectionPackageId,
                protectionPackagePrice,
                reservationExtras,
                note,
                paymentInformation,
                status,
                total,
                totalDay,
                history
            );

            return reservation;
        }

        #region Behaviors
        public void SetCustomerId(Guid customerId)
        {
            CustomerId = customerId;
        }

        public void SetPickUpLocationId(Guid pickUpLocationId)
        {
            PickUpLocationId = pickUpLocationId;
        }

        public void SetPickUpDate(DateOnly pickUpDate)
        {
            PickUpDate = pickUpDate;
        }

        public void SetPickUpTime(TimeOnly pickUpTime)
        {
            PickUpTime = pickUpTime;
        }

        public void SetDeliveryDate(DateOnly deliveryDate)
        {
            DeliveryDate = deliveryDate;
        }

        public void SetDeliveryTime(TimeOnly deliveryTime)
        {
            DeliveryTime = deliveryTime;
        }
        public void SetTotalDay(int totalDay)
        {
            TotalDay = totalDay;
        }
        public void SetVehicleId(Guid vehicleId)
        {
            VehicleId = vehicleId;
        }

        public void SetVehicleDailyPrice(decimal vehicleDailyPrice)
        {
            VehicleDailyPrice = vehicleDailyPrice;
        }

        public void SetProtectionPackageId(Guid protectionPackageId)
        {
            ProtectionPackageId = protectionPackageId;
        }

        public void SetProtectionPackagePrice(decimal protectionPackagePrice)
        {
            ProtectionPackagePrice = protectionPackagePrice;
        }

        public void SetReservationExtras(IEnumerable<ReservationExtra> reservationExtras)
        {
            _reservationExtras.Clear();
            _reservationExtras.AddRange(reservationExtras);
        }

        public void SetNote(string note)
        {
            Note = note;
        }

        public void SetPaymentInformation(PaymentInformation paymentInformation)
        {
            PaymentInformation = paymentInformation;
        }

        public void SetReservationStatus(Status status)
        {
            Status = status;
        }

        public void SetTotal(decimal total)
        {
            Total = total;
        }

        public void SetPickupDateTime()
        {
            var date = new DateTime(PickUpDate, PickUpTime);
            PickUpDatetime = date;
        }

        public void SetDeliveryDateTime()
        {
            var date = new DateTime(DeliveryDate, DeliveryTime);
            DeliveryDatetime = date;
        }

        private void SetReservationNumber()
        {
            var date = DateTime.Now;
            Random random = new();
            var num = string.Concat(Enumerable.Range(0, 8).Select(_ => random.Next(10)));
            string number = "RSV-" + date.Year + "-" + num;
            ReservationNumber = new(number);
        }

        public void SetHistory(ReservationHistory history)
        {
            _histories.Add(history);
        }

        //public void SetPickUpForm(Form pickUpForm)
        //{
        //    PickUpForm = pickUpForm;
        //}

        //public void SetDeliveryForm(Form deliveryForm)
        //{
        //    DeliveryForm = deliveryForm;
        //}
        #endregion
    }
}
