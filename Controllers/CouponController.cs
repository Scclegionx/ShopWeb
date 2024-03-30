using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Repositories;
using ShopWeb.Models.ViewModels.CouponVM;
using Microsoft.EntityFrameworkCore;

namespace ShopWeb.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<IActionResult> Index()
        {
            var coupons = await _couponRepository.GetAllCouponsAsync();
            return View(coupons);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCouponViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var coupon = new Coupon
                {
                    Code = viewModel.Code,
                    DiscountAmount = viewModel.DiscountAmount,
                    ExpirationDate = viewModel.ExpirationDate
                };
                await _couponRepository.AddCouponAsync(coupon);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var coupon = await _couponRepository.GetCouponByIdAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Coupon coupon)
        {
            if (id != coupon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _couponRepository.UpdateCouponAsync(coupon);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CouponExists(coupon.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var coupon = await _couponRepository.GetCouponByIdAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _couponRepository.DeleteCouponAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CouponExists(Guid id)
        {
            return _couponRepository.GetCouponByIdAsync(id) != null;
        }
    }
}
