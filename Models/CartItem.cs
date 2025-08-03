namespace Gadget_Basket.Models;

public class CartItem
{
    public int CartItemId { get; set; }

    public string UserId { get; set; }

    public long ProductID { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; } = 1;
}