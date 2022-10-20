namespace API.DTOs
{
  public class MemberDto
  {
    public int Id { get; set; }

    public string Username { get; set; }

    public string PhotoUrl { get; set; }

    public int Age { get; set; }

    public string KnownAs { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public DateTime LastActive { get; set; } = DateTime.Now;

    public string Gender { get; set; }

    public string Introduction { get; set; }

    public string LookingFor { get; set; }

    public string Interest { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    // One user can have many photos
    // Need to fully define a relationship
    public ICollection<PhotoDto> Photos { get; set; }
  }
}