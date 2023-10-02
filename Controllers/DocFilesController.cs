using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocFilesController : ControllerBase
    {
        private readonly MarauderContext _context;

        public DocFilesController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/DocFiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocFile>>> GetDocFiles()
        {
            if (_context.DocFiles == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.DocFiles.Where(d => d.UserId == userId).Select(d => new DocFile() {
                DocFileId = d.DocFileId,
                Title = d.Title,
                UserId = d.UserId,
                User = d.User,
                Moveables = d.Moveables
            }).ToListAsync();
        }

        // GET: api/DocFiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocFile>> GetDocFile(int id)
        {
            if (_context.DocFiles == null)
            {
                return NotFound();
            }

            var docFile = await _context.DocFiles.FindAsync(id);

            if (docFile == null)
            {
                return NotFound();
            }

            return docFile;
        }

        // PUT: api/DocFiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<DocFile>>> PutDocFile(int id, DocFile docFile)
        {
            if (id != docFile.DocFileId)
            {
                return BadRequest();
            }

            _context.Entry(docFile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocFileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.DocFiles.Where(d => d.UserId == docFile.UserId).Select(d => new DocFile() {
                DocFileId = d.DocFileId,
                Title = d.Title,
                UserId = d.UserId,
                User = d.User,
                Moveables = d.Moveables
            }).ToListAsync();
        }

        // POST: api/DocFiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<DocFile>>> PostDocFile(DocFile docFile)
        {
            if (_context.DocFiles == null)
            {
                return Problem("Entity set 'MarauderContext.DocFiles'  is null.");
            }

            docFile.UserId = HttpContext.Request.Cookies["user"];

            _context.DocFiles.Add(docFile);

            await _context.SaveChangesAsync();

            return await _context.DocFiles.Where(d => d.UserId == docFile.UserId).Select(d => new DocFile() {
                DocFileId = d.DocFileId,
                Title = d.Title,
                UserId = d.UserId,
                User = d.User,
                Moveables = d.Moveables
            }).ToListAsync();
        }

        // DELETE: api/DocFiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<DocFile>>> DeleteDocFile(int id)
        {
            if (_context.DocFiles == null)
            {
                return NotFound();
            }
            var docFile = await _context.DocFiles.FindAsync(id);
            if (docFile == null)
            {
                return NotFound();
            }

            _context.DocFiles.Remove(docFile);
            await _context.SaveChangesAsync();

            return await _context.DocFiles.Where(d => d.UserId == docFile.UserId).Select(d => new DocFile() {
                DocFileId = d.DocFileId,
                Title = d.Title,
                UserId = d.UserId,
                User = d.User,
                Moveables = d.Moveables
            }).ToListAsync();
        }

        private bool DocFileExists(int id)
        {
            return (_context.DocFiles?.Any(e => e.DocFileId == id)).GetValueOrDefault();
        }
    }
}
