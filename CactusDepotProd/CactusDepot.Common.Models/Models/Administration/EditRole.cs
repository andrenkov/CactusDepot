using System.ComponentModel.DataAnnotations;

namespace CactusDepot.Shared.Models.Administration
{
    public class EditRoleModel
    {
        public EditRoleModel()
        {
            Users = new List<string>();
        }

        [Required]
        public string Id { get; set; } = null!;
        [Required(ErrorMessage ="Role name is required")]
        public string RoleName { get; set; } = null!;
        public List<string> Users { get; set; }

    }
}
