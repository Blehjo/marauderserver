using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityPostController : ControllerBase
    {
        private readonly MarauderContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CommunityPostController(MarauderContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunityPost>>> GetPosts()
        {
            if (_context.CommunityPosts == null)
            {
                return NotFound();
            }

            //var favorites = await _context.Favorites.Where(f => f.ContentId == )

            return await _context.CommunityPosts.Select(x => new CommunityPost()
            {
                CommunityPostId = x.CommunityPostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                User = x.User,
                UserId = x.UserId,
                CommunityComments = x.CommunityComments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.CommunityPostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommunityPost>> GetPost(int? id)
        {
            if (_context.CommunityPosts == null)
            {
                return NotFound();
            }

            var post = await _context.CommunityPosts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var userInfo = _context.Users.Find(post.UserId);

            return new CommunityPost()
            {
                CommunityPostId = post.CommunityPostId,
                PostValue = post.PostValue,
                MediaLink = post.MediaLink,
                UserId = post.UserId,
                User = userInfo,
                CommunityComments = post.CommunityComments,
                Favorites = _context.Favorites.Where(f => f.ContentId == post.CommunityPostId && f.ContentType == "post").ToList(),
                DateCreated = post.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, post.MediaLink)
            };
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<CommunityPost>>> GetUserPosts()
        {
            if (_context.CommunityPosts == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.CommunityPosts.Where(p => p.UserId == userId).Select(x => new CommunityPost()
            {
                CommunityPostId = x.CommunityPostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                UserId = x.UserId,
                CommunityComments = x.CommunityComments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.CommunityPostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<CommunityPost>>> GetSingleUserPosts(string id)
        {
            if (_context.CommunityPosts == null)
            {
                return NotFound();
            }

            return await _context.CommunityPosts.Where(p => p.UserId == id).Select(x => new CommunityPost()
            {
                CommunityPostId = x.CommunityPostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                UserId = x.UserId,
                CommunityComments = x.CommunityComments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.CommunityPostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<CommunityPost>>> PutPost(int? id, CommunityPost post)
        {
            if (id != post.CommunityPostId)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.CommunityPosts.Where(p => p.UserId == post.UserId).Select(x => new CommunityPost()
            {
                CommunityPostId = x.CommunityPostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                UserId = x.UserId,
                CommunityComments = x.CommunityComments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.CommunityPostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // POST: api/Post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CommunityPost>>> PostPost([FromForm] CommunityPost post)
        {
            if (_context.CommunityPosts == null)
            {
                return Problem("Entity set 'MarauderContext.Posts'  is null.");
            }

            if (post.ImageFile != null)
            {
                post.MediaLink = await SaveImage(post.ImageFile);
            }

            post.UserId = HttpContext.Request.Cookies["user"];

            _context.CommunityPosts.Add(post);

            await _context.SaveChangesAsync();

            return await _context.CommunityPosts.Where(p => p.UserId == post.UserId).Select(x => new CommunityPost()
            {
                CommunityPostId = x.CommunityPostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                UserId = x.UserId,
                CommunityComments = x.CommunityComments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.CommunityPostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<CommunityPost>>> DeletePost(int? id)
        {
            if (_context.CommunityPosts == null)
            {
                return NotFound();
            }
            var post = await _context.CommunityPosts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.CommunityPosts.Remove(post);
            await _context.SaveChangesAsync();

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.CommunityPosts.Where(c => c.UserId == userId).ToListAsync();
        }

        private bool PostExists(int? id)
        {
            return (_context.CommunityPosts?.Any(e => e.CommunityPostId == id)).GetValueOrDefault();
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
