using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.ViewModels.ResponseVM;
using ShopWeb.Repositories;
using ShopWeb.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ShopWeb.Controllers
{
    public class ResponseController : Controller
    {
        private readonly IResponseRepository _responseRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ResponseController(IResponseRepository responseRepository, UserManager<ApplicationUser> userManager)
        {
            _responseRepository = responseRepository;
            this.userManager = userManager;
        }

        public async Task<IActionResult> customerIndex()
        {
            var userId = Guid.Parse(userManager.GetUserId(User));
            var responses = await _responseRepository.GetResponsesByUserIdAsync(userId);
            return View(responses);
        }

        // GET: Response
        [Authorize(Roles = "Admin")]
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
            var sentUserId = response.UserId.ToString();
            var sentUser = await userManager.FindByIdAsync(sentUserId);
            var model = new ResponseViewModel
            {
                Id = response.Id,
                UserName = sentUser.UserName ?? "User Not Found",
                Heading = response.Heading,
                Content = response.Content,
                CreatedAt = response.CreatedAt,
            };
            return View(model);
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
                    CreatedAt = DateTime.Now,
                    State = "Pending"
                };
                await _responseRepository.AddResponseAsync(response);
                return RedirectToAction("customerIndex");
            }
            return View(responseViewModel);
        }

        // GET: Response/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _responseRepository.GetResponseByIdAsync(id);
            var model = new ResponseViewModel
            {
                Id = response.Id,
                UserId = response.UserId,
                Heading = response.Heading,
                Content = response.Content,
                CreatedAt = response.CreatedAt
            };
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
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
                return RedirectToAction("customerIndex");
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

        [HttpGet]
        public async Task<IActionResult> UpdateState(Guid id)
        {
            var response = await _responseRepository.GetResponseByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            if (response.State == "Pending")
            {
                response.State = "Done";
            }

            await _responseRepository.UpdateResponseAsync(response);

            return RedirectToAction("Index");
        }
    }
}
