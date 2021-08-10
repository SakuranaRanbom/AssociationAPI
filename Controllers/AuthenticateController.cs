using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SQLitePCL;
using TeamAPI.Models;

namespace TeamAPI.Controllers
{   
    /// <summary>
    /// 登录注册验证
    /// </summary>
    [Route("auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        public AuthenticateController(UserManager<IUser> userManager,
            RoleManager<IRole> roleManager,
            IConfiguration configuration)
        {
            
            UserManager = userManager;
            
            RoleManager = roleManager;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public RoleManager<IRole> RoleManager { get; }
        public  UserManager<IUser> UserManager { get; }

        
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="用户注册模型"></param>
        /// <returns></returns>
        [HttpPost("register", Name = nameof(AddUserAsync))]
        
        public async Task<IActionResult> AddUserAsync([FromBody]RegisterUser registerUser)
        {
            
            var user = new IUser
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email
            };
            IdentityResult result = await UserManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError("Error", result.Errors.FirstOrDefault()?.Description);
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        [HttpPost("token2", Name = nameof(GenerateTokenAsync))]
        public async Task<IActionResult> GenerateTokenAsync([FromBody]LoginUser loginUser)
        {
            var user = await UserManager.FindByNameAsync(loginUser.UserName);
            
            if (user == null)
            {
                return Unauthorized();
            }

            var result = UserManager.PasswordHasher.VerifyHashedPassword(user,
                user.PasswordHash, loginUser.Password);
            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized();
            }

            var userClaims = await UserManager.GetClaimsAsync(user);
            var userRoles = await UserManager.GetRolesAsync(user);
            foreach (var roleItem in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
            if (user.UserName == "admin1")
            {
                claims.Add(new Claim("Admin", "true"));
            }
            claims.AddRange(userClaims);

            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: tokenConfigSection["Issuer"],
                audience: tokenConfigSection["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(9999),
                signingCredentials: signCredential
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                
                expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
            });
            
            
            
        }
        
        
        public class PersonInfo
        {
            public string _userName { get; }
            //不跟get，接口返回空
            [Phone]
            public string _phonenum { get; set; }
            
            
            
            

            public PersonInfo(string userName, string phonenum)
            {
                _userName = userName;
               
                _phonenum = phonenum;
                
                
            }
            
        }
        
        [Authorize]
        [HttpGet("info")]
        public ActionResult<PersonInfo>  GetInfo() //必须ActionResult才能成功返回对象
        {
            var name =  HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = UserManager.FindByNameAsync(name);
            
            //Console.WriteLine(user.Result.UserName);
            var info = new PersonInfo(user.Result.UserName, user.Result.PhoneNumber);
            //Console.WriteLine(info._userName);
            return info;
        }
        
        public class updateInfo
        {
            public string gender { get; set; }
            public string phonenum { get; set; }

            

        }
        [Authorize]
        [HttpPost("updateInfo")]
        public async Task<IdentityResult> EditInfo([FromBody] updateInfo info)
        {
            //Console.WriteLine("123");
            var phonenum = info.phonenum;
            var gender = info.gender;
            var name =  HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = UserManager.FindByNameAsync(name);
            /*Console.WriteLine(phonenum);
            Console.WriteLine(gender);
            if(phonenum == "") Console.WriteLine("1");
            if(!string.IsNullOrEmpty(phonenum)) Console.WriteLine("2");*/
            if(!string.IsNullOrEmpty(phonenum)) user.Result.PhoneNumber = phonenum;
            var result = await UserManager.UpdateAsync(user.Result);
            return result;

        }

        [Authorize]
        [HttpGet("admin")]
        public ActionResult<bool> isSuperAdmin()
        {
            var _isAdmin = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Admin").Value;
            if (_isAdmin.Equals("true"))
            {
                return true;
            }

            return false;
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<bool> ChangePassword([FromForm] string CurrentPassword,[FromForm]string NewPassword)
        {
            if (CurrentPassword == null || NewPassword == null || CurrentPassword == "" || NewPassword == "")
            {
                return false;
            }
            else
            {
                var id =  HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var user = UserManager.FindByNameAsync(id);
                var res  =await UserManager.ChangePasswordAsync(user.Result, CurrentPassword, NewPassword);
                if (res.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

    }
}