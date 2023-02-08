namespace Homework.Web.ViewModels;

public class HomeViewModel {
    public List<Product> productList { get; set; }
    public string trendingBrand { get; set; }

    public HomeViewModel() {
        productList = new List<Product>();
        trendingBrand = "";
    }
}

public class Product {
    public int id { get; set; }
    public string? title { get; set; }
    public double price { get; set; }
    public string? brand { get; set; }
}