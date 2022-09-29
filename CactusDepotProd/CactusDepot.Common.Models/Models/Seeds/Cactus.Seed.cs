using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CactusDepot.Shared.Models.Seeds
{
    public partial class CactusSeed
    {
        public CactusSeed()
        {
            OnCreated();
        }

        [Key]
        public virtual int SeedId { get; set; }

        [Required(ErrorMessage = "Please enter species name")]
        [Display(Name = "Species name")]
        //[StringLength(255), MinLength(3)]
        [Column(TypeName = "nvarchar(255)")]
        public virtual string SeedName { get; set; } = null!;

        //[StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Parent #")]
        public virtual string? Parent1CatalogNum { get; set; }

        //[StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Pollinated with")]
        public virtual string? Parent2CatalogNum { get; set; }

        //[StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        [Display(Name = "Note")]
        public virtual string? SeedNote { get; set; }

        //[Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Collected Date")]
        public virtual DateTime? SeedCollectedDate { get; set; } = DateTime.Now;

        //[StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        [Display(Name = "Image")]
        public virtual string? SeedSource { get; set; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Qty")]
        public virtual int? SeedSeedsQty { get; set; } = 20;

        [Display(Name = "Year")]
        public virtual int? SeedYear { get; set; } = DateTime.Now.Year;

        [Display(Name = "Seedlings catalog #")]
        public virtual int? SeedCatalogNum { get; set; }

        [Display(Name = "Last sowed (Year)")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public virtual int? SeedLastSowedYear { get; set; }

        [Display(Name = "Available")]
        [DefaultValue(true)]
        public virtual bool SeedAvailable { get; set; } = true;

        [Display(Name = "Rating")]
        [Range(0, 10)]
        [DefaultValue(0)]
        public virtual int? SeedRating { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }
}
