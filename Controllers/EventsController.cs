using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CollinEventplanner.Data;
using CollinEventplanner.Models;

namespace CollinEventplanner.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? category)
        {
            var categories = await _context.Events
                .Select(e => e.Category)
                .Distinct() // controle op categories die al gezien zijn, soort .pop bij python
                .ToListAsync();

            ViewData["Categories"] = categories;


            // "else", als die op "selecteer een categorie" staat, dan ga je terug naar alle events,
            // maar dat is vrij onlogisch, oplossing een linkje terug naar de eventspagina, maar "selecteer een categorie" doet hetzelfde
            var query = _context.Events.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category == category);
            }

            var events = await query.ToListAsync();

            return View(events);

        }

            // GET: Events/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Name,Description,Location,DateTime,Cost,MaxParticipants,AvailableSpots,ImageFileName,Category")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Name,Description,Location,DateTime,Cost,MaxParticipants,AvailableSpots,ImageFileName,Category")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }


        // Toevoegen: meer tickets selecteren/ pop-up van bevestinging

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReserveTicket(int eventId, int participantId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            var participant = await _context.Participants.FindAsync(participantId);

            var ticket = new Ticket
            {
                EventId = eventId,
                ParticipantId = participantId,
                IsPaid = false
            };

            @event.AvailableSpots--;

            _context.Tickets.Add(ticket);
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
