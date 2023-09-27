using Microsoft.AspNetCore.Mvc;
using rest_api_hateoas.Domain.Dtos;
using rest_api_hateoas.Domain.Entities;
using rest_api_hateoas.Hateoas;

namespace rest_api_hateoas.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IUrlHelper _urlHelper;

    public ProductsController(IUrlHelper urlHelper)
    {
        _urlHelper = urlHelper;
    }

    [HttpPost(Name = nameof(CreateProduct))]
    public IActionResult CreateProduct([FromBody] ProductDto model)
    {
        var product = new Product(model.Name, model.Price);

        var productHateoas = LinksGenerateHateoas(product, "create-product");

        return Ok(productHateoas);
    }

    [HttpGet("{id}", Name = nameof(GetProduct))]
    public IActionResult GetProduct([FromRoute] Guid id)
    {
        var product = new Product("Macbook Pro M2", 15.000m);

        var productHateoas = LinksGenerateHateoas(product, "get-product");

        return Ok(productHateoas);
    }

    [HttpGet(Name = nameof(GetProducts))]
    public IActionResult GetProducts()
    {
        List<Product> products = new () {
            new Product("Macbook Pro M2", 15.000m),
            new Product("Iphone 15 Pro Max", 6.800m),
            new Product("Keychron K3 V2", 350m),
        };

        List<object> listProductsHateoas = new();

        foreach(var product in products) 
        {
            var productHateoas = LinksGenerateHateoas(product, "get-products");
            listProductsHateoas.Add(productHateoas);
        }

        var productList = new {
            Products = listProductsHateoas.ToArray()
        };

        return Ok(productList);
    }

    [HttpPut("{id}", Name = nameof(PutProduct))]
    public IActionResult PutProduct([FromRoute] Guid id, [FromBody] ProductDto model)
    {
        var product = new Product(model.Name, model.Price);

        var productHateoas = LinksGenerateHateoas(product, "update-product");

        
        return Ok(productHateoas);
    }

    [HttpDelete("{id}", Name = nameof(DeleteProduct))]
    public IActionResult DeleteProduct([FromRoute] Guid id)
    {
        var productHateoas = LinksGenerateHateoas(null, "delete-product");
        return Ok(productHateoas);
    }

    private object LinksGenerateHateoas(Product? product, string rel) 
    {
        List<LinkModel> links = new()
        {
            new (_urlHelper.Link(nameof(CreateProduct), new { })!, rel: "create-product", method: "POST"),
            new (_urlHelper.Link(nameof(GetProducts), new { })!, rel: "get-products", method: "GET"),
            new (_urlHelper.Link(nameof(GetProduct), new { product?.Id })!, rel: "get-product", method: "GET"),
            new (_urlHelper.Link(nameof(PutProduct), new { id = product?.Id })!, rel: "update-product", method: "PUT"),
            new (_urlHelper.Link(nameof(DeleteProduct), new { id = product?.Id })!, rel: "delete-product", method: "DELETE")
        };

        return new ResourceHateoas<Product>()
            .GenerateLinksHateoas(product, links.ToArray(), "product", rel);
    }
}