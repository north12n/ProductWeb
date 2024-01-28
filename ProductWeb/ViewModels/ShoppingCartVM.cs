

public class ShoppingCartVM
{
    public OrderHeader OrderHeader { get; set; } //ราคารวม
    public IEnumerable<ShoppingCart> ListCart { get; set; }
    public IFormFile file { get; set; }
}
