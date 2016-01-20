using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using FireSharp.Config;
using Newtonsoft.Json.Linq;
using TapTap;
using Android.Support.V4.Widget;
using Android.Support.V7.App;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace TapTapApplication
{
	[Activity (Label = "ActivePastActivity", Theme="@style/MyTheme")]			
	public class ActivePastActivity : ActionBarActivity
	{
		SupportToolbar supportToolbar;
		ListView ordersList;
		FirebaseClient fbClient;
		IFirebaseConfig fbConfig;
		string filter;
		List<Order> orders;

		//HttpWebRequest webRequest;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.ActivePast);

			supportToolbar = this.FindViewById<SupportToolbar> (Resource.Layout.back_menu);
			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);

			fbConfig = new FirebaseConfig {
				AuthSecret = "FO85aksCBndB5fXAykNFQstlLqqYrHsiq4myZTQW", 
				BasePath = "https://scorching-heat-9815.firebaseio.com/"
			};

			fbClient = new FirebaseClient(fbConfig);

			filter = Intent.GetStringExtra ("Query");
		}

		protected override void OnStart() {
			base.OnStart ();
			filter = Intent.GetStringExtra ("Query");
			ordersList = this.FindViewById<ListView> (Resource.Id.lvOrders);

			orders = GetOrders(filter, orders);

			ordersList.Adapter = new OrderAdapter (this, orders);


		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Layout.back_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public List<Order> GetOrders(string orderType, List<Order> orders) {
			//Get the user's Facebook User Id
			//If the app just opens
			//Get all the orders, set limit of 25 for now (they won't have 1000 orders through)
			//Get them according to if active orders or past order are called
			//If the app was already open
			//Get all the orders
			//Update the lists accordingly

			//string facebookId = MainActivity.facebookId;
			orders = new List<Order> ();

			FirebaseResponse response = fbClient.Get ("orders", "orderBy=\"user_id\"&equalTo=\"" + "facebook:10152966033413443" + "\"");
			JObject ordersResponse = response.ResultAs<JObject> ();


			foreach (JProperty tuple in ordersResponse.Properties()) {
				Order order = new Order ();
				string orderStatus = tuple.Value ["order_status"].ToString ();

				if ((orderType == "black") && (orderStatus == "black")) {
					order.Coffee = tuple.Value ["coffee"].ToString ();
					order.Size = tuple.Value ["size"].ToString ();
					order.CafeId = tuple.Value ["cafe_id"].ToString ();
					order.OrderTime = Convert.ToDateTime (GetDate (tuple.Value ["order_time"].ToString ()));
					order.OrderStatus = tuple.Value ["order_status"].ToString ();
					orders.Add (order);
				} else if ((orderType == "not") && orderStatus != "black") {
					order.Coffee = tuple.Value ["coffee"].ToString ();
					order.Size = tuple.Value ["size"].ToString ();
					order.CafeId = tuple.Value ["cafe_id"].ToString ();
					order.OrderTime = Convert.ToDateTime (GetDate (tuple.Value ["order_time"].ToString ()));
					order.OrderStatus = tuple.Value ["order_status"].ToString ();
					orders.Add (order);
				}


			}

			return orders;
		}

		public string GetCafeName(string pastClick, List<Order> orders, string cafeId, int position) {
			FirebaseResponse cafeResponse = fbClient.GetAsync("cafes/" + cafeId).Result;
			JObject jsonResponse = cafeResponse.ResultAs<JObject> ();

			JToken j = jsonResponse.GetValue ("name");

			return j.ToString();
		}

		public DateTime GetDate(string unixTime) {
			return new DateTime (1970, 1, 1, 0, 0, 0).AddSeconds (Convert.ToDouble (unixTime));
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

		/*protected override void OnSaveInstanceState (Bundle outState)
		{
			if (mDrawerLayout.IsDrawerOpen ((int)GravityFlags.Left)) {
				outState.PutString ("DrawerState", "Opened");
			} else {
				outState.PutString ("DrawerState", "Closed");
			}

			base.OnSaveInstanceState (outState);
		}

		protected override void OnPostResume ()
		{
			base.OnPostResume ();
			mToggle.SyncState ();
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			mToggle.SyncState();
		}

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			mToggle.OnConfigurationChanged(newConfig);
		}*/
	}
}

