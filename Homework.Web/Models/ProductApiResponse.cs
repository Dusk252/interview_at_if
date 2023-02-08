namespace Homework.Web.Models;

public class Product {
    public int id { get; set; }
    public string? title { get; set; }
    public double price { get; set; }
    public double discountPercentage { get; set; }
    public string? brand { get; set; }
}

public class ProductApiResponse {
    public List<Product>? products { get; set; }
}