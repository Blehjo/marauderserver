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
    public class NoteController : ControllerBase
    {
        private readonly MarauderContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;

        public NoteController(MarauderContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/Note
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNote()
        {
            if (_context.Notes == null)
            {
                return NotFound();
            }

            return await _context.Notes.Select(n => new Note() {
                NoteId = n.NoteId,
                NoteValue = n.NoteValue,
                MediaLink = n.MediaLink,
                XCoord = n.XCoord,
                YCoord = n.YCoord,
                DateCreated = n.DateCreated,
                PanelId = n.PanelId,
                Panel = n.Panel,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, n.MediaLink)
            }).ToListAsync();
        }

        // GET: api/Note/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            if (_context.Notes == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return new Note() 
            {
                NoteId = note.NoteId,
                NoteValue = note.NoteValue,
                MediaLink = note.MediaLink,
                XCoord = note.XCoord,
                YCoord = note.YCoord,
                DateCreated = note.DateCreated,
                PanelId = note.PanelId,
                Panel = note.Panel,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, note.MediaLink)
            };
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetSingleUserNotes(int id)
        {
            if (_context.Notes == null)
            {
                return NotFound();
            }

            return await _context.Notes.Where(n => n.PanelId == id).Select(n => new Note() {
                NoteId = n.NoteId,
                NoteValue = n.NoteValue,
                MediaLink = n.MediaLink,
                XCoord = n.XCoord,
                YCoord = n.YCoord,
                DateCreated = n.DateCreated,
                PanelId = n.PanelId,
                Panel = n.Panel,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, n.MediaLink)
            }).ToListAsync();
        }

        // PUT: api/Note/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Note>>> PutNote(int id, Note note)
        {
            if (id != note.NoteId)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.Notes.Where(n => n.PanelId == note.PanelId).Select(n => new Note() {
                NoteId = n.NoteId,
                NoteValue = n.NoteValue,
                MediaLink = n.MediaLink,
                XCoord = n.XCoord,
                YCoord = n.YCoord,
                DateCreated = n.DateCreated,
                PanelId = n.PanelId,
                Panel = n.Panel,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, n.MediaLink)
            }).ToListAsync();
        }

        // POST: api/Note
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Note>>> PostNote(Note note)
        {
          if (_context.Notes == null)
          {
              return Problem("Entity set 'MarauderContext.Note'  is null.");
          }
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return await _context.Notes.Where(n => n.PanelId == note.PanelId).Select(n => new Note() {
                NoteId = n.NoteId,
                NoteValue = n.NoteValue,
                MediaLink = n.MediaLink,
                XCoord = n.XCoord,
                YCoord = n.YCoord,
                DateCreated = n.DateCreated,
                PanelId = n.PanelId,
                Panel = n.Panel,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, n.MediaLink)
            }).ToListAsync();
        }

        // DELETE: api/Note/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Note>>> DeleteNote(int id)
        {
            if (_context.Notes == null)
            {
                return NotFound();
            }
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return await _context.Notes.Where(n => n.PanelId == note.PanelId).Select(n => new Note() {
                NoteId = n.NoteId,
                NoteValue = n.NoteValue,
                MediaLink = n.MediaLink,
                XCoord = n.XCoord,
                YCoord = n.YCoord,
                DateCreated = n.DateCreated,
                PanelId = n.PanelId,
                Panel = n.Panel,
                ImageSource = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, n.MediaLink)
            }).ToListAsync();
        }

        private bool NoteExists(int id)
        {
            return (_context.Notes?.Any(e => e.NoteId == id)).GetValueOrDefault();
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
