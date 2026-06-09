using System.ComponentModel.DataAnnotations;

namespace dotNetMVCWebApp1.Models.DTO;

public class CreateProductRequest
{
    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Product price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
}