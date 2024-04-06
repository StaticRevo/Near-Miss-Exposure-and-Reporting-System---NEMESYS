using System.Collections.Generic; // Required for ICollection<T>
using Nemesis.Models;

public class Profile
{
    public int ProfileId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ProfileType { get; set; }
    public byte[] ProfilePicture { get; set; }

    // Navigation property for Credentials
    public Credentials Credentials { get; set; }

    // Navigation properties for Report and Investigation
    // Assuming a reporter can have multiple reports and an investigator can have multiple investigations
    public virtual ICollection<Report> Reports { get; set; }
    public virtual ICollection<Investigation> Investigations { get; set; }

    public Profile()
    {
        // Initialize the collections to prevent null reference exceptions
        Reports = new HashSet<Report>();
        Investigations = new HashSet<Investigation>();
    }
}
