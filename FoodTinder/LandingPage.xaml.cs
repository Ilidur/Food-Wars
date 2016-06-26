using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
        //private DispatcherTimer timer;
        private SpeachDetector detector;

        List<string> keyWords;

        public LandingPage()
        {
            this.InitializeComponent();

            //timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, 3);
            //timer.Tick += SwapPage;
            //timer.Start();

            keyWords = new List<string>();
            keyWords.Add("start");

            detector = new SpeachDetector(keyWords);

            detector.SwapPageCallback += SwapPage;
            detector.Initialise();

           
            //await detector.OnSearchStart();
        }

        /// <summary>
        /// Call this to start fooding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async Task SwapPage()
        {
            //timer.Stop();

            await detector.OnSearchStop();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                this.Frame.Navigate(typeof(SuggestionPage));
            });

        }
    }
}
