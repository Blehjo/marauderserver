using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GltfsController : ControllerBase
    {
        private readonly MarauderContext _context;

        public GltfsController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/Gltfs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gltf>>> GetGltfs()
        {
            if (_context.Gltfs == null)
            {
                return NotFound();
            }

            return await _context.Gltfs.Select(g => new Gltf() {
                GltfId = g.GltfId,
                FileInformation = g.FileInformation,
                UserId = g.UserId,
                User = g.User
            }).ToListAsync();
        }

        // GET: api/Gltfs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gltf>> GetGltf(int id)
        {
            if (_context.Gltfs == null)
            {
                return NotFound();
            }

            var gltf = await _context.Gltfs.FindAsync(id);

            if (gltf == null)
            {
                return NotFound();
            }

            return new Gltf() {
                GltfId = gltf.GltfId,
                FileInformation = gltf.FileInformation,
                UserId = gltf.UserId,
                User = gltf.User
            };
        }

        // PUT: api/Gltfs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Gltf>>> PutGltf(int id, Gltf gltf)
        {
            if (id != gltf.GltfId)
            {
                return BadRequest();
            }

            _context.Entry(gltf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!GltfExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.Gltfs.Where(g => g.UserId == gltf.UserId).Select(g => new Gltf() {
                GltfId = g.GltfId,
                FileInformation = g.FileInformation,
                UserId = g.UserId,
                User = g.User
            }).ToListAsync();
        }

        // POST: api/Gltfs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Gltf>>> PostGltf(Gltf gltf)
        {
            if (_context.Gltfs == null)
            {
                return Problem("Entity set 'MarauderContext.Gltfs'  is null.");
            }
            
            _context.Gltfs.Add(gltf);
            await _context.SaveChangesAsync();

            return await _context.Gltfs.Where(g => g.UserId == gltf.UserId).Select(g => new Gltf() {
                GltfId = g.GltfId,
                FileInformation = g.FileInformation,
                UserId = g.UserId,
                User = g.User
            }).ToListAsync();
        }

        // DELETE: api/Gltfs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Gltf>>> DeleteGltf(int id)
        {
            if (_context.Gltfs == null)
            {
                return NotFound();
            }

            var gltf = await _context.Gltfs.FindAsync(id);
            if (gltf == null)
            {
                return NotFound();
            }

            _context.Gltfs.Remove(gltf);
            await _context.SaveChangesAsync();

            return await _context.Gltfs.Where(g => g.UserId == gltf.UserId).Select(g => new Gltf() {
                GltfId = g.GltfId,
                FileInformation = g.FileInformation,
                UserId = g.UserId,
                User = g.User
            }).ToListAsync();
        }

        private bool GltfExists(int id)
        {
            return (_context.Gltfs?.Any(e => e.GltfId == id)).GetValueOrDefault();
        }
    }
}
