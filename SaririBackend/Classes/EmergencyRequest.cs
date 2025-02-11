using System.ComponentModel.DataAnnotations;

namespace SaririBackend.Classes
{
    public class EmergencyRequest
    {
        [Key]
        public int requestID { get; set; } // the ID of the request
        public int patientID { get; set; } // the Patient ID
        public int hospitalID { get; set; } // Hospital ID
        public string? severityLevel { get; set; } // the level of the emergency 

        public DateTime timeOfRequest { get; set; } // the time of the request
        public Boolean status { get; set; } // the status of the request true==was handled false== refuse to accept the request
    }
}
