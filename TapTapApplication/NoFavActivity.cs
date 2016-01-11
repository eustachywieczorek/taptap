
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
using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using TapTap;

namespace TapTapApplication
{
	[Activity (Label = "NoFavActivity", Theme="@style/MyTheme")]			
	public class NoFavActivity : ActionBarActivity//, ListView.IOnItemClickListener
	{
		ListView leftDrawerControls;
		List<string> controls;
		LinearLayout mLeftDrawer, body;
		DrawerLayout mDrawerLayout;
		SupportToolbar supportToolbar;
		MyActionBarDrawerToggle mToggle;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.NoFav);

			controls = SetControls ();

			supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.action_menu);
			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mLeftDrawer = FindViewById<LinearLayout> (Resource.Id.left_drawer);
			//mRightDrawer = FindViewById<LinearLayout> (Resource.Id.right_drawer);
			leftDrawerControls = FindViewById<ListView> (Resource.Id.left_drawer_controls);
			body = FindViewById<LinearLayout> (Resource.Id.linPushCustomer);
			leftDrawerControls.Adapter = new LeftDrawerAdapter(this, controls);

			mLeftDrawer.Tag = 0;
			//mRightDrawer.Tag = 1;



			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);

			mDrawerLayout.SetDrawerListener (mToggle);

			mToggle = new MyActionBarDrawerToggle (this, mDrawerLayout, Resource.String.openDrawer, 
				Resource.String.closeDrawer);
			mToggle.SyncState();
		}

		public List<string> SetControls() {
			List<string> controls = new List<string> ();
			controls.Add ("ACTIVE ORDERS");
			controls.Add ("PAST ORDERS");
			controls.Add ("LOYALTY TRACKING");
			controls.Add ("LOGOUT");

			return controls;
		}

		/*public void OnItemClick(AdapterView parent, View view, int position, long id) {
			int selectedItem = parent.SelectedItemPosition;

			//parent.is

			if (parent.ChildCount > 1) {
				if (
					} else if (parent.ChildCount == 1) {

					}

					}

					/*		async void AsyncMe() {

	//		var auth0 = new Auth0Client(
	//			"testappforportfolio.au.auth0.com",
	//			"ht0ONVPDNc1PxHT98ruZFRHzUVH2Q7iO");
	//		var user = await auth0.LoginAsync(this);


	//		Dictionary <string, string> list = new Dictionary<string, string> ();
		
			//FirebaseResponse response = await firebaseClient.GetAsync("c
			//ool/m80/");
			//list = response.ResultAs<Dictionary<string, string>>();
	//		btnPastOrders.Text = list.Count.ToString ();
		}
*/

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
