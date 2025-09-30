using CommunityToolkit.Maui.Views;
using MauiApp2.ClassManaging;
using MauiApp2.Data;
using MauiApp2.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MauiApp2.Resources.Animation;

namespace MauiApp2.Minipages
{
    public partial class SchedulingPopup : Popup
    {
        private readonly UserControl _usrControl;
        private readonly string _componentName;
        private ObservableCollection<SchedulingData> data = new ObservableCollection<SchedulingData>();
        public ObservableCollection<SchedulingData> Data { get { return data; } }
        public object? Result { get; private set; }

        private readonly List<string> Classrooms_ComboBoxEntries = 
        [
            "A11","A12","A13","A14",
            "A21","A22","A23","A24",
            "C21","C22","C23","C24",
            "C31","C32","C33","C34",
            "C41","C42","C43","C44",
            "D11","D12","D13","D14"
        ];

        private readonly List<string> Weekdays_ComboBoxEntries = 
        [
            Weekdays.Monday.ToString(), 
            Weekdays.Tuesday.ToString(), 
            Weekdays.Wednesday.ToString(),
            Weekdays.Thursday.ToString(), 
            Weekdays.Friday.ToString(), 
            Weekdays.Saturday.ToString()
        ];

        private readonly List<string> Periods_ComboBoxEntries = 
        [
            "19:00",
            "19:50",
            "21:00",
            "21:50",
        ];

        private string Identification = string.Empty;
        private string Classroom = string.Empty;
        private Weekdays Weekday;
        private TimeSpan From = TimeSpan.Zero, To = TimeSpan.Zero;

        public SchedulingPopup(UserControl usrControl, string componentName)
        {
            InitializeComponent();
            _usrControl = usrControl;
            _componentName = componentName;
            UpdateCollection();

            ClassroomEntry.ItemsSource = Classrooms_ComboBoxEntries;
            ClassroomEntry.SelectedIndex = 0;

            WeekdayEntry.ItemsSource = Weekdays_ComboBoxEntries;
            WeekdayEntry.SelectedIndex = 0;

            PeriodStartEntry.ItemsSource = Periods_ComboBoxEntries; 
            PeriodStartEntry.SelectedIndex = 0;

            PeriodEndEntry.ItemsSource = Periods_ComboBoxEntries; 
            PeriodEndEntry.SelectedIndex = 0;
        }
        async void OnExitClicked(object sender, EventArgs e)
        {
            await CloseAsync();
        }

        public async void OnRemoveRequest(object? sender, EventArgs e)
        {
#if !DEBUG
            //Deny access if user is not administrator
            bool? permissionState = App.ActiveUser?.VerifyPermissions(User.UserPermissions.Administrator);
            if (permissionState.HasValue && !permissionState.Value)
            {
                //await DisplayAlert("Erro", "Acesso Negado.", "OK");
                return;
            }
#endif
            try
            {
                if (sender is Button btn && btn.BindingContext is SchedulingData selectedData)
                {
                    InterfaceAnimator.AnimatePop(btn);

                    await _usrControl.RemoveComponentApplicationInfoAsync(selectedData.Identification);

                    UpdateCollection();
                }
            }
            catch (Exception ex) 
            {
                await App.Logger.WriteExceptionAsync(ex);
            }
        }

        public void OnScheduleClicked(object sender, EventArgs e)
        {
            ScheduleListView.IsVisible = false;
            ScheduleEditView.IsVisible = true;
            //await CloseAsync();
        }

        public void OnCancelClicked(object sender, EventArgs e)
        {
            ScheduleListView.IsVisible = true;
            ScheduleEditView.IsVisible = false;
            //await CloseAsync();
        }

        public async void OnConfirmClicked(object sender, EventArgs e)
        {
            try
            {
                var Capp_Info = new ComponentApplicationInfo
                {
                    Identification = Identification,
                    Classroom = Classroom,
                    Day = Weekday,
                    PeriodStart = From,
                    PeriodEnd = To,
                };
                await _usrControl.AddComponentApplicationInfoAsync(Capp_Info, _componentName);
            }
            catch(Exception ex)
            {
                await App.Logger.WriteExceptionAsync(ex);
            }
            await CloseAsync();
        }

        public async void OnTextChangeVerify(object sender, EventArgs e)
        {
            string newText = string.Empty;
            if(sender is Entry entry)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        char[] hexChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'a', 'b', 'c', 'd', 'e', 'f' };      
                        var text = entry.Text;

                        foreach (var item in text)
                        {
                            
                            if (hexChars.Contains(item))
                            {
                                newText += item;
                            }

                            if (newText.Length > 6)
                                break;
                        }
                    });
                }
                catch (Exception ex)
                {
                    await App.Logger.WriteExceptionAsync(ex);
                    await CloseAsync();
                }
                entry.Text = newText.ToUpper();
            }
        }

        public void OnTextConfirm(object? sender, EventArgs e)
        {
            if(sender is Entry entry)
            {
                Identification = "TIN00" + entry.Text;
            }
        }

        public void OnClassConfirm(object? sender, EventArgs e)
        {
            if (sender is Picker picker)
            {
                Classroom = Classrooms_ComboBoxEntries[picker.SelectedIndex];
            }
        }

        public void OnWeekdayConfirm(object? sender, EventArgs e)
        {
            if (sender is Picker picker)
            {
                Weekday = (Weekdays)picker.SelectedIndex;
            }
        }

        public void OnPStartConfirm(object? sender, EventArgs e)
        {
            if (sender is Picker picker)
            {
                From = TimeSpan.Parse(Periods_ComboBoxEntries[picker.SelectedIndex]);
            }
        }
        public void OnPEndConfirm(object? sender, EventArgs e)
        {
            if (sender is Picker picker)
            {
                To = TimeSpan.Parse(Periods_ComboBoxEntries[picker.SelectedIndex]);
            }
        }

        private void UpdateCollection()
        {
            var grades = _usrControl._context.Components.ToList();
            data.Clear();

            foreach (var grade in grades)
            {
                if(grade.Name == _componentName)
                {
                    foreach(var component in grade.AvailableInfo)
                    {
                        data.Add(new SchedulingData { 
                            GradeIdentification = grade.Code,
                            Classroom           = component.Classroom, 
                            Day                 = component.Day, 
                            Identification      = component.Identification, 
                            PeriodStart         = component.PeriodStart,
                            PeriodEnd           = component.PeriodEnd,
                            }
                        );
                    }
                }
            }
            SchedulingList.ItemsSource = data;
        }
    }
}

