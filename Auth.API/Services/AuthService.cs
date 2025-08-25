using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.API.Services {
    public class AuthService: IAuthService {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, IConfiguration configuration) {
            _userManager = userManager;
            _configuration = configuration;            
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request) {
            // check if exist
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null) {
                throw new ArgumentException("User with this email alreaddy exists.");
            }

            // create one
            var user = new User {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var res = await _userManager.CreateAsync(user, request.Password);
            if (!res.Succeeded) { 
                var err = string.Join(", ", res.Errors.Select(e=> e.Description));
                throw new ArgumentException($"User creation failed!: {err}");
            }

            var token = GenerateJwtToken(user);
            return new AuthResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(1) };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request) {
            // find the man
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) throw new ArgumentException("Invalid email.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid) throw new ArgumentException("invalid password.");

            // Generate JWT token
            var token = GenerateJwtToken(user);
            return new AuthResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(1) };

        }

        private string GenerateJwtToken(User user) {
            const string JWT = "Jwt";
            const string SECRET = "Secret";
            const string EXPIRY_IN_MINUTES = "ExpiryInMinutes";
            const string ISSUER = "Issuer";
            const string AUDIENCE = "Audience";

            var jwtSettings = _configuration.GetSection(JWT);
            var key = Encoding.UTF8.GetBytes(jwtSettings[SECRET]);
            var claims = new[]{ // use anon type
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };


            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings[EXPIRY_IN_MINUTES])),
                Issuer = jwtSettings[ISSUER],
                Audience = jwtSettings[AUDIENCE],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
