using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ShoppingCart
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; }            // เอามาโชว์

    [Range(1, 1000, ErrorMessage = "Please enter a value between {1} and{2}")]
    public int Count { get; set; }


    public string UserId { get; set; }

    [ForeignKey("UserId")]
    [ValidateNever]
    public User User { get; set; }
}