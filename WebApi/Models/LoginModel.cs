using Microsoft.VisualBasic;

namespace WebApi.Models
{
    public class LoginModel
    {
        public String Username { get; set; }
        public String Password { get; set; }

        public LoginModel()
        {

        }

        public LoginModel(String username, String password)
        {
            this.Username = username;
            this.Password = password;
        }

    }
}
