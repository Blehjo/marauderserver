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
    public class ArtificialIntelligenceChatController : ControllerBase
    {
        private readonly MarauderContext _context;

        public ArtificialIntelligenceChatController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/ArtificialIntelligenceChat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtificialIntelligenceChat>>> GetArtificialIntelligenceChat()
        {
          if (_context.ArtificialIntelligenceChat == null)
          {
              return NotFound();
          }
            return await _context.ArtificialIntelligenceChat.ToListAsync();
        }

        // GET: api/ArtificialIntelligenceChat/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtificialIntelligenceChat>> GetArtificialIntelligenceChat(int id)
        {
          if (_context.ArtificialIntelligenceChat == null)
          {
              return NotFound();
          }
            var artificialIntelligenceChat = await _context.ArtificialIntelligenceChat.FindAsync(id);

            if (artificialIntelligenceChat == null)
            {
                return NotFound();
            }

            return artificialIntelligenceChat;
        }

        // PUT: api/ArtificialIntelligenceChat/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtificialIntelligenceChat(int id, ArtificialIntelligenceChat artificialIntelligenceChat)
        {
            if (id != artificialIntelligenceChat.ArtificialIntelligenceId)
            {
                return BadRequest();
            }

            _context.Entry(artificialIntelligenceChat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtificialIntelligenceChatExists(id))
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

        // POST: api/ArtificialIntelligenceChat
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArtificialIntelligenceChat>> PostArtificialIntelligenceChat(ArtificialIntelligenceChat artificialIntelligenceChat)
        {
          if (_context.ArtificialIntelligenceChat == null)
          {
              return Problem("Entity set 'MarauderContext.ArtificialIntelligenceChat'  is null.");
          }
            _context.ArtificialIntelligenceChat.Add(artificialIntelligenceChat);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArtificialIntelligenceChatExists(artificialIntelligenceChat.ArtificialIntelligenceId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetArtificialIntelligenceChat", new { id = artificialIntelligenceChat.ArtificialIntelligenceId }, artificialIntelligenceChat);
        }

        // DELETE: api/ArtificialIntelligenceChat/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtificialIntelligenceChat(int id)
        {
            if (_context.ArtificialIntelligenceChat == null)
            {
                return NotFound();
            }
            var artificialIntelligenceChat = await _context.ArtificialIntelligenceChat.FindAsync(id);
            if (artificialIntelligenceChat == null)
            {
                return NotFound();
            }

            _context.ArtificialIntelligenceChat.Remove(artificialIntelligenceChat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtificialIntelligenceChatExists(int id)
        {
            return (_context.ArtificialIntelligenceChat?.Any(e => e.ArtificialIntelligenceId == id)).GetValueOrDefault();
        }
    }
}
