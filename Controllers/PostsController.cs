using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly MarauderContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostController(MarauderContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            //var favorites = await _context.Favorites.Where(f => f.ContentId == )

            return await _context.Posts.Select(x => new Post() {
                PostId = x.PostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                User = x.User,
                UserId = x.UserId,
                Comments = x.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.PostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int? id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var userInfo = _context.Users.Find(post.UserId);

            return new Post()
            {
                PostId = post.PostId,
                PostValue = post.PostValue,
                MediaLink = post.MediaLink,
                UserId = post.UserId,
                User = userInfo,
                Comments = post.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == post.PostId && f.ContentType == "post").ToList(),
                DateCreated = post.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, post.MediaLink)
            };
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Post>>> GetUserPosts()
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.Posts.Where(p => p.UserId == userId).Select(x => new Post()
            {
                PostId = x.PostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                UserId = x.UserId,
                Comments = x.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.PostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetSingleUserPosts(string id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }

            return await _context.Posts.Where(p => p.UserId == id).Select(x => new Post()
            {
                PostId = x.PostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                UserId = x.UserId,
                Comments = x.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.PostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Post>>> PutPost(int? id, Post post)
        {
            if (id != post.PostId)
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

            return await _context.Posts.Where(p => p.UserId == post.UserId).Select(x => new Post()
            {
                PostId = x.PostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                UserId = x.UserId,
                Comments = x.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.PostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // POST: api/Post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Post>>> PostPost([FromForm] Post post)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'MarauderContext.Posts'  is null.");
            }

            if (post.ImageFile != null)
            {
                post.MediaLink = await SaveImage(post.ImageFile);
            }

            post.UserId = HttpContext.Request.Cookies["user"];

            _context.Posts.Add(post);

            await _context.SaveChangesAsync();

            return await _context.Posts.Where(p => p.UserId == post.UserId).Select(x => new Post()
            {
                PostId = x.PostId,
                PostValue = x.PostValue,
                MediaLink = x.MediaLink,
                UserId = x.UserId,
                Comments = x.Comments,
                Favorites = _context.Favorites.Where(f => f.ContentId == x.PostId && f.ContentType == "post").ToList(),
                DateCreated = x.DateCreated,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Post>>> DeletePost(int? id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.Posts.Where(c => c.UserId == userId).ToListAsync();
        }

        private bool PostExists(int? id)
        {
            return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
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
