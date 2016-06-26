using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FoodTinder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class results : Page
    {
        public results()
        {
            this.InitializeComponent();
        }

        private void CreateSuggestion(string type, int trackNum)
        {
            if (type == null)
            {
                return;
            }

            /*
            MyUserControl1 suggestion = new MyUserControl1();

            Canvas.SetLeft(suggestion, trackNum * RenderSize.Width / 3 + 20);
            suggestion.Name = type;
            suggestion.Tag = false; //Set to true if picked
            mainCanvas.Children.Add(suggestion);

            TextBlock foodType = (TextBlock)suggestion.FindName("text");
            foodType.Text = type;

            activeSuggestions.Add(type, suggestion);
            */
        }
    }
}
