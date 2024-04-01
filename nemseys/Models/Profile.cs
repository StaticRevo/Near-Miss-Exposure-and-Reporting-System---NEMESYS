public class Profile
{
    public int ProfileId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ProfileType { get; set; }

    public Credentials Credentials { get; set; }
}
