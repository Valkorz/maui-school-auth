using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp2.Data;
using MauiApp2.Models;

namespace MauiApp2.Converters
{
    class PermissionToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
        {

            if (value is User.UserPermissions userPermissions && parameter is User.UserPermissions requiredPermission)
            {
                App.Logger.WriteLineAsync($"value: {userPermissions}, parameter: {requiredPermission}");
                return userPermissions.HasFlag(requiredPermission);
            }
            App.Logger.WriteLineAsync("hahahaha deactivate");
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
        {
            return null;
        }
    }
}
