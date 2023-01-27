namespace API.Helpers
{
  public class UserParams
  {
    private const int MaxPageSize = 50;

    public int PageNumber { get; set; } = 1; // always return the first page unless otherwise

    private int _pageSize = 10; // how many we are going to return

    private int myVar;

    public int PageSize
    {
      get => _pageSize;
      set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string CurrentUsername { get; set; }

    public string Gender { get; set; }

    public int MinAge { get; set; } = 18;

    public int MaxAge { get; set; } = 100;

    public int MyProperty { get; set; }

    public string OrderBy { get; set; } = "lastActive";

  }
}