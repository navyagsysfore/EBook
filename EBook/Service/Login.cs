using EBook.Models;
using EBook.Services.Interface;
using EBook.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EBook.Services.Iface;
using EBook.Services.JWTDetails;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;

namespace EBook.Service
{
    public class Login : ILoginService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly JWTClaimsDetails _jwtDetails;
        private readonly ILogger<Login> _logger;

        public Login(IServiceProvider serviceProvider, IOptions<JWTClaimsDetails> jwtDetails, ILogger<Login> logger)
        {
            _serviceProvider = serviceProvider;
            _jwtDetails = jwtDetails.Value;
            _logger = logger;
        }


        public async Task<string> SignupAsync(UserDTO loginService)
        {
            if (loginService == null)
                throw new ArgumentNullException(nameof(loginService));

            if (string.IsNullOrWhiteSpace(loginService.Username) || string.IsNullOrWhiteSpace(loginService.Password))
                throw new ArgumentException("Username or password cannot be empty or whitespace");

            if (string.Equals(loginService.Username, loginService.Password, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Username & password cannot be equal");

            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(loginService.Password, 12);

            var user = new Users
            {
                Username = loginService.Username,
                Password = passwordHash,
                Role = Roles.User.ToString() 
            };

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<JwtContext>();
                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return ex.Message;
            }
        }

        public async Task<string> RoleAsync(UserDTO loginService)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<JwtContext>();
                    var user = await context.Users.SingleOrDefaultAsync(u => u.Username == loginService.Username);
                    if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(loginService.Password, user.Password))
                    {
                        Console.WriteLine("Invalid username or password");
                        return string.Empty;
                    }

                    string roleString = user.Role;

                    return roleString;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }


        public async Task<string> GenerateToken(UserDTO login, string role)
        {
            try
            {
                if (string.IsNullOrEmpty(role))
                {
                    throw new AuthenticationException("Invalid username or password");
                }

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtDetails.Key));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, role)
                };

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtDetails.Issuer,
                    audience: _jwtDetails.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(13),
                    signingCredentials: signinCredentials
                );

                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                _logger.LogInformation($"Generated Token for user '{login.Username}': {token}");

                return token;
            }
            catch (AuthenticationException ex)
            {
                _logger.LogWarning($"Authentication error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during token generation: {ex.Message}");
                throw new Exception("An error occurred during token generation");
            }
        }

    }
}