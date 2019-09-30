using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyNewWebApplication.Models
{
    public class UserModel
    {
        public string name { get; set; }
        [Key]
        [Required]
        public string email { get; set; }
        public string password { get; set; }
    }
    public class LoginModel
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string token { get; set; }
    }
    public class StringToJsonConverter
    {
        public string Token { get; set; }
        public string Expiry { get; set; }


        public StringToJsonConverter(string token)
        {
            Token = token;
        }
    }
    public class Credentials
    {
        private string type { get; set; }
        private string value { get; set; }
        public Credentials(string type, string value)
        {
            this.type = type;
            this.value = value;
        }
        public string[] getCredentials()
        {
            string[] x = new string[2];
            x[0] = this.type;
            x[1] = this.value;
            return x;
        }

    }
}
