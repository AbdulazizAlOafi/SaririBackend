namespace SaririBackend.Classes
{
    public class MedicalHistory
    {
        public int Id { get; set; } // the ID of the record
        public int PatientId { get; set; } // the ID of the patient associated with the record

        public string? Condition { get; set; } // the Condition of the patient
        public string? Medications { get; set; } // the medications of the patient

        public DateTime lastUpdated { get; set; } // last time the record was updated
    }
}
