using Homework.Web.Models;

namespace Homework.Web.Interfaces;

public interface IProductApiService {
    Task<ProductApiResponse> GetProducts();
}