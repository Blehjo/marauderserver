using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PinsController : ControllerBase
    {
        private readonly MarauderContext _context;

        public PinsController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/Pins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pin>>> GetPins()
        {
            if (_context.Pins == null)
            {
                return NotFound();
            }

            return await _context.Pins.Select(p => new Pin() {
                PinId = p.PinId,
                PinLocation = p.PinLocation,
                IsAnalog = p.IsAnalog,
                DeviceId = p.DeviceId,
                Device = p.Device,
                Actions = p.Actions
            }).ToListAsync();
        }

        // GET: api/Pins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pin>> GetPin(int id)
        {
            if (_context.Pins == null)
            {
                return NotFound();
            }

            var pin = await _context.Pins.FindAsync(id);

            if (pin == null)
            {
                return NotFound();
            }

            return new Pin() {
                PinId = pin.PinId,
                PinLocation = pin.PinLocation,
                IsAnalog = pin.IsAnalog,
                DeviceId = pin.DeviceId,
                Device = pin.Device,
                Actions = pin.Actions
            };
        }

        // PUT: api/Pins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Pin>>> PutPin(int id, Pin pin)
        {
            if (id != pin.PinId)
            {
                return BadRequest();
            }

            _context.Entry(pin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PinExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.Pins.Where(p => p.DeviceId == pin.DeviceId).Select(p => new Pin() {
                PinId = p.PinId,
                PinLocation = p.PinLocation,
                IsAnalog = p.IsAnalog,
                DeviceId = p.DeviceId,
                Device = p.Device,
                Actions = p.Actions
            }).ToListAsync();
        }

        // POST: api/Pins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Pin>>> PostPin(Pin pin)
        {
            if (_context.Pins == null)
            {
                return Problem("Entity set 'MarauderContext.Pins'  is null.");
            }

            _context.Pins.Add(pin);
            await _context.SaveChangesAsync();

            return await _context.Pins.Where(p => p.DeviceId == pin.DeviceId).Select(p => new Pin() {
                PinId = p.PinId,
                PinLocation = p.PinLocation,
                IsAnalog = p.IsAnalog,
                DeviceId = p.DeviceId,
                Device = p.Device,
                Actions = p.Actions
            }).ToListAsync();
        }

        // DELETE: api/Pins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Pin>>> DeletePin(int id)
        {
            if (_context.Pins == null)
            {
                return NotFound();
            }

            var pin = await _context.Pins.FindAsync(id);
            if (pin == null)
            {
                return NotFound();
            }

            _context.Pins.Remove(pin);
            await _context.SaveChangesAsync();

            return await _context.Pins.Where(p => p.DeviceId == pin.DeviceId).Select(p => new Pin() {
                PinId = p.PinId,
                PinLocation = p.PinLocation,
                IsAnalog = p.IsAnalog,
                DeviceId = p.DeviceId,
                Device = p.Device,
                Actions = p.Actions
            }).ToListAsync();
        }

        private bool PinExists(int id)
        {
            return (_context.Pins?.Any(e => e.PinId == id)).GetValueOrDefault();
        }
    }
}
