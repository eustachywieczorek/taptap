using Android.App;
using Android.Widget;
using Android.OS;

using System.Collections.Generic;

using FireSharp;
using FireSharp.Config;
using FireSharp.Response;

using Newtonsoft.Json.Linq;

namespace TapTapApplication
{
	[Activity (Label = "TapTapApplication", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		List<string> cafes, coffees, sizes;
		FirebaseClient fbClient;

		ExpandableListView coffeeList, cafeList, sizeList;
		Button payButton;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			// Get our button from the layout resource,
			// and attach an event to it

			fbClient = new FirebaseClient (new FirebaseConfig {
				AuthSecret = "FO85aksCBndB5fXAykNFQstlLqqYrHsiq4myZTQW", 
				BasePath = "https://scorching-heat-9815.firebaseio.com/"
			});

			FirebaseResponse fbResponseCoffee = fbClient.Get ("coffee/");
			JObject coffeeObjs = fbResponseCoffee.ResultAs<JObject> ();

			FirebaseResponse fbResponseCafe = fbClient.Get ("cafes/");
			JObject cafeObjs = fbResponseCafe.ResultAs<JObject> ();

			cafes = new List<string> ();
			coffees = new List<string> ();
			sizes = new List<string> ();


			sizes.Add ("Small");
			sizes.Add ("Medium");
			sizes.Add ("Large");

			//property.Name finds each coffee type
			foreach (JProperty property in coffeeObjs.Properties()) {
				coffees.Add (property.Name);
			}

			foreach (JProperty property in cafeObjs.Properties()) {
				JToken hey = property.Value ["name"];
				cafes.Add (hey.ToString ());
			}

			payButton = FindViewById<Button> (Resource.Id.btnPay);
			coffeeList = FindViewById<ExpandableListView> (Resource.Id.lvCoffee);
			coffeeList.SetAdapter(new ExpandableListAdapter(this, coffees, "Coffee"));
			cafeList = FindViewById<ExpandableListView>(Resource.Id.lvCafe);
			cafeList.SetAdapter (new ExpandableListAdapter (this, cafes, "Cafe"));
			sizeList = FindViewById<ExpandableListView> (Resource.Id.lvSize);
			sizeList.SetAdapter(new ExpandableListAdapter(this, sizes, "Size"));
		}
	}
}
