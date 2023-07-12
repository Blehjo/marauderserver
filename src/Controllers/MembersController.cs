using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marauderserver.Data;
using marauderserver.Models;

namespace marauderserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly MarauderContext _context;

        public MembersController(MarauderContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
          if (_context.Members == null)
          {
              return NotFound();
          }
            return await _context.Members.ToListAsync();
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Member>>> GetCommunityMembers(int id)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }

            return await _context.Members.Where(m => m.CommunityId == id).Select(x => new Member()
            {
                MemberId = x.MemberId,
                DateCreated = x.DateCreated,
                Community = x.Community,
                UserId = x.UserId,
                User = x.User
            }).ToListAsync();
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            if (_context.Members == null)
            {
                return Problem("Entity set 'MarauderContext.Members'  is null.");
            }

            member.UserId = HttpContext.Request.Cookies["user"];

            //if (_context.Members.Any(m => m.UserId == member.UserId))
            //{
            //    return Problem("User already a member");
            //}

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
