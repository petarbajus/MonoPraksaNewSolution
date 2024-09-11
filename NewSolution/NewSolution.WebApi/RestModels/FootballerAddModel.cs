namespace NewSolution.WebApi.RestModels
{
    public class FootballerAddModel
    {
        public string Name { get; set; } 
        public DateOnly? DOB { get; set; }        // Date of birth (optional)
        public Guid? ClubId { get; set; }      
    }
}
