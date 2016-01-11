using Android.App;
using Android.Widget;
using Android.OS;

using System.Collections.Generic;

using FireSharp;
using FireSharp.Config;
using FireSharp.Response;

using Newtonsoft.Json.Linq;
using Android.Content;

using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

using TapTap;

namespace TapTapApplication
{
	[Activity (Label = "OrderActivity", Theme="@style/MyTheme")]
	public class OrderActivity : ActionBarActivity
	{
		LinearLayout mLeftDrawer, body;
		DrawerLayout mDrawerLayout;
		SupportToolbar supportToolbar;
		MyActionBarDrawerToggle mToggle;
		List<string> cafes, coffees, sizes;
		FirebaseClient fbClient;

		ListView leftDrawerControls;
		List<string> controls;
		ExpandableListView coffeeList, cafeList, sizeList;
		Button payButton;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.OrderActivity);
			// Get our button from the layout resource,
			// and attach an event to it

			controls = SetControls ();

			supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.action_menu);
			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mLeftDrawer = FindViewById<LinearLayout> (Resource.Id.left_drawer);
			mLeftDrawer.Tag = 0;

			leftDrawerControls = FindViewById<ListView> (Resource.Id.left_drawer_controls);
			body = FindViewById<LinearLayout> (Resource.Id.linPushCustomer);
			leftDrawerControls.Adapter = new LeftDrawerAdapter(this, controls);





			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);

			mToggle = new MyActionBarDrawerToggle (this, mDrawerLayout, Resource.String.openDrawer, 
				Resource.String.closeDrawer);
			mToggle.SyncState();

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

			payButton.Click += delegate {
				StartActivity(typeof(PayActivity));
			};

			coffeeList = FindViewById<ExpandableListView> (Resource.Id.lvCoffee);
			coffeeList.SetAdapter(new ExpandableListAdapter(this, coffees, "Coffee"));
			cafeList = FindViewById<ExpandableListView>(Resource.Id.lvCafe);
			cafeList.SetAdapter (new ExpandableListAdapter (this, cafes, "Cafe"));
			sizeList = FindViewById<ExpandableListView> (Resource.Id.lvSize);
			sizeList.SetAdapter(new ExpandableListAdapter(this, sizes, "Size"));

			coffeeList.ExpandGroup (0);
			cafeList.ExpandGroup (0);
			sizeList.ExpandGroup (0);
		
			/* This should worK??? 
			 * coffeeList.SetScrollContainer (false);
			cafeList.SetScrollContainer (false);
			sizeList.SetScrollContainer (false);*/

			coffeeList.ItemClick += List_ItemClick;
			cafeList.ItemClick += List_ItemClick;
			sizeList.ItemClick += List_ItemClick;
		}

		public List<string> SetControls() {
			List<string> controls = new List<string> ();
			controls.Add ("ACTIVE ORDERS");
			controls.Add ("PAST ORDERS");
			controls.Add ("LOYALTY TRACKING");
			controls.Add ("LOGOUT");

			return controls;
		}

		public void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e) {

			ExpandableListView t = (ExpandableListView)e.View;
				ExpandableListAdapter f = (ExpandableListAdapter)t.Adapter;
				string hey = string.Concat (f.ListName, ": ", f.DataList [e.Position]);

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