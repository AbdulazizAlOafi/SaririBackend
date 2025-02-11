using System.ComponentModel.DataAnnotations;

namespace SaririBackend.Classes
{
    public class TransferService
    {
        [Key]
        public int transferID { get; set; } // the Transfer ID
        public int originHospitalID { get; set; } // the Original Hospital ID
        public int destinationHospitalID { get; set; } // the Destination Hospital ID

        public int patientID { get; set; } // the Patient ID

        public Boolean status {  get; set; } // the Status of the request

        public DateTime transferTime { get; set; } // the time of the request
    }
}
