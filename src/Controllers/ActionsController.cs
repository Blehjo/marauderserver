using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly MarauderContext _context;

        public ActionController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/Action
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Action>>> GetActions()
        {
            if (_context.Actions == null)
            {
                return NotFound();
            }

            return await _context.Actions.Select(a => new Models.Action() {
                ActionId = a.ActionId,
                EventType = a.EventType,
                Activity = a.Activity,
                PinId = a.PinId,
                Pin = a.Pin
            }).ToListAsync();
        }

        // GET: api/Action/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Action>> GetAction(int id)
        {
            if (_context.Actions == null)
            {
                return NotFound();
            }

            var action = await _context.Actions.FindAsync(id);

            if (action == null)
            {
                return NotFound();
            }

            return new Models.Action() {
                ActionId = action.ActionId,
                EventType = action.EventType,
                Activity = action.Activity,
                PinId = action.PinId,
                Pin = action.Pin
            };
        }

        // PUT: api/Action/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Models.Action>>> PutAction(int id, Models.Action action)
        {
            if (id != action.ActionId)
            {
                return BadRequest();
            }

            _context.Entry(action).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ActionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.Actions.Where(a => a.PinId == action.PinId).Select(a => new Models.Action() {
                ActionId = a.ActionId,
                EventType = a.EventType,
                Activity = a.Activity,
                PinId = a.PinId,
                Pin = a.Pin
            }).ToListAsync();
        }

        // POST: api/Action
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Models.Action>>> PostAction(Models.Action action)
        {
            if (_context.Actions == null)
            {
                return Problem("Entity set 'MarauderContext.Actions'  is null.");
            }

            _context.Actions.Add(action);
            await _context.SaveChangesAsync();

            return await _context.Actions.Where(a => a.PinId == action.PinId).Select(a => new Models.Action() {
                ActionId = a.ActionId,
                EventType = a.EventType,
                Activity = a.Activity,
                PinId = a.PinId,
                Pin = a.Pin
            }).ToListAsync();
        }

        // DELETE: api/Action/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Models.Action>>> DeleteAction(int id)
        {
            if (_context.Actions == null)
            {
                return NotFound();
            }
            var action = await _context.Actions.FindAsync(id);
            if (action == null)
            {
                return NotFound();
            }

            _context.Actions.Remove(action);
            await _context.SaveChangesAsync();

            return await _context.Actions.Where(a => a.PinId == action.PinId).Select(a => new Models.Action() {
                ActionId = a.ActionId,
                EventType = a.EventType,
                Activity = a.Activity,
                PinId = a.PinId,
                Pin = a.Pin
            }).ToListAsync();
        }

        private bool ActionExists(int id)
        {
            return (_context.Actions?.Any(e => e.ActionId == id)).GetValueOrDefault();
        }
    }
}
