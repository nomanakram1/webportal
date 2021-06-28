using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPortal.Models.ViewModel
{
    public class ReservationsVM
    {
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
        public string ContactPhone { get; set; }
        public string DestinationAddress { get; set; }
        public string DestinationCity { get; set; }
        public string OfficePhone { get; set; }
        public int TransportType { get; set; }
        public DateTime PickupTime { get; set; }
        public DateTime AppointmentTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public string Comments { get; set; }
        public string RequestedBy { get; set; }
        public string CallBackNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Distance { get; set; }
        public bool WillCall { get; set; }
    }
}
