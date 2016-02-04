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

using FireSharp;
using FireSharp.Response;
using FireSharp.Exceptions;
using Newtonsoft.Json;

using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Newtonsoft.Json.Linq;

using FireSharp.Config;
using System.Threading.Tasks;
using Android.Net;

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
		FirebaseClient fbClient;
		FrameLayout changingFragment;
		List<Order> orders;
		IFirebaseConfig fbConfig;


		FirebaseResponse favourites;
		JObject favouritesJson;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Home);

			supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.action_menu);
			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);
			SupportActionBar.Title = "TapTap Coffee";

			controls = GetDrawerOptions ();
			changingFragment = FindViewById<FrameLayout> (Resource.Id.variable_frlayout);
		}

		protected override void OnStart() {
			base.OnStart ();

			/*if (TempStorage.Favourites == null) {
				//TempStorage.Favourites = new List<Favourite> ();

				//favourites = await fbClient.GetAsync ("users/facebook:" + Profile.CurrentProfile.Id + "/favourites");
				//favouritesJson = favourites.ResultAs<JObject> ();
			} else {
				for (int i = 0; i < TempStorage.Favourites.Count; i++) {
					TempStorage.Favourites.Add (new Favourite 
						{ 
							Coffee = favourite.Value ["coffee"].ToString (), 
							CafeId = favourite.Value["cafe_id"].ToString(),
							Price = favourite.Value ["price"].ToString (), 
							Size = favourite.Value ["size"].ToString ()//, 
							//Milk = favourite.Value["milk"].ToString()
						});
				}
			}*/

			//Serving as a method of checking changes -> favouritesJson = TempStorage.FavouritesJson;
			//I believe Firsharp already has some of this functionality.
		}

		protected override void OnResumeFragments ()
		{
			base.OnResumeFragments ();

			if (TempStorage.Favourites.Count == 0) {
				WelcomeFragment welcomeFragment = new WelcomeFragment ();
				FragmentManager.BeginTransaction ().Replace (Resource.Id.variable_frlayout, welcomeFragment).Commit ();
			} else {
				FavouritesFragment favouritesFragment = new FavouritesFragment (TempStorage.Favourites, new FavouritesAdapter(this, TempStorage.Favourites));
				//Some silly bug happens here...
				FragmentManager.BeginTransaction ().Replace (Resource.Id.variable_frlayout, favouritesFragment).Commit ();
			}
		}

		public void AssessNetwork() {
		}

		public List<string> GetDrawerOptions() {
			return new List<string> { "ACTIVE ORDERS", "PAST ORDERS", "LOGOUT" };
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
			//Cases 0-2: Active Orders, Past Orders, Logout
			//Loyalty tracking will come soon
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
			case 2:
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