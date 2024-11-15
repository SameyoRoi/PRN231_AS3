using BO;

namespace PE_SE173338_PE.DTO
{
    public class CategoryDTO
    {
        public string categoryId { get; set; } = null!;

        public string categoryName { get; set; } = null!;

        public string CategoryDescription { get; set; } = null!;

        public string? FromCountry { get; set; }
        
       // public virtual ICollection<SilverJewelryDTO> SilverJewelries { get; set; } = new List<SilverJewelryDTO>();
    }
}
