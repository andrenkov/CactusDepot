using System.ComponentModel.DataAnnotations;

namespace CactusDepot.Shared.Models.Administration
{
    public class CreateRoleModel
    {
        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; } = null!;
    }
}
