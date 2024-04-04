using Microsoft.AspNetCore.Mvc;
using Nemesis.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

public class AccountController : Controller
{
    private readonly DataContext _context;

    public AccountController(DataContext context)
    {
        _context = context;
    }

    private bool IsUserAuthenticated(string username, string password)
    {
        Debug.WriteLine($"Authenticating user: {username}, {password}");

        // Find the credentials associated with the provided username
        var credentials = _context.Credentials.FirstOrDefault(c => c.Username == username);

        if (credentials == null)
        {
            Debug.WriteLine($"No credentials found for user: {username}");
            // No credentials found for the provided username
            return false;
        }

        // Hash the provided password with the stored salt
        byte[] hashedPasswordBytes = HashPassword(password, credentials.Salt);

        // Convert the byte array to a base64 string for comparison
        string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);

        // Compare the hashed password with the stored password hash
        if (hashedPassword == Convert.ToBase64String(credentials.PasswordHash))
        {
            // Authentication successful
            return true;
        }

        // Authentication failed
        return false;
    }



    private byte[] HashPassword(string password, byte[] salt)
    {
        // Hash the password using PBKDF2 with the stored salt and 10000 iterations
        byte[] hashedBytes = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);

        return hashedBytes;
    }


    // Login action
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Login(string username, string password, string returnUrl)
    {

        if (IsUserAuthenticated(username, password))
        {
            // If authentication succeeds, retrieve the associated Profile
            var profile = _context.Profiles.FirstOrDefault(p => p.Credentials.Username == username);

            // Redirect the user to the returnUrl or default to "/home/myreports"
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("MyReports", "Home");
            }
        }
        else
        {
            // If authentication fails, return the login view with an error message
            ModelState.AddModelError("", "Invalid username or password");
            return RedirectToAction("SignIn", "Home");
        }
    }


    // Signup action
    [HttpPost]
    public ActionResult Signup(Profile profile, string password, string role)
    {
        // Hash the password
        string hashedPassword = HashPassword(password);

        // Set ProfileType based on the selected radio button value
        if (role == "investigator")
        {
            profile.ProfileType = "Investigator";
        }
        else if (role == "reporter")
        {
            profile.ProfileType = "Reporter";
        }
        else
        {
            // Handle other cases if necessary
        }

        // Save the profile and credentials to the database
        SaveProfileAndCredentials(profile, profile.Email, hashedPassword);

        // Redirect to the sign-in page after successful signup
        return RedirectToAction("SignIn", "Home");
    }


    private string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Hash the password using PBKDF2 with 10000 iterations
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // Combine the salt and hashed password
        return $"{Convert.ToBase64String(salt)}:{hashed}";
    }

    private void SaveProfileAndCredentials(Profile profile, string email, string hashedPassword)
    {
        // Check if the email address already exists in the database
        var existingProfile = _context.Profiles.FirstOrDefault(p => p.Email == email);

        if (existingProfile != null)
        {
            // Handle the case where the email address already exists
            // For example, you could return an error message or redirect the user
            // In this example, I'm throwing an exception for demonstration purposes
            throw new InvalidOperationException("Email address already exists");
        }

        // Save profile to the database
        _context.Profiles.Add(profile);
        _context.SaveChanges();

        // Split the stored string back into its salt and hashed password components
        var parts = hashedPassword.Split(':');
        var salt = Convert.FromBase64String(parts[0]);
        var passwordHash = Convert.FromBase64String(parts[1]); // Convert hashed password to byte array

        // Save credentials to the database
        Credentials credentials = new Credentials
        {
            Username = email,
            PasswordHash = passwordHash,
            Salt = salt,
            ProfileId = profile.ProfileId
        };
        _context.Credentials.Add(credentials);
        _context.SaveChanges();
    }


}
