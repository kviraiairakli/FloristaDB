using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Orders", Schema = "dbo")]
    public class Order
    {
        public Order()
        {
            OrderId = string.Empty;
            UserId = string.Empty;
            OrderDate = DateTime.Now; // Initialize date
        }

        [Key]
        [Column("Order_id")]
        [StringLength(5)]
        public string OrderId { get; set; }

        [Column("Order_date", TypeName = "date")]
        public DateTime OrderDate { get; set; }

        [Column("User_id")]
        [StringLength(5)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User? User { get; set; } // Mark as nullable if FK can be null, otherwise ensure always loaded

        [Column("Order_price", TypeName = "money")]
        public decimal OrderPrice { get; set; }
    }
}