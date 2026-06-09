using dotNetMVCWebApp1.Models;
using dotNetMVCWebApp1.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotNetMVCWebApp1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Products3Controller : ControllerBase
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
    [Authorize(Roles = "Admin,User")]
    public IActionResult GetProducts()
    {
        return Ok(products);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public IActionResult GetProductById(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(CreateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            // Automatically returns 400 Bad Request with the validation errors
            return BadRequest(ModelState);
        }
        
        var product = new Product
        {
            Id = products.Max(p => p.Id) + 1,
            Name = request.Name,
            Price = request.Price
        };

        products.Add(product);

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = product.Id },
            product);
    }
}