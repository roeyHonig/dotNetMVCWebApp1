using dotNetMVCWebApp1.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotNetMVCWebApp1.Controllers;

[ApiController]
[Route("api/[controller]")] //  api/products2
public class Products2Controller : ControllerBase
{
    private static readonly List<Product> products =
        [
            new Product
            {
                Id = 1,
                Name = "Keyboard",
                Price = 100
            },

            new Product
            {
                Id = 2,
                Name = "Mouse",
                Price = 50
            },

            new Product
            {
                Id = 3,
                Name = "Monitor",
                Price = 900
            }
        ];

    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("search")]
    public IActionResult Search(string? name)
    {
        var results = products;

        if (!string.IsNullOrWhiteSpace(name))
        {
            results = products
                .Where(p => p.Name.Contains(name,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return Ok(results);
    }

    [HttpGet("filter")]
    public IActionResult Filter(
        decimal? minPrice,
        decimal? maxPrice)
    {
        var query = products.AsEnumerable();

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice);

        return Ok(query);
    }
}