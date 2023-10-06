using Base.API.Controllers;
using Base.API.Models;
using Base.API.SecurityExtension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Data;
using UserManagement.Interface;
using UserManagement.Manager;
using UserManagement.Models;
using UserManagement.ViewModels;
using static System.Net.WebRequestMethods;
using Base.API.Manager;

namespace UserManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    public class UserController : BaseController
    {
        private readonly IUserManager userManager;
        private readonly IForgetPasswordManager forgetPasswordManager;
        IConfiguration _configuration;
        PermissionController _permissionController;

        public UserController(AppDbContext db, IConfiguration configuration)
        {
            userManager = new UserManager(db);
            _configuration = configuration;
            _permissionController = new PermissionController(db);
            forgetPasswordManager = new ForgetPasswordManager(db);
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginReq login)
        {
            var getUser = userManager.GetUserInfoByUserName(login.UserName);
            if (getUser != null && getUser.IsActive)
            {
                var password = Decrypt(getUser.Password);
                if (login.Password == password)
                {
                    var auth = new AuthenticateController(_configuration);
                    LoginModel loginModel = new LoginModel()
                    {
                        UserId = getUser.Id,
                        Password = password,
                        Username = login.UserName,
                        RoleId = getUser.RoleId
                    };
                    var token = auth.CreateToken(loginModel);
                    var menus = _permissionController.GetPermissionVmList(getUser.RoleId);

                    LoginRes res = new LoginRes()
                    {
                        Token = token,
                        menus = menus
                    };
                    return Ok(res);
                }
            }
            return Unauthorized();
        }
       // [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        [HttpPost]
        public IActionResult Register([FromBody] UserInfo userInfo)
        {
            try
            {
                if (userInfo != null && !(string.IsNullOrEmpty(userInfo.Email) && string.IsNullOrEmpty(userInfo.UserName) && string.IsNullOrEmpty(userInfo.PhoneNumber)) && !string.IsNullOrEmpty(userInfo.Password))
                {
                    var pass = Encrypt(userInfo.Password);
                    userInfo.Password = pass;
                    userInfo.IsActive = true;
                    AuditInsert(userInfo);
                    if (userManager.Add(userInfo))
                    {
                        return Ok();
                    }

                }
                return BadRequest();
            }
            catch (Exception)
            {

                return BadRequest();
            }


        }

        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult GetAll()
        {
            string sql = "Select u.Id, u.UserName, u.Email,u.PhoneNumber,u.EmployeeCode, u.RoleId, r.Name as RoleName from UserInfos as u join Roles as r on u.RoleId=r.Id Where u.IsActive=1";
            var users = userManager.ExecuteRawSql(sql);
            var list = (from DataRow dr in users.Rows
                        select new UserInfoVm()
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            UserName = dr["UserName"].ToString(),
                            Email = dr["Email"].ToString(),
                            PhoneNumber = dr["PhoneNumber"].ToString(),
                            EmployeeCode = dr["EmployeeCode"].ToString(),
                            RoleName = dr["RoleName"].ToString(),
                            RoleId = Convert.ToInt32(dr["RoleId"])
                        }).ToList();
            return Ok(list);
        }



        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult GetById(int id)
        {
            var user = userManager.GetById(id);
            if (user != null)
            {
                user.Password = "";
            }

            return Ok(user);

        }
        [HttpGet]
        //Basic Authentication will go here
        public IActionResult GetByEmployeeId(int employeeId)
        {
            try
            {
                var user = userManager.GetByEmployeeId(employeeId);
                if (user != null)
                {
                    return Ok("User found");
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception)
            {

                return BadRequest();
            }


        }

        [HttpPut]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Update([FromBody] UserInfoVm user)
        {

            var getUser = userManager.GetById(user.Id);
            if (getUser != null)
            {
                getUser.UserName = user.UserName;
                getUser.Email = user.Email;
                getUser.PhoneNumber = user.PhoneNumber;
                getUser.RoleId = user.RoleId;
                AuditUpdate(getUser);
                if (userManager.Update(getUser))
                {
                    getUser.Password = "";
                    return Ok(getUser);
                }
            }

            return BadRequest("User update fail");
        }

        [HttpDelete]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Delete([FromBody] int id)
        {
            var user = userManager.GetById(id);
            if (user != null)
            {
                AuditDelete(user);
                if (userManager.Update(user))
                {
                    return Ok("Successfully deleted");
                }
            }

            return BadRequest("User deletation failed");
        }
        private string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }

        [HttpGet]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                var senderMail = _configuration["Email"];
                var senderPassword = _configuration["Password"];
                var user = userManager.GetByEmailNo(email);
                if (user == null)
                {
                    return BadRequest("No user found");
                }
                var otp = RandomNumber();
                var expire = DateTime.Now.AddMinutes(5);
                ForgetPassword forgetPassword = new ForgetPassword()
                {
                    Email = email,
                    Otp = otp,
                    ValidTill = expire,

                };
                if (forgetPasswordManager.Add(forgetPassword))
                {
                    var body = "Dear User, <br /> Currently someone is trying to reset your passsword of visitor management system. If it was you then use your one time password is <strong>" + otp + "</strong> within " + expire+ ". If it wasn't you ignore this email. Your account is safe. <br /> Thanks from <br /> Visitor Management System.";
                    Utility.SendMail(email, "Reset password", body, senderMail, senderPassword);
                }
                return Ok();


            }
            catch (Exception)
            {

                return BadRequest("Something went wrong");
            }


        }
        private string RandomNumber()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }

        [HttpPost]
        public IActionResult VerifyOtp(OtpVerifyVm obj)
        {
            try
            {
                var data = forgetPasswordManager.GetByEmail(obj.Email);
                if (data == null)
                {
                    return BadRequest("Invalid email");
                }
                var currentTime = DateTime.Now;

                if (data.Otp == obj.Otp)
                {
                    if (data.ValidTill < currentTime)
                    {
                        return BadRequest("Otp has been expired");
                    }
                    return Ok(data);

                }
                else
                {
                    return BadRequest("Invalid otp");
                }
            }
            catch (Exception)
            {

                return BadRequest("Something went wrong");
            }


        }

        [HttpPost]
        public IActionResult ResetPassword(ChangePasswordVm obj)
        {
            try
            {
                if (obj.NewPassword == obj.ConfirmPassword)
                {
                    var getUser = userManager.GetByEmailNo(obj.Email);
                    if (getUser == null)
                    {
                        return BadRequest("User not found");
                    }
                    var password = Encrypt(obj.NewPassword);
                    getUser.Password = password;
                    if (userManager.Update(getUser))
                    {
                        return Ok("Password updated");
                    }
                    return BadRequest("Password update failed");
                }
                else
                {
                    return BadRequest("Password not match");
                }

            }
            catch (Exception)
            {

                return BadRequest("Something went wrong");
            }

        }


        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult ChangePassword(ChangePasswordVm obj)
        {
            try
            {
                var getUser = userManager.GetById(UserData.UserId);
                if (getUser == null)
                {
                    return BadRequest("User not found");
                }
                if (obj.NewPassword == obj.ConfirmPassword)
                {


                    var password = Encrypt(obj.NewPassword);
                    getUser.Password = password;
                    if (userManager.Update(getUser))
                    {
                        return Ok("Password updated");
                    }
                    return BadRequest("Password update failed");
                }
                else
                {
                    return BadRequest("Password not match");
                }
            }
            catch (Exception)
            {

                return BadRequest("Something went wrong");
            }
        }
        [HttpPost]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult ResetPasswordByAdmin(ChangePasswordVm obj)
        {
            try
            {
                var getUser = userManager.GetById(obj.UserId);
                if (getUser == null)
                {
                    return BadRequest("User not found");
                }
                if (obj.NewPassword == obj.ConfirmPassword)
                {


                    var password = Encrypt(obj.NewPassword);
                    getUser.Password = password;
                    if (userManager.Update(getUser))
                    {
                        return Ok("Password updated");
                    }
                    return BadRequest("Password update failed");
                }
                else
                {
                    return BadRequest("Password not match");
                }
            }
            catch (Exception)
            {

                return BadRequest("Something went wrong");
            }
        }
    }
}
