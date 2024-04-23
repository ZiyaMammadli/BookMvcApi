using BookStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class BookDbContext:DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

    public DbSet<Book> books { get; set; }

}
