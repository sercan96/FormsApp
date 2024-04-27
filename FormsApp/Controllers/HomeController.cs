using FormsApp.Models;
using FormsApp.Models.ContextClasses;
using FormsApp.Models.Entities;
using FormsApp.Models.VMModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.IO;

namespace FormsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Repository _rep;

        /*
         * Bu metotta, MyContext sýnýfý, ConfigureServices metodu içerisinde AddDbContext metodu kullanýlarak servisler koleksiyonuna eklenir. Böylece, MyContext türündeki nesneler, Controller'larýn constructor'larýna otomatik olarak enjekte edilir ve kullanýlabilir hale gelir.
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

            var productVM = new ProductViewModel()
            {
                Products = products,
                Categories = _rep.Categories(),
                SelectedCategory = category,
            };

            //ViewBag.Categories = new SelectList(_rep.Categories(),"CategoryId","Name",category);
            return View(productVM);
        }

        // Category(Add) --- ValidationLogic(Validasyon geçiþleri saðlanmýþ mý?) --- DTO --- BLL(Manager)(Kurallar) --- Database

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_rep.Categories(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product model, IFormFile imageFile)
        {

            var extension = "";

            if(imageFile != null)
            {
                var allowedExtension = new[] { ".jpg", ".png", ".jpeg" };
                extension = Path.GetExtension(imageFile?.FileName);
                if (!allowedExtension.Contains(extension))
                {
                    ModelState.AddModelError("", "Geçerli bir resim yükleyiniz");
                }
            }

            if(ModelState.IsValid)
            {
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()} {extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/img", randomFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.Image = randomFileName;
                _rep.Add(model);
                _rep.Save();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var entity = _rep.Products().FirstOrDefault(x => x.ProductId == id);
            if(entity == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_rep.Categories(), "CategoryId", "Name");
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id,Product model,IFormFile? imageFile)
        {
            if(id != model.ProductId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var extension = Path.GetExtension(imageFile?.FileName);
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()} {extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/img", randomFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }


                    model.Image = randomFileName;

                }
                ViewBag.Categories = new SelectList(_rep.Categories(), "CategoryId", "Name");
                _rep.EditProduct(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }


        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var entity = _rep.Products().FirstOrDefault(p => p.ProductId == id);

            if(entity == null)
            {
                return NotFound();
            }

            return View("DeleteToConfirm",entity);
        }
        [HttpPost]
        public IActionResult Delete(int id,int productId) 
        {
            if (id != productId)
            {
                return NotFound();
            }

            var entity = _rep.Products().FirstOrDefault(p => p.ProductId == productId);

            if (entity == null)
            {
                return NotFound();
            }
            _rep.Delete(entity);
            _rep.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditProducts(List<Product> products)
        {
            foreach (var product in products)
            {
                _rep.EditIsActives(product);
            }
            return RedirectToAction("Index");   
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
