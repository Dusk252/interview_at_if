using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Homework.Web.Models;
using Homework.Web.ViewModels;
using Homework.Web.Interfaces;

namespace Homework.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductApiService _productApi;

    public HomeController(ILogger<HomeController> logger, IProductApiService productApi)
    {
        _logger = logger;
        _productApi = productApi;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ProductApiResponse res = await _productApi.GetProducts();
        HomeViewModel model = new HomeViewModel();
        Dictionary<string, int> brandCount = new Dictionary<string, int>();
        if (res.products != null) {
            foreach(var product in res.products) {
                if (product.discountPercentage >= 10) {
                    // add to list to be displayed if percentage > 10
                    model.productList.Add(new ViewModels.Product {
                        id = product.id,
                        title = product.title ?? "",
                        price = product.price,
                        brand = product.brand ?? ""
                    });
                    if (product.brand == null)
                        continue;
                    if (brandCount.ContainsKey(product.brand))
                        // key exists, update count
                        brandCount[product.brand] += 1;
                    else
                        // key doesn't exist, add
                        brandCount.Add(product.brand, 1);
                }
            }
            // get key for highest val
            model.trendingBrand = brandCount.MaxBy(kv => kv.Value).Key;
        }
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
