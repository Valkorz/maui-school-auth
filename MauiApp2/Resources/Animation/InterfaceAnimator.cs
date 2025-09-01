using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;

namespace MauiApp2.Resources.Animation
{
    public class InterfaceAnimator
    {
        private static List<string> ActiveAnimators = [];
        
        /// <summary>
        /// A "Pop" animation for an element when it is hovered, clicked or another action is done
        /// </summary>
        /// <param name="element"> The Dotnet MAUI element to be animated </param>
        /// <param name="length"> The animation duration in milliseconds </param>
        public static async void AnimatePop<T>(T element, uint length = 500, double scaling = 2) where T : View
        {
            if (ActiveAnimators.Contains(nameof(AnimatePop))) return;
            
            ActiveAnimators.Add(nameof(AnimatePop));

            await App.Logger.WriteLineAsync($"Animation for {element} started.");
            await element.ScaleTo(element.Scale * scaling, length / 3);
            await Task.Delay((int)length / 3);
            await element.ScaleTo(element.Scale / scaling, length / 3);

            ActiveAnimators.Remove(nameof(AnimatePop));
        }
    }
}
