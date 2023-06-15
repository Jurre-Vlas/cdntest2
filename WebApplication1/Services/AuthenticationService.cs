using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Data;
using Eindopdrachtcnd2.Data.Entities;
using Eindopdrachtcnd2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eindopdrachtcnd2.Services
{
    public interface IAuthenticationService
    {
        Task<ServiceResult> RegisterAsync(RegisterUserDTO registerUserDTO);
        Task<ServiceResult<Dictionary<string, object>>> LoginAsync(LoginDTO loginDTO);
        Task<ServiceResult> UpdateUserRole(string userId, string newRole);

        Task<ServiceResult<bool>> IsTokenValidAsync(string token);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(ApplicationDbContext db, ILogger<AuthenticationService> logger, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ServiceResult> RegisterAsync(RegisterUserDTO registerUserDTO)
        {
            var result = new ServiceResult();

            try
            {
                if (registerUserDTO == null)
                {
                    result.ErrorMessage = "User details are empty";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                //Check if user exists
                var userExists = await _userManager.FindByEmailAsync(registerUserDTO.Email);

                if (userExists != null)
                {
                    result.ErrorMessage = "User already exists";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                //Add user to databa
                User user = new User()
                {
                    Email = registerUserDTO.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerUserDTO.Username,
                    Name = registerUserDTO.Name,
                };

                var role = "User";

                var creatingUser = await _userManager.CreateAsync(user, registerUserDTO.Password);

                if (!creatingUser.Succeeded)
                {
                    result.ErrorMessage = "User creation failed! Please check user details and try again.";
                    result.ErrorCode = ErrorCodeEnum.InternalServerError;
                    _logger.LogError($"User creation failed! Please check user details and try again. {creatingUser}");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, role);
                    result.ErrorCode = ErrorCodeEnum.Success;
                    _logger.LogInformation($"User created successfully. {creatingUser}");
                }

            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }

            return result;

        }

        public async Task<ServiceResult<bool>> IsTokenValidAsync(string token)
        {

            var result = new ServiceResult<bool>();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))

                };


                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                result.ErrorCode = ErrorCodeEnum.Success;
                result.Result = true;
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                result.ErrorMessage = "token is invalid";
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }

            return result;

        }


        public async Task<ServiceResult<Dictionary<string, object>>> LoginAsync(LoginDTO loginDTO)
        {
            var result = new ServiceResult<Dictionary<string, object>>();

            try
            {
                if (loginDTO != null)
                {
                    if (string.IsNullOrEmpty(loginDTO.Username) || string.IsNullOrEmpty(loginDTO.Password))
                    {
                        result.ErrorMessage = "Username or password cannot be empty";
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        return result;
                    }
                }
                
                var user = await _userManager.FindByNameAsync(loginDTO.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var userRoles = await _userManager.GetRolesAsync(user);

                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var jwtToken = GetToken(authClaims);
                    var token = new { token = new JwtSecurityTokenHandler().WriteToken(jwtToken), expiration = jwtToken.ValidTo };
                    var resultData = new Dictionary<string, object>
                    {
                        { "token", token.token },
                        { "expiration", token.expiration },
                        { "name", user.Name }
                    };

                    result.Result = resultData;
                    result.ErrorCode = ErrorCodeEnum.Success;
                    _logger.LogInformation($"User logged in successfully. Token: {resultData["token"]}, Name: {resultData["name"]}");
                    return result;
                }

                result.ErrorMessage = "Invalid Authentication";
                result.ErrorCode = ErrorCodeEnum.Unauthorized;
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }

            return result;
        }

        public async Task<ServiceResult> UpdateUserRole(string userId, string newRole)
        {
            var result = new ServiceResult();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    // Haal de huidige rollen van de gebruiker op
                    var userRoles = await _userManager.GetRolesAsync(user);

                    // Verwijder de gebruiker uit alle huidige rollen
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);

                    if (!removeResult.Succeeded)
                    {
                        result.ErrorMessage = "Failed to remove user roles";
                        result.ErrorCode = ErrorCodeEnum.InternalServerError;
                        return result;
                    }
                    // Voeg de gebruiker toe aan de nieuwe rol
                    var addResult = await _userManager.AddToRoleAsync(user, newRole);
                    if (!addResult.Succeeded)
                    {
                        result.ErrorMessage = "Failed to add user to role";
                        result.ErrorCode = ErrorCodeEnum.InternalServerError;
                        return result;
                    }
                    
                    await _userManager.UpdateAsync(user);
                    result.ErrorMessage = "User role updated successfully";
                    result.ErrorCode = ErrorCodeEnum.Success;
                    return result;
                }
                result.ErrorMessage = "User not found";
                result.ErrorCode = ErrorCodeEnum.NotFound;
                return result;
            }
            catch (Exception ex)
            {
                //result.IsSuccess = false;
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }
            return result;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddHours(3),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
             );
            return token;
        }
    }
}
