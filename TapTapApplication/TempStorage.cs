using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using FireSharp;
using FireSharp.Response;
using FireSharp.Config;

using TapTap;
using FireSharp.Interfaces;

namespace TapTapApplication
{
	public static class TempStorage
	{
		
		private static IFirebaseConfig fbConfig;
		public static IFirebaseClient FbClient;

		public static void SetupDb() {
			fbConfig = new FirebaseConfig {
				AuthSecret = "FO85aksCBndB5fXAykNFQstlLqqYrHsiq4myZTQW", 
				BasePath = "https://scorching-heat-9815.firebaseio.com/"
			};

			FbClient = new FirebaseClient (fbConfig);
		}
	
		public static string ProfileId { get; set; }
		public static List<Order> Orders { get; set; }
		public static List<string> Sizes { get; set; }
		public static List<string> Milks { get; set; }
		public static List<Cafe> Cafes { get; set; }
		public static List<Favourite> Favourites { get; set; }
		public static List<Coffee> Coffees {get;set;}

		public static int[] DrawableLogos {get;set;}
		public static int ChosenLogo {get;set;}

		//During the order process

		public static string CafeId { get; set; }
		public static string Cafe { get; set; }
		public static string Coffee { get; set; }
		public static string Price { get; set; }
		public static string Size { get; set; }
		public static string Milk { get; set; }

		public static string PublishableKey = "pk_test_yYppP7KoXweDH8hy6hUCacCK";

		public static void GetOrders() {
			FirebaseResponse response = FbClient.Get ("orders", string.Concat("orderBy=\"user_id\"&equalTo=\"", ProfileId, "\""));
			JObject ordersJson = response.ResultAs<JObject> ();

			TempStorage.Orders = new List<Order>();

			if (ordersJson != null) {
				//Something's going on here
				foreach (JProperty doc in ordersJson.Properties()) {
					TempStorage.Orders.Add (new Order {
						CafeId = doc.Value ["cafe_id"].ToString (),
						Coffee = doc.Value ["coffee"].ToString (), 
						OrderStatus = doc.Value ["order_status"].ToString (), 
						OrderTime = GetDate (doc.Value ["order_time"].ToString ()), 
						Size = doc.Value ["size"].ToString (), 
						Price = doc.Value["price"].ToString()
					});
				}
			}
		}

		public static double GetPrepTime(string cafeId) {
			for (int i = 0; i < TempStorage.Cafes.Count; i++) {
				if (TempStorage.Cafes[i].CafeId == cafeId) {
					return double.Parse(TempStorage.Cafes [i].PrepTime);
				}
			}

			return -1.0;
		}

		public static void GetCoffees() {
			TempStorage.Coffees = new List<Coffee> ();

			FirebaseResponse coffeeResponse = TempStorage.FbClient.Get ("coffee/");

			JObject coffeeJson = coffeeResponse.ResultAs<JObject> ();

			foreach (JProperty property in coffeeJson.Properties()) {
				TempStorage.Coffees.Add (new Coffee {
					Price = property.Value["price"].ToString(), 
					CoffeeType = property.Name
				});
			}
		}

		public static string GetCafeId(string cafe) {
			for (int i = 0; i < TempStorage.Cafes.Count; i++) {
				if (cafe == TempStorage.Cafes [i].CafeName) {
					return TempStorage.Cafes [i].CafeId;
				}
			}

			return cafe;
		}

		public static void GetSizes() {
			Sizes = new List<string> { "Small", "Regular", "Large" };
		}

		public static void GetMilks() {
			Milks = new List<string> { "This", "That", "Another" };
		}

		public static void GetCafes() {
			FirebaseResponse response = FbClient.Get ("cafes");
			JObject cafesJson = response.ResultAs<JObject> ();

			TempStorage.Cafes = new List<Cafe>();
			//don't forget location, this will vary
			foreach (JProperty doc in cafesJson.Properties()) {
				TempStorage.Cafes.Add (new Cafe {
					CafeId = doc.Name,
					CafeName = doc.Value ["name"].ToString (), 
					PrepTime = doc.Value ["prep_time"].ToString ()
					//Image Address coming soon!
				});
			}
		}

		public static string GetPrice(string coffee) {
			for (int i = 0; i < TempStorage.Coffees.Count; i++) {
				if (coffee == TempStorage.Coffees [i].CoffeeType) {
					return TempStorage.Coffees [i].Price;
				}
			}

			return coffee;
		}

		public static void GetFavourites() {
			FirebaseResponse response = FbClient.Get (string.Concat("users/", ProfileId, "/favourites"));
			JObject favouritesJson = response.ResultAs<JObject> ();

			TempStorage.Favourites = new List<Favourite>();

			if (favouritesJson != null) {
				foreach (JProperty doc in favouritesJson.Properties()) {
					TempStorage.Favourites.Add (new Favourite {
						CafeId = doc.Value ["cafe_id"].ToString (),
						Coffee = doc.Value ["coffee"].ToString (), 
						Price = doc.Value ["price"].ToString (), 
						Size = doc.Value ["size"].ToString (), 
						Milk = doc.Value["milk"].ToString()
					});
				}
			}
		}

		public static DateTime GetDate(string unixTime) {
			return new DateTime (1970, 1, 1, 0, 0, 0).AddSeconds (Convert.ToDouble (unixTime));
		}
	}
}

