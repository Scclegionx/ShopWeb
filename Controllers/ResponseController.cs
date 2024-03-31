using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.ViewModels.ResponseVM;
using ShopWeb.Repositories;
using ShopWeb.Models.Domain;

namespace ShopWeb.Controllers
{
    public class ResponseController : Controller
    {
        private readonly IResponseRepository _responseRepository;

        public ResponseController(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
        }

        // GET: Response
        public async Task<IActionResult> Index()
        {
            var responses = await _responseRepository.GetAllResponsesAsync();
            return View(responses);
        }

        // GET: Response/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _responseRepository.GetResponseByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        // GET: Response/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Response/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResponseViewModel responseViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = new Response
                {
                    UserId = responseViewModel.UserId,
                    Heading = responseViewModel.Heading,
                    Content = responseViewModel.Content,
                    CreatedAt = DateTime.Now
                };
                await _responseRepository.AddResponseAsync(response);
                return RedirectToAction(nameof(Index));
            }
            return View(responseViewModel);
        }

        // GET: Response/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _responseRepository.GetResponseByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        // POST: Response/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ResponseViewModel responseViewModel)
        {
            if (id != responseViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = new Response
                {
                    Id = responseViewModel.Id,
                    UserId = responseViewModel.UserId,
                    Heading = responseViewModel.Heading,
                    Content = responseViewModel.Content,
                    CreatedAt = responseViewModel.CreatedAt
                };
                await _responseRepository.UpdateResponseAsync(response);
                return RedirectToAction(nameof(Index));
            }
            return View(responseViewModel);
        }

        // GET: Response/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _responseRepository.GetResponseByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        // POST: Response/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _responseRepository.DeleteResponseAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
