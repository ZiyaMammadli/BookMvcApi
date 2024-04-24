namespace BookStore.DTOs;

public class BookPutDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double SalePrice { get; set; }
    public double CostPrice { get; set; }
    public IFormFile Imagefile { get; set; }
}
