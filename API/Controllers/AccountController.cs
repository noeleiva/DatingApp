using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext context, ITokenService tokenService)
    {
      _context = context;
      _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
      if (await UserExists(registerDto.Username.ToLower())) return new BadRequestObjectResult("Username is taken");

      using var hmac = new HMACSHA512();
      var user = new AppUser
      {
        UserName = registerDto.Username.ToLower(),
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        PasswordSalt = hmac.Key
      };
      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return new UserDto
      {
        Username = user.UserName,
        Token = _tokenService.CreateToken(user)
      };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      AppUser user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

      if (user == null) return new UnauthorizedObjectResult("Invalid User");

      using var hmac = new HMACSHA512(user.PasswordSalt);

      var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

      for (int i = 0; i < computeHash.Length; i++)
      {
        if (computeHash[i] != user.PasswordHash[i]) return new UnauthorizedObjectResult("Invalid Password");
      }

      return new UserDto
      {
        Username = user.UserName,
        Token = _tokenService.CreateToken(user)
      };
    }

    private async Task<bool> UserExists(string username)
    {
      return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
  }
}