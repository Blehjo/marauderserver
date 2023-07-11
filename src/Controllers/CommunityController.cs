using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly MarauderContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CommunityController(MarauderContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/Community
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Community>>> GetCommunities()
        {
            if (_context.Communities == null)
            {
                return NotFound();
            }
            
            return await _context.Communities.Select(x => new Community() {
                CommunityId = x.CommunityId,
                CommunityName = x.CommunityName,
                Description = x.Description,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                User = x.User,
                Members = x.Members,
                Channels = x.Channels,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // GET: api/Community/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Community>> GetCommunity(int id)
        {
            if (_context.Communities == null)
            {
                return NotFound();
            }

            var community = await _context.Communities.FindAsync(id);

            if (community == null)
            {
                return NotFound();
            }

            community.ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, community.MediaLink);

            return new Community() 
            {
                CommunityId = community.CommunityId,
                CommunityName = community.CommunityName,
                Description = community.Description,
                DateCreated = community.DateCreated,
                UserId = community.UserId,
                MediaLink = community.MediaLink,
                User = community.User,
                Members = community.Members,
                Channels = community.Channels,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, community.MediaLink)
            };
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Community>>> GetSingleUserCommunities(string id)
        {
            if (_context.Communities == null)
            {
                return NotFound();
            }

            return await _context.Communities.Where(c => c.UserId == id).Select(x => new Community()
            {
                CommunityId = x.CommunityId,
                CommunityName = x.CommunityName,
                Description = x.Description,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // PUT: api/Community/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Community>>> PutCommunity(int id, Community community)
        {
            if (id != community.CommunityId)
            {
                return BadRequest();
            }

            _context.Entry(community).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.Communities.Where(c => c.UserId == community.UserId).Select(x => new Community()
            {
                CommunityId = x.CommunityId,
                CommunityName = x.CommunityName,
                Description = x.Description,
                DateCreated = x.DateCreated,
                UserId = x.UserId,
                MediaLink = x.MediaLink,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.MediaLink)
            }).ToListAsync();
        }

        // POST: api/Community
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Community>> PostCommunity([FromForm] Community community)
        {
            if (_context.Communities == null)
            {
                return Problem("Entity set 'MarauderContext.Communities'  is null.");
            }

            community.UserId = HttpContext.Request.Cookies["user"];

            if (community.ImageFile != null)
            {
                community.MediaLink = await SaveImage(community.ImageFile);
            }
            
            _context.Communities.Add(community);

            await _context.SaveChangesAsync();

            return new Community() 
            {
                CommunityId = community.CommunityId,
                CommunityName = community.CommunityName,
                Description = community.Description,
                DateCreated = community.DateCreated,
                UserId = community.UserId,
                MediaLink = community.MediaLink,
                User = community.User,
                Members = community.Members,
                Channels = community.Channels,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, community.MediaLink)
            };
        }

        // DELETE: api/Community/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Community>>> DeleteCommunity(int id)
        {
            if (_context.Communities == null)
            {
                return NotFound();
            }
            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.Communities.Where(c => c.UserId == userId).ToListAsync();
        }

        private bool CommunityExists(int id)
        {
            return (_context.Communities?.Any(e => e.CommunityId == id)).GetValueOrDefault();
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
