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
    public class MessageController : ControllerBase
    {
        private readonly MarauderContext _context;

        public MessageController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/Message
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
          if (_context.Messages == null)
          {
              return NotFound();
          }
            var userId = HttpContext.Request.Cookies["user"];

            var user = await _context.Users.FindAsync(userId);

            return await _context.Messages.Where(m => m.UserId == userId || m.MessageValue == user.Username).Select(x => new Message()
            {
                MessageId = x.MessageId,
                MessageValue = x.MessageValue,
                DateCreated = x.DateCreated,
                User = x.User,
                UserId = x.UserId,
                ReceiverId = x.ReceiverId,
                MessageComments = x.MessageComments
            }).ToListAsync();
        }

        // GET: api/Message/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
          if (_context.Messages == null)
          {
              return NotFound();
          }
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Message/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, Message message)
        {
            if (id != message.MessageId)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        // POST: api/Message
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            if (_context.Messages == null)
            {
                return Problem("Entity set 'PlanetNineDatabaseContext.Messages'  is null.");
            }

            message.UserId = Request.Cookies["user"];

            List<Message> returnedMessage = await _context.Messages.Where(m => m.MessageValue == message.MessageValue && m.UserId == message.UserId).ToListAsync();
            
            if (returnedMessage.Count() > 0)
            {
                return CreatedAtAction("GetMessage", returnedMessage);
            }

            message.Receiver = await _context.Users.FindAsync(message.ReceiverId);

            _context.Messages.Add(message);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.MessageId }, message);
        }

        // DELETE: api/Message/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> DeleteMessage(int id)
        {
            if (_context.Messages == null)
            {
                return NotFound();
            }

            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);

            await _context.SaveChangesAsync();

            var userId = HttpContext.Request.Cookies["user"];

            return await _context.Messages.Where(m => m.UserId == userId).ToListAsync();
        }

        private bool MessageExists(int id)
        {
            return (_context.Messages?.Any(e => e.MessageId == id)).GetValueOrDefault();
        }
    }
}
