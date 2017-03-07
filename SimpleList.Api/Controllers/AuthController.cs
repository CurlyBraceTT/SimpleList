using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using SimpleList.Api.Models;
using SimpleList.Api.Utils;

namespace SimpleList.Api.Controllers
{
    public class AuthController : Controller
    {
        private ILogger<AuthController> _logger;
        private IConfigurationRoot _config;
        private SignInManager<IdentityUser> _signInManager;

        public AuthController(SignInManager<IdentityUser> signInManager, ILogger<AuthController> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
            _signInManager = signInManager;
        }

        [HttpPost("api/auth/standartlogin")]
        [ValidateModel]
        public async Task<ActionResult> StandartLogin([FromBody] CredentialModel model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.User, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while logging in: {ex}");
            }

            return BadRequest("Failed to login");
        }

        [HttpPost("api/auth/login")]
        public async Task<ActionResult> Login([FromBody]string user)
        {
            try
            {
                var externalToken = Request.Headers[HeaderNames.Authorization].ToString();
                var logged = await ValidateGoogleToken(externalToken);

                if (logged != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, logged.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _config["Tokens:Issuer"],
                        audience: _config["Tokens:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(2),
                        signingCredentials: creds
                    );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Exception thrown while logging in: {ex}");
            }

            return BadRequest("Failed to login");
        }

        private async Task<IdentityUser> ValidateGoogleToken(string token)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            JObject parsed = JObject.Parse(content);

            if (string.Equals(parsed["aud"].ToString(), _config["Google:ClientId"], StringComparison.OrdinalIgnoreCase))
            {
                var user = new IdentityUser();
                user.UserName = parsed["name"].ToString();
                return user;
            }

            return null;
        }
    }
}
