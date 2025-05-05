using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaririBackend.Classes
{
    public class EmergencyRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int requestID { get; set; } // the ID of the request
        public int patientID { get; set; } // the Patient ID
        public int hospitalID { get; set; } // Hospital ID
        public string? severityLevel { get; set; } // the level of the emergency 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime timeOfRequest { get; set; } // the time of the request
        public bool? status { get; set; } // the status of the request true==was handled false== refuse to accept the request
    }
}
