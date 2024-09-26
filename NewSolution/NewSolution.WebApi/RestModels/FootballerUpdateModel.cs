namespace NewSolution.WebApi.RestModels
{
    public class FootballerUpdateModel
    {
        public string? Name { get; set; }
        public DateOnly DOB { get; set; }
        public Guid? ClubId { get; set; }
    }
}
