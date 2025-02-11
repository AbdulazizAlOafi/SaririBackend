using System.ComponentModel.DataAnnotations;

namespace SaririBackend.Classes
{
    public class Patient
    {
        [Key]
        public int patientID {  get; set; } // Patient Id
        public string paitentName { get; set; } // Patient Name
        public int paitentAge { get; set; } // Patient Age

        public int? emergencyContact {  get; set; } // Emergency contact of the patient if any
        
        public int? userID { get; set; } // the user ID of the patient

        public int? recordID { get; set; } // the id of the patient record

    }
}
