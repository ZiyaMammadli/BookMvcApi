namespace BookStore.Entities;

public class Book:BaseEntity
{
    public string Name { get; set; }    
    public double SalePrice { get; set; }  
    public double CoatPrice {  get; set; }
    public string ImageUrl { get; set; }    
}
