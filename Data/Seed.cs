using System.Collections.Generic;
using DatingApp.API.models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            this._context = context;

        }

        public void SeedUsers()
        {
            //  _context.Users.RemoveRange(_context.Users);
            //  _context.SaveChanges();

            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                //create the password hash
                byte[] passwordHash, passowrdSalt;
                CreatePasswordHash("password", out passwordHash, out passowrdSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passowrdSalt;
                user.Username = user.Username.ToLower();

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}