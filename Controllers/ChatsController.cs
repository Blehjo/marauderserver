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
                ArtificialIntelligences = x.ArtificialIntelligences,
                ArtificialIntelligenceId = x.ArtificialIntelligenceId,
                ChatComments = x.ChatComments,
                UserChatComments = x.UserChatComments,
                Comments = x.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.ChatId && f.ContentType == "chat").ToList()
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
                ArtificialIntelligences = _context.ArtificialIntelligences.Where(a => a.ArtificialIntelligenceId == x.ArtificialIntelligenceId).First(),
                ArtificialIntelligenceId = x.ArtificialIntelligenceId,
                ChatComments = x.ChatComments,
                Comments = x.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.ChatId && f.ContentType == "chat").ToList()
            }).ToListAsync();
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetSingleUserChats(string id)
        {
            if (_context.Chats == null)
            {
                return NotFound();
            }

            var userInfo = _context.Users.Find(id);

            return await _context.Chats.Where(c => c.UserId == id).Select(x => new Chat()
            {
                ChatId = x.ChatId,
                Title = x.Title,
                Type = x.Type,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                User = userInfo,
                ArtificialIntelligences = _context.ArtificialIntelligences.Find(x.ArtificialIntelligenceId),
                ArtificialIntelligenceId = x.ArtificialIntelligenceId,
                ChatComments = x.ChatComments,
                Comments = x.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.ChatId && f.ContentType == "chat").ToList()
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

            var userInfo = _context.Users.Find(chat.UserId);

            var chatComments = await _context.ChatComments.Where(ch => ch.ChatId == id).Select(x => new ChatComment()
            {
                ChatId = x.ChatId,
                ChatCommentId = x.ChatCommentId,
                ChatValue = x.ChatValue,
                DateCreated = x.DateCreated,
            }).ToListAsync();

            var userChatComments = await _context.UserChatComments.Where(ch => ch.ChatId == id).Select(x => new UserChatComment()
            {
                ChatId = x.ChatId,
                UserChatCommentId = x.UserChatCommentId,
                CommentValue = x.CommentValue,
                MediaLink = x.MediaLink,
                Type = x.Type,
                ImageFile = x.ImageFile,
                ImageSource = x.ImageSource,
                User = x.User,
                DateCreated = x.DateCreated,
            }).ToListAsync();

            return new Chat()
            {
                ChatId = chat.ChatId,
                Title = chat.Title,
                Type = chat.Type,
                DateCreated = chat.DateCreated,
                UserId = chat.UserId,
                User = userInfo,
                ArtificialIntelligences = _context.ArtificialIntelligences.Find(chat.ArtificialIntelligenceId),
                ArtificialIntelligenceId = chat.ArtificialIntelligenceId,
                ChatComments = chatComments,
                UserChatComments = userChatComments,
                Comments = chat.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == chat.ChatId && f.ContentType == "chat").ToList()
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
