using Microsoft.AspNetCore.Mvc;

namespace MyMvcApp.Controllers;

[ApiController]
[Route("api/[controller]")]
// ControllerBase and not Controller because it's only API and doesn't need view support
public class ProductsController : ControllerBase
{
    [HttpGet]
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