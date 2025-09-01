using CommunityToolkit.Maui.Views;
using MauiApp2.Data;
using MauiApp2.ClassManaging;
using MauiApp2.Models;
using System.Threading.Tasks;

namespace MauiApp2.Minipages
{
    public partial class GradingPopup : Popup
    {
        private readonly UserControl _usrControl;
        public object? Result { get; private set; }
        public GradingPopup(UserControl usrControl)
        {
            InitializeComponent();
            _usrControl = usrControl;
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            // Retorna os dados preenchidos para quem chamou o popup
            Result = new GradeData()
            {
                GradeName = NameEntry.Text,
                Description = Description.Text,
                Semester = Convert.ToInt32(Semester.Text)
            };

            await CloseAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await CloseAsync();
        }

    }
}

