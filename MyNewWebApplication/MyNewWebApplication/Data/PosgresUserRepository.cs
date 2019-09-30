using MyNewWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNewWebApplication.Data
{
    public class PosgresUserRepository : IPosgresContext
    {

        private readonly ApplicationDbContext _context;
        public PosgresUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateUser(UserModel user)
        {
            _context.RegisteredUsers.Add(user);
            _context.SaveChanges();
            return true;
        }
        public IEnumerable<UserModel> GetAll()
        {
            // return users without passwords
            return _context.RegisteredUsers.ToList();
        }

        public UserModel GetUser(string email)
        {
            return _context.RegisteredUsers.Find(email);
        }
    }
}
