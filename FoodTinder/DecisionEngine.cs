using System;
using System.Collections.Generic;
using System.IO;
using Windows.Data.Json;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FoodWarDecisionEngine
{
    class Tag
    {
        public int ID; // 1
        public string name; // pizza
    }

    class Restaurant
    {
        public string name; // pizza express
        public List<Tag> tags = new List<Tag>(); // (1,pizza), (2,burgers)
        public string rating; // whatever
    }

    class DecisionStorage
    {
        public static Action LoadFinished;

        public static Dictionary<string, int> allPossibleFoods = new Dictionary<string, int>();
        static List<string> linesToSave = new List<string>();
        static string fileName = @"C:\Users\Public\TestFolder\LocationsJson.txt";
        static List<Restaurant> allRestaurants = new List<Restaurant>();
        static List<Tag> allTags = new List<Tag>();

        public static void AddFood(string newFood)
        {
            // if the food has NOT already been shouted, add a new entry
            if (!allPossibleFoods.ContainsKey(newFood))
            {
                allPossibleFoods.Add(newFood, 1);
            }
            else
            {
                allPossibleFoods[newFood]++;
            }
        }

        // this assumes a "C:\Users\Public\TestFolder" folder on the machine
        public static void SaveToFile(Dictionary<string, int> allFoods)
        {
            foreach (var foodType in allFoods)
            {
                string foodAndValue = foodType.Key + "\t" + foodType.Value;
                linesToSave.Add(foodAndValue);
            }

            if (File.Exists(fileName))
            {
                // remove contents before writing new info
                string text = File.ReadAllText(fileName);
                text = text.Replace(text, "");
            }

            System.IO.File.WriteAllLines(fileName, linesToSave);
        }

        public static List<string> GetListOfAllFoods()
        {
            List<string> allFoodStrings = new List<string>();

            if(allTags.Count > 0)
            {
                allFoodStrings.Clear();
                foreach(Tag currentTag in allTags)
                {
                    allFoodStrings.Add(currentTag.name);
                    // if the food has NOT already been shouted, add a new entry
                    if (!allPossibleFoods.ContainsKey(currentTag.name))
                    {
                        allPossibleFoods.Add(currentTag.name, 0);
                    }
                }
            }

            return allFoodStrings;
        }

        public static List<Tag> GetListOfAllTags()
        {
            List<Tag> allFoodTags = new List<Tag>();

            if (allTags.Count > 0)
            {
                foreach (Tag currentTag in allTags)
                {
                    allFoodTags.Add(currentTag);
                }
            }

            return allFoodTags;
        }

        public static List<Restaurant> GetListOfAllRestaurants()
        {
            List<Restaurant> allRestaurantsList = new List<Restaurant>();

            if (allRestaurants.Count > 0)
            {
                foreach (Restaurant currentRestaurant in allRestaurants)
                {
                    allRestaurantsList.Add(currentRestaurant);
                }
            }

            return allRestaurantsList;
        }

        public static async void Deserialize(string JsonFileName)
        {
            // read the file
            string JsonData = "";

            //var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/" + JsonFileName));
            //using (var inputStream = await file.OpenReadAsync())
            //using (var classicStream = inputStream.AsStreamForRead())
            //using (var streamReader = new StreamReader(classicStream))
            //{
            //    while (streamReader.Peek() >= 0)
            //    {
            //        JsonData = streamReader.ReadToEnd();
            //    }

            //    // read file, start loading
            //    if (LoadFinished != null)
            //    { 
            //        LoadFinished();
            //    }

            //}

            JsonData = System.IO.File.ReadAllText("Assets\\Data.txt");

            // save string as JSON
            JsonObject fileDataAsJsonObj = JsonObject.Parse(JsonData);

            // deserialize and save into object array
            JsonArray restaurantsArray = fileDataAsJsonObj.GetNamedArray("allLocations");

            // get all tag IDs
            JsonArray IDsArray = fileDataAsJsonObj.GetNamedArray("tags");

            allTags.Clear();

            foreach (JsonValue IDJsonVal in IDsArray)
            {
                JsonObject IDJsonObj = IDJsonVal.GetObject();
                Tag newTag = new Tag();
                newTag.ID = (int)IDJsonObj.GetNamedNumber("id");
                newTag.name = IDsArray.GetObjectAt((uint)newTag.ID - 1).GetNamedString("name");

                allTags.Add(newTag);
            }

            LoadFinished();

            foreach (JsonValue restaurantJsonVal in restaurantsArray)
            {
                JsonObject restaurantJsonObj = restaurantJsonVal.GetObject();
                Restaurant newRestaurant = new Restaurant();
                newRestaurant.name = restaurantJsonObj.GetNamedValue("name").GetString();

                JsonArray tagsArray = restaurantJsonObj.GetNamedArray("tags");

                for (int i = 0; i < tagsArray.Count; ++i)
                {
                    Tag newTag = new Tag();
                    newTag.ID = (int)tagsArray.GetNumberAt((uint)i);
                    newTag.name = IDsArray.GetObjectAt((uint)newTag.ID - 1).GetNamedString("name");

                    newRestaurant.tags.Add(newTag);
                }

                newRestaurant.rating = restaurantJsonObj.GetNamedValue("rating").GetString();
                allRestaurants.Add(newRestaurant);
            }
        }
    }

    /*
    string testJSON = "{\"updated_at\":1405482225,\"settings\":{\"conception_mode\":1,\"app_lock_string\":\"\",\"user_cycle_length\":21,\"week_starts_monday\":1}}";

            // readable format
            //{
            //    "allLocations":
            //    [
            //        {
            //            "name":"Shakespeare's Pub",
            //            "tags":
            //            [
            //                1, 2, 3, 4
            //            ],
            //            "rating": "3.49"
            //        }
            //    ],
            //    "tags":
            //    [
            //        {
            //            "id": 1,
            //            "name":"burger"
            //        },
            //        {
            //            "id": 2,
            //            "name":"pizza"
            //        },
            //        {
            //            "id": 3,
            //            "name":"sunday roast"
            //        },
            //        {
            //            "id": 4,
            //            "name":"hot dog"
            //        }
            //    ]
            //}

            string testJSON2 = "{\"allLocations\":[{\"name\":\"Shakespeare's Pub\",\"tags\":[1, 2, 3, 4],\"rating\": \"3.49\"},{\"name\":\"Other Pub\",\"tags\":[3, 2, 1, 4],\"rating\": \"5.1\"}],\"tags\":[{\"id\": 1,\"name\":\"burger\"},{\"id\": 2,\"name\":\"pizza\"},{\"id\": 3,\"name\":\"sunday roast\"},{\"id\": 4,\"name\":\"hot dog\"}]}";

            string testFilename = "LocationsJson.txt";
            string fullLocationsList = "Data.txt";

            DecisionStorage.Deserialize(fullLocationsList);
    */
}
