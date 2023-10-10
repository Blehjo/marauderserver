using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;
using Microsoft.AspNetCore.SignalR;
using marauderserver.Hubs;
using Microsoft.Extensions.Hosting;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShapeController : ControllerBase
    {
        private readonly MarauderContext _context;

        private readonly IHubContext<EditorHub> _editorHub;

        public ShapeController(MarauderContext context, IHubContext<EditorHub> editorHub)
        {
            _context = context;
            _editorHub = editorHub;
        }

        // GET: api/Shape
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shape>>> GetShapes()
        {
            if (_context.Shapes == null)
            {
                return NotFound();
            }

            return await _context.Shapes.ToListAsync();
        }

        // GET: api/Shape/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Shape>>> GetShape(int id)
        {
            if (_context.Shapes == null)
            {
                return NotFound();
            }

            return await _context.Shapes.Where(s => s.GltfId == id).Select(x => new Shape()
            {
                ShapeId = x.ShapeId,
                ShapeName = x.ShapeName,
                PositionX = x.PositionX,
                PositionY = x.PositionY,
                PositionZ = x.PositionZ,
                Height = x.Height,
                Width = x.Width,
                Depth = x.Depth,
                Radius = x.Radius,
                Length = x.Length,
                Color = x.Color,
                ColorValue = x.ColorValue,
                GltfId = x.GltfId,
                Gltf = x.Gltf
            }).ToListAsync();
        }

        // GET: api/Shape/shape/5
        [HttpGet("shape/{id}")]
        public async Task<ActionResult<Shape>> GetSingleShape(int id)
        {
            if (_context.Shapes == null)
            {
                return NotFound();
            }

            var shape = await _context.Shapes.FindAsync(id);

            if (shape == null)
            {
                return NotFound();
            }

            return new Shape()
            {
                ShapeId = shape.ShapeId,
                ShapeName = shape.ShapeName,
                PositionX = shape.PositionX,
                PositionY = shape.PositionY,
                PositionZ = shape.PositionZ,
                Height = shape.Height,
                Width = shape.Width,
                Depth = shape.Depth,
                Radius = shape.Radius,
                Length = shape.Length,
                Color = shape.Color,
                ColorValue = shape.ColorValue,
                GltfId = shape.GltfId,
                Gltf = shape.Gltf
            };
        }
        // PUT: api/Shape/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShape(int id, Shape shape)
        {
            if (id != shape.ShapeId)
            {
                return BadRequest();
            }

            _context.Entry(shape).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShapeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await _editorHub.Clients.All.SendAsync("shapeReceived", shape);

            return NoContent();
        }

        // POST: api/Shape
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<ActionResult<Shape>> PostShape(int id, Shape shape)
        {
            if (_context.Shapes == null)
            {
                return Problem("Entity set 'MarauderContext.Shapes'  is null.");
            }

            _context.Shapes.Add(shape);

            await _context.SaveChangesAsync();

            await _editorHub.Clients.All.SendAsync("shapeReceived", shape);

            return CreatedAtAction("GetShape", new { id = shape.ShapeId }, shape);
        }

        // DELETE: api/Shape/5
        [HttpDelete("{id}/{shapeId}")]
        public async Task<ActionResult<IEnumerable<Shape>>> DeleteShape(int id, int shapeId)
        {
            if (_context.Shapes == null)
            {
                return NotFound();
            }
            var shape = await _context.Shapes.FindAsync(id);
            if (shape == null)
            {
                return NotFound();
            }

            _context.Shapes.Remove(shape);
            await _context.SaveChangesAsync();

            return await _context.Shapes.Where(s => s.GltfId == id).ToListAsync();
        }

        private bool ShapeExists(int id)
        {
            return (_context.Shapes?.Any(e => e.ShapeId == id)).GetValueOrDefault();
        }
    }
}
