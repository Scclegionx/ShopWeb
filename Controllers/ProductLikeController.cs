using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Migrations;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.LikeVM;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductLikeController : ControllerBase
    {
        private readonly IProductLikeRepository productLikeRepository;

        public ProductLikeController(IProductLikeRepository productLikeRepository)
        {
            this.productLikeRepository = productLikeRepository;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new ProductLike
            {
                UserId = addLikeRequest.UserId,
                ProductId = addLikeRequest.ProductId,
            };

            await productLikeRepository.AddLiketoProduct(model);
            return Ok();
        }
        [HttpGet]
        [Route("{productId:guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikeForProduct([FromRoute] Guid productId)
        {
            var totalLikes = await productLikeRepository.GetTotalLikes(productId);
            return Ok(totalLikes);
        }
        [HttpDelete]
        [Route("Remove")]
        public async Task<IActionResult> RemoveLike([FromBody] RemoveLikeRequest removeLikeRequest)
        {
            await productLikeRepository.RemoveLikeFromProduct(removeLikeRequest.UserId, removeLikeRequest.ProductId);
            return Ok();
        }

    }
}
