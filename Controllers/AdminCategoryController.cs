using Microsoft.AspNetCore.Mvc;
using ShopWeb.Data;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels;

namespace ShopWeb.Controllers
{
    public class AdminCategoryController : Controller
    {
        private readonly ShopWebDbContext shopWebDbContext;
        public AdminCategoryController(ShopWebDbContext shopWebDbContext) 
        {
            this.shopWebDbContext = shopWebDbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddCategoryRequest addCategoryRequest)
        {
            var model = new Category
            {
                Name = addCategoryRequest.Name,
                Description = addCategoryRequest.Description
            };
            shopWebDbContext.Categories.Add(model);
            shopWebDbContext.SaveChanges();
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult List() 
        {
            var models = shopWebDbContext.Categories.ToList();
            return View(models);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var exist = shopWebDbContext.Categories.FirstOrDefault(x => x.Id == id);
            var model = new EditCategoryRequest
            {
                Id = exist.Id,
                Name = exist.Name,
                Description = exist.Description
            };
            
            return View(model);
        }
    }
}
