using MyNewWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNewWebApplication.Data
{
    interface IPosgresContext
    {

        IEnumerable<UserModel> GetAll();
        UserModel GetUser(string email);
        bool CreateUser(UserModel user);
    }
}
