using API.DTOs;
using API.Interfaces;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Entities;

namespace API.Controllers
{
  [Authorize]
  public class UsersController : BaseApiController
  {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
    {
      _photoService = photoService;
      _httpContextAccessor = httpContextAccessor;
      _mapper = mapper;
      _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
      var users = await _userRepository.GetMembersAsync();

      return new OkObjectResult(users);
    }

    [HttpGet("{username}", Name = "GetUser")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
      return await _userRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
      // var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var username = ClaimsPrincipleExtensions.GetUsername(_httpContextAccessor);

      var user = await _userRepository.GetUserByUsernameAsync(username);

      _mapper.Map(memberUpdateDto, user);

      _userRepository.Update(user);

      if (await _userRepository.SaveAllAsync()) return new NoContentResult();

      return new BadRequestObjectResult("Failed to update user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
      var username = ClaimsPrincipleExtensions.GetUsername(_httpContextAccessor);
      var user = await _userRepository.GetUserByUsernameAsync(username);
      var result = await _photoService.AddPhotoAsync(file);
      if (result.Error != null) return new BadRequestObjectResult(result.Error.Message);

      var photo = new Photo
      {
        Url = result.SecureUrl.AbsoluteUri,
        PublicId = result.PublicId
      };

      if (user.Photos.Count == 0)
      {
        photo.IsMain = true;
      }

      user.Photos.Add(photo);

      if (await _userRepository.SaveAllAsync())
      {
        return new CreatedAtRouteResult("GetUser", new { username = username }, _mapper.Map<PhotoDto>(photo));
      }

      return new BadRequestObjectResult("Failed to update user");

    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
      var username = ClaimsPrincipleExtensions.GetUsername(_httpContextAccessor);

      var user = await _userRepository.GetUserByUsernameAsync(username);

      var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

      if (photo.IsMain) return new BadRequestObjectResult("This is already your main photo");

      var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

      if (currentMain != null) currentMain.IsMain = false;

      photo.IsMain = true;

      if (await _userRepository.SaveAllAsync())
        return new NoContentResult();

      return new BadRequestObjectResult("Failed to set main photo");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
      var username = ClaimsPrincipleExtensions.GetUsername(_httpContextAccessor);

      var user = await _userRepository.GetUserByUsernameAsync(username);

      var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

      if (photo == null) return new NotFoundResult();

      if (photo.IsMain) return new BadRequestObjectResult("You cannot delete your main photo");

      if (photo.PublicId != null)
      {
        var result = await _photoService.DeletePhotoAsync(photo.PublicId);
        if (result.Error != null) return new BadRequestObjectResult(result.Error.Message);
      }

      user.Photos.Remove(photo);

      if (await _userRepository.SaveAllAsync()) return new OkResult();

      return new BadRequestObjectResult("Failed to delete the photo");
    }

  }
}