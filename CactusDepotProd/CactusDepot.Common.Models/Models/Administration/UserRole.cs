using System.ComponentModel.DataAnnotations;

namespace CactusDepot.Shared.Models.Administration
{
    public class UserRoleModel
    {
        [Required]
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}
