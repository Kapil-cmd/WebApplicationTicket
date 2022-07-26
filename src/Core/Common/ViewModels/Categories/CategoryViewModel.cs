using System.ComponentModel.DataAnnotations;
namespace Common.ViewModels.Categories
{
    public class CategoryViewModel
    {
        public int CId { get; set; }
        public string CategoryName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
    public class AddCategoryViewModel
    {
        public int CId { get; set; }
        [Required(ErrorMessage = "CategoryName is required"), MaxLength(50), Display(Name = "CategoryName")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "CreatedBy User is required"),MaxLength(25), Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }
        [Required,Display(Name = "CreatedDateTime")]
        public DateTime CreatedDateTime { get; set; }
        

    }
    public class EditCategoryViewModel
    {
        public int CId { get; set; }
        [Required(ErrorMessage = "CategoryName is required"), MaxLength(50), Display(Name = "CategoryName")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "ModifiedBy User is required"), MaxLength(25), Display(Name = "ModifiedBy")]
        public string ModifiedBy { get; set; }
        [Required, Display(Name = "ModifiedDateTime")]
        public DateTime? ModifiedDateTime { get; set; }
    }
}
