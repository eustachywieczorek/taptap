using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using TapTap;
//using FireSharp;
//using FireSharp.Config;
//using FireSharp.Response;
using Newtonsoft.Json.Linq;

using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;


using FireSharp;
using FireSharp.Config;
using FireSharp.Response;

using TapTapApplication;
using Android.Graphics.Drawables;

namespace TapTapApplication
{
	[Activity (Label = "OrderActivity", Theme="@style/MyTheme")]
	public class OrderActivity : ActionBarActivity, ExpandableListView.IOnChildClickListener {
		SupportToolbar supportToolbar;
		//Data variables
		FirebaseClient fbClient;
		IFirebaseConfig fbConfig;
		FirebaseResponse response;

		Dictionary<string, List<string>> coffeeData;
		List<string> coffeeDataHeaders;

		Button payButton;

		//Universal variables
		LayoutInflater inflater;
		LinearLayout currentLayout;
		View addedLayout;

		//Choosing cafe

		ListView lvCafes;
		List<Cafe> cafes;

		//Choosing coffee
		List<string> coffee, size, milk;
		ExpandableListView coffeeLv;
		ExpandableListAdapter coffeeAdapter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Order);

			supportToolbar = this.FindViewById<SupportToolbar> (Resource.Layout.order_menu);
			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);

			fbConfig = new FirebaseConfig {
				AuthSecret = "FO85aksCBndB5fXAykNFQstlLqqYrHsiq4myZTQW", 
				BasePath = "https://scorching-heat-9815.firebaseio.com/"
			};

			fbClient = new FirebaseClient(fbConfig);

			//FirebaseResponse response = fbClient.Get ("orders/", "orderBy=\"user_id\"&equalTo=\"facebook:10152966033413443\"");
			//JObject result = response.ResultAs<JObject> ();
		}

		public override bool OnCreateOptionsMenu (IMenu menu) {
			MenuInflater.Inflate (Resource.Layout.order_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					Intent homeActivity = new Intent (this, typeof(HomeActivity));
					NavigateUpTo (homeActivity);
					break;
			}

			return base.OnOptionsItemSelected (item);
		}

		protected override void OnStart() {
			base.OnStart ();
			currentLayout = FindViewById<LinearLayout> (Resource.Id.linAction);

			ShowCafes ();

			lvCafes.ItemClick += delegate {
				ShowCoffees();
			};
		}

		public bool OnChildClick(ExpandableListView parent, View v,
			int groupPosition, int childPosition, long id) {
		
			//Make a second selection, it adds that into the listitem...

			List<string> headers = new List<string>(coffeeData.Keys);

			List<string> defaultHeaders = new List<string> ();
			defaultHeaders.Add ("Coffee");
			defaultHeaders.Add ("Size");
			defaultHeaders.Add ("Milk");

			string currentKey = headers [groupPosition];

			List<string> values = coffeeData [currentKey];

			string fixedKey;
			int cutOff = currentKey.IndexOf (":");
			string newGroupText;

			if (cutOff > 0) {
				fixedKey = currentKey.Substring (0, cutOff);

				newGroupText = string.Empty;
				newGroupText = string.Concat (fixedKey, ": ", 
					coffeeLv.ExpandableListAdapter.GetChild (groupPosition, childPosition).ToString ());
			} else {
				newGroupText = string.Concat(coffeeLv.ExpandableListAdapter.GetGroup(groupPosition).ToString(), ": ", 
					coffeeLv.ExpandableListAdapter.GetChild (groupPosition, childPosition).ToString ());
			}



			headers [groupPosition] = newGroupText;

			coffeeData.Remove (currentKey);
			coffeeData.Add (headers [groupPosition], values);


			coffeeLv.CollapseGroup (groupPosition);
			coffeeAdapter.UpdateData (coffeeData);
			
			return true;
		}

		void ShowCafes() {			
			addedLayout = LayoutInflater.Inflate(Resource.Layout.ChooseCafe, null); 
			lvCafes = addedLayout.FindViewById<ListView> (Resource.Id.cafes);
			currentLayout.AddView (addedLayout);

			cafes = GetCafes ();

			lvCafes.Adapter = new CafeAdapter (this, cafes);
		}

		public void ProcessPayments() {//string coffee, string milk, string size) {
			//sort the input stuff out later
			Intent payActivity = new Intent (this, typeof(PayActivity));
			//payActivity.PutExtra ("Coffee", coffee);
			//payActivity.PutExtra ("Milk", milk);
			//payActivity.PutExtra ("Size", size);
			//There needs to be more data set via the intent e.g. cafe etc.

			StartActivity (payActivity);
		}

		void ShowCoffees() {

			currentLayout.RemoveAllViews ();
			addedLayout = LayoutInflater.Inflate(Resource.Layout.ChooseCoffee, null);  
			currentLayout.AddView (addedLayout);

			coffeeLv = currentLayout.FindViewById<ExpandableListView> (Resource.Id.lvCoffee);
			payButton = currentLayout.FindViewById<Button> (Resource.Id.btnPay);

			//JObject data = await JsonClass.

			size = GetSizes();
			milk = GetMilks();
			coffee = GetCoffees(coffee);

			coffeeData = new Dictionary<string, List<string>> ();
			coffeeData = GetCoffeeData (coffeeData);

			coffeeAdapter = new ExpandableListAdapter (this, coffeeData, coffeeDataHeaders);

			coffeeLv.SetAdapter(coffeeAdapter);

			coffeeLv.SetOnChildClickListener (this);

			payButton.Click += delegate {
				ProcessPayments();
			};
		}
			
		public List<Cafe> GetCafes() {
			List<Cafe> want = new List<Cafe> ();
			FirebaseResponse cafeResponse = fbClient.GetAsync ("cafes/").Result;

			JObject cafes = cafeResponse.ResultAs<JObject> ();

			foreach (JProperty p in cafes.Properties()) {
				Cafe cafe = new Cafe ();
				cafe.CafeName = p.Value ["name"].ToString();
				cafe.Location = "The University of Queensland";
				cafe.Online = "Offline";

				want.Add (cafe);
			}

			return want;
		}

		public Dictionary<string, List<string>> GetCoffeeData(Dictionary<string, List<string>> data) {
			data.Add ("Coffee", coffee);
			data.Add ("Milk", milk);
			data.Add ("Size", size);
			return data;
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

		/*public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				if (mDrawerLayout.IsDrawerOpen (mLeftDrawer)) {
					mDrawerLayout.CloseDrawer (mLeftDrawer);
				} else {
					mDrawerLayout.OpenDrawer (mLeftDrawer);
				}
				return true;
			case Resource.Id.order:
				StartActivity (typeof(OrderActivity));
				return true;
			default:
				return base.OnOptionsItemSelected (item);
			}

			case Resource.Id.fav:
			Intent goBack = new Intent (this, typeof(HomeActivity));
			NavigateUpTo (goBack);
			break;
			case Resource.Id.fav:
			Toast.MakeText (this, Resource.String.app_name, ToastLength.Long);
			break;
		}*/

		/*protected override void OnSaveInstanceState (Bundle outState)
		{
			if (mDrawerLayout.IsDrawerOpen ((int)GravityFlags.Left)) {
				outState.PutString ("DrawerState", "Opened");
			} else {
				outState.PutString ("DrawerState", "Closed");
			}

			base.OnSaveInstanceState (outState);
		}*/



		protected override void OnPostResume ()
		{
			base.OnPostResume ();
		//	mToggle.SyncState ();
		}

		/*protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			mToggle.SyncState();
		}*/

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			//mToggle.OnConfigurationChanged(newConfig);
		}
	}
}