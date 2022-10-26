using System.Security.Claims;

namespace API.Extensions
{
  public static class ClaimsPrincipleExtensions
  {
    public static string GetUsername(IHttpContextAccessor httpContextAccessor)
    {
      var username = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      // return user.FindFirst(ClaimTypes.Name)?.Value;
      return username;

    }

    public static int GetUserId(this ClaimsPrincipal user, IHttpContextAccessor httpContextAccessor)
    {
      var username = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      // return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
      return int.Parse(username);
    }
  }
}