using System.ComponentModel.DataAnnotations;

namespace CactusDepot.Shared.Models.Administration
{
    public class EditUserModel
    {
        public EditUserModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email name is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
