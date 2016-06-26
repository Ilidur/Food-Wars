using System;
using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
//using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FoodTinder
{
    struct FoodItem
    {
        public string name;
        public int points;
        public int ID;

        public FoodItem(string _name, int _points, int _ID)
        {
            name = _name;
            points = _points;
            ID = _ID;

        }

        public void AddPoints()
        {
            points++;
        }
    }


    class Points
    {
        static List<FoodWarDecisionEngine.Restaurant> allRestruants;
        static List<FoodItem> allFoodItems;
        static List<FoodItem> topFoodItems;

        public Points()
        {
            List<FoodWarDecisionEngine.Tag> foodItems = FoodWarDecisionEngine.DecisionStorage.GetListOfAllTags();
            allFoodItems = new List<FoodItem>();
            foreach (FoodWarDecisionEngine.Tag currentFoodItem in foodItems)
            {
                allFoodItems.Add(new FoodItem(currentFoodItem.name, 0, currentFoodItem.ID));
            }

            allRestruants = FoodWarDecisionEngine.DecisionStorage.GetListOfAllRestaurants();
        }

        public void addPointsToFoodItem(string foodName)
        {
            int index = 0;
            foreach (FoodItem currentFoodItem in allFoodItems)
            {
                if (currentFoodItem.name == foodName)
                {
                    break;
                }
                else
                {
                    index++;
                }
            }
            allFoodItems[index].AddPoints();
        }

        public List<FoodItem> getTopXFoodItems(int x)
        {
            topFoodItems = new List<FoodItem>();
            for (int i = 0; i < x; i++)
            {
                int highestPoints = 0;
                FoodItem currentBestFoodItem = new FoodItem();
                foreach (FoodItem currentFood in allFoodItems)
                {
                    if (highestPoints < currentFood.points)
                    {
                        currentBestFoodItem = currentFood;
                        highestPoints = currentFood.points;
                    }
                }
                if (currentBestFoodItem.points > 0)
                    topFoodItems.Add(currentBestFoodItem);
            }
            return topFoodItems;
        }

        public List<FoodWarDecisionEngine.Restaurant> getAllRecommendedRestaurants()
        {
            List<FoodWarDecisionEngine.Restaurant> RecommendedRestaurants = new List<FoodWarDecisionEngine.Restaurant>();
            HashSet<int> recommendedIDs = new HashSet<int>();
            foreach (FoodItem currentFoodItem in topFoodItems)
            {
                recommendedIDs.Add(currentFoodItem.ID);
            }

            foreach (FoodWarDecisionEngine.Restaurant currentRestaurant in allRestruants)
            {
                foreach (FoodWarDecisionEngine.Tag currentTag in currentRestaurant.tags)
                {
                    if (recommendedIDs.Contains(currentTag.ID))
                    {
                        RecommendedRestaurants.Add(currentRestaurant);
                        break;
                    }
                }
            }
            return RecommendedRestaurants;
        }

};

}