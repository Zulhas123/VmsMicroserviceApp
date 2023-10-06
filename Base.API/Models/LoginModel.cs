using System.ComponentModel.DataAnnotations;

namespace Base.API.Models
{
    public class LoginModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int  RoleId { get; set; }
    }
}
