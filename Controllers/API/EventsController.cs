using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CollinEventplanner.Data;
using CollinEventplanner.Models;

namespace CollinEventplanner.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // Te doen nog, aanpassen van andere CRUD,
        // Want op heden "GET" aangepast, maar andere doen het dan niet...

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEvents()
        {
            var events = await _context.Events
                .Select(e => new
                {
                    e.EventId,
                    e.Name,
                    e.Category,
                    Tickets = e.Tickets.Select(t => new
                    {
                        t.TicketId,
                        t.IsPaid,
                        // Volgende toegevoegd: != null ? t.Participant.Name : "Geen naam",
                        // hierdoor krijgt geen invoer van naam "Geen naam", maar ik kan dit niet weglaten, anders krijg ik niet mijn gewilde output.
                        ParticipantName = t.Participant != null ? t.Participant.Name : "Geen naam"
                    })
                })
                .ToListAsync();

            return (events);
        }


        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
