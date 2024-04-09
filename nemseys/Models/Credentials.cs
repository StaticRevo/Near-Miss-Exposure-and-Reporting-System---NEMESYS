public class Credentials
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public byte[] Salt { get; set; }
    public string PasswordHash { get; set; } // Define PasswordHash property
    public int ProfileId { get; set; }
}
