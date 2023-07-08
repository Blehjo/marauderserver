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
    public class ChatController : ControllerBase
    {
        private readonly MarauderContext _context;

        public ChatController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/Chat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
        {
            if (_context.Chats == null)
            {
                return NotFound();
            }

            return await _context.Chats.Select(x => new Chat()
            {
                ChatId = x.ChatId,
                Title = x.Title,
                Type = x.Type,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                User = x.User,
                ArtificialIntelligence = x.ArtificialIntelligence,
                ChatComments = x.ChatComments,
                Comments = x.Comments,
                Favorites = x.Favorites
            }).ToListAsync();
        }

        [HttpGet("user/chats")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetSingleUserChats()
        {
            if (_context.Chats == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.Chats.Where(c => c.UserId == userId).Select(x => new Chat()
            {
                ChatId = x.ChatId,
                Title = x.Title,
                Type = x.Type,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                User = x.User,
                ArtificialIntelligence = x.ArtificialIntelligence,
                ChatComments = x.ChatComments,
                Comments = x.Comments,
                Favorites = x.Favorites
            }).ToListAsync();
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetSingleUserChats(string id)
        {
            if (_context.Chats == null)
            {
                return NotFound();
            }

            return await _context.Chats.Where(c => c.UserId == id).Select(x => new Chat()
            {
                ChatId = x.ChatId,
                Title = x.Title,
                Type = x.Type,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                User = x.User,
                ArtificialIntelligence = x.ArtificialIntelligence,
                ChatComments = x.ChatComments,
                Comments = x.Comments,
                Favorites = x.Favorites
            }).ToListAsync();
        }

        // GET: api/Chat/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chat>> GetChat(int id)
        {
          if (_context.Chats == null)
          {
              return NotFound();
          }
            var chat = await _context.Chats.FindAsync(id);

            if (chat == null)
            {
                return NotFound();
            }

            return new Chat()
            {
                ChatId = chat.ChatId,
                Title = chat.Title,
                Type = chat.Type,
                DateCreated = chat.DateCreated,
                UserId = chat.UserId,
                User = chat.User,
                ArtificialIntelligence = chat.ArtificialIntelligence,
                ChatComments = chat.ChatComments,
                Comments = chat.Comments,
                Favorites = chat.Favorites
            };
        }

        // PUT: api/Chat/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChat(int id, Chat chat)
        {
            if (id != chat.ChatId)
            {
                return BadRequest();
            }

            _context.Entry(chat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatExists(id))
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

        // POST: api/Chat
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            if (_context.Chats == null)
            {
                return Problem("Entity set 'PlanetNineDatabaseContext.Chats'  is null.");
            }

            chat.UserId = HttpContext.Request.Cookies["user"];

            _context.Chats.Add(chat);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChat", new { id = chat.ChatId }, chat);
        }

        // DELETE: api/Chat/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Chat>>> DeleteChat(int id)
        {
            if (_context.Chats == null)
            {
                return NotFound();
            }
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.Chats.Where(c => c.UserId == userId).ToListAsync();
        }

        private bool ChatExists(int id)
        {
            return (_context.Chats?.Any(e => e.ChatId == id)).GetValueOrDefault();
        }
    }
}
