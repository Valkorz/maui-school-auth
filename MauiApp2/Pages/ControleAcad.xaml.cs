using CommunityToolkit.Maui.Extensions;
using MauiApp2.Data;
using MauiApp2.Minipages;
using MauiApp2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;


namespace MauiApp2.Pages {
    public partial class ControleAcad : ContentPage, INotifyPropertyChanged
    {
        private readonly UserControl _usrControl;

        private UserUpdateModel _usrUpdateModel;

        public UserUpdateModel SelectedUser { 
            get { return _usrUpdateModel; } 
            set 
            {
                _usrUpdateModel = value;
                OnPropertyChanged();
            } 
        }

        private bool _isUserActive;
        public bool IsUserActive
        {
            get { return _isUserActive; } set
            {
                _isUserActive = value;
                OnPropertyChanged();
            }
        }


        public ControleAcad(UserControl usrControl)
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {
                App.Logger.WriteExceptionAsync(ex);
            }
            _usrControl = usrControl;
            IsUserActive = false;


        }

        //todo: transformar em converter
        public async void UpdatePageContents()
        {
            try
            {
                //Rd.IsToggled = (SelectedUser.Permissions & User.UserPermissions.Read) == User.UserPermissions.Read;
                //Wr.IsToggled = (SelectedUser.Permissions & User.UserPermissions.Write) == User.UserPermissions.Write;
                //ModSelf.IsToggled = (SelectedUser.Permissions & User.UserPermissions.ModifySelf) == User.UserPermissions.ModifySelf;
                //ModOther.IsToggled = (SelectedUser.Permissions & User.UserPermissions.ModifyOther) == User.UserPermissions.ModifyOther;
                //IgnCoold.IsToggled = (SelectedUser.Permissions & User.UserPermissions.IgnoreCooldown) == User.UserPermissions.IgnoreCooldown;
                //Admin.IsToggled = (SelectedUser.Permissions & User.UserPermissions.Administrator) == User.UserPermissions.Administrator;

                DoC.Date = SelectedUser.TimeOfCreation;
                UsrName.Text = SelectedUser.Name;
                Pass.Text = SelectedUser.GetPassword(string.Empty);
                Email.Text = SelectedUser.Email;
            }
            catch (Exception ex) 
            {
                await App.Logger.WriteExceptionAsync(ex);
            }
        }

        public void OnToggled(object? sender, EventArgs e)
        {
            if(sender is Microsoft.Maui.Controls.Switch @switch)
            {
                if (@switch == Rd)
                    UpdatePermissionByState(@switch.IsToggled, User.UserPermissions.Read);
                else if (@switch == Wr)
                    UpdatePermissionByState(@switch.IsToggled, User.UserPermissions.Write);
                else if (@switch == ModSelf)
                    UpdatePermissionByState(@switch.IsToggled, User.UserPermissions.ModifySelf);
                else if (@switch == ModOther)
                    UpdatePermissionByState(@switch.IsToggled, User.UserPermissions.ModifyOther);
                else if (@switch == IgnCoold)
                    UpdatePermissionByState(@switch.IsToggled, User.UserPermissions.IgnoreCooldown);
                else if (@switch == Admin)
                    UpdatePermissionByState(@switch.IsToggled, User.UserPermissions.Administrator);
            }
        }

        public void OnDateChanged(object? sender, EventArgs e)
        {
            if (sender == null || SelectedUser == null)
            {
                return;
            }

            if (sender is DatePicker date_picker)
            {
                SelectedUser.TimeOfCreation = date_picker.Date;
                Debug.WriteLine($"Set user time of creation to: {SelectedUser.TimeOfCreation}");
            }
        }
        public void OnNameChanged(object? sender, EventArgs e)
        {
            if (sender == null || SelectedUser == null)
            {
                return;
            }

            if (sender is Entry entry)
            {
                SelectedUser.Name = entry.Text == null ? string.Empty : entry.Text;
            }
        }

        public void OnPassChanged(object? sender, EventArgs e)
        {
            if(sender == null || SelectedUser == null)
            {
                return;
            }

            if (sender is Entry entry)
            {
                SelectedUser.SetPassword(entry.Text);
            }
        }

        public async void OnEmailChanged(object? sender, EventArgs e)
        {
            if (sender == null || SelectedUser == null)
            {
                return;
            }

            if (sender is Entry entry)
            {
                int status = SelectedUser.SetEmail(entry.Text);

                if (status == -1)
                {
                    entry.Text = SelectedUser.Email;
                    await DisplayAlert("Erro", "Email não pôde ser verificado.", "OK");
                }
                else
                {
                    Debug.WriteLine($"Set user email to: {SelectedUser.Email} with status: {status}");
                }
            }
        }

        private static string DateTextFormat(string input)
        {
            string str_out = input;
            if(str_out.Length < 10)
            {
                if (str_out[2] != '/') str_out = str_out.Insert(2, "/");
                if (str_out[4] != '/') str_out = str_out.Insert(4, "/");
            }
            Debug.WriteLine($"\n Output: {str_out}\r\n");
            return str_out;
        }

        private void UpdatePermissionByState(bool state, User.UserPermissions permission)
        {
            if (SelectedUser == null)
            {
                return;
            }

            if (state)
            {
                SelectedUser.Permissions |= permission;
            }
            else if (!state)
            {
                SelectedUser.Permissions &= ~permission;
            }

            App.Logger.WriteLineAsync($"\n New user permission level: {(int)SelectedUser.Permissions}\r\n");
        }

        public async void OnCompletion(object? sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                if (entry.Text == null)
                    return;
                
                //Format display string
                string digits = new string(entry.Text.Where(char.IsDigit).ToArray()); //Select digits only

                if (digits.Length > 6)
                    digits = digits.Substring(0, 6);


                if (digits.Length > 0)
                {   
                    entry.Text = int.Parse(digits).ToString("D6");                   //Add zeros to empty characters
                }
                else
                {
                    entry.Text = string.Empty;
                }

                entry.CursorPosition = entry.Text.Length;                           //Move cursor to the line end

                //Update fields
                User? usr = await _usrControl.GetUserByIdAsync(Convert.ToInt32(entry.Text));
                await App.Logger.WriteLineAsync($"Is null? {usr == null}");
                if (usr == null)
                {
                    EntryDate.Text = "00/00/0000";
                    Username.Text = "-";
                    SelectedUser = new UserUpdateModel();
                    UpdatePageContents();
                }
                else
                {
                    EntryDate.Text = usr.TimeOfCreation.ToShortDateString();
                    Username.Text = usr.Name;
                    SelectedUser = new UserUpdateModel
                    {
                        Id = usr.Id,
                        Name = usr.Name,
                        TimeOfCreation = usr.TimeOfCreation,
                        Email = usr.Email,
                        Password = usr.Password,
                        Permissions = usr.Permissions,
                    };
                    UpdatePageContents();
                }
                OnPropertyChanged();
                IsUserActive = true;

                await App.Logger.WriteLineAsync($"user permissions: {SelectedUser.Permissions}");
            }
        }

        public async void OnUpdateRequest(object? sender, EventArgs e)
        {
#if !DEBUG
            if (App.ActiveUser != null && !App.ActiveUser.Permissions.HasFlag(User.UserPermissions.Administrator))
            {
                return;
            }
#endif

            try
            {
                if (SelectedUser == null)
                {
                    await DisplayAlert("Erro", "Usuário nulo", "OK");
                    return;
                }

                SelectedUser.Id = Convert.ToInt32(Reg.Text);
                int result = await _usrControl.PushUserAsync(SelectedUser);

                await App.Logger.WriteLineAsync($"\nResult: {result}\r\n");

                if (result >= 0)
                {
                    await DisplayAlert("Sucesso", "Cadastro atualizado com sucesso!", "OK");
                    return;
                }

                await DisplayAlert("Erro", "Ocorreu um problema ao atualizar o cadastro.", "OK");
                return;
            } catch(Exception ex)
            {
                await App.Logger.WriteExceptionAsync(ex);
            }
        }

        public async void OnCalendarOpen(object? sender, EventArgs e)
        {
#if !DEBUG
            if (App.ActiveUser != null && !App.ActiveUser.Permissions.HasFlag(User.UserPermissions.Administrator))
            {
                return;
            }
#endif

            var popup = new CalendarPopup(_usrControl, SelectedUser);
            await this.ShowPopupAsync(popup);
        }

        private void OnCancelRequest(object? sender, EventArgs e)
        {
            IsUserActive = false;            
        }

        private async void OnUserListRequest(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                var popup = new UserListPopup(_usrControl);
                await this.ShowPopupAsync(popup);
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        protected new void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

