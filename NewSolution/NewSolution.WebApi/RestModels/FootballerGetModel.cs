namespace NewSolution.WebApi.RestModels
{
    public class FootballerGetModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }       
        public DateOnly? DOB { get; set; }       
        //public string? ClubName { get; set; }
        public Guid? ClubId { get; set; }

    }
}
