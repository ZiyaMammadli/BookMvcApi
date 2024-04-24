namespace BookStoreMVC.ViewModels;

public class BookPostViewModel
{
    public string Name { get; set; }
    public double SalePrice { get; set; }
    public double CostPrice { get; set; }
    public IFormFile Imagefile { get; set; }
}
