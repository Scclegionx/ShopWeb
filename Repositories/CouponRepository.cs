using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ShopWebDbContext _context;

        public CouponRepository(ShopWebDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {
            return await _context.Coupon.ToListAsync();
        }

        public async Task<Coupon> GetCouponByIdAsync(Guid id)
        {
            return await _context.Coupon.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCouponAsync(Coupon coupon)
        {
            _context.Coupon.Add(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCouponAsync(Coupon coupon)
        {
            _context.Coupon.Update(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCouponAsync(Guid id)
        {
            var coupon = await _context.Coupon.FindAsync(id);
            if (coupon != null)
            {
                _context.Coupon.Remove(coupon);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetDiscountAmountByCodeAsync(string couponCode)
        {
            var coupon = await _context.Coupon.FirstOrDefaultAsync(c => c.Code == couponCode);
            if (coupon != null && coupon.ExpirationDate >= DateTime.Today)
            {
                return coupon.DiscountAmount;
            }
            return 0; // Return 0 if coupon not found or expired
        }
    }
}
