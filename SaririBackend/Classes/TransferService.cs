namespace SaririBackend.Classes
{
    public class TransferService
    {
        public int Id { get; set; } // the Transfer ID
        public int OriginHospitalID { get; set; } // the Original Hospital ID
        public int DestinationHospitalID { get; set; } // the Destination Hospital ID

        public int PatientID { get; set; } // the Patient ID

        public Boolean Status {  get; set; } // the Status of the request

        public DateTime TransferTime { get; set; } // the time of the request
    }
}
