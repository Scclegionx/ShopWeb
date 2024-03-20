using Microsoft.AspNetCore.Identity;

namespace ShopWeb.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Avatar { get; set; }
        public string Address { get; set; }
    }
}
