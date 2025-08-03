using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gadget_Basket.Models;

public class Product
{
    [Key]
    public long ProductID { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    public string ProductName { get; set; }

    [Required(ErrorMessage = "Short description is required")]
    [StringLength(250, ErrorMessage = "Short description cannot exceed 250 characters")]
    public string ShortDescription { get; set; }

    [Required(ErrorMessage = "Long description is required")]
    [StringLength(1000, ErrorMessage = "Long description cannot exceed 1000 characters")]
    public string LongDescription { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [DisplayName("Price")]
    [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100000")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Category ID is required")]
    [DisplayName("Category")]
    public int CategoryID { get; set; }

    public Category? Category { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    [Url(ErrorMessage = "Invalid URL format")]
    public string ImageURL { get; set; }

    [Range(0, 100, ErrorMessage = "Discount must be between 0% and 100%")]
    public decimal Discount { get; set; }
}