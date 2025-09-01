using CommunityToolkit.Maui.Extensions;
using MauiApp2.Data;
using MauiApp2.Models;
using MauiApp2.Resources.Animation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using MauiApp2.Minipages;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;

namespace MauiApp2.Pages
{
    public partial class GradingManagement : ContentPage
    {
        private readonly UserControl _usrControl;
        private string _componentName = string.Empty;
        private GradeData filter = new GradeData { GradeCode = "NONE", GradeName = "NONE",Semester = 0};

        private ObservableCollection<GradeData> data = new ObservableCollection<GradeData>();
        public ObservableCollection<GradeData> Data { get { return data; } }
        public GradingManagement(UserControl usrControl)
        {
            InitializeComponent();
            _usrControl = usrControl;
            UpdateCollection();
        }

        public async void OnAddRequest(object? sender, EventArgs e)
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

            if (sender is Button btn)
            {
                InterfaceAnimator.AnimatePop(btn);

                var popup = new GradingPopup(_usrControl);
                await this.ShowPopupAsync(popup);

                if (popup.Result is GradeData data) 
                {
                    await _usrControl.AddComponentAsync(new ClassManaging.StudentGradeComponent
                    (
                        data.GradeName,
                        data.Description,
                        data.Semester
                    ));

                    UpdateCollection();
                }
 
            }
        }

        public async void OnRemoveRequest(object? sender, EventArgs e)
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
            if (sender is Button btn && btn.BindingContext is GradeData selectedGrade)
            {
                InterfaceAnimator.AnimatePop(btn);

                var identification = selectedGrade.GradeCode;
                await _usrControl.RemoveComponentAsync(identification);
                UpdateCollection();
            }

        }

        public async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
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

            if (e.CurrentSelection.FirstOrDefault() is GradeData selectedGrade)
            {
                _componentName = selectedGrade.GradeName;
                var popup = new SchedulingPopup(_usrControl, _componentName);
                await this.ShowPopupAsync(popup);
            }
        }

        public async void OnSchedulingSelect(object? sender, EventArgs e)
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
            if (sender is Button btn && btn.BindingContext is GradeData selectedGrade) 
            {
                InterfaceAnimator.AnimatePop(btn);

                string componentName = selectedGrade.GradeName;
                _componentName = selectedGrade.GradeName;
                var popup = new SchedulingPopup(_usrControl, _componentName);
                await this.ShowPopupAsync(popup);
            }
        }

        public async void OnFilterSetRequest(object? sender, EventArgs e)
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

            if (sender is Button btn)
            {
                InterfaceAnimator.AnimatePop(btn);

                var popup = new FiltersPopup(_usrControl);
                await this.ShowPopupAsync(popup);

                if (popup.Result is GradeData data)
                {
                    filter = data;
                    ListaFiltros.Text = $"FILTRO: Cód.: {filter.GradeCode}, Nome: {filter.GradeName}, Semestre: {filter.Semester}";

                    UpdateCollection();
                }

            }
        }

        private void UpdateCollection()
        {
            var grades = _usrControl._context.Components.ToList();
            data.Clear();

            foreach (var grade in grades)
            {
                if( (grade.Code == filter.GradeCode || filter.GradeCode == "NONE") && 
                    (grade.Name == filter.GradeName || filter.GradeName == "NONE") &&
                    (grade.Semester == filter.Semester || filter.Semester == 0))
                {
                    data.Add(new GradeData { GradeCode = grade.Code, GradeName = grade.Name, Description = grade.Description, Semester = grade.Semester });
                }

            }
            GradingList.ItemsSource = data;
        }
    }
}

