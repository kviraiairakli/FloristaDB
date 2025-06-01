public class Order
{
    public int? Order_id { get; set; } // Made nullable
    public DateTime Order_date { get; set; }
    public decimal Total_amount { get; set; }
    public int? User_id { get; set; } // Made nullable
}