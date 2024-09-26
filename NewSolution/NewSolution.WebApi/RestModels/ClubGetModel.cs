namespace NewSolution.WebApi.RestModels
{
    public class ClubGetModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? CharacteristicColor { get; set; }
        public DateOnly? FoundationDate { get; set; }
        //public List<String>? footballerNames { get; set; }
    }
}
    