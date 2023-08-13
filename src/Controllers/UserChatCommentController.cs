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
    public class UserChatCommentController : ControllerBase
    {
        private readonly MarauderContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserChatCommentController(MarauderContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/UserChatComment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserChatComment>>> GetUserChatComments()
        {
            if (_context.UserChatComments == null)
            {
                return NotFound();
            }

            return await _context.UserChatComments.Select(x => new UserChatComment()
            {
                UserChatCommentId = x.UserChatCommentId,
                CommentValue = x.CommentValue,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                ChatId = x.ChatId,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // GET: api/UserChatComment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserChatComment>> GetUserChatComment(int id)
        {
            if (_context.UserChatComments == null)
            {
                return NotFound();
            }

            var userChatComment = await _context.UserChatComments.FindAsync(id);

            if (userChatComment == null)
            {
                return NotFound();
            }

            return userChatComment;
        }

        // GET: api/Comment/5
        [HttpGet("chat/{id}")]
        public async Task<ActionResult<IEnumerable<UserChatComment>>> GetPostComments(int id)
        {
            if (_context.UserChatComments == null)
            {
                return NotFound();
            }

            return await _context.UserChatComments.Select(x => new UserChatComment()
            {
                UserChatCommentId = x.UserChatCommentId,
                CommentValue = x.CommentValue,
                DateCreated = x.DateCreated,
                User = x.User,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                ChatId = x.ChatId,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).Where(c => c.ChatId == id).ToListAsync();
        }

        // PUT: api/UserChatComment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserChatComment(int id, UserChatComment userChatComment)
        {
            if (id != userChatComment.UserChatCommentId)
            {
                return BadRequest();
            }

            _context.Entry(userChatComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserChatCommentExists(id))
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

        // POST: api/UserChatComment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<UserChatComment>>> PostUserChatComment(int id, [FromForm] UserChatComment userChatComment)
        {
            if (_context.UserChatComments == null)
            {
                return Problem("Entity set 'MarauderContext.UserChatComments'  is null.");
            }

            if (userChatComment.ImageFile != null)
            {
                userChatComment.MediaLink = await SaveImage(userChatComment.ImageFile);
            }

            userChatComment.UserId = HttpContext.Request.Cookies["user"];

            userChatComment.ChatId = id;

            _context.UserChatComments.Add(userChatComment);

            await _context.SaveChangesAsync();

            return await _context.UserChatComments.Select(x => new UserChatComment()
            {
                UserChatCommentId = x.UserChatCommentId,
                CommentValue = x.CommentValue,
                DateCreated = x.DateCreated,
                User = x.User,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                ChatId = x.ChatId,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).Where(c => c.ChatId == id).ToListAsync();
        }

        // DELETE: api/UserChatComment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserChatComment(int id)
        {
            if (_context.UserChatComments == null)
            {
                return NotFound();
            }
            var userChatComment = await _context.UserChatComments.FindAsync(id);
            if (userChatComment == null)
            {
                return NotFound();
            }

            _context.UserChatComments.Remove(userChatComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserChatCommentExists(int id)
        {
            return (_context.UserChatComments?.Any(e => e.UserChatCommentId == id)).GetValueOrDefault();
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
