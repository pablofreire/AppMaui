using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SportSphere.API.Data;
using SportSphere.Shared.DTOs;
using SportSphere.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SportSphere.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Token, string Message)> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return (false, string.Empty, "Usuário não encontrado.");
            }

            //if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            //{
            //    return (false, string.Empty, "Senha incorreta.");
            //}

            var token = GenerateJwtToken(user);
            return (true, token, "Login realizado com sucesso.");
        }

        public async Task<(bool Success, string Message)> RegisterAsync(UserRegistrationDto registrationDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registrationDto.Email))
            {
                return (false, "Email já está em uso.");
            }

            if (await _context.Users.AnyAsync(u => u.Username == registrationDto.Username))
            {
                return (false, "Nome de usuário já está em uso.");
            }

            //var passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

            var user = new UserModel
            {
                Username = registrationDto.Username,
                Email = registrationDto.Email,
                PasswordHash = registrationDto.Password,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                ProfilePictureUrl = "https://via.placeholder.com/150",
                Bio = "Novo usuário do SportSphere",
                RegistrationDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow,
                IsActive = true,
                Role = "User",
                DefaultLocation = new LocationModel
                {
                    Latitude = 0,
                    Longitude = 0,
                    Address = "Endereço não definido",
                    City = "Cidade não definida",
                    State = "Estado não definido",
                    Country = "País não definido",
                    PostalCode = "00000-000"
                }
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, "Registro realizado com sucesso.");
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return true;
        }

        private string GenerateJwtToken(UserModel user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "SportSphereSecretKey12345678901234567890");
            var issuer = jwtSettings["Issuer"] ?? "SportSphere";
            var audience = jwtSettings["Audience"] ?? "SportSphereUsers";
            var durationInMinutes = int.Parse(jwtSettings["DurationInMinutes"] ?? "60");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(durationInMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 