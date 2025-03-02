using Microsoft.AspNetCore.Mvc;
using TaskManagement.DbContexts;
using TaskManagement.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly TaskItemContext _context;
        private readonly IMapper _mapper;

        public UsersController(TaskItemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
