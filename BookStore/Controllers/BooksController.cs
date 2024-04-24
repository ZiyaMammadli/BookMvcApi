using AutoMapper;
using BookStore.Data;
using BookStore.DTOs;
using BookStore.Entities;
using BookStore.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public BooksController(BookDbContext context,IMapper mapper,IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        [HttpGet]
        public async Task <IActionResult> GetAll()
        {
            List<BookGetDto> bookGetDtos = new List<BookGetDto>();
            var books = await _context.books.ToListAsync();
            foreach (var book in books)
            {
                BookGetDto bookDto =_mapper.Map<BookGetDto>(book);
                bookGetDtos.Add(bookDto);
            }
            return Ok(bookGetDtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            try
            {
                var book = await _context.books.FindAsync(id);
                if (book is null) throw new NotFoundException(404, "Book is not found");
                return Ok(_mapper.Map<BookGetDto>(book));
            }
            catch(NotFoundException ex) 
            {
                return StatusCode(ex.statusCode,ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookPostDto bookPostDto)
        {
            try
            {
                string HostUrl = $"{Request.Scheme}://{Request.Host}";
                string fileName = null;
                string path = Path.Combine(_env.WebRootPath, "Uploads/book", bookPostDto.Imagefile.FileName);
                fileName = HostUrl + "/Uploads/Book/" + bookPostDto.Imagefile.FileName;
                using(FileStream stream=new FileStream(path, FileMode.Create))
                {
                    bookPostDto.Imagefile.CopyTo(stream);
                }
                var book=_mapper.Map<Book>(bookPostDto);
                book.CreatedDate = DateTime.Now;
                book.UpdatedDate = DateTime.Now;
                book.ImageUrl = fileName;
                await _context.books.AddAsync(book);
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<BookGetDto>(book));

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(BookPutDto bookPutDto)
        {
            try
            {
                if (!await _context.books.AnyAsync(b => b.Id == bookPutDto.Id)) throw new NotFoundException(404, "Book is not found");
                var currentBook = await _context.books.FirstOrDefaultAsync(b => b.Id == bookPutDto.Id);
                if (currentBook is null) throw new NotFoundException(404, "Book is not found");
                if (bookPutDto.Imagefile is not null)
                {
                    string HostUrl = $"{Request.Scheme}://{Request.Host}";
                    string fileName = null;
                    string path = Path.Combine(_env.WebRootPath, "Uploads/book", bookPutDto.Imagefile.FileName);
                    fileName = HostUrl + "/Uploads/Book/" + bookPutDto.Imagefile.FileName;
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        bookPutDto.Imagefile.CopyTo(stream);
                    }
                    
                    if (System.IO.File.Exists(currentBook.ImageUrl))
                    {
                        System.IO.File.Delete(currentBook.ImageUrl);
                    }
                    currentBook.ImageUrl=fileName;
                }
                currentBook.SalePrice = bookPutDto.SalePrice;
                currentBook.CostPrice = bookPutDto.CostPrice;
                currentBook.Name= bookPutDto.Name;
                currentBook.UpdatedDate=DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<BookGetDto>(currentBook));
            }
            catch (NotFoundException ex)
            {
                return StatusCode(ex.statusCode, ex.Message);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete( int id)
        {
            try
            {
                var book = await _context.books.FindAsync(id);
                if (book is null) throw new NotFoundException(404, "Book is not found");
                _context.books.Remove(book);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(NotFoundException ex)
            {
                return StatusCode(ex.statusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

    }
}
