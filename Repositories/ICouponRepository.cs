using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetAllCouponsAsync();
        Task<Coupon> GetCouponByIdAsync(Guid id);
        Task AddCouponAsync(Coupon coupon);
        Task UpdateCouponAsync(Coupon coupon);
        Task DeleteCouponAsync(Guid id);
        Task<decimal> GetDiscountAmountByCodeAsync(string couponCode);
    }
}
