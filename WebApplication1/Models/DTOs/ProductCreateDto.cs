using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(20)]
        public string ProductCategory { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(3)]
        public string ProductQuantity { get; set; } = string.Empty;

        [Required]
        public decimal ProductPrice { get; set; }

        [StringLength(255)]
        public string ProductImagePath { get; set; } = string.Empty;
    }
}