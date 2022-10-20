using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
  public class BuggyController : BaseApiController
  {
    private readonly DataContext _context;

    public BuggyController(DataContext context)
    {
      _context = context;
    }

    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
      return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
      var thing = _context.Users.Find(-1);
      if (thing == null) return new NotFoundResult();

      return new OkObjectResult(thing);
    }

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
      var thing = _context.Users.Find(-1);
      var thingToReturn = thing.ToString();
      return thingToReturn;
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
      // The below bad request doesn't send back a 404 error. Need to figure out how to get it working
      // return new ObjectResult("This was not a good request") { StatusCode = 400 };
      return new BadRequestObjectResult("This was not a good request");


    }

  }
}