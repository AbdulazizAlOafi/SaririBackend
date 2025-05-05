using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaririBackend.Classes
{
    [Index(nameof(paitentNationalID), IsUnique = true)] // Enforce uniqueness
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int patientID {  get; set; } // Patient Id
        public string paitentName { get; set; } // Patient Name
        public string paitentNationalID { get; set; } // Patient Age

        public string? emergencyContact {  get; set; } // Emergency contact of the patient if any
        
        public int? userID { get; set; } // the user ID of the patient

        public int? recordID { get; set; } // the id of the patient record

    }
}
