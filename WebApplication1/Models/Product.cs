public class Product
{
    public int Product_id { get; set; } // Assuming Product_id is database-generated
    public required string Product_name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Image_url { get; set; }
    public int Category_id { get; set; }
}