﻿
using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels.Tickets
{
    public class TicketViewModel
    {
        public string TicketId { get; set; }
        public string TicketDetails { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string AssignedTo { get; set; }
        public string CategoryName { get; set; }    
        public StatusEnum Status { get; set; }
        public string ImageName { get; set; }
    }
   
    public class EditTicketViewmodel
    {
        public string TicketId { get; set; }
        [Required(ErrorMessage = "TicketDetails is required"), MaxLength(200),Display(Name = "Ticket Details")]
        public string TicketDetails { get; set; }
        [Required(ErrorMessage = "ModifiedBy User is required"), MaxLength(25), Display(Name = "CreatedBy")]
        public string ModifiedBy { get; set; }
        [Display(Name = "ModifiedDateTime")]
        public DateTime ModifiedDateTime { get; set; }
        [MaxLength(25), Display(Name = "AssignedTo")]
        public string AssignedTo { get; set; }
        public string ImageName { get; set; }
        [Required(ErrorMessage ="Status is Required"),Display(Name ="Status")]
        public StatusEnum Status { get; set; }
    }
}