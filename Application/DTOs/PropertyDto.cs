namespace Application.DTOs
{
    public class PropertyDto
    {
        public string IdProperty { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string CodeInternal { get; set; } = null!;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string Image { get; set; }
    }
}
