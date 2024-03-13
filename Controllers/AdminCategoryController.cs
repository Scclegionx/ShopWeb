using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.CategoryVM;
using ShopWeb.Repositories;


namespace ShopWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly ICateRepository cateRepository;
        public AdminCategoryController(ICateRepository cateRepository) 
        {
            this.cateRepository = cateRepository;
        }
        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryRequest addCategoryRequest)
        {
            ValidateCategory(addCategoryRequest);
            if (ModelState.IsValid == false)
            {
                return View();
            }

            var model = new Category
            {
                Name = addCategoryRequest.Name,
                Description = addCategoryRequest.Description
            };

            await cateRepository.AddAsync(model);
            
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> List() 
        {
            var models = await cateRepository.GetAllAsync();
            return View(models);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var exist = await cateRepository.GetAsync(id);
            var model = new EditCategoryRequest
            {
                Id = exist.Id,
                Name = exist.Name,
                Description = exist.Description
            };
            
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryRequest editCategoryRequest)
        {
            var model = new Category
            {
                Id = editCategoryRequest.Id,
                Name = editCategoryRequest.Name,
                Description = editCategoryRequest.Description
            };
            var updatedCate =  await cateRepository.UpdateAsync(model);
            if (updatedCate != null)
            {
                return RedirectToAction("List");
            } else
            {
                return RedirectToAction("List");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exist = await cateRepository.GetAsync(id);
            var model = new EditCategoryRequest
            {
                Id = exist.Id,
                Name = exist.Name,
                Description = exist.Description
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditCategoryRequest editCategoryRequest)
        {
            var deletedCate = await cateRepository.DeleteAsync(editCategoryRequest.Id);
            if (deletedCate != null)
            {
                return RedirectToAction("List");
            } else
            {
                return RedirectToAction("List");
            }
        }

        private void ValidateCategory(AddCategoryRequest addCategoryRequest)
        {
            if (addCategoryRequest.Name is not null && addCategoryRequest.Description is not null)
            {
                if (addCategoryRequest.Name == addCategoryRequest.Description)
                {
                    ModelState.AddModelError("Description", "Tên và mô tả không được trùng nhau!");
                }
            }
        }   
    }
}
