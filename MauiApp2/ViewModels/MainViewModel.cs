using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using MauiApp2.Models;

namespace MauiApp2.ViewModels
{
    public class MainViewModel 
    {
        public ObservableCollection<ImageData> ImageSources {  get; set; }

        public MainViewModel() 
        {
            ImageSources = new ObservableCollection<ImageData>()
            {
                new ImageData{ImageSource="facens1.jpg", Text="img1"},
                new ImageData{ImageSource="facens2.jpg", Text="img2"},
                new ImageData{ImageSource="facens3.jpg", Text="img3"},
                new ImageData{ImageSource="facens4.jpg", Text="img4"}
            };
        }
    }
}
