using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class UsersController : BaseApiController
  {
    private readonly DataContext _context;
    public UsersController(DataContext context)
    {
      _context = context;
    }

    // get all the Users
    // Synchronous code below, not very efficient. Here we blocking threads
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
      return await _context.Users.ToListAsync();
    }

    // get a specific user
    // sample: api/users/3
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
      return await _context.Users.FindAsync(id);
    }
  }
}