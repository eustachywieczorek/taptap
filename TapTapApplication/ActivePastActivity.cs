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


		private MyActionBarDrawerToggle mToggle;
		private DrawerLayout mDrawerLayout;
		private SupportToolbar supportToolbar;
		private LinearLayout mLeftDrawer;
		public ListView lvControls;
		public Intent nextActivity;

		FirebaseClient fbClient;
		IFirebaseConfig fbConfig;
		ListView lvOrders;
		ListAdapter la;
		List<string> controls;
		List<Order> orders;
		string filter;
		OrderAdapter oa;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.ActivePast);

			controls = new List<string> ();
			controls.Add ("ACTIVE ORDERS");
			controls.Add ("PAST ORDERS");
			controls.Add ("LOYALTY TRACKING");
			controls.Add ("LOGOUT");

			la = new ListAdapter (this, controls);

			fbConfig = new FirebaseConfig {
				AuthSecret = "FO85aksCBndB5fXAykNFQstlLqqYrHsiq4myZTQW", 
				BasePath = "https://scorching-heat-9815.firebaseio.com/"
			};

			fbClient = new FirebaseClient(fbConfig);

			filter = Intent.GetStringExtra ("Query");



			mDrawerLayout = this.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mLeftDrawer = this.FindViewById<LinearLayout> (Resource.Id.left_drawer);

			mToggle = new MyActionBarDrawerToggle (this, mDrawerLayout, Resource.String.openDrawer, 
				Resource.String.closeDrawer);


			mDrawerLayout.SetDrawerListener (mToggle);

			mToggle.SyncState ();

			supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.action_menu);

			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);

			lvControls = FindViewById<ListView> (Resource.Id.left_drawer_controls);
			lvControls.Adapter = la;

			lvControls.ItemClick += LvControls_ItemClick;

			orders = new List<Order> ();
			orders = GetOrders (filter, orders);
			lvOrders = this.FindViewById<ListView> (Resource.Id.lvOrders);
			oa = new OrderAdapter (this, orders);
			lvOrders.Adapter = oa;





		}

		public List<Order> GetOrders(string pastClick, List<Order> orders) {
			FirebaseResponse response = fbClient.GetAsync ("orders/").Result;
			string id;

			JObject jsonResponse = response.ResultAs<JObject> ();

			Order orderObj = new Order();

			orders = new List<Order> ();
			int i = 0;

			foreach (JProperty tuple in jsonResponse.Properties()) {
				//Is it Nishant?

				id = tuple.Value["user_id"].ToString();

				if (id == "facebook:10152966033413443") {
					//This could get ugly if there's an error
					if ((filter == "black") && tuple.Value["order_status"].ToString() == filter) {
						
						orderObj.Coffee = tuple.Value ["coffee"].ToString(); 
						orderObj.Size = tuple.Value ["size"].ToString ();
						orderObj.CafeId = tuple.Value ["cafe_id"].ToString ();
						orderObj.OrderTime = Convert.ToDateTime(GetDate(tuple.Value ["order_time"].ToString()));
						orderObj.OrderStatus = tuple.Value ["order_status"].ToString ();

						orders.Add(orderObj);

						orderObj.Cafe = GetCafeName (filter, orders, orderObj.CafeId, i);
						i++;
					} else if ((filter != "black") && tuple.Value["order_status"].ToString() != "black") {

						orderObj.Coffee = tuple.Value ["coffee"].ToString(); 
						orderObj.Size = tuple.Value ["size"].ToString ();
						orderObj.CafeId = tuple.Value ["cafe_id"].ToString ();
						orderObj.OrderTime = Convert.ToDateTime(GetDate(tuple.Value ["order_time"].ToString()));
						orderObj.OrderStatus = tuple.Value ["order_status"].ToString ();

						orders.Add(orderObj);

						orderObj.Cafe = GetCafeName (filter, orders, orderObj.CafeId, i);
						i++;
					}


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

		void LvControls_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			int clickedItem = e.Position;

			switch(clickedItem) {
			case 0:
				nextActivity = new Intent (this, typeof(ActivePastActivity));
				nextActivity.PutExtra ("Query", "black");
				StartActivity (nextActivity);
				break;
			case 1:
				nextActivity = new Intent (this, typeof(ActivePastActivity));
				nextActivity.PutExtra ("Query", "not");
				StartActivity (nextActivity);
				break;
			case 2:
				//Loyalty Tracking
				break;
			case 3:
				//Logout
				break;
			}
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
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
		}

		protected override void OnSaveInstanceState (Bundle outState)
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

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Layout.action_menu, menu);
			return base.OnCreateOptionsMenu (menu);
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
		}
	}
}

