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
    public class PanelController : ControllerBase
    {
        private readonly MarauderContext _context;

        public PanelController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/Panel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Panel>>> GetPanels()
        {
            if (_context.Panels == null)
            {
                return NotFound();
            }

            return await _context.Panels.Select(p => new Panel() {
                PanelId = p.PanelId,
                Title = p.Title,
                XCoord = p.XCoord,
                YCoord = p.YCoord,
                DateCreated = p.DateCreated,
                UserId = p.UserId,
                User = p.User,
                Notes = p.Notes
            }).ToListAsync();
        }

        // GET: api/Panel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Panel>> GetPanel(int id)
        {
          if (_context.Panels == null)
          {
              return NotFound();
          }
            var panel = await _context.Panels.FindAsync(id);

            if (panel == null)
            {
                return NotFound();
            }

            return new Panel() {
                PanelId = panel.PanelId,
                Title = panel.Title,
                XCoord = panel.XCoord,
                YCoord = panel.YCoord,
                DateCreated = panel.DateCreated,
                UserId = panel.UserId,
                User = panel.User,
                Notes = panel.Notes
            };
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Panel>>> GetSingleUserPanels(string id)
        {
            if (_context.Panels == null)
            {
                return NotFound();
            }

            return await _context.Panels.Where(p => p.UserId == id).Select(p => new Panel() {
                PanelId = p.PanelId,
                Title = p.Title,
                XCoord = p.XCoord,
                YCoord = p.YCoord,
                DateCreated = p.DateCreated,
                UserId = p.UserId,
                User = p.User,
                Notes = p.Notes
            }).ToListAsync();
        }

        // GET: api/Panel/users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Panel>>> GetUserPanel()
        {
            if (_context.Panels == null)
            {
                return NotFound();
            }

            var user = HttpContext.Request.Cookies["user"];

            return await _context.Panels.Where(p => p.UserId == user).Select(p => new Panel() {
                PanelId = p.PanelId,
                Title = p.Title,
                XCoord = p.XCoord,
                YCoord = p.YCoord,
                DateCreated = p.DateCreated,
                UserId = p.UserId,
                User = p.User,
                Notes = p.Notes
            }).ToListAsync();
        }

        // PUT: api/Panel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Panel>>> PutPanel(int id, Panel panel)
        {
            if (id != panel.PanelId)
            {
                return BadRequest();
            }

            _context.Entry(panel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PanelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.Panels.Where(p => p.UserId == userId).Select(p => new Panel() {
                PanelId = p.PanelId,
                Title = p.Title,
                XCoord = p.XCoord,
                YCoord = p.YCoord,
                DateCreated = p.DateCreated,
                UserId = p.UserId,
                User = p.User,
                Notes = p.Notes
            }).ToListAsync();
        }

        // POST: api/Panel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Panel>>> PostPanel(Panel panel)
        {
          if (_context.Panels == null)
          {
              return Problem("Entity set 'MarauderContext.Panels'  is null.");
          }

            panel.UserId = HttpContext.Request.Cookies["user"];

            _context.Panels.Add(panel);
            await _context.SaveChangesAsync();

            return await _context.Panels.Where(p => p.UserId == panel.UserId).Select(p => new Panel() {
                PanelId = p.PanelId,
                Title = p.Title,
                XCoord = p.XCoord,
                YCoord = p.YCoord,
                DateCreated = p.DateCreated,
                UserId = p.UserId,
                User = p.User,
                Notes = p.Notes
            }).ToListAsync();
        }

        // DELETE: api/Panel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Panel>>> DeletePanel(int id)
        {
            if (_context.Panels == null)
            {
                return NotFound();
            }
            var panel = await _context.Panels.FindAsync(id);
            if (panel == null)
            {
                return NotFound();
            }

            _context.Panels.Remove(panel);
            await _context.SaveChangesAsync();

            return await _context.Panels.Where(p => p.UserId == panel.UserId).Select(p => new Panel() {
                PanelId = p.PanelId,
                Title = p.Title,
                XCoord = p.XCoord,
                YCoord = p.YCoord,
                DateCreated = p.DateCreated,
                UserId = p.UserId,
                User = p.User,
                Notes = p.Notes
            }).ToListAsync();
        }

        private bool PanelExists(int id)
        {
            return (_context.Panels?.Any(e => e.PanelId == id)).GetValueOrDefault();
        }
    }
}
