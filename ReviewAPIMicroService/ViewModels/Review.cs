namespace ReviewAPIMicroService.ViewModels
{
    public class Review
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
