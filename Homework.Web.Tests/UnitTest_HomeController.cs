using Microsoft.Extensions.Logging;
using Homework.Web.Controllers;
using Homework.Web.ViewModels;
using Moq;
using Moq.Protected;
using System.Net;
using Homework.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Homework.Web.Tests;

public class UnitTest_HomeController
{
    private ProductApiService MockProductApiService(StringContent test) {
        // mock httphandler to use to create the httpclient used in the service
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // response goes here
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = test,
                })
                .Verifiable();

        var mockClient = new HttpClient(mockHandler.Object);
        mockClient.BaseAddress = new Uri("https://dummyjson.com");
        mockClient.DefaultRequestHeaders.Add("Accept", "application/json");
        return new ProductApiService(mockClient);
    }

    [Fact]
    public async Task Index_CorrectlyPopulatesViewOfExpectedTypeCorrectlyBasedOnValidData()
    {
        // Arrange
        const string test = @"{""products"":[{""id"":1,""title"":""iPhone 9"",""description"":""An apple mobile which is nothing like apple"",""price"":549,""discountPercentage"":12.96,""rating"":4.69,""stock"":94,""brand"":""Apple"",""category"":""smartphones"",""thumbnail"":""https://i.dummyjson.com/data/products/1/thumbnail.jpg"",""images"":[""https://i.dummyjson.com/data/products/1/1.jpg"",""https://i.dummyjson.com/data/products/1/2.jpg"",""https://i.dummyjson.com/data/products/1/3.jpg"",""https://i.dummyjson.com/data/products/1/4.jpg"",""https://i.dummyjson.com/data/products/1/thumbnail.jpg""]},{""id"":2,""title"":""iPhone X"",""description"":""SIM-Free, Model A19211 6.5-inch Super Retina HD display with OLED technology A12 Bionic chip with ..."",""price"":899,""discountPercentage"":17.94,""rating"":4.44,""stock"":34,""brand"":""Apple"",""category"":""smartphones"",""thumbnail"":""https://i.dummyjson.com/data/products/2/thumbnail.jpg"",""images"":[""https://i.dummyjson.com/data/products/2/1.jpg"",""https://i.dummyjson.com/data/products/2/2.jpg"",""https://i.dummyjson.com/data/products/2/3.jpg"",""https://i.dummyjson.com/data/products/2/thumbnail.jpg""]},{""id"":3,""title"":""Samsung Universe 9"",""description"":""Samsung's new variant which goes beyond Galaxy to the Universe"",""price"":1249,""discountPercentage"":15.46,""rating"":4.09,""stock"":36,""brand"":""Samsung"",""category"":""smartphones"",""thumbnail"":""https://i.dummyjson.com/data/products/3/thumbnail.jpg"",""images"":[""https://i.dummyjson.com/data/products/3/1.jpg""]},{""id"":4,""title"":""OPPOF19"",""description"":""OPPO F19 is officially announced on April 2021."",""price"":280,""discountPercentage"":17.91,""rating"":4.3,""stock"":123,""brand"":""OPPO"",""category"":""smartphones"",""thumbnail"":""https://i.dummyjson.com/data/products/4/thumbnail.jpg"",""images"":[""https://i.dummyjson.com/data/products/4/1.jpg"",""https://i.dummyjson.com/data/products/4/2.jpg"",""https://i.dummyjson.com/data/products/4/3.jpg"",""https://i.dummyjson.com/data/products/4/4.jpg"",""https://i.dummyjson.com/data/products/4/thumbnail.jpg""]}],""total"":4,""skip"":0,""limit"":30}";
        var mockLogger = new Mock<ILogger<HomeController>>();
        var mockProductApiService = MockProductApiService(new StringContent(test, Encoding.UTF8, "application/json"));
        
        var controller = new HomeController(mockLogger.Object, mockProductApiService);

        // Act
        var result = await controller.Index();

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var modelResult = Assert.IsType<HomeViewModel>(viewResult.ViewData.Model);
        Assert.Equal(4, modelResult.productList.Count);
        Assert.Equal("Apple", modelResult.trendingBrand);
    }
}