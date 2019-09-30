using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MyNewWebApplication.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Security.Claims;
using MyNewWebApplication.Data;
using System.Collections.Generic;

namespace MyNewWebApplication.Controllers
{
    // [Route("api/[controller]/[ActionName]")]
    [Route("api/")]
    //[AllowAnonymous]
    [ApiController]
    [Produces("application/json")]
    public class ValuesController : ControllerBase
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private IConfiguration _config;

        public ValuesController(IConfiguration config)
        {
            _config = config;
        }

        [EnableCors("AllowAll")]
        [HttpPost]
        [HttpOptions]
        [Route("register")]
        public bool Register([FromBody] UserModel user)
        {
            bool result = false;
            var x = db.RegisteredUsers.Find(user.email);
            if (x == null)
            {
                PosgresUserRepository register = new PosgresUserRepository(db);
                var z = register.CreateUser(user);
                if (z)
                {
                    result = true;
                }
            }

            return result;
        }
        [EnableCors("AllowAll")]
        [HttpPost]
        [HttpOptions]
        [Route("authenticate")]

        public object Authorise(LoginModel login)
        {
            var jwt = login.token;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var claims = token.Claims;
            // bool[] check = { false, false, false, false, false };
            Credentials[] check = new Credentials[5];
            int j = 0;
            foreach (var i in claims)
            {
                if (j < 5)
                {
                    check[j] = new Credentials(i.Type, i.Value);
                }
                j++;
            }
            if (check[0].getCredentials()[1] == login.email && check[0].getCredentials()[1] == login.password)
            {
                return Ok("authorized");
            }
            else
            {
                return BadRequest();
            }
        }
        //aud,iss,exp,password,email


        [EnableCors("AllowAll")]

        [HttpOptions]
        [HttpPost]
        [Route("token")]
        public JObject Token([FromBody]LoginModel login)
        {
            string response;
            var user = Authenticate(login);
            JObject json = null;
            if (user != null)
            {
                string tokenString = BuildToken(user);
                StringToJsonConverter token = new StringToJsonConverter(tokenString);
                response = JsonConvert.SerializeObject(token);
                json = JObject.Parse(response);
            }
            else
            {
                StringToJsonConverter token = new StringToJsonConverter("null");
                response = JsonConvert.SerializeObject(token);
                json = JObject.Parse(response);
            }

            return json;
        }

        private string BuildToken(UserModel user)
        {

            List<Claim> claim = new List<Claim> { new Claim("email", user.email), new Claim("password", user.password) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
                claims: claim,
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private UserModel Authenticate(LoginModel login)
        {
            var user = db.RegisteredUsers.Find(login.email);
            if (user.password.Equals(login.password))
                return user;
            else
                return null;
        }




        //public IActionResult Token()
        //{
        //    //string tokenString = "test";
        //    var header = Request.Headers["Authorization"];
        //    if (header.ToString().StartsWith("Basic"))
        //    {
        //        var credValue = header.ToString().Substring("Basic ".Length).Trim();
        //        var usernameAndPassenc = Encoding.UTF8.GetString(Convert.FromBase64String(credValue)); //admin:pass
        //        var usernameAndPass = usernameAndPassenc.Split(":");
        //        //check in DB username and pass exist

        //        if (usernameAndPass[0] == "Admin" && usernameAndPass[1] == "pass")
        //        {
        //            var claimsdata = new[] { new Claim(ClaimTypes.Name, usernameAndPass[0]) };
        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        //            var token = new JwtSecurityToken(
        //                 issuer: "mysite.com",
        //                 audience: "mysite.com",
        //                 expires: DateTime.Now.AddMinutes(1),
        //                 claims: claimsdata,
        //                 signingCredentials: signInCred
        //                );
        //            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        //            return Ok(tokenString);
        //        }
        //    }
        //    return BadRequest("wrong request");
        //}



        //    private string BuildToken(UserModel user)
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        //      _config["Jwt:Issuer"],
        //      expires: DateTime.Now.AddMinutes(30),
        //      signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


        //private UserModel Authenticate(LoginModel login)
        //{
        //    UserModel user = null;
        //    string connetionString;
        //    connetionString = "Server=localhost;Port=5432;Database=MyApplication;User ID=postgres;Password=9200163022@";
        //    NpgsqlConnection con = new NpgsqlConnection(connetionString);
        //    con.Open();
        //    NpgsqlCommand com;
        //    NpgsqlDataReader data;
        //    String sql;
        //    //string[] output;
        //    sql = "select  name,email, password  from users where email='" + login.email + "' and password='" + login.password + "' ;";
        //    com = new NpgsqlCommand(sql, con);
        //    data = com.ExecuteReader();



        //    while (data.Read())
        //    {


        //        user = new UserModel
        //        {
        //            name = data["name"].ToString(),
        //            email = data["email"].ToString(),
        //            password = data["password"].ToString()

        //        };
        //    }
        //   con.Close();
        //    return user;
        //}
        //private static void ReadSingleRow(IDataRecord record)
        //{
        //    Console.WriteLine(String.Format("{0}, {1}, {2}", record[0], record[1], record[2]));
        //}


    }

}