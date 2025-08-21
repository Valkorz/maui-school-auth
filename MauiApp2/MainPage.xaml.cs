using MauiApp2.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        private readonly UserControl _usrControl;
        private IDispatcherTimer? _timer;

        public MainPage(UserControl usrControl)
        {
            _usrControl = usrControl;
            try
            {
                InitializeComponent();

                App.Logger?.WriteLineAsync("Entered main page");
                App.Logger?.WriteLineAsync($"DB Path: {Path.Combine(FileSystem.AppDataDirectory, "Users.db3")}");
            }
            catch (Exception ex)
            {
                App.Logger?.WriteExceptionAsync(ex);
            }
        }

        protected override async void OnAppearing()
        {
            try
            {
                base.OnAppearing();


                //Add admin user if not existing
                if (!await _usrControl.ContainsUserByAsync(u => u.Id == 1))
                {
                    User admin = new
                        (id: 0,
                        name: "Admin",
                        password: "123",
                        defaultPermissions: User.UserPermissions.Administrator,
                        email: "foo@gmail.com");

                    await _usrControl.AddUserAsync(admin);
                }

                //SetupTimer();
            }
            catch (Exception ex)
            {
                if (App.Logger != null)
                    await App.Logger.WriteExceptionAsync(ex);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }

        private async void OnNextPageClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MauiApp2.Page2));
        }

        private void SetupTimer()
        {
            try
            {
                if (_timer != null && _timer.IsRunning)
                {
                    return;
                }


                _timer = Dispatcher.CreateTimer();
                _timer.Interval = TimeSpan.FromSeconds(8);
                _timer.Tick += (sender, e) =>
                {
                    if (ImageCarousel.ItemsSource is not System.Collections.IList items || items.Count == 0)
                        return;

                    int nextPosition = (ImageCarousel.Position + 1) % items.Count;

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        ImageCarousel.Position = nextPosition;
                        Debug.WriteLine($"new Position: {ImageCarousel.Position}");
                    });
                };
                _timer.Start();
            }
            catch (Exception ex)
            {
                App.Logger?.WriteExceptionAsync(ex);
            }
        }
    }
}
