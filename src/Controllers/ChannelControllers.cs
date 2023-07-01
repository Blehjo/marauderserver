using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly MarauderContext _context;

        public ChannelController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/Channel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Channel>>> GetChannels()
        {
            if (_context.Channels == null)
            {
                return NotFound();
            }

            return await _context.Channels.Select(x => new Channel() {
                ChannelId = x.ChannelId,
                Description = x.Description,
                CommunityId = x.CommunityId,
                Community = x.Community,
                ChannelComments = x.ChannelComments,
                DateCreated = x.DateCreated
            }).ToListAsync();
        }

        // GET: api/Channel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Channel>> GetChannel(int id)
        {
          if (_context.Channels == null)
          {
              return NotFound();
          }
            var channel = await _context.Channels.FindAsync(id);

            if (channel == null)
            {
                return NotFound();
            }

            return new Channel() {
                ChannelId = channel.ChannelId,
                Description = channel.Description,
                CommunityId = channel.CommunityId,
                Community = channel.Community,
                ChannelComments = channel.ChannelComments,
                DateCreated = channel.DateCreated
            };
        }

        [HttpGet("community/{id}")]
        public async Task<ActionResult<IEnumerable<Channel>>> GetSingleCommunityChannels(int id)
        {
            if (_context.Channels == null)
            {
                return NotFound();
            }

            return await _context.Channels.Where(c => c.CommunityId == id).Select(c => new Channel() {
                ChannelId = c.ChannelId,
                Description = c.Description,
                CommunityId = c.CommunityId,
                Community = c.Community,
                ChannelComments = c.ChannelComments,
                DateCreated = c.DateCreated
            }).ToListAsync();
        }

        // PUT: api/Channel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Channel>>> PutChannel(int id, Channel channel)
        {
            if (id != channel.ChannelId)
            {
                return BadRequest();
            }

            _context.Entry(channel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChannelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.Channels.Where(c => c.CommunityId == channel.ChannelId).Select(c => new Channel() {
                ChannelId = c.ChannelId,
                Description = c.Description,
                CommunityId = c.CommunityId,
                Community = c.Community,
                ChannelComments = c.ChannelComments,
                DateCreated = c.DateCreated
            }).ToListAsync();
        }

        // POST: api/Channel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Channel>>> PostChannel(Channel channel)
        {
          if (_context.Channels == null)
          {
              return Problem("Entity set 'MarauderContext.Channels'  is null.");
          }

            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();

            return await _context.Channels.Where(c => c.CommunityId == channel.ChannelId).Select(c => new Channel() {
                ChannelId = c.ChannelId,
                Description = c.Description,
                CommunityId = c.CommunityId,
                Community = c.Community,
                ChannelComments = c.ChannelComments,
                DateCreated = c.DateCreated
            }).ToListAsync();
        }

        // DELETE: api/Channel/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Channel>>> DeleteChannel(int id)
        {
            if (_context.Channels == null)
            {
                return NotFound();
            }
            var channel = await _context.Channels.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }

            _context.Channels.Remove(channel);
            await _context.SaveChangesAsync();

            return await _context.Channels.Where(c => c.CommunityId == channel.ChannelId).Select(c => new Channel() {
                ChannelId = c.ChannelId,
                Description = c.Description,
                CommunityId = c.CommunityId,
                Community = c.Community,
                ChannelComments = c.ChannelComments,
                DateCreated = c.DateCreated
            }).ToListAsync();
        }

        private bool ChannelExists(int id)
        {
            return (_context.Channels?.Any(e => e.ChannelId == id)).GetValueOrDefault();
        }
    }
}
