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

        List<LinkModel> rels = new()
        {
            new (_urlHelper.Link(nameof(CreateProduct), new { })!, rel: "self", method: "POST"),
            new (_urlHelper.Link(nameof(GetProducts), new { })!, rel: "get-products", method: "GET"),
            new (_urlHelper.Link(nameof(GetProduct), new { product.Id })!, rel: "get-product", method: "GET"),
            new (_urlHelper.Link(nameof(PutProduct), new { id = product.Id })!, rel: "update-product", method: "PUT"),
            new (_urlHelper.Link(nameof(DeleteProduct), new { id = product.Id })!, rel: "delete-product", method: "DELETE")
        };

        var productHateoas = LinksGenerateHateoas(product, rels.ToArray());

        return Ok(productHateoas);
    }

    [HttpGet("{id}", Name = nameof(GetProduct))]
    public IActionResult GetProduct([FromRoute] Guid id)
    {
        var product = new Product("Macbook Pro M2", 15.000m);

        List<LinkModel> rels = new()
        {
            new (_urlHelper.Link(nameof(GetProduct), new { product.Id })!, rel: "self", method: "GET"),
            new (_urlHelper.Link(nameof(PutProduct), new { id = product.Id })!, rel: "update-product", method: "PUT"),
            new (_urlHelper.Link(nameof(DeleteProduct), new { id = product.Id })!, rel: "delete-product", method: "DELETE")
        };

        var productHateoas = LinksGenerateHateoas(product, rels.ToArray());

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
            List<LinkModel> rels = new()
            {
                new (_urlHelper.Link(nameof(GetProducts), new { })!, rel: "self", method: "GET"),
                new (_urlHelper.Link(nameof(GetProduct), new { product.Id })!, rel: "get-product", method: "GET"),
                new (_urlHelper.Link(nameof(PutProduct), new { id = product.Id })!, rel: "update-product", method: "PUT"),
                new (_urlHelper.Link(nameof(DeleteProduct), new { id = product.Id })!, rel: "delete-product", method: "DELETE")
            };

            var productHateoas = LinksGenerateHateoas(product, rels.ToArray());
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

        List<LinkModel> rels = new()
        {
            new (_urlHelper.Link(nameof(PutProduct), new { id = product.Id })!, rel: "self", method: "PUT"),
            new (_urlHelper.Link(nameof(GetProduct), new { product.Id })!, rel: "get-product", method: "GET"),
            new (_urlHelper.Link(nameof(DeleteProduct), new { id = product.Id })!, rel: "delete-product", method: "DELETE")
        };

        var productHateoas = LinksGenerateHateoas(product, rels.ToArray());
        
        return Ok(productHateoas);
    }

    [HttpDelete("{id}", Name = nameof(DeleteProduct))]
    public IActionResult DeleteProduct([FromRoute] Guid id)
    {
        List<LinkModel> rels = new()
        {
            new (_urlHelper.Link(nameof(DeleteProduct), new { id })!, rel: "self", method: "DELETE"),
            new (_urlHelper.Link(nameof(CreateProduct), new { })!, rel: "create-product", method: "POST"),
            new (_urlHelper.Link(nameof(GetProducts), new { })!, rel: "get-products", method: "GET")
        };

        var productHateoas = LinksGenerateHateoas(null, rels.ToArray());
        return Ok(productHateoas);
    }

    private object LinksGenerateHateoas(Product? product, LinkModel[] rels) 
    {
        return new ResourceHateoas<Product>()
            .GenerateLinksHateoas(product, rels, "product");
    }
}