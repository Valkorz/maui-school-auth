using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq.Expressions;
//using Java.Lang;
//using Android.Service.Controls.Actions;

namespace MauiApp2.Data
{
    //The class that implements methods for controlling the database
    public class UserControl(UserDbContext context)
    {
        public readonly UserDbContext _context = context;

        //Gets users in the database as IEnumerable
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public List<User> GetUsers()
        {
            return [.. _context.Users];
        }

        //Find user existance by specific lambda condition
        public bool ContainsUserBy(Func<User,bool> predicate)
        {
            return _context.Users.Any(predicate);
        }

        public async Task<bool> ContainsUserByAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.AnyAsync(predicate);
        }

        //Returns NULL if user has not been found
        public async Task<User?> GetUserByIdAsync(int targetId)
        {
            try
            {
                return await _context.Users.SingleAsync(x => x.Id == targetId);
            }
            catch
            {
                return null;
            }
        }

        //Adds user to database and returns a procedure status (sucess/failure)
        public async Task<int> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync();
        }

        //Pushes changes to an existing user
        public async Task<int> PushUserAsync(User user)
        {
            User target;

            try
            {
                target = await _context.Users.SingleAsync(u => u.Id == user.Id);
            }
            catch (InvalidOperationException)
            {
                return await AddUserAsync(user);
            }

            //Modify properties
            target.Password = user.Password;
            target.Name = user.Name;
            target.TimeOfCreation = user.TimeOfCreation;
            target.Permissions = user.Permissions;
            target.Email = user.Email;

            return await _context.SaveChangesAsync();
        }
    }
}
