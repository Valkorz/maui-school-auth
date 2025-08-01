using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq.Expressions;
using System.Diagnostics;
using MauiApp2.ClassManaging;
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
            Debug.WriteLine($"\nemail : {target.Email}\r\n");


            return await _context.SaveChangesAsync();
        }

        // GRADING STUFF

        //Add grading compontent
        public async Task<int> AddComponentAsync(StudentGradeComponent component)
        {
            _context.Components.Add(component);
            return await _context.SaveChangesAsync();
        }

        //returns all existing components
        public async Task<List<StudentGradeComponent>> GetExistingComponentsAsync()
        {
            return await _context.Components.ToListAsync();
        }

        //Updates existing component
        public async Task<int> PushComponentAsync(StudentGradeComponent component)
        {
            StudentGradeComponent target;
            try
            {
                target = await _context.Components.SingleAsync(c => c.Name == component.Name);
            }
            catch (InvalidOperationException)
            {
                return await AddComponentAsync(component);
            }
            //Modify properties
            target.Description = component.Description;
            target.Semester = component.Semester;
            target.TargetCourses = component.TargetCourses;
            target.AvailableInfo = component.AvailableInfo;
            return await _context.SaveChangesAsync();
        }

        //Add new component application info
        public async Task<int> AddComponentApplicationInfoAsync(ComponentApplicationInfo info, string componentName)
        {
            var component = await _context.Components.SingleAsync(c => c.Name == componentName);
            component.AvailableInfo.Add(info);
            return await _context.SaveChangesAsync();
        }

        //Finds component by matching identification
        public async Task<StudentGradeComponent?> GetComponentByIdentificationAsync(string identification)
        {
            try
            {
                return await _context.Components.SingleAsync(c => c.AvailableInfo.Any(i => i.Identification == identification));
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
