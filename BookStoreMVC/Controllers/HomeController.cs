using BookStoreMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BookStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        Uri baseAdress = new Uri("https://localhost:7169/api");
        private readonly HttpClient _httpClient;
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAdress;
        }

        public async Task< IActionResult> Index()
        {
           List< BookGetViewModel> bookGetViewModels = new List< BookGetViewModel>();
            var responseMessage = await _httpClient.GetAsync(baseAdress + "/books/getall");
            if(responseMessage.IsSuccessStatusCode) 
            { 
                var datas=await responseMessage.Content.ReadAsStringAsync();
                bookGetViewModels=JsonConvert.DeserializeObject<List<BookGetViewModel>>(datas);
            }
            return View(bookGetViewModels);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(BookPostViewModel bookPostViewModel)
        {
            var dataStr=JsonConvert.SerializeObject(bookPostViewModel);
            var stringContent=new StringContent(dataStr,Encoding.UTF8,"application/json");
            var responseMessage = await _httpClient.PostAsync(baseAdress + "/books/Create", stringContent);
            if(responseMessage.IsSuccessStatusCode)
            {
                TempData["successed"] = "Book succesfuly created";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]                
        public async Task<IActionResult> Delete(int id)
        {
            var responseMessage = await _httpClient.DeleteAsync(baseAdress + "/books/Delete/"+id);
            if (responseMessage.IsSuccessStatusCode)
            {
                TempData["successed"] = "Book succesfuly deleted";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
