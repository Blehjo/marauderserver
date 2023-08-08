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
    public class ChannelCommentController : ControllerBase
    {
        private readonly MarauderContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHubContext<ChatHub> _chatHub;

        public ChannelCommentController(MarauderContext context, IWebHostEnvironment hostEnvironment, IHubContext<ChatHub> chatHub)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
            _chatHub = chatHub;
        }

        // GET: api/ChannelComment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChannelComment>>> GetChannelComments()
        {
            if (_context.ChannelComments == null)
            {
                return NotFound();
            }

            return await _context.ChannelComments.Select(x => new ChannelComment() {
                ChannelCommentId = x.ChannelCommentId,
                CommentValue = x.CommentValue,
                ChannelId = x.ChannelId,
                Channels = x.Channels,
                DateCreated = x.DateCreated,
                MediaLink = x.MediaLink,
                Type = x.Type,
                UserId = x.UserId,
                User = x.User,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)}).ToListAsync();
        }

        // GET: api/ChannelComment/5
        [HttpGet("{channelId}")]
        public async Task<ActionResult<IEnumerable<ChannelComment>>> GetChannelComment(int channelId)
        {
            if (_context.ChannelComments == null)
            {
                return NotFound();
            }

            return await _context.ChannelComments.Where(c => c.ChannelId == channelId).Select(x => new ChannelComment()
            {
                ChannelCommentId = x.ChannelCommentId,
                CommentValue = x.CommentValue,
                ChannelId = x.ChannelId,
                Channels = x.Channels,
                DateCreated = x.DateCreated,
                MediaLink = x.MediaLink,
                Type = x.Type,
                UserId = x.UserId,
                User = x.User,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // PUT: api/ChannelComment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChannelComment(int id, ChannelComment channelComment)
        {
            if (id != channelComment.ChannelCommentId)
            {
                return BadRequest();
            }

            _context.Entry(channelComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChannelCommentExists(id))
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

        // POST: api/ChannelComment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<ChannelComment>>> PostChannelComment(int id, [FromForm] ChannelComment channelComment)
        {
            if (_context.ChannelComments == null)
            {
                return Problem("Entity set 'MarauderContext.ChannelComments'  is null.");
            }

            channelComment.UserId = HttpContext.Request.Cookies["user"];

            if (channelComment.ImageFile != null)
            {
                channelComment.MediaLink = await SaveImage(channelComment.ImageFile);
            }

            channelComment.ChannelId = id;

            _context.ChannelComments.Add(channelComment);

            await _context.SaveChangesAsync();

            await _chatHub.Clients.All.SendAsync("messageReceived", channelComment);

            return await _context.ChannelComments.Select(x => new ChannelComment() 
            {
                ChannelCommentId = x.ChannelCommentId,
                CommentValue = x.CommentValue,
                MediaLink = x.MediaLink,
                Type = x.Type,
                DateCreated = x.DateCreated,
                ChannelId = x.ChannelId,
                Channels = x.Channels,
                UserId = x.UserId,
                User = x.User
            }).Where(c => c.ChannelId == id).ToListAsync();
        }

        // DELETE: api/ChannelComment/5
        [HttpDelete("{id}/{channelId}")]
        public async Task<ActionResult<IEnumerable<ChannelComment>>> DeleteChannelComment(int id, int channelId)
        {
            if (_context.ChannelComments == null)
            {
                return NotFound();
            }
            var channelComment = await _context.ChannelComments.FindAsync(id);
            if (channelComment == null)
            {
                return NotFound();
            }

            _context.ChannelComments.Remove(channelComment);
            await _context.SaveChangesAsync();

            return await _context.ChannelComments.Where(c => c.ChannelId == channelId).ToListAsync();
        }

        private bool ChannelCommentExists(int id)
        {
            return (_context.ChannelComments?.Any(e => e.ChannelCommentId == id)).GetValueOrDefault();
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
