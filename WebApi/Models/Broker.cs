using Microsoft.VisualBasic;

namespace WebApi.Models
{
    public class Broker
    {
        public String Username { get; set; }
        public String Password { get; set; }

        public Broker()
        {

        }

        public Broker(String username, String password)
        {
            this.Username = username;
            this.Password = password;
        }

    }
}
