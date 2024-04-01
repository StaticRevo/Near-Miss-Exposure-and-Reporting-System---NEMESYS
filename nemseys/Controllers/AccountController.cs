using Microsoft.AspNetCore.Mvc;
using Nemesis.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly DataContext _context;

    public AccountController(DataContext context)
    {
        _context = context;
    }

    private bool IsUserAuthenticated(string username, string password)
    {
        // Placeholder method for user authentication
        // Implement your actual authentication logic here
        // For demonstration purposes, always return true
        return true;
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
            // If authentication succeeds, redirect the user to the returnUrl or default to "/home/myreports"
            return Redirect(string.IsNullOrEmpty(returnUrl) ? "/home/myreports" : returnUrl);
        }
        else
        {
            // If authentication fails, return the login view with an error message
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }
    }

    // Signup action
    [HttpPost]
    public ActionResult Signup(Profile profile, string password)
    {
        // Hash the password
        string hashedPassword = HashPassword(password);

        // Save the profile and credentials to the database
        SaveProfileAndCredentials(profile, profile.Email, hashedPassword);

        // Redirect to the login page after successful signup
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
