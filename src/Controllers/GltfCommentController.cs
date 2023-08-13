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
    public class GltfCommentController : ControllerBase
    {
        private readonly MarauderContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public GltfCommentController(MarauderContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/GltfComment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GltfComment>>> GetGltfComments()
        {
            if (_context.GltfComments == null)
            {
                return NotFound();
            }

            return await _context.GltfComments.Select(x => new GltfComment()
            {
                GltfCommentId = x.GltfCommentId,
                CommentValue = x.CommentValue,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                GltfId = x.GltfId,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // GET: api/GltfComment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GltfComment>> GetGltfComment(int id)
        {
          if (_context.GltfComments == null)
          {
              return NotFound();
          }
            var gltfComment = await _context.GltfComments.FindAsync(id);

            if (gltfComment == null)
            {
                return NotFound();
            }

            return gltfComment;
        }

        [HttpGet("gltf/{id}")]
        public async Task<ActionResult<IEnumerable<GltfComment>>> GetPostComments(int id)
        {
            if (_context.GltfComments == null)
            {
                return NotFound();
            }

            return await _context.GltfComments.Select(x => new GltfComment()
            {
                GltfCommentId = x.GltfCommentId,
                CommentValue = x.CommentValue,
                DateCreated = x.DateCreated,
                User = x.User,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                GltfId = x.GltfId,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).Where(c => c.GltfId == id).ToListAsync();
        }

        // PUT: api/GltfComment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGltfComment(int id, GltfComment gltfComment)
        {
            if (id != gltfComment.GltfCommentId)
            {
                return BadRequest();
            }

            _context.Entry(gltfComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GltfCommentExists(id))
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

        // POST: api/GltfComment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<GltfComment>>> PostGltfComment(int id, [FromForm] GltfComment gltfComment)
        {
            if (_context.GltfComments == null)
            {
                return Problem("Entity set 'MarauderContext.GltfComments'  is null.");
            }

            if (gltfComment.ImageFile != null)
            {
                gltfComment.MediaLink = await SaveImage(gltfComment.ImageFile);
            }

            gltfComment.UserId = HttpContext.Request.Cookies["user"];

            gltfComment.GltfId = id;

            _context.GltfComments.Add(gltfComment);

            await _context.SaveChangesAsync();

            return await _context.GltfComments.Select(x => new GltfComment()
            {
                GltfCommentId = x.GltfCommentId,
                CommentValue = x.CommentValue,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                GltfId = x.GltfId,
                Favorites = x.Favorites,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).Where(c => c.GltfId == id).ToListAsync();
        }

        // DELETE: api/GltfComment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGltfComment(int id)
        {
            if (_context.GltfComments == null)
            {
                return NotFound();
            }
            var gltfComment = await _context.GltfComments.FindAsync(id);
            if (gltfComment == null)
            {
                return NotFound();
            }

            _context.GltfComments.Remove(gltfComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GltfCommentExists(int id)
        {
            return (_context.GltfComments?.Any(e => e.GltfCommentId == id)).GetValueOrDefault();
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
