using System.ComponentModel.DataAnnotations;

namespace CactusDepot.Shared.Models.Administration
{
    public class UserRolesModel
    {
        [Required]
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}
