using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.API.Services {
    public class AuthService: IAuthService {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser>userManager, IConfiguration configuration) {
            _userManager = userManager;
            _configuration = configuration;            
        }

        public async Task<string> RegisterAsync(RegisterRequest request) {
            // check if exist
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null) {
                throw new ArgumentException("User with this email alreaddy exists.");
            }

            // create one
            var user = new IdentityUser {
                Email = request.Email,
                UserName = request.Email,
            };

            var res = await _userManager.CreateAsync(user);
            if (!res.Succeeded) { 
                var err = string.Join(", ", res.Errors.Select(e=> e.Description));
                throw new ArgumentException($"User creation failed!: {err}");
            }
            return "User registered successfully!";
        }

        public async Task<string> LoginAsync(LoginRequest request) {
            // find the man
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null) throw new ArgumentException("Invalid email or password.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (isPasswordValid) throw new ArgumentException("invalid email or passwor.");

            // Generate JWT token
            var token = GenerateJwtToken(user);
            return token;

        }

        private string GenerateJwtToken(IdentityUser user) {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Name, user.UserName!)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
