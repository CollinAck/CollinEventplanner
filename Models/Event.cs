using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CollinEventplanner.Models
{
    public class Event
    {
        [Key] // Primaire sleutel
        public int EventId { get; set; }

        [Required] // Moet ingevuld worden
        [Display(Name = "Event")] // Naam die wordt weergeven in programma
        public string Name { get; set; }

        [MaxLength(100)]
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Locatie")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Tijd/Datum")]
        public DateTime DateTime { get; set; }

        [Required]
        [Precision(18,2)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Ticketprijs")]
        public decimal Cost { get; set; }

        [Required]
        [Display(Name = "Max")]
        public int MaxParticipants { get; set; }

        [Required]
        [Display(Name = "Left")]
        public int AvailableSpots { get; set; }

        [Display(Name = "")]
        public string? ImageFileName { get; set; }

        [Required]
        public string Category {  get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>(); // Event kan meer tickets hebben

    }
}
