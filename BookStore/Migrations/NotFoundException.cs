using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookStore.Migrations;

public class NotFoundException:Exception
{
    public int statusCode {  get; set; }    
    public NotFoundException()
    {
        
    }
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(int StatusCode,string message)
    {
        statusCode = StatusCode;
    }
}
