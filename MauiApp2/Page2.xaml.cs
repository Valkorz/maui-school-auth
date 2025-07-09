using MauiApp2.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Globalization;

namespace MauiApp2;
public partial class Page2 : ContentPage
{
    private readonly UserControl _usrControl;

    public Page2(UserControl usrControl)
	{
        InitializeComponent();
        _usrControl = usrControl;
    }

    //Verify existence of user and authenticate if possible
    public async void OnSubmitClicked(object? sender, EventArgs e)
    {
        int id_value        = Convert.ToInt32(entry_ID.Text);
        string pass_value   = entry_pass.Text;

        try
        {
            User usr = await _usrControl.GetUserByIdAsync(id_value);
            if (usr != null && usr.IsPasswordMatching(pass_value))
            {
                await DisplayAlert("Sucesso", $"Bem vindo. {usr.Name}!", "OK");
                App.ActiveUser = usr;
                await Shell.Current.GoToAsync(nameof(MauiApp2.ControlPanel));
            }
            else if(usr != null && !usr.IsPasswordMatching(pass_value))
            {
                await DisplayAlert("Erro", "Senha incorreta.", "OK");
            }
            else if(usr == null)
            {
                await DisplayAlert("Erro", "Usuário não encontrado.", "OK");
            }

        } catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.ToString(), "OK");
        }
    }

    public async void OnCompletion(object? sender, EventArgs e)
    {
        if(sender is Entry entry)
        {
            if (entry.Text == null)
                return;

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
        }

        //if (!await _usrControl.ContainsUserByAsync(u => u.Id == Convert.ToInt32(entry_ID.Text)))
        //{
        //    await DisplayAlert("Erro", "Registro inexistente", "OK");
        //}
    }
}