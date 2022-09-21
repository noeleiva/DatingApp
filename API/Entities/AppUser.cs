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

  }
}