using Microsoft.AspNetCore.Mvc;

namespace dotNetMVCWebApp1.Controllers;

[ApiController]
[Route("api/[controller]")] // If the class name is "ProductsController" the route will be "api/products"
// ControllerBase and not Controller because it's only API and doesn't need view support
public class ProductsController : ControllerBase
{
    [HttpGet] // GET request
    public IActionResult GetProducts()
    {
        return Ok(new[]
        {
            "Keyboard",
            "Mouse",
            "Monitor"
        });
    }
}