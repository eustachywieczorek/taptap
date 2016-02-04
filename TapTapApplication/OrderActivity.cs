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
	public class OrderActivity : ActionBarActivity {
		SupportToolbar supportToolbar;
		//Data variables

		string selectedCoffee, selectedMilk, selectedSize;

		bool favChecked = false;

		public static Dictionary<string, List<string>> coffeeData;
		List<string> coffeeDataHeaders;
		List<string> headers;
		Button payButton;

		//Universal variables
		LayoutInflater inflater;
		FrameLayout currentLayout;
		View addedLayout;

		//check to see the types on the other pages.
		Android.Support.V7.App.AlertDialog.Builder orderDialog;



		IMenuItem favourite;

		//Choosing cafe
		public static ExpandableListAdapter coffeeAdapter;
		ListView lvCafes;
		List<Cafe> cafes;
		CafeFragment cafeFragment;
		public static bool CoffeeFragPresented { get; set; }

		//Choosing coffee
		//List<string> coffee, size, milk;
		//ExpandableListView coffeeLv;
		//ExpandableListAdapter coffeeAdapter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Order);

			supportToolbar = this.FindViewById<SupportToolbar> (Resource.Layout.order_menu);
			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);
			SupportActionBar.Title = "TapTap Coffee";

			//make sure you have a dismiss button or something for devices
			orderDialog = new Android.Support.V7.App.AlertDialog.Builder (this);
			orderDialog.SetTitle ("Woah There!");
			orderDialog.SetMessage ("You forgot to fill in every field.");
			orderDialog.SetPositiveButton ("Thanks!", delegate {
				
			});

			//Implement  variable pricing...

			//coffeeData = new Dictionary<string, List<string>> ();
			//GetCoffeeData ();
			//coffeeDataHeaders = new List<string> (coffeeData.Keys);

			//coffeeAdapter = new ExpandableListAdapter (this, coffeeData, coffeeDataHeaders);

			cafeFragment = new CafeFragment (TempStorage.Cafes, new CafeAdapter (this, TempStorage.Cafes));
			FragmentManager.BeginTransaction ().Replace (Resource.Id.frame_action, cafeFragment).Commit ();

		}

		/*public override bool OnCreateOptionsMenu (IMenu menu) {
			MenuInflater.Inflate (Resource.Layout.order_menu, menu);
			favourite = menu.FindItem (Resource.Id.fav);

			favourite.SetVisible (false);

			return base.OnCreateOptionsMenu (menu);
		}*/
	
		protected override void OnStart() {
			base.OnStart ();
		}

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