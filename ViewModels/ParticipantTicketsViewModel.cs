using CollinEventplanner.Models;

namespace CollinEventplanner.ViewModels
{
    public class ParticipantTicketsViewModel
    {
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
