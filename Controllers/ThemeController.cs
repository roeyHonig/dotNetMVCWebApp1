using Microsoft.AspNetCore.Mvc;

namespace dotNetMVCWebApp1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThemeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetTheme()
    {
        var theme = Request.Cookies["theme"] ?? "not-detected";

        return Ok(new
        {
            message = "Theme detected from cookie",
            theme
        });
    }
}

