public class Credentials
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public byte[] Salt { get; set; }
    public byte[] PasswordHash { get; set; }
    public int ProfileId { get; set; }
}
