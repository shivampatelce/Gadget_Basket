using System.ComponentModel.DataAnnotations;

namespace Gadget_Basket.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    
    [Required(ErrorMessage = "Please enter valid category name.")]
    public string CategoryName { get; set; }
    
    public List<Product>? Products { get; set; }
    
}