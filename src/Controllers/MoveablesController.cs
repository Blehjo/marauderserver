using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoveablesController : ControllerBase
    {
        private readonly MarauderContext _context;

        public MoveablesController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/Moveables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moveable>>> GetMoveables()
        {
            if (_context.Moveables == null)
            {
                return NotFound();
            }

            return await _context.Moveables.Select(m => new Moveable() {
                MoveableId = m.MoveableId,
                XCoord = m.XCoord,
                YCoord = m.YCoord,
                ZCoord = m.YCoord,
                FileId = m.FileId,
                DocFile = m.DocFile
            }).ToListAsync();
        }

        // GET: api/Moveables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Moveable>> GetMoveable(int id)
        {
            if (_context.Moveables == null)
            {
                return NotFound();
            }

            var moveable = await _context.Moveables.FindAsync(id);

            if (moveable == null)
            {
                return NotFound();
            }

            return new Moveable() {
                MoveableId = moveable.MoveableId,
                XCoord = moveable.XCoord,
                YCoord = moveable.YCoord,
                ZCoord = moveable.YCoord,
                FileId = moveable.FileId,
                DocFile = moveable.DocFile
            };
        }

        // PUT: api/Moveables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Moveable>>> PutMoveable(int id, Moveable moveable)
        {
            if (id != moveable.MoveableId)
            {
                return BadRequest();
            }

            _context.Entry(moveable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoveableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.Moveables.Where(m => m.FileId == moveable.FileId).Select(m => new Moveable() {
                MoveableId = m.MoveableId,
                XCoord = m.XCoord,
                YCoord = m.YCoord,
                ZCoord = m.YCoord,
                FileId = m.FileId,
                DocFile = m.DocFile
            }).ToListAsync();
        }

        // POST: api/Moveables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Moveable>>> PostMoveable(Moveable moveable)
        {
            if (_context.Moveables == null)
            {
                return Problem("Entity set 'MarauderContext.Moveables'  is null.");
            }

            _context.Moveables.Add(moveable);
            await _context.SaveChangesAsync();

            return await _context.Moveables.Where(m => m.FileId == moveable.FileId).Select(m => new Moveable() {
                MoveableId = m.MoveableId,
                XCoord = m.XCoord,
                YCoord = m.YCoord,
                ZCoord = m.YCoord,
                FileId = m.FileId,
                DocFile = m.DocFile
            }).ToListAsync();
        }

        // DELETE: api/Moveables/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Moveable>>> DeleteMoveable(int id)
        {
            if (_context.Moveables == null)
            {
                return NotFound();
            }
            var moveable = await _context.Moveables.FindAsync(id);
            if (moveable == null)
            {
                return NotFound();
            }

            _context.Moveables.Remove(moveable);
            await _context.SaveChangesAsync();

            return await _context.Moveables.Where(m => m.FileId == moveable.FileId).Select(m => new Moveable() {
                MoveableId = m.MoveableId,
                XCoord = m.XCoord,
                YCoord = m.YCoord,
                ZCoord = m.YCoord,
                FileId = m.FileId,
                DocFile = m.DocFile
            }).ToListAsync();
        }

        private bool MoveableExists(int id)
        {
            return (_context.Moveables?.Any(e => e.MoveableId == id)).GetValueOrDefault();
        }
    }
}
