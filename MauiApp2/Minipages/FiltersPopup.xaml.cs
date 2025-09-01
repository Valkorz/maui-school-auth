using MauiApp2.Data;
using MauiApp2.Models;
using CommunityToolkit.Maui.Views;

namespace MauiApp2.Minipages;

public partial class FiltersPopup : Popup
{
    private readonly UserControl _usrControl;
    public object? Result { get; private set; }

    List<string> comboListCodes     = [];
    List<string> comboListNames     = [];
    List<string> comboListSemesters = [];
    public FiltersPopup(UserControl usrControl)
    {
        InitializeComponent();
        _usrControl = usrControl;
        UpdateComboList();
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        // Retorna os dados preenchidos para quem chamou o popup
        Result = new GradeData()
        {
            GradeName = comboListNames[NameEntry.SelectedIndex],
            GradeCode = comboListCodes[CodeEntry.SelectedIndex],
            Semester = Convert.ToInt32(comboListSemesters[Semester.SelectedIndex])
        };

        await CloseAsync();
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    public async void UpdateComboList()
    {
        var componentsList = await _usrControl.GetExistingComponentsAsync();
        
        comboListCodes.Clear();
        comboListCodes.Add("NONE");

        comboListNames.Clear();
        comboListNames.Add("NONE");

        comboListSemesters.Clear();
        comboListSemesters.Add("0");

        foreach (var component in componentsList) 
        {
            if(!comboListCodes.Contains(component.Code)) comboListCodes.Add(component.Code);
            if(!comboListNames.Contains(component.Name)) comboListNames.Add(component.Name);
            if(!comboListSemesters.Contains(component.Semester.ToString())) comboListSemesters.Add(component.Semester.ToString());
        }
        comboListSemesters  = [.. comboListSemesters.OrderBy(x => int.Parse(x))];

        CodeEntry.ItemsSource   = comboListCodes;
        CodeEntry.SelectedIndex = 0;

        NameEntry.ItemsSource   = comboListNames;
        NameEntry.SelectedIndex = 0;

        Semester.ItemsSource    = comboListSemesters;
        Semester.SelectedIndex  = 0;

    }
}