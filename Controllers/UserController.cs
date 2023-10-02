using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MarauderContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(MarauderContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            return await _context.Users.Select(user => new User()
            {
                UserId = user.UserId,
                Username = user.Username,
                FirstName = user.FirstName,
                About = user.About,
                ImageLink = user.ImageLink,
                Favorites = user.Favorites,
                Chats = user.Chats,
                Comments = user.Comments,
                Communities = user.Communities,
                Devices = user.Devices,
                DocFiles = user.DocFiles,
                Panels = user.Panels,
                Messages = user.Messages,
                ArtificialIntelligences = user.ArtificialIntelligences,
                Posts = user.Posts,
                Followers = user.Followers,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, user.ImageLink)
            }).AsNoTracking().ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, user.ImageLink);

            //return user;
            return new User()
            {
                UserId = user.UserId,
                Username = user.Username,
                FirstName = user.FirstName,
                About = user.About,
                ImageLink = user.ImageLink,
                Favorites = user.Favorites,
                Chats = user.Chats,
                Comments = user.Comments,
                Communities = user.Communities,
                Devices = user.Devices,
                DocFiles = user.DocFiles,
                Panels = user.Panels,
                Messages = user.Messages,
                ArtificialIntelligences = user.ArtificialIntelligences,
                Posts = user.Posts,
                Followers = user.Followers,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, user.ImageLink)
            };
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<User>> GetUsername(string username)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            user.ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, user.ImageLink);

            //return user;
            return new User()
            {
                UserId = user.UserId,
                Username = user.Username,
                FirstName = user.FirstName,
                About = user.About,
                ImageLink = user.ImageLink,
                Favorites = user.Favorites,
                Chats = user.Chats,
                Comments = user.Comments,
                Communities = user.Communities,
                Devices = user.Devices,
                DocFiles = user.DocFiles,
                Panels = user.Panels,
                Messages = user.Messages,
                ArtificialIntelligences = user.ArtificialIntelligences,
                Posts = user.Posts,
                Followers = user.Followers,
                ImageSource = String.Format("{0}://{1}{2}/images/{3}", Request.Scheme, Request.Host, Request.PathBase, user.ImageLink)
            };
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromForm] User user)
        {
            user.UserId = id;

            var local = _context.Set<User>()
                .Local
                .FirstOrDefault(entry => entry.UserId.Equals(id));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            if (user.ImageFile != null)
            {
                user.ImageLink = await SaveImage(user.ImageFile);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromForm] User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'PlanetNineDatabaseContext.Users'  is null.");
            }

            if (user.ImageFile != null)
            {
                user.ImageLink = await SaveImage(user.ImageFile);
            }

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
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
