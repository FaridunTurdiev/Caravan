using CaravanApi.Context;
using CaravanApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CaravanApi.Services;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Azure.Core;
using CaravanApi.DTO;
using CaravanApi.Temlates;

namespace CaravanApi.Controllers
{
    //Manages users.
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        //Creates a controller to manage user.
        public UserController(AppDbContext db, IConfiguration configuration, IEmailService emailService)
        {
            Console.WriteLine($"{nameof(UserController)}.ctor()");
            _db = db;
            _configuration = configuration;
            _emailService = emailService;
        }

        //Gets all users.
        [HttpGet("getAllUsers")]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _db.Users.ToListAsync());
        }

        //Checks if the User was registered or not.
        [HttpPost("authenticate")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Authenticate(User userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == userObj.Username);

            if (user == null)
                return NotFound(new { Message = "User not Found." });

            if(user.UserRole == UserRole.Seller)
                return BadRequest();//here will be logic to check SellerId

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest(new { Message = "Wrong password." });

            user.Token = CreatJwt(user);
            var newAccessToken = user.Token;
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _db.SaveChangesAsync();

            return Ok(new TokenDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


        // Register user information to the DB.
        [HttpPost("register")]
        public async Task<ActionResult> Register(User userObj)
        {
            if (userObj == null)
                return BadRequest();

            if (await CheckUsernameExist(userObj.Username))
                return BadRequest(new { Message = "Username is already in use! Choos another username." });

            if (await CheckEmailExist(userObj.Email))
                return BadRequest(new { Message = "This email is already in use! Choos another email" });

            if (await CheckSellerExist(userObj.SellerId))
                return BadRequest(new { Message = "The Current SellerId is already in use" });

            //Checking if the Password is enough strong
            var password = CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(password))
                return BadRequest(new { Message = $"Password: {password}" });
            PasswordHasher.HashPassword(userObj.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userObj.Password = Convert.ToBase64String(passwordHash);
            userObj.PasswordHash = passwordHash;
            userObj.PasswordSalt = passwordSalt;
            userObj.UserRole = UserRole.Buyer;
            userObj.Token = "";

            await _db.Users.AddAsync(userObj);
            await _db.SaveChangesAsync();
            return Ok(new
            {
                Status = 200,
                Message = "User Registeration succeeded :)"
            });
        }

        //Checking if Username is already in use or not
        private Task<bool> CheckUsernameExist(string username)
            => _db.Users.AnyAsync(u => u.Username == username);

        //Checking if the Email is already in use or not
        private Task<bool> CheckEmailExist(string email)
            => _db.Users.AnyAsync(u => u.Email == email);

        //Checking if the Email is already in use or not
        private Task<bool> CheckSellerExist(string seller)
            => _db.Users.AnyAsync(u => u.SellerId == seller);

        //Checking if the Password is enough strong
        private static string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();

            if (password.Length < 12)
                sb.Append("Minimum lenght of your Password has to be 12 Characters" + Environment.NewLine);
            if (!(Regex.IsMatch(password, "[a-z]")) && (Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
                sb.Append("Your Password has to be containing [a-z], [A-Z] and [0-9]" + Environment.NewLine);
            if (!Regex.IsMatch(password, "[!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~]"))
                sb.Append("Your Password has to contain special charachters" + Environment.NewLine);

            return sb.ToString();

        }

        //Creats JWT when user Authentication was successful
        private string CreatJwt(User user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("RealyRealySecretKeyWhichUWillNotGet2024");
            var identity = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            new Claim(ClaimTypes.Name, $"{user.Username}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };
            var token = jwtHandler.CreateToken(tokenDescriptor);
            return jwtHandler.WriteToken(token);
        }

        // Creats Refresh token for Authenticated user
        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInDb = _db.Users
                .Any(u => u.RefreshToken == refreshToken);

            if (tokenInDb)
                return CreateRefreshToken();

            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("RealyRealySecretKeyWhichUWillNotGet2024");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Your Token is Invalid!!!");

            return principal;
        }

        // Endapoint to create Refresh and Access Token
        [HttpPost("refreshToken")]
        public async Task<ActionResult> RefreshToken(TokenDto tokenDto)
        {
            if (tokenDto is null)
                return BadRequest("Invalid Client Request");

            string accessToken = tokenDto.AccessToken;
            string refreshToken = tokenDto.RefreshToken;

            var principal = GetPrincipleFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid Request");

            var newAccessToken = CreatJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;

            await _db.SaveChangesAsync();
            return Ok(new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }

        //Endpoint to send email for reseting password
        [HttpPost("sendResetPasswordEmail")]
        public async Task<ActionResult> SendResetPasswordEmail(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Email == email);

            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email doesn't exist!"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);

            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);

            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password!", ResetPasswordEmail.ResetPassowrsEmailBody(email, emailToken));

            _emailService.SendEmail(emailModel);
            _db.Entry(user).State = EntityState.Modified;

            await _db.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Email was sent!"
            });
        }

        //Reset Password Endpoint
        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User doesn't exist!"
                });
            }
            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiry;
            if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid Reset Link!"
                });
            }
            PasswordHasher.HashPassword(resetPasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.Password = Convert.ToBase64String(passwordHash);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Your password was successfully reseted :)"
            });
        }
    }
}
