using CommunityToolkit.Maui.Views;
using MauiApp2.ClassManaging;
using MauiApp2.Data;
using MauiApp2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.ObjectModel;

namespace MauiApp2.Minipages;

public partial class CalendarPopup : Popup
{
    public UserUpdateModel TargetUser;
    public string UserCode = string.Empty;
    private readonly UserControl _usrControl;
    private ObservableCollection<SchedulingData> data = new ObservableCollection<SchedulingData>();
    public ObservableCollection<SchedulingData> Data { get { return data; } }
    public object? Result { get; private set; }
    public CalendarPopup(UserControl usrControl, UserUpdateModel targetUser)
    {
        InitializeComponent();
        _usrControl = usrControl;
        TargetUser = new UserUpdateModel
        {
            Id = targetUser.Id,
            Name = targetUser.Name,
            Email = targetUser.Email,
            Password = targetUser.Password,
            Permissions = targetUser.Permissions,
            TimeOfCreation = targetUser.TimeOfCreation,
        };
        UpdateButtons();

        //foreach (var entry in _usrControl.GetUsers().SingleOrDefault(x => x.Name == targetUser.Name).GradingComponents)
        //{
        //    App.Logger.WriteLineAsync($"entry info: {entry.Code},{entry.Name}, {entry.Classroom}");
        //}
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
        var grades = _usrControl._context.Components
                    .Include(c => c.AvailableInfo)
                    .ToList();
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
                        GradeIdentification = grade.Code,
                    }
                    );
               }
           }            
        }
        SchedulingList.ItemsSource = data;
    }

    private async void UpdateButtons()
    {
        //await App.Logger.WriteLineAsync($"Reading user '{TargetUser.Name}' schedule (size: {TargetUser.GradingComponents.Count})... ");
        //foreach(var binder in TargetUser.GradingComponents)
        //{
        //    await App.Logger.WriteLineAsync($"{binder.Name} ({binder.Id}): @{binder.Classroom}, {binder.Day} | {binder.PeriodStart}");
        //}
        
        await App.Logger.WriteLineAsync("Updating buttons...");
        try
        {
            foreach (var element in Calendar.GetVisualTreeDescendants())
            {
                if (element is Button btn)
                {
                    string? cellIdentifier = btn.AutomationId;
                    btn.Text = "";
                    btn.Style = (Style?)this.Resources["CalendarCellButtonStyle"];

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

                    var user = await _usrControl._context.Users
                        .Include(c => c.GradingComponents)
                        .FirstOrDefaultAsync(x => x.Id == TargetUser.Id);
                    var schedule = user?.GradingComponents
                        .FirstOrDefault(gc =>
                    (gc.Day == day && gc.PeriodStart == time) || (gc.Day == day && TimeSpan.Compare(gc.PeriodStart, time) <= 0 && TimeSpan.Compare(gc.PeriodEnd, time) >= 0));
                    await App.Logger.WriteLineAsync($"Schedule: {schedule?.Name}, t1 = {schedule?.PeriodStart}");

                    if (schedule != null)
                    {
                        btn.Text = schedule.Name;
                        btn.Style = (Style?)this.Resources["CalendarCellButtonStyleSelected"];
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
            //bool result = TargetUser.AddGradingComponent(selectedSchedule.ToComponentBinder(), true);
            //await App.Logger.WriteLineAsync($"Added grading component success: {result}");
            //await App.Logger.WriteLineAsync($"Number of grading components: {TargetUser.GradingComponents.Count}");
            await App.Logger.WriteLineAsync($"Schedule grading code: {selectedSchedule.GradeIdentification}");

            int res_1 = await _usrControl.AddGradingComponent(selectedSchedule.ToComponentBinder(), TargetUser.Id, true);
            await _usrControl.PushUserAsync(TargetUser);

            await App.Logger.WriteLineAsync($"Operation result: {res_1}");
            var usr = _usrControl.GetUsers().FirstOrDefault(x => x.Id == TargetUser.Id);

            foreach (var comp in usr.GradingComponents) 
            {
                await App.Logger.WriteLineAsync($"component: {comp.Name}");
            }

            await App.Logger.WriteLineAsync($"Number of components: {_usrControl.GetUsers().SingleOrDefault(x => x.Id == TargetUser.Id)?.GradingComponents.Count}");

            UpdateButtons();
            Calendar.IsVisible = true;
            ScheduleListView.IsVisible = false;
        }

    }
}