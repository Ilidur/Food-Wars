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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FoodTinder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LandingPage : Page
    {
        Canvas mainCanvas;

        private DispatcherTimer timer;

        public LandingPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Tick += SwapPage;
            timer.Start();
        }

        /// <summary>
        /// Call this to start fooding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwapPage(object sender, object e)
        {
            timer.Stop();
            this.Frame.Navigate(typeof(SuggestionPage));
            ///code
        }
    }
}
