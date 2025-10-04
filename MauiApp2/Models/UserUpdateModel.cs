using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp2.Data;

namespace MauiApp2.Models
{
    public class UserUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public User.UserPermissions Permissions { get; set; }
        public DateTime TimeOfCreation { get; set; }


        public int SetEmail(string email)
        {
            //Verify email formatting
            if (!email.EndsWith("@gmail.com"))
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
            if (password == Password)
            {
                return true;
            }
            return false;
        }

        //User must contain all permissions specified by parameter, or else the function will return false (unless the user contains administrator privileges).
        public bool VerifyPermissions(params User.UserPermissions[] targetPermissions)
        {
            //Check if user is administrator (which ignores the need for other lower level permissions)
            if ((Permissions & User.UserPermissions.Administrator) == User.UserPermissions.Administrator)
            {
                return true;
            }

            foreach (var permission in targetPermissions)
            {
                if ((Permissions & permission) == permission)
                {
                    continue;
                }
                return false;
            }

            return true;
        }

    }
}
