namespace SaririBackend.Classes
{
    public class Patient
    {
        public int Id {  get; set; } // Patient Id
        public string Name { get; set; } // Patient Name
        public int Age { get; set; } // Patient Age

        public int? ERContact {  get; set; } // Emergency contact of the patient if any
        
        public int UserId { get; set; } // the user ID of the patient

        public int RecordId { get; set; } // the id of the patient record

    }
}
