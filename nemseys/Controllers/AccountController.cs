using Microsoft.AspNetCore.Mvc;
using Nemesis.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly DataContext _context;

    public AccountController(DataContext context)
    {
        _context = context;
    }

    private bool IsUserAuthenticated(string username, string password)
    {
        // Check if the email exists in the database
        var profile = _context.Profiles.FirstOrDefault(p => p.Email == username);

        if (profile == null)
        {
            // If the profile doesn't exist, authentication fails
            return false;
        }

        // Retrieve the stored hashed password and salt from the database
        var credentials = _context.Credentials.FirstOrDefault(c => c.ProfileId == profile.ProfileId);

        if (credentials == null)
        {
            // If the credentials don't exist, authentication fails
            return false;
        }

        // Hash the provided password using the stored salt
        string hashedPassword = HashPassword(password, credentials.Salt);

        // Check if the hashed password matches the stored hashed password
        bool passwordMatches = hashedPassword.Equals(credentials.PasswordHash);

        return passwordMatches;
    }

    private string HashPassword(string password, byte[] salt)
    {
        // Hash the password using PBKDF2 with 10000 iterations
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashed;
    }

    private bool ComparePasswords(byte[] hashedPassword, byte[] storedHashedPassword)
    {
        // Compare two hashed passwords
        return hashedPassword.SequenceEqual(storedHashedPassword);
    }


    // Login action
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(string username, string password, string returnUrl)
    {
        if (IsUserAuthenticated(username, password))
        {
            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                // Add more claims as needed
            };

            // Create identity from claims
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Create authentication properties
            var authProperties = new AuthenticationProperties
            {
                // Configure properties like IsPersistent, ExpiresUtc, etc.
                IsPersistent = true,
                RedirectUri = returnUrl
            };

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

            // Redirect the user to the returnUrl or default to "/home/myreports"
            return RedirectToAction("MyReports", "Home");
        }
        else
        {
            TempData["ErrorMessage"] = "Invalid email or password. Please try again.";
            return RedirectToAction("SignIn", "Home");
        }
    }

    public async Task<ActionResult> Logout()
    {
        // Sign out the user and remove the authentication cookie
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Redirect the user to the home page or login page
        return RedirectToAction("Index", "Home");
    }


    // Signup action
    [HttpPost]
    public ActionResult Signup(Profile profile, string password, string role, IFormFile profilePicture)
    {
        // Hash the password
        string hashedPassword = HashPassword(password);

        // Check if the email address already exists in the database
        var emailExists = _context.Profiles.Any(p => p.Email == profile.Email);

        if (emailExists)
        {
            // If email already exists, return a JSON response indicating the error
            return Json(new { success = false, message = "Email address already exists" });
        }

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

        // Handle profile picture upload
        if (profilePicture != null && profilePicture.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                profilePicture.CopyTo(memoryStream);
                profile.ProfilePicture = memoryStream.ToArray();
            }
        }

        // Save the profile and credentials to the database
        SaveProfileAndCredentials(profile, profile.Email, hashedPassword);

        // Return a JSON response indicating success
        return Json(new { success = true, redirectTo = Url.Action("SignIn", "Home") });
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
            throw new InvalidOperationException("Email address already exists");
        }

        // Save profile to the database
        _context.Profiles.Add(profile);
        _context.SaveChanges();

        // Split the stored string back into its salt and hashed password components
        var parts = hashedPassword.Split(':');
        var salt = Convert.FromBase64String(parts[0]);
        var passwordHash = parts[1]; // Convert hashed password to byte array

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
