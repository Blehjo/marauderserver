using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly MarauderContext _context;

        public FavoriteController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/favorite
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favorite>>> GetFavorite()
        {
          if (_context.Favorites == null)
          {
              return NotFound();
          }
            return await _context.Favorites.ToListAsync();
        }

        // GET: api/favorite/user
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Favorite>>> GetUserFavorites()
        {
            if (_context.Favorites == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.Favorites.Where(f => f.UserId == userId).Select(x => new Favorite()
            {
                FavoriteId = x.FavoriteId,
                ContentId = x.ContentId,
                UserId = x.UserId,
                User = x.User,
                ContentType = x.ContentType,
                DateCreated = x.DateCreated
            }).ToListAsync();
        }


        // GET: api/favorite/user/id
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Favorite>>> GetSingleUserFavorites(string id)
        {
            if (_context.Favorites == null)
            {
                return NotFound();
            }

            return await _context.Favorites.Where(f => f.UserId == id).ToListAsync();
        }

        // GET: api/favorite/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Favorite>> GetFavorite(int id)
        {
          if (_context.Favorites == null)
          {
              return NotFound();
          }
            var favorite = await _context.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }

            return favorite;
        }

        // PUT: api/favorite/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavorite(int id, Favorite favorite)
        {
            if (id != favorite.FavoriteId)
            {
                return BadRequest();
            }

            _context.Entry(favorite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteExists(id))
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

        // POST: api/favorite
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Favorite>> PostFavorite(Favorite favorite)
        {
          if (_context.Favorites == null)
          {
              return Problem("Entity set 'MarauderContext.Favorite'  is null.");
          }

            favorite.UserId = HttpContext.Request.Cookies["user"];

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavorite", new { id = favorite.FavoriteId }, favorite);
        }

        // DELETE: api/favorite/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorite(int id)
        {
            if (_context.Favorites == null)
            {
                return NotFound();
            }
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FavoriteExists(int id)
        {
            return (_context.Favorites?.Any(e => e.FavoriteId == id)).GetValueOrDefault();
        }
    }
}
