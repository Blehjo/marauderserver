using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatCommentController : ControllerBase
    {

        private readonly MarauderContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ChatCommentController(MarauderContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: api/ChatComment
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<ChatComment>>> GetUserChatComments(int id)
        {
          if (_context.ChatComments == null)
          {
              return NotFound();
          }
            return await _context.ChatComments.Where(c => c.ChatId == id).Select(x => new ChatComment()
            {
                ChatCommentId = x.ChatCommentId,
                ChatId = x.ChatId,
                ChatValue = x.ChatValue,
                MediaLink = x.MediaLink,
                DateCreated = x.DateCreated,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ChatValue)
            }).ToListAsync();
        }

        // GET: api/ChatComment
        [HttpGet("chat/{id}")]
        public async Task<ActionResult<IEnumerable<ChatComment>>> GetChatChatComments(int id)
        {
            if (_context.ChatComments == null)
            {
                return NotFound();
            }

            return await _context.ChatComments.Where(c => c.ChatId == id).Select(x => new ChatComment()
            {
                ChatCommentId = x.ChatCommentId,
                ChatId = x.ChatId,
                ChatValue = x.ChatValue,
                MediaLink = x.MediaLink,
                DateCreated = x.DateCreated,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ChatValue)
            }).ToListAsync();
        }

        // GET: api/ChatComment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatComment>>> GetChatComments()
        {
            if (_context.ChatComments == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.ChatComments.Where(c => c.Chat.UserId == userId).Select(x => new ChatComment()
            {
                ChatCommentId = x.ChatCommentId,
                ChatId = x.ChatId,
                ChatValue = x.ChatValue,
                MediaLink = x.MediaLink,
                DateCreated = x.DateCreated,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ChatValue)
            }).ToListAsync();
        }

        // GET: api/ChatComment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ChatComment>>> GetChatComment(int id)
        {
            if (_context.ChatComments == null)
            {
                return NotFound();
            }

            var chatComment = await _context.ChatComments.FindAsync(id);

            if (chatComment == null)
            {
                return NotFound();
            }

            return await _context.ChatComments.Where(c => c.ChatId == id).Select(x => new ChatComment()
            {
                ChatCommentId = x.ChatCommentId,
                ChatId = x.ChatId,
                ChatValue = x.ChatValue,
                MediaLink = x.MediaLink,
                DateCreated = x.DateCreated,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ChatValue)
            }).ToListAsync();
        }

        // PUT: api/ChatComment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatComment(int id, ChatComment chatComment)
        {
            if (id != chatComment.ChatCommentId)
            {
                return BadRequest();
            }

            _context.Entry(chatComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatCommentExists(id))
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

        // POST: api/ChatComment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<ChatComment>>> PostChatComment(int id, [FromForm] ChatComment chatComment)
        {
            if (_context.ChatComments == null)
            {
              return Problem("Entity set 'PlanetNineDatabaseContext.ChatComments'  is null.");
            }

            if (chatComment.ImageFile != null)
            {
                chatComment.MediaLink = await SaveImage(chatComment.ImageFile);
            }

            chatComment.ChatId = id;

            _context.ChatComments.Add(chatComment);

            await _context.SaveChangesAsync();

            return await _context.ChatComments.Where(c => c.ChatId == id).Select(x => new ChatComment()
            {
                ChatCommentId = x.ChatCommentId,
                ChatId = x.ChatId,
                ChatValue = x.ChatValue,
                MediaLink = x.MediaLink,
                DateCreated = x.DateCreated,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ChatValue)
            }).ToListAsync();
        }

        // DELETE: api/ChatComment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatComment(int id)
        {
            if (_context.ChatComments == null)
            {
                return NotFound();
            }
            var chatComment = await _context.ChatComments.FindAsync(id);
            if (chatComment == null)
            {
                return NotFound();
            }

            _context.ChatComments.Remove(chatComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChatCommentExists(int id)
        {
            return (_context.ChatComments?.Any(e => e.ChatCommentId == id)).GetValueOrDefault();
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
