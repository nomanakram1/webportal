using System;
using System.ComponentModel.DataAnnotations;

namespace WebPortal.Models
{
    public class Reservations
    {
        [Key]
        public int ReservationId { get; set; }
        public string ReservationUI { get; set; }
        public string Department { get; set; }
        public DateTime TransportDate { get; set; }
        public int CostCenterNumber { get; set; }
        public int MRN { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PickupAddress { get; set; }
        public string PickupCity { get; set; }
        public int PickupZip { get; set; }
        public string ContactPhone { get; set; }
        public string DestinationAddress { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationZip { get; set; }
        public string OfficePhone { get; set; }
        public int TransportType { get; set; }
        public DateTime PickupTime { get; set; }
        public DateTime AppointmentTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public string Comments { get; set; }
        public string RequestedBy { get; set; }
        public string CallBackNumber { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
