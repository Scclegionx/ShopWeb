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
        [HttpPost]
        public IActionResult Edit(EditCategoryRequest editCategoryRequest)
        {
            var model = new Category
            {
                Id = editCategoryRequest.Id,
                Name = editCategoryRequest.Name,
                Description = editCategoryRequest.Description
            };
            var exist = shopWebDbContext.Categories.Find(model.Id);
            if (exist != null)
            {
                exist.Name = model.Name;
                exist.Description = model.Description;
                shopWebDbContext.SaveChanges();
                return RedirectToAction("List");   
            }
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
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
        [HttpPost]
        public IActionResult Delete(EditCategoryRequest editCategoryRequest)
        {
            var exist = shopWebDbContext.Categories.Find(editCategoryRequest.Id);
            if (exist != null)
            {
                shopWebDbContext.Categories.Remove(exist);
                shopWebDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("List");
        }
    }
}
