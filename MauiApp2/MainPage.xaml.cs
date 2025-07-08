using MauiApp2.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly UserControl _usrControl;

        public MainPage(UserControl usrControl)
        {
            InitializeComponent();
            Debug.WriteLine($"DB Path: {Path.Combine(FileSystem.AppDataDirectory, "Users.db3")}");
            _usrControl = usrControl;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Add admin user if not existing
            if (! await _usrControl.ContainsUserByAsync(u => u.Id == 1))
            {
                User admin = new
                    (id: 0,
                    name: "Admin",
                    password: "123",
                    defaultPermissions: User.UserPermissions.Administrator);

                await _usrControl.AddUserAsync(admin);
            }
        }

        private async void OnNextPageClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MauiApp2.Page2));
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
