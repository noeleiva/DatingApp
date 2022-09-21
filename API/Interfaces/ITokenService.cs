using API.Entities;

namespace API.Interfaces
{
  public interface ITokenService
  {
    // interface contract between itself and any class that implements it
    // doesn't contain any implementation logic
    string CreateToken(AppUser user);
  }
}