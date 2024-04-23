using AutoMapper;
using BookStore.Data;
using BookStore.DTOs;
using BookStore.Entities;
using BookStore.Migrations;
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
                fileName = HostUrl+"/Uploads/Book/"+ Guid.NewGuid().ToString() + bookPostDto.Imagefile.FileName;
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
        [HttpPost]
        public Task<IActionResult> Update()
        {

        }

    }
}
