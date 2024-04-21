using FormsApp.Models;
using FormsApp.Models.ContextClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace FormsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Repository _rep;

        /*
         * Bu metotta, MyContext s�n�f�, ConfigureServices metodu i�erisinde AddDbContext metodu kullan�larak servisler koleksiyonuna eklenir. B�ylece, MyContext t�r�ndeki nesneler, Controller'lar�n constructor'lar�na otomatik olarak enjekte edilir ve kullan�labilir hale gelir.
        */
        public HomeController(ILogger<HomeController> logger, MyContext db)
        {
            _logger = logger;
            _rep = new Repository(db);
        }

        [HttpGet]
        public IActionResult Index(string searchString,string category)
        {
            var products = _rep.Products();
            if(!String.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(category) && category != "0")
            {    
                products = products.Where(p => p.CategoryId == int.Parse(category)).ToList();
            }

            ViewBag.Categories = new SelectList(_rep.Categories(),"CategoryId","Name");
            return View(products);
        }

        // Category(Add) --- ValidationLogic(Validasyon ge�i�leri sa�lanm�� m�?) --- DTO --- BLL(Manager)(Kurallar) --- Database

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
