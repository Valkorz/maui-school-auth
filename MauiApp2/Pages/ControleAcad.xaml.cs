using MauiApp2.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;


namespace MauiApp2.Pages {
    public partial class ControleAcad : ContentPage
    {
        private readonly UserControl _usrControl;
        private User? SelectedUser;

        public ControleAcad(UserControl usrControl)
        {
            InitializeComponent();
            _usrControl = usrControl;
        }

        public async void UpdatePageContents()
        {
            //TODO update all fields when the user is changed
        }

        public async void OnCompletion(object? sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
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
                if (usr == null)
                {
                    EntryDate.Text = "00/00/0000";
                    Username.Text = "-";
                }
                else
                {
                    EntryDate.Text = usr.TimeOfCreation.ToShortDateString();
                    Username.Text = usr.Name;
                    SelectedUser = usr;
                }
            }
        }
    }
}

