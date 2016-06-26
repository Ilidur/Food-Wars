﻿///ADD METHOD FOR COORDINATE TRANSLATION
///
///

using System;
using System.Diagnostics;
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
        private Dictionary<string, bool> suggestionTracker;
        private Dictionary<string, Grid> activeSuggestions;
        private Dictionary<string, Grid> pickedSuggestions;

        public SuggestionPage()
        {
            this.InitializeComponent();

            //Timer stuff
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0,0,timerSpan);
            timer.Tick += TimerTick;
            timer.Start();

            //Main canvas area
            //Dimensions should changed depending on situation.
            mainCanvas = new Canvas();
            //mainCanvas.Width = RenderSize.Width;
            //mainCanvas.Height = RenderSize.Height;
            this.Content = mainCanvas;

            //Suggestions manager stuff
            activeSuggestions = new Dictionary<string, Grid>();
            suggestionTracker = new Dictionary<string, bool>();

            suggestionTracker.Add("Chips", true);
            suggestionTracker.Add("Fish", true);
            suggestionTracker.Add("Burger", true); //Populate propper

            listOfFoodTypes = new List<string>(suggestionTracker.Keys);
        }

        //ADD SPAWNER PROPER
        private void TimerTick(object sender, object e)
        {
            Random rand = new Random();
            CreateSuggestion(PickRandomType(), rand.Next(300));
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
        private void CreateSuggestion(string type, float horizValue)
        {
            if(type == null)
            {
                return;
            }

            if (!activeSuggestions.ContainsKey(type))
            {
                bool tempTag = GetTypeAcceptDeny(type);

                Grid suggestion = new Grid();
                suggestion.Width = 100f;
                suggestion.Height = 100f;
                if(tempTag)
                {
                    suggestion.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                }
                else
                {
                    suggestion.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                }
                Canvas.SetTop(suggestion, 100);
                Canvas.SetLeft(suggestion, horizValue);
                suggestion.Name = type;
                suggestion.Tag = tempTag;
                mainCanvas.Children.Add(suggestion);

                TextBlock foodType = new TextBlock();
                foodType.Text = type;
                foodType.VerticalAlignment = VerticalAlignment.Bottom;
                foodType.HorizontalAlignment = HorizontalAlignment.Center;
                suggestion.Children.Add(foodType);

                activeSuggestions.Add(type, suggestion);
            }
            else
            {
                Debug.WriteLine("ERR: FOOD TYPE EXISTS");
            }
        }
        
        /// <summary>
        /// Changes the accept deny value of the suggestion type.
        /// </summary>
        /// <param name="type">Type to change for</param>
        /// <returns>The new type</returns>
        private bool GetTypeAcceptDeny(string type)
        {
                suggestionTracker[type] = !suggestionTracker[type];
                return suggestionTracker[type];
        }

        /// <summary>
        /// Call when animation for travel finishes.
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
        private void PickSuggestion(string type)
        {
            if (activeSuggestions.ContainsKey(type))
            {
                pickedSuggestions.Add(type, activeSuggestions[type]);

                //MOVE TO POSITION ADDITIONAL LOGIC

                activeSuggestions.Remove(type);
            }
        }

    }
}
