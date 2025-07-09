
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
        public string Email { get; set; } = string.Empty;
        public UserPermissions Permissions { get; set; }

        public User() { }
        public User(int id, string name, string password, UserPermissions defaultPermissions, string email)
        {
            Id = id;
            Name = name;
            TimeOfCreation = DateTime.Now;
            SetPassword(password, "ABCDEFG");
            SetEmail(email);   

            //set default permissions
            Permissions = defaultPermissions;
        }

        public User Clone()
        {
            var user = new User
            {
                Id = this.Id,
                Name = this.Name,
                Password = this.Password,
                Permissions = this.Permissions,
                TimeOfCreation = TimeOfCreation
            };
            return user;
        }

        public int SetEmail(string email)
        {
            //Verify email formatting
            if(!email.EndsWith("@gmail.com"))
            {
                return -1;
            }
            
            //Logic for setting the email. Verifies if the email exists and sends a verification code.

            Email = email;
            return 0;
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
