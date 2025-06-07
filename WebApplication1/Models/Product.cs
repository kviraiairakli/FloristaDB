using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Products", Schema = "dbo")]
    public class Product
    {
        public Product()
        {
            ProductId = string.Empty;
            ProductCategory = string.Empty;
            ProductName = string.Empty;
            ProductQuantity = string.Empty; // Consider changing to int and updating DB
            ProductImagePath = string.Empty;
        }

        [Key]
        [Column("Product_id")]
        [StringLength(2)]
        public string ProductId { get; set; }

        [Column("Product_category")]
        [StringLength(20)]
        public string ProductCategory { get; set; }

        [Column("Product_name")]
        [StringLength(60)]
        public string ProductName { get; set; }

        [Column("Product_quantity")]
        [StringLength(3)] // Reconsider if this should be int
        public string ProductQuantity { get; set; }

        [Column("Product_price", TypeName = "money")]
        public decimal ProductPrice { get; set; }

        [Column("Product_ImagePath")]
        [StringLength(255)]
        public string ProductImagePath { get; set; }
    }
}