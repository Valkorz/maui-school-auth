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
using MauiApp2.Attributes;
//using Java.Lang;
//using Android.Service.Controls.Actions;

namespace MauiApp2.Data
{
    //The class that implements methods for controlling the database
    [Preserve]
    public class UserControl(UserDbContext context)
    {
        public readonly UserDbContext _context = context;

        //Gets users in the database as IEnumerable
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                await App.Logger.WriteLineAsync("Requested for users list.");
                var usrList = await _context.Users.ToListAsync();
                await App.Logger.WriteLineAsync($"Got list: {usrList}");
                return usrList;
            } catch(Exception ex)
            {
                await App.Logger.WriteExceptionAsync(ex);
                return [];
            }
        }

        public List<User> GetUsers()
        {
            return [.. _context.Users];
        }

        //Find user existance by specific lambda condition
        public bool ContainsUserBy(Func<User,bool> predicate)
        {
            App.Logger.WriteLineAsync("Requested user lookup by predicate...");
            var usr = _context.Users.Any(predicate);
            App.Logger.WriteLineAsync($"User has been found? {usr}");
            return usr;
        }

        public async Task<bool> ContainsUserByAsync(Expression<Func<User, bool>> predicate)
        {
            await App.Logger.WriteLineAsync("Requested user lookup by predicate...");
            var usr = await _context.Users.AnyAsync(predicate);
            await App.Logger.WriteLineAsync($"User has been found? {usr}");
            return usr;
        }

        //Returns NULL if user has not been found
        public async Task<User?> GetUserByIdAsync(int targetId)
        {
            try
            {
                await App.Logger.WriteLineAsync($"Looking for user with ID: {targetId}...");
                var usr = await _context.Users.SingleAsync(x => x.Id == targetId);
                await App.Logger.WriteLineAsync($"Found user: {usr.Name}");
                return usr;
            }
            catch (Exception ex) 
            {
                await App.Logger.WriteExceptionAsync(ex);
                return null;
            }
        }

        //Adds user to database and returns a procedure status (sucess/failure)
        public async Task<int> AddUserAsync(User user)
        {
            try
            {
                await App.Logger.WriteLineAsync($"Requested add user operation for: {user.Id}:{user.Name}");
                _context.Users.Add(user);
                return await _context.SaveChangesAsync();
            } catch(Exception ex)
            {
                await App.Logger.WriteExceptionAsync(ex);
                return -1;
            }
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
            catch (Exception ex)
            {
                await App.Logger.WriteExceptionAsync(ex);
                return null;
            }
        }

        //Remove component 
        public async Task<int> RemoveComponentAsync(string identification)
        {
            try
            {
                await App.Logger.WriteLineAsync($"Requesting for removal: {identification}");
                var usr = await _context.Components.SingleAsync(x => x.Code == identification);
                await App.Logger.WriteLineAsync($"Found usr: {usr}");
                _context.Components.Remove(usr);
                int status = await _context.SaveChangesAsync();

                if (status == 0)
                {
                    await App.Logger.WriteLineAsync("Success.");
                }
                else await App.Logger.WriteLineAsync("Failure.");

                return status;
            }
            catch (Exception ex) 
            {
                await App.Logger.WriteExceptionAsync(ex);
                return -1;
            }
        }
    }
}
