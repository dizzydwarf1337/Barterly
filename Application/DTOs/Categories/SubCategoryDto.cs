namespace Application.DTOs.Categories
{
    public class SubCategoryDto
    {
        public Guid Id { get; set; }
        public string TitlePL { get; set; }
        public string TitleEN { get; set; }
        public Guid CategoryId { get; set; }
    }
}
