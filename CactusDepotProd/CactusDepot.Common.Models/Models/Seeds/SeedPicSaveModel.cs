using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CactusDepot.Shared.Models.Seeds
{
    public partial class SeedPicSaveModel
    {

        [Required]
        [Display(Name = "Seed ID")]
        public virtual int? SeedId { get; set; }

        [Required(ErrorMessage = "Please enter species name")]
        [Display(Name = "Species name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9.""'\s-]*$"), StringLength(255)]
        public virtual string? SeedName { get; set; }
        [StringLength(50)]
        [Display(Name = "Parent #")]
        public virtual string? CatalogNum { get; set; }

        public virtual IFormFile? SeedPhotoFile { get; set; }
    }
}
