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
    public class CommentController : ControllerBase
    {
        private readonly MarauderContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CommentController(MarauderContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/Comment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }

            return await _context.Comments.Select(x => new Comment() {
                CommentId = x.CommentId,
                CommentValue = x.CommentValue,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                User = x.User,
                PostId = x.PostId,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)}).ToListAsync();
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }
            
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            comment.ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, comment.MediaLink);

            return comment;
        }

        // GET: api/Comment/5
        [HttpGet("post/{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetPostComments(int id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }

            //var userInfo = _context.Users.Find(id);

            return await _context.Comments.Select(x => new Comment()
            {
                CommentId = x.CommentId,
                CommentValue = x.CommentValue,
                DateCreated = x.DateCreated,
                User = x.User,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                PostId = x.PostId,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).Where(c => c.PostId == id).ToListAsync();
        }

        // PUT: api/Comment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> PostComment(int id, [FromForm] Comment comment)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'PlanetNineDatabaseContext.Comments'  is null.");
            }

            if (comment.ImageFile != null)
            {
                comment.MediaLink = await SaveImage(comment.ImageFile);
            }

            comment.UserId = HttpContext.Request.Cookies["user"];

            comment.PostId = id;

            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();

            var userInfo = _context.Users.Find(comment.UserId);

            return await _context.Comments.Select(x => new Comment()
            {
                CommentId = x.CommentId,
                CommentValue = x.CommentValue,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                User = x.User,
                MediaLink = x.MediaLink,
                PostId = x.PostId,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).Where(c => c.PostId == id).ToListAsync();
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return (_context.Comments?.Any(e => e.CommentId == id)).GetValueOrDefault();
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
