namespace API.Entities
{

  // user a common .net entity. Therefore we used AppUser to identified our user
  public class AppUser
  {

    // protected - only access by this class or inherited classes
    // private - only accessed within the class itself

    // .net will recognize this is a primary key of our DB table
    public int Id { get; set; }

    public string UserName { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }

    public DateTime DateOfBirth { get; set; }

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
    public ICollection<Photo> Photos { get; set; }

    // public int GetAge()
    // {
    //   return DateOfBirth.CalculateAge();
    // }

  }
}