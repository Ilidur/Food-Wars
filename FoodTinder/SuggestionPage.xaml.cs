﻿///ADD METHOD FOR COORDINATE TRANSLATION
///
///

using System;
using System.Diagnostics;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FoodTinder
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SuggestionPage : Page
    {
        //Timer stuff
        private int timerSpan = 1;
        private DispatcherTimer timer;

        //Main food scrolling area
        Canvas mainCanvas;

        //Suggestions
        private List<string> listOfFoodTypes;
        private Dictionary<string, MyUserControl1> activeSuggestions;

        //Spawn control
        private int numOfTracks;
        private List<bool> trackValidity;

        //Speech Detection
        List<string> constraints;
        SpeechDetector detector;

        //Points
        Points pointsSystem;

        public SuggestionPage()
        {
            this.InitializeComponent();

            //Track stuff
            numOfTracks = 5;
            trackValidity = new List<bool>();
            for (int i = 0; i < numOfTracks; i++)
            {
                trackValidity.Add(true);
            }

            
            //Timer stuff
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, timerSpan);
            timer.Tick += TimerTick;
            timer.Start();

            //Main canvas area
            //Dimensions should changed depending on situation.
            mainCanvas = new Canvas();
            //mainCanvas.Width = RenderSize.Width;
            //mainCanvas.Height = RenderSize.Height;
            this.Content = mainCanvas;

            //Suggestions manager stuff
            activeSuggestions = new Dictionary<string, MyUserControl1>();

            string fullLocationsList = "Data.txt";

            FoodWarDecisionEngine.DecisionStorage.LoadFinished += Loading;

            FoodWarDecisionEngine.DecisionStorage.Deserialize(fullLocationsList);
            constraints = FoodWarDecisionEngine.DecisionStorage.GetListOfAllFoods();

            //
            pointsSystem = new Points();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        public async void Loading()
        {
                constraints = FoodWarDecisionEngine.DecisionStorage.GetListOfAllFoods();

            detector = new SpeechDetector(constraints, Dispatcher);
            await detector.Initialise();
                detector.PickedFood += PickSuggestion;

                listOfFoodTypes = new List<string>(constraints);
        }

        //ADD SPAWNER PROPER
        private void TimerTick(object sender, object e)
        {
            Random rand = new Random();
            CreateSuggestion(PickRandomType(), PickTrack());
        }

        /// <summary>
        /// Returns index to valid track.
        /// </summary>
        /// <returns></returns>
        private int PickTrack()
        {
            List<int> validTracks = new List<int>();

            for (int i = 0; i < trackValidity.Count; ++i)
            {
                if (trackValidity[i])
                {
                    validTracks.Add(i);
                }
            }

            if (validTracks.Count == 0)
            {
                return -1;
            }
            else
            {
                Random rand = new Random();
                return validTracks[rand.Next(validTracks.Count)];
            }   
        }

        /// <summary>
        /// Gets a random food key.
        /// </summary>
        /// <returns>String representing the food type key.</returns>
        private string PickRandomType()
        {
            int size = listOfFoodTypes.Count;
            Random rand = new Random();

            string type = null;

            if (listOfFoodTypes.Count != 0)
            {
                type = listOfFoodTypes[rand.Next(size)];

                while (activeSuggestions.ContainsKey(type))
                {
                    type = listOfFoodTypes[rand.Next(size)];
                }

                listOfFoodTypes.Remove(type);
            }
            
            return type;
        }

        /// <summary>
        /// <para />TODO: GIVE ANIMATIONS & REVIEW!!1!
        /// 
        /// <para />Creates a suggestion box based on the parameters.
        ///         If one exists in the 'suggestions' dictionary it outputs error.
        ///         
        /// <para />acceptDenny - true - I want this
        /// <para />acceptDenny - false - I dont want this
        /// 
        /// </summary>
        /// <param name="type">Food type.</param>
        /// <param name="acceptDenny">
        ///     'true' - I want this;
        ///     'false' - I dont want this;
        ///     Parameter is stored in the suggestion.Tag directly as 'bool';
        /// </param>
        private void CreateSuggestion(string type, int trackNum)
        {
            if (type == null)
            {
                return;
            }

            if (!activeSuggestions.ContainsKey(type) && trackNum != -1)
            {
                OccupyTrack(trackNum, 2000);

                MyUserControl1 suggestion = new MyUserControl1();

                Canvas.SetLeft(suggestion, trackNum * RenderSize.Width / numOfTracks + 20);
                suggestion.Name = type;
                suggestion.Tag = false; //Set to true if picked
                mainCanvas.Children.Add(suggestion);

                TextBlock foodType = (TextBlock)suggestion.FindName("text");
                foodType.Text = type;

                activeSuggestions.Add(type, suggestion);

                Animate(suggestion);
            }
            else
            {
                Debug.WriteLine("ERR: FOOD TYPE EXISTS");
            }
        }
        
        /// <summary>
        /// Call when animation for travel finishes.
        /// 
        /// ADD MORE LOGIC
        /// </summary>
        private void DoneWithSuggestion(string type)
        {
            if (activeSuggestions.ContainsKey(type))
            {
                mainCanvas.Children.Remove(activeSuggestions[type]);
                activeSuggestions.Remove(type);
            }
        }

        ///HOOK INTO VOICE
        async private void PickSuggestion(string type)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                if(type == "done")
                {
                    this.Frame.Navigate(typeof(results));
                }
                else if (activeSuggestions.ContainsKey(type))
                {
                    FoodWarDecisionEngine.DecisionStorage.AddFood(type);

                    ((Grid)activeSuggestions[type].FindName("grid")).Background = new SolidColorBrush(Windows.UI.Colors.Green);

                    activeSuggestions.Remove(type);
                }
            });
        }

        private async void OccupyTrack(int index, int msDelay)
        {
            trackValidity[index] = false;

            await Task.Delay(msDelay);

            FreeUpTrack(index);
        }

        private void FreeUpTrack(int index)
        {
            trackValidity[index] = true;
        }

        private void Animate(UIElement xElement)
        {
            Random rnd = new Random();
            DoubleAnimation xWoosh = new DoubleAnimation()
            {
                From = -50,
                To = 1200,
                Duration = TimeSpan.FromSeconds(rnd.Next(10, 30))
            };

            xElement.RenderTransform = new CompositeTransform();
            Storyboard xSb = new Storyboard();
            Storyboard.SetTargetProperty(xWoosh, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");

            Storyboard.SetTarget(xWoosh, xElement);

            xSb.Children.Add(xWoosh);
            xSb.Begin();


        }
    }
}
