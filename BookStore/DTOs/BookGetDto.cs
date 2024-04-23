using Microsoft.Identity.Client;

namespace BookStore.DTOs;

public class BookGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public double SalePrice { get; set; }  

}
