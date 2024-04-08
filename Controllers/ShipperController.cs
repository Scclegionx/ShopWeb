using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.PurchaseVM;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    public class ShipperController : Controller
    {
        private readonly IPurchaseRepository purchaseRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ShipperController(IPurchaseRepository purchaseRepository, UserManager<ApplicationUser> userManager)
        {
            this.purchaseRepository = purchaseRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allPurchase = await purchaseRepository.GetAllPurchases();
            var listForView = new List<Purchase>();
            foreach (var purchase in allPurchase)
            {
                if (purchase.ShipperID == null)
                {
                    listForView.Add(purchase);
                }
            }
            var model = new AcceptPurchaseRequest
            {
                Purchases = listForView
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyOrder()
        {
            var shipperId = Guid.Parse(userManager.GetUserId(User));
            var myOrder = await purchaseRepository.GetPurchaseByShipperId(shipperId);
            return View(myOrder);
        }


        [HttpGet]
        public async Task<IActionResult> AcceptPurchase(Guid Id)
        {
            var chosenPurchase = await purchaseRepository.GetPurchaseById(Id);
            var userId = chosenPurchase.UserId.ToString();
            var user = await userManager.FindByIdAsync(userId);
            var model = new AcceptPurchaseRequest
            {
                Id = chosenPurchase.Id,
                UserName = user.UserName,
                PurchaseDate = chosenPurchase.PurchaseDate,
                TotalPrice = chosenPurchase.TotalPrice,
                Address = chosenPurchase.Address,
                Note = chosenPurchase.Note,
                PaymentMethod = chosenPurchase.PaymentMethod,
                State = chosenPurchase.State,
                ShipperID = chosenPurchase.ShipperID
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptPurchase(AcceptPurchaseRequest acceptPurchaseRequest, Guid ShipperId)
        {
            var purchase = await purchaseRepository.GetPurchaseById(acceptPurchaseRequest.Id);

            if (purchase == null)
            {
                // Handle the case where the purchase is not found
                return NotFound();
            }

            // Update the purchase properties with the values from the AcceptPurchaseRequest
            purchase.State = "Shipper đã nhận đơn";
            purchase.ShipperID = ShipperId;

            // Save the changes to the database
            await purchaseRepository.UpdatePurchaseAsync(purchase);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateState(Guid id)
        {
            var purchase = await purchaseRepository.GetPurchaseById(id);
            if (purchase == null)
            {
                return NotFound();
            }

            if (purchase.State == "Shipper đã nhận đơn")
            {
                purchase.State = "Đang giao";
            }
            else if (purchase.State == "Đang giao")
            {
                purchase.State = "Done";
            }

            await purchaseRepository.UpdatePurchaseAsync(purchase);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var purchase = await purchaseRepository.GetPurchaseById(id);
            if (purchase == null)
            {
                return NotFound();
            }

            purchase.State = "None";
            purchase.ShipperID = null;

            await purchaseRepository.UpdatePurchaseAsync(purchase);

            return RedirectToAction("Index");
        }

    }
}
