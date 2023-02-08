using Homework.Web.Interfaces;
using Homework.Web.Models;

namespace Homework.Web.Services;

public class ProductApiService : IProductApiService {

    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient client) {
        _httpClient = client;
    }

    public async Task<ProductApiResponse> GetProducts() {
        using HttpResponseMessage res = await _httpClient.GetAsync("/products?select=id,title,price,discountPercentage,brand");

        if (!res.IsSuccessStatusCode)
            throw new Exception("API call has failed.");

        var obj =  await res.Content.ReadFromJsonAsync<ProductApiResponse>();

        if (obj == null)
            throw new Exception("API call returned no data / data in an invalid format.");

        return obj;
    }

}