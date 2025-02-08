namespace SaririBackend.Classes
{
    public class EmergencyRequest
    {
        public int Id { get; set; } // the ID of the request
        public int PatientId { get; set; } // the Patient ID
        public int HospitalId { get; set; } // Hospital ID
        public string? severityLevel { get; set; } // the level of the emergency 

        public DateTime TimeOfRequest { get; set; } // the time of the request
        public Boolean Status { get; set; } // the status of the request true==was handled false== refuse to accept the request
    }
}
