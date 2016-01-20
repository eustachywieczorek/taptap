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

using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace TapTapApplication
{
	[Activity (Label = "HomeActivity", Theme="@style/MyTheme")]			
	public class HomeActivity : ActionBarActivity
	{
		SupportToolbar supportToolbar;
		MyActionBarDrawerToggle mToggle;
		DrawerLayout mDrawerLayout;
		LinearLayout mLeftDrawer;
		ListView lvControls;
		List<string> controls;
		Intent nextActivity;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Home);

			supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.action_menu);
			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);

			controls = GetDrawerOptions ();
		}

		public List<string> GetDrawerOptions() {
			List<string> controlsTemp = new List<string> ();
			controlsTemp.Add ("ACTIVE ORDERS");
			controlsTemp.Add ("PAST ORDERS");
			controlsTemp.Add ("LOYALTY TRACKING");
			controlsTemp.Add ("LOGOUT");

			return controlsTemp;
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Layout.action_menu, menu);

			mDrawerLayout = this.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mLeftDrawer = this.FindViewById<LinearLayout> (Resource.Id.left_drawer);
			lvControls = FindViewById<ListView> (Resource.Id.left_drawer_controls);
			mToggle = new MyActionBarDrawerToggle (this, mDrawerLayout, Resource.String.openDrawer, Resource.String.closeDrawer);
			lvControls.Adapter = new ListAdapter(this, GetDrawerOptions());
			lvControls.ItemClick += LvControls_ItemClick;

			mDrawerLayout.SetDrawerListener (mToggle);
			mToggle.SyncState ();

			return base.OnCreateOptionsMenu (menu);
		}

		public void LvControls_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			switch (e.Position) {
			case 0:
				nextActivity = new Intent (this, typeof(ActivePastActivity));
				nextActivity.PutExtra ("Query", "not");
				StartActivity (nextActivity);
				break;
			case 1:
				nextActivity = new Intent (this, typeof(ActivePastActivity));
				nextActivity.PutExtra ("Query", "black");
				StartActivity (nextActivity);
				break;

			case 3:
				LoginManager.Instance.LogOut ();
				nextActivity = new Intent (this, typeof(MainActivity));
				StartActivity (nextActivity);
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
				break;
			case Resource.Id.order:
				StartActivity (typeof(OrderActivity));
				break;
			}

			return base.OnOptionsItemSelected (item);
		}
	}
}