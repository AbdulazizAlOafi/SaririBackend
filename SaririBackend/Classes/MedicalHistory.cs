using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaririBackend.Classes
{
    public class MedicalHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int recordID { get; set; } // the ID of the record
        public int patientID { get; set; } // the ID of the patient associated with the record

        public string? conditions { get; set; } // the Condition of the patient
        public string? medications { get; set; } // the medications of the patient
       
        public DateTime lastUpdated { get; set; } = DateTime.UtcNow; // last time the record was updated
    }
}
