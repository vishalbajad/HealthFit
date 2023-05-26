using Microsoft.AspNetCore.Mvc;
using Data_Layer.Services;
using Data_Layer.Repositories;
using Data_Layer.DBContext;
using HealthFit_APIs.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthFit.JwtAuthentication.Model;
namespace HealthFit_APIs.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly HealthFitDbContext _dbContext;
        private readonly UserRepository userRepository;
        private readonly UserService userService;
        private readonly JournalRepository journalRepository;
        private readonly JournalService journalService;
        private readonly AppSettingsConfigurations appSettingsConfigurations;

        public AuthenticationController(IOptions<AppSettingsConfigurations> options, ILogger<AuthenticationController> logger, IWebHostEnvironment environment)
        {
            appSettingsConfigurations = options.Value;
            _logger = logger;
            _dbContext = new HealthFitDbContext(appSettingsConfigurations.HealthFitDBConnectionString);
            userRepository = new UserRepository(_dbContext);
            userService = new UserService(userRepository);
        }

        /// <summary>
        /// JWT Credentails authentication
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!(string.IsNullOrWhiteSpace(loginModel?.Username)) && !(string.IsNullOrWhiteSpace(loginModel?.Password)))
            {
                // In real time , this creadentials should be valid from database
                if (loginModel.Username.Equals(appSettingsConfigurations.JwtAuthenticationUsername) && loginModel.Password.Equals(appSettingsConfigurations.JwtAuthenticationPassword))
                {
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginModel.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                    authClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    var token = GetToken(authClaims);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            return Unauthorized();
        }

        /// <summary>
        /// Generate Jwt Token
        /// </summary>
        /// <param name="authClaims"></param>
        /// <returns></returns>
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsConfigurations.JwtSecret));

            var token = new JwtSecurityToken(
                issuer: appSettingsConfigurations.JwtValidIssuer,
                audience: appSettingsConfigurations.JwtValidAudienceUrl,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
