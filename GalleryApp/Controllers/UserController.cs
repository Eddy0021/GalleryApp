using GalleryApp.DTO;
using GalleryApp.IRepository;
using GalleryApp.Models;
using GalleryApp.Repository;
using GalleryApp.ResponeData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GalleryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        
        IUserRepository _userRepository;

        public UserController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
            
        }

        [HttpPost]
        [Route("Signin")]
        public async Task<IActionResult> Singin(LoginDTO request)
        {
            try
            {
                var result  = await _userRepository.GetByUsernamePassword(request);
                string token = GenerateToken(result);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Add(UploadUserResponse user)
        {
            try
            {
                var response = await _userRepository.Create(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CheckEmail")]
        public async Task<IActionResult> ChangePasswordRequest(CheckEmailResponse user)
        {
            try
            {
                var userReturn = await _userRepository.ChangePasswordRequest(user);
                return Ok(userReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdatePasswordResponse user)
        {
            try
            {
                var response = await _userRepository.Update(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim("username", user.Username),
                new Claim("userID", user.UserID.ToString()),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials); ;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
