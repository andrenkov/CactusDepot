using Microsoft.AspNetCore.Identity;

namespace CactusDepot.Shared.Models.Administration
{
    public class ApplicationUser : IdentityUser
    {
        public string? City { get; set; }
    }
}
