using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2.Data
{
    public class User
    {
        [Flags]
        public enum UserPermissions{
            None            = 0,
            Read            = 1,
            Write           = 2,
            ModifySelf      = 4,
            ModifyOther     = 8,
            IgnoreCooldown  = 16,
            Administrator   = 256,
        }
        
        //User properties
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime TimeOfCreation { get; set; }
        public string Password { get; set; } = string.Empty;
        public UserPermissions Permissions { get; set; }

        public User() { }
        public User(int id, string name, string password, UserPermissions defaultPermissions)
        {
            Id = id;
            Name = name;
            TimeOfCreation = DateTime.Now;
            SetPassword(password, "ABCDEFG");

            //set default permissions
            Permissions = defaultPermissions;
        }

        //Returns -1 if failure, 0 if success
        public int SetPassword(string password, string? salt = null)
        {
            Password = password;
            //Implement encryption mechanism later
            return 0; //sucess
        }

        public string GetPassword(string salt)
        {
            //Procedure for decrypting password
            return Password;  //Implement later
        }

        public bool IsPasswordMatching(string password, string? salt = null)
        {
            //Logic for decrypting passwords and checking if final content
            //Is same as input
            if(password == Password)
            {
                return true;
            }
            return false;
        }

        //User must contain all permissions specified by parameter, or else the function will return false (unless the user contains administrator privileges).
        public bool VerifyPermissions(params UserPermissions[] targetPermissions)
        {
            //Check if user is administrator (which ignores the need for other lower level permissions)
            if ((Permissions & UserPermissions.Administrator) == UserPermissions.Administrator)
            {
                return true;
            }

            foreach (var permission in targetPermissions)
            {
                if((Permissions & permission) == permission)
                {
                    continue;
                }
                return false;
            }

            return true;
        }

    }
}
