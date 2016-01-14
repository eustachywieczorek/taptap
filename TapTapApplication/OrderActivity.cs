using Android.App;
using Android.Widget;
using Android.OS;

using System.Collections.Generic;

using FireSharp;
using FireSharp.Config;
using FireSharp.Response;
using System;
using Newtonsoft.Json.Linq;
using Android.Content;

using Android.Views;
using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

using TapTapApplication;
using Android.Graphics.Drawables;

namespace TapTapApplication
{
	[Activity (Label = "OrderActivity", Theme="@style/MyTheme")]
	public class OrderActivity : ActionBarActivity {
		//Data variables
		FirebaseClient fbClient;
		IFirebaseConfig fbConfig;
		FirebaseResponse response;

		//Universal variables
		LayoutInflater inflater;
		LinearLayout testLayout;
		View addedLayout;

		//Choosing cafe
		ImageView imgTest;

		//Choosing coffee
		List<string> coffee, size, milk;
		ExpandableListView coffeeLv, sizeLv, milkLv;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Order);
		
			fbConfig = new FirebaseConfig {
				AuthSecret = "FO85aksCBndB5fXAykNFQstlLqqYrHsiq4myZTQW", 
				BasePath = "https://scorching-heat-9815.firebaseio.com/"
			};

			fbClient = new FirebaseClient (fbConfig);

			testLayout = FindViewById<LinearLayout> (Resource.Id.linAction);
			addedLayout = LayoutInflater.Inflate(Resource.Layout.ChooseCafe, null);  
			testLayout.AddView (addedLayout);

			imgTest = FindViewById<ImageView> (Resource.Id.imgRekty);
			imgTest.Click += delegate {
				ChooseCoffee();

				ExpandableListAdapter sizeAdapter = new ExpandableListAdapter (this, size, "Size");
				ExpandableListAdapter milkAdapter = new ExpandableListAdapter (this, milk, "Milk");
				ExpandableListAdapter coffeeAdapter = new ExpandableListAdapter (this, coffee, "Coffee");


			};


		}
			

		public void ChooseCoffee() {
			testLayout.RemoveView (addedLayout);
			addedLayout = LayoutInflater.Inflate(Resource.Layout.ChooseCoffee, null);
			testLayout.AddView (addedLayout);


			coffeeLv = FindViewById<ExpandableListView> (Resource.Id.lvCoffee);
			sizeLv = FindViewById<ExpandableListView> (Resource.Id.lvSize);
			milkLv = FindViewById<ExpandableListView> (Resource.Id.lvMilk);

			size = GetSizes();
			milk = GetMilks();
			coffee = GetCoffees(coffee);

			coffeeLv.SetAdapter(new ExpandableListAdapter(this, coffee, "Coffee"));
			sizeLv.SetAdapter(new ExpandableListAdapter (this, size, "Size"));
			milkLv.SetAdapter(new ExpandableListAdapter (this, milk, "Milk"));

			coffeeLv.ExpandGroup (0);





			//Drawable indicator = Drawable.CreateFromPath("@drawable/logo");
			//int indicatorWidth = indicator.Bounds.Width();

			//coffeeLv.SetIndicatorBoundsRelative(coffeeLv.Width - indicatorWidth, coffeeLv.Width);
			//sizeLv.SetIndicatorBoundsRelative(sizeLv.Width - indicatorWidth, sizeLv.Width);
			//milkLv.SetIndicatorBoundsRelative(milkLv.Width - indicatorWidth, milkLv.Width);
		}
			
		public List<string> GetSizes() {
			//Might need firebase call at one stage
			List<string> sizes = new List<string> ();
			sizes.Add ("Small");
			sizes.Add ("Medium");
			sizes.Add ("Large");
			return sizes;
		}

		public List<string> GetMilks() {
			List<string> milks = new List<string> ();
			milks.Add ("Skim");
			milks.Add ("Soy");
			milks.Add ("Full Cream");
			milks.Add ("Another type");
			return milks;
		}

		public List<string> GetCoffees(List<string> coffee) {
			FirebaseResponse coffeeResponse = fbClient.GetAsync ("coffee/").Result;
			JObject coffeeJson = coffeeResponse.ResultAs<JObject> ();
			coffee = new List<string> ();

			foreach (JProperty property in coffeeJson.Properties()) {
				coffee.Add(property.Name);
			}

			return coffee;
		}
	}
}