using MauiApp2.Data;
using MauiApp2.Pages;
using MauiApp2.ClassManaging;
using MauiApp2.Resources.Animation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using MauiApp2.Models;

namespace MauiApp2
{
    public partial class ControlPanel : ContentPage
    {
        private readonly UserControl _usrControl;
        private ObservableCollection<SchedulingData> data = new ObservableCollection<SchedulingData>();
        public ObservableCollection<SchedulingData> Data { get { return data; } }
        public ControlPanel(UserControl usrControl)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

#if !DEBUG
            if ((App.ActiveUser?.Permissions & User.UserPermissions.Administrator) != User.UserPermissions.Administrator)
            {
                //disable admin panel if user does not contain the privileges.
                admin_panel.IsEnabled = false;
                admin_panel.IsVisible = false;
            }
#endif
            _usrControl = usrControl;
            Welcome.Text = $"Bem vindo(a), {App.ActiveUser?.Name}!";
            var weekday = (DateTime.Now.DayOfWeek) switch
            {
                DayOfWeek.Monday => Weekdays.Monday,
                DayOfWeek.Tuesday => Weekdays.Tuesday,
                DayOfWeek.Wednesday => Weekdays.Wednesday,
                DayOfWeek.Thursday => Weekdays.Thursday,
                DayOfWeek.Friday => Weekdays.Friday,
                _ => Weekdays.Saturday,
            };

            UpdateCollection(weekday);
            App.Logger.WriteLineAsync($"Bem vindo(a), {App.ActiveUser?.Name}!");
        }

        public async void OnDbControlClicked(object sender, EventArgs e)
        {
#if !DEBUG
            //Deny access if user is not administrator
            bool? permissionState = App.ActiveUser?.VerifyPermissions(User.UserPermissions.Administrator);
            Debug.WriteLine($"Permission state: {permissionState}");
            if (permissionState.HasValue && !permissionState.Value)
            {
                await DisplayAlert("Erro", "Acesso Negado.", "OK");
                return;
            }
#endif

            await Shell.Current.GoToAsync(nameof(ControleAcad));
        }

        public async void OnSignOut(object? sender, EventArgs e)
        {
            bool ans = await DisplayAlert("Sign Out", "Gostaria de desconectar-se?", "Sim", "Não");
            if (ans)
            {
                App.ActiveUser = null;
                await Shell.Current.GoToAsync("///MainPage");
            }
        }

        public async void OnGradingClicked(object? sender, EventArgs e)
        {
#if !DEBUG
            //Deny access if user is not administrator
            bool? permissionState = App.ActiveUser?.VerifyPermissions(User.UserPermissions.Administrator);
            if (permissionState.HasValue && !permissionState.Value)
            {
                await DisplayAlert("Erro", "Acesso Negado.", "OK");
                return;
            }
#endif
            await Shell.Current.GoToAsync(nameof(GradingManagement));
        }

        public void OnFocus(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {   
                InterfaceAnimator.AnimatePop(btn);
                Debug.WriteLine("\nFocus\n");
            }
        }

        private void UpdateCollection(Weekdays day)
        {
            var grades = _usrControl._context.Components
                        .Include(c => c.AvailableInfo)
                        .ToList();
            data.Clear();

            foreach (var grade in grades)
            {
                string gradeName = grade.Name;
                foreach (var component in grade.AvailableInfo)
                {
                    if (component.Day == day)
                    {
                        data.Add(new SchedulingData
                        {
                            GradeName = gradeName,
                            Classroom = component.Classroom,
                            Day = component.Day,
                            Identification = component.Identification,
                            PeriodStart = component.PeriodStart,
                            PeriodEnd = component.PeriodEnd,
                            GradeIdentification = grade.Code,
                        }
                        );
                    }
                }
            }
            SchedulingList.ItemsSource = data;
        }

        private void OnThemeToggle(object? sender, ToggledEventArgs e)
        {
            if (Application.Current == null)
                return;

            App.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        }

        private async void OnSettingsClicked(object? sender, EventArgs e)
        {
            if(Application.Current == null) return;

            await Shell.Current.GoToAsync(nameof(Settings));
        }
    }
}