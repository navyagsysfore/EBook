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

namespace EBook.Service
{
    public class Login : ILoginService
    {
        private readonly JwtContext _context;
        private readonly JWTClaimsDetails _jwtDetails;

        public Login(JwtContext context, IOptions<JWTClaimsDetails> jwtDetails)
        {
            _context = context;
            _jwtDetails = jwtDetails.Value;
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
                Role = Roles.User
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
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
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginService.Username);
                if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(loginService.Password, user.Password))
                {
                    Console.WriteLine("Invalid username or password");
                    return string.Empty;
                }

                return user.Role.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public string GenerateToken(UserDTO login, string role)
        {
            try
            {
                if (string.IsNullOrEmpty(role))
                    throw new ArgumentException("Not a valid username or password");

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
                    expires: DateTime.Now.AddMinutes(8),
                    signingCredentials: signinCredentials
                );

                return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
    }
}
