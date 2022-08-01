using Common.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Tickets
{
    public class AddTicketViewModel
    {
       [Required(ErrorMessage = "TicketDetails is required"), MaxLength(200), Display(Name = "Ticket Details")]
        public string TicketDetails { get; set; }
        [MaxLength(25), Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "Categoryname is required"), MaxLength(50), Display(Name = "CategoryName")]
        public string CategoryName { get; set; }
       
        [Display(Name = "Upload File")]
        public IFormFile? Imagefile { get; set; }
        [Display(Name = "ImageName")]
        public string ImageName { get; set; }
    }
}
