namespace BookStore.Exceptions;

public class NotFoundException : Exception
{
    public int statusCode { get; set; }
    public NotFoundException()
    {

    }
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(int StatusCode, string message):base(message)
    {
        statusCode = StatusCode;
    }
}
