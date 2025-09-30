using CommunityToolkit.Maui.Views;
using MauiApp2.ClassManaging;
using MauiApp2.Data;
using MauiApp2.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.ObjectModel;

namespace MauiApp2.Minipages;

public partial class CalendarPopup : Popup
{
    public User TargetUser;
    private readonly UserControl _usrControl;
    private ObservableCollection<SchedulingData> data = new ObservableCollection<SchedulingData>();
    public ObservableCollection<SchedulingData> Data { get { return data; } }
    public object? Result { get; private set; }
    public CalendarPopup(UserControl usrControl, User targetUser)
    {
        InitializeComponent();
        _usrControl = usrControl;
        TargetUser = targetUser;
        UpdateButtons();
    }

    async void OnExitClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        if(sender is Button)
        {
            Calendar.IsVisible = true;
            ScheduleListView.IsVisible = false;
        }

        await _usrControl.PushUserAsync(TargetUser);
        UpdateButtons();
    }

    private void OnPeriodSelected(object sender, EventArgs e)
    {
        if(sender is Button btn)
        {
            string time = btn.AutomationId[..5];
            string day = btn.AutomationId[6..];
            var weekday = day switch
            {
                "Segunda" => Weekdays.Monday,
                "Terca" => Weekdays.Tuesday,
                "Quarta" => Weekdays.Wednesday,
                "Quinta" => Weekdays.Thursday,
                "Sexta" => Weekdays.Friday,
                _ => Weekdays.Saturday,
            };
            Calendar.IsVisible = false;
            ScheduleListView.IsVisible = true;
            UpdateCollection(TimeSpan.Parse(time), weekday);      
        }
    }

    private void UpdateCollection(TimeSpan time, Weekdays day)
    {
        var grades = _usrControl._context.Components.ToList();
        data.Clear();

        foreach (var grade in grades)
        {
           string gradeName = grade.Name;
           foreach (var component in grade.AvailableInfo)
           {
               if(component.PeriodStart == time && component.Day == day)
                {
                    data.Add(new SchedulingData
                    {
                        GradeName = gradeName,
                        Classroom = component.Classroom,
                        Day = component.Day,
                        Identification = component.Identification,
                        PeriodStart = component.PeriodStart,
                        PeriodEnd = component.PeriodEnd,
                    }
                    );
               }
           }            
        }
        SchedulingList.ItemsSource = data;
    }

    private async void UpdateButtons()
    {
        await App.Logger.WriteLineAsync($"Reading user '{TargetUser.Name}' schedule (size: {TargetUser.GradingComponents.Count})... ");
        foreach(var binder in TargetUser.GradingComponents)
        {
            await App.Logger.WriteLineAsync($"{binder.Name} ({binder.Id}): @{binder.Classroom}, {binder.Day} | {binder.PeriodStart}");
        }
        
        await App.Logger.WriteLineAsync("Updating buttons...");
        try
        {
            foreach (var element in Calendar.GetVisualTreeDescendants())
            {
                if (element is Button btn)
                {
                    string? cellIdentifier = btn.AutomationId;

                    if (cellIdentifier == null) 
                        continue;

                    var time = TimeSpan.Parse(cellIdentifier[..5]);
                    var day = cellIdentifier[6..] switch
                    {
                        "Segunda" => Weekdays.Monday,
                        "Terca" => Weekdays.Tuesday,
                        "Quarta" => Weekdays.Wednesday,
                        "Quinta" => Weekdays.Thursday,
                        "Sexta" => Weekdays.Friday,
                        _ => Weekdays.Saturday,
                    };

                    var schedule = TargetUser.GradingComponents.FirstOrDefault(gc =>
                    gc.Day == day && gc.PeriodStart == time);
                    await App.Logger.WriteLineAsync($"Schedule: {schedule}");

                    if (schedule != null)
                    {
                        btn.Text = schedule.Name;
                        continue;
                    }
                    btn.Text = "";
                }
            }
        }
        catch (Exception ex) 
        {
            await App.Logger.WriteExceptionAsync(ex);
        }
    }

    private async void OnSelectClicked(object? sender, EventArgs e)
    {
#if !DEBUG
        //Deny access if user is not administrator
        bool? permissionState = App.ActiveUser?.VerifyPermissions(User.UserPermissions.Administrator);
        if (permissionState.HasValue && !permissionState.Value)
        {
            await App.Logger.WriteExceptionAsync(new Exception("User does not contain administrator privileges."));
            return;
        }
#endif
        if (sender is Button btn && btn.BindingContext is SchedulingData selectedSchedule)
        {
            bool result = TargetUser.AddGradingComponent(selectedSchedule.ToComponentBinder(), true);
            await App.Logger.WriteLineAsync($"Added grading component success: {result}");
            await App.Logger.WriteLineAsync($"Number of grading components: {TargetUser.GradingComponents.Count}");

            await _usrControl.PushUserAsync(TargetUser);
            UpdateButtons();
            Calendar.IsVisible = true;
            ScheduleListView.IsVisible = false;
        }

    }
}