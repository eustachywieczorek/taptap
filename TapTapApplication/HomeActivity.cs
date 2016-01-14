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
	public class HomeActivity : ActionBarActivity
	{

		private MyActionBarDrawerToggle mToggle;
		private DrawerLayout mDrawerLayout;
		private SupportToolbar supportToolbar;
		private LinearLayout mLeftDrawer;
		public ListView lvControls;
		private List<string> controls;
		public Intent nextActivity;

		LinearLayout testLayout;
		View addedLayout;
		//LayoutInflater inflater;
		ListAdapter la;

		//NoFavourite specific
		Button craftOne;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Home);

			controls = new List<string> ();
			controls.Add ("ACTIVE ORDERS");
			controls.Add ("PAST ORDERS");
			controls.Add ("LOYALTY TRACKING");
			controls.Add ("LOGOUT");

			la = new ListAdapter (this, controls);

			testLayout = FindViewById<LinearLayout> (Resource.Id.linHomeAction);

			//NB: First, check database for favourites or if new user
			addedLayout = LayoutInflater.Inflate(Resource.Layout.NoFavourite, null);  
			testLayout.AddView (addedLayout);

			supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.action_menu);

			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);

			mDrawerLayout = this.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mLeftDrawer = this.FindViewById<LinearLayout> (Resource.Id.left_drawer);

			mToggle = new MyActionBarDrawerToggle (this, mDrawerLayout, Resource.String.openDrawer, 
				Resource.String.closeDrawer);


			mDrawerLayout.SetDrawerListener (mToggle);

			mToggle.SyncState ();

			lvControls = FindViewById<ListView> (Resource.Id.left_drawer_controls);
			lvControls.Adapter = la;

			lvControls.ItemClick += LvControls_ItemClick;



			craftOne = FindViewById<Button> (Resource.Id.btnCraft);
			craftOne.Click += delegate {
				OrderNow();
			};
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

		public void OrderNow() {
			Intent intent = new Intent (this, typeof(OrderActivity));

			StartActivity (typeof(OrderActivity));
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
