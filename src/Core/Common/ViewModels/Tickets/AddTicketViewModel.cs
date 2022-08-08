using Common.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Tickets
{
    public class AddTicketViewModel
    {
        [Required(ErrorMessage = "TicketDetails is required"), MaxLength(200), Display(Name = "Ticket Details")]
        public string TicketDetails { get; set; }

        [Required(ErrorMessage = "Categoryname is required"), MaxLength(50), Display(Name = "CategoryName")]
        public string CategoryName { get; set; }
        public IFormFile? Imagefile { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }
        public StatusEnum Status { get; set; }
        public List<ListCategory> categories { get; set; }

    }
    public class ListCategory
        {
        [Key]
        public string CId { get; set; }
        public string CategoryName { get; set; }
    }
}