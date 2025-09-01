using CommunityToolkit.Maui.Views;
using MauiApp2.ClassManaging;
using MauiApp2.Data;
using MauiApp2.Models;
using MauiApp2.Resources.Animation;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiApp2.Minipages;

public partial class UserListPopup : Popup
{
    private readonly UserControl _usrControl;
    private ObservableCollection<UserData> data = new ObservableCollection<UserData>();
    public ObservableCollection<UserData> Data { get { return data; } }
    public object? Result { get; private set; }

    public UserListPopup(UserControl usrControl)
	{
        InitializeComponent();
        _usrControl = usrControl;
        UpdateCollection();
    }

    private void UpdateCollection()
    {
        var users = _usrControl._context.Users.ToList();
        data.Clear();

        foreach (var user in users)
        {
            data.Add(new UserData
            {
                Id = user.Id,
                Name = user.Name,
                TimeOfCreation = user.TimeOfCreation,
                Email = user.Email,
                Permissions = user.Permissions
            }
            );
        }
        UserList.ItemsSource = data;
    }
}