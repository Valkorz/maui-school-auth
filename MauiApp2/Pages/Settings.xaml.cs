using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiApp2.Data;

namespace MauiApp2.Pages;

public partial class Settings : ContentPage
{
    private readonly UserControl _usrControl;
    private bool isSideBarOpen = false;

    public Settings(UserControl usrControl)
	{
        BindingContext = this;
		_usrControl = usrControl;
        InitializeComponent();
	}

	private async void OnToggleSidebarClicked(object sender, EventArgs e)
	{
		isSideBarOpen = !isSideBarOpen;
        await Sidebar.TranslateTo(isSideBarOpen? 0 : -260, 0, 250, Easing.CubicInOut);
	}

	private void OnSettingsSelectPage(object sender, EventArgs e)
	{
		if(sender is Button btn)
		{
			Profile.IsVisible = btn.AutomationId == "0";
            Appearance.IsVisible = btn.AutomationId == "1";
            Notifications.IsVisible = btn.AutomationId == "2";
        }
	}
}