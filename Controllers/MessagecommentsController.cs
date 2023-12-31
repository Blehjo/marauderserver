using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;
using marauderserver.Hubs;
using marauderserver.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagecommentController : ControllerBase
    {
        private readonly MarauderContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHubContext<ChatHub> _chatHub;

        public MessagecommentController(MarauderContext context, IWebHostEnvironment hostEnvironment, IHubContext<ChatHub> chatHub)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _chatHub = chatHub;
        }

        // GET: api/MessageComment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageComment>>> GetMessageComments()
        {
            if (_context.MessageComments == null)
            {
                return NotFound();
            }
            return await _context.MessageComments.Select(x => new MessageComment() {
                MessageCommentId = x.MessageCommentId,
                MessageValue = x.MessageValue,
                DateCreated = x.DateCreated,
                MediaLink = x.MediaLink,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink) }).ToListAsync();
        }

        // GET: api/MessageComment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MessageComment>>> GetMessageComment(int id)
        {
            if (_context.MessageComments == null)
            {
                return NotFound();
            }

            return await _context.MessageComments.Where(m => m.MessageId == id).Select(x => new MessageComment()
            {
                MessageCommentId = x.MessageCommentId,
                MessageId = x.MessageId,
                MessageValue = x.MessageValue,
                DateCreated = x.DateCreated,
                MediaLink = x.MediaLink,
                Favorites = x.Favorites,
                UserId = x.UserId,
                User = x.User,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // PUT: api/MessageComment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessageComment(int id, MessageComment messageComment)
        {
            if (id != messageComment.MessageCommentId)
            {
                return BadRequest();
            }

            _context.Entry(messageComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageCommentExists(id))
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

        // POST: api/MessageComment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<MessageComment>>> PostMessageComment(int id, [FromForm] MessageComment messageComment)
        {
            messageComment.MessageId = id;

            messageComment.UserId = Request.Cookies["user"];

            if (_context.MessageComments == null)
            {
                return Problem("Entity set 'MarauderContext.MessageComment'  is null.");
            }

            if (messageComment.ImageFile != null)
            {
                messageComment.MediaLink = await SaveImage(messageComment.ImageFile);
            }

            _context.MessageComments.Add(messageComment);
            
            await _context.SaveChangesAsync();

            await _chatHub.Clients.All.SendAsync("messageReceived", messageComment);

            return await _context.MessageComments.Where(m => m.MessageId == id).Select(x => new MessageComment()
            {
                MessageCommentId = x.MessageCommentId,
                MessageId = x.MessageId,
                MediaLink = x.MediaLink,
                Favorites =x.Favorites,
                MessageValue = x.MessageValue,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // DELETE: api/MessageComment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageComment(int id)
        {
            if (_context.MessageComments == null)
            {
                return NotFound();
            }
            var messageComment = await _context.MessageComments.FindAsync(id);
            if (messageComment == null)
            {
                return NotFound();
            }

            _context.MessageComments.Remove(messageComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageCommentExists(int id)
        {
            return (_context.MessageComments?.Any(e => e.MessageCommentId == id)).GetValueOrDefault();
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
