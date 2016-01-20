using Android.App;
using Android.Widget;
using Android.OS;

using System.Collections.Generic;

using FireSharp;
using FireSharp.Config;
using FireSharp.Response;

using Newtonsoft.Json.Linq;
using Android.Content;

using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;

namespace TapTapApplication
{
	[Activity (Label = "PayActivity", Theme="@style/MyTheme")]
	public class PayActivity : ActionBarActivity
	{
		List<string> cafes, coffees, sizes;
		FirebaseClient fbClient;

		ExpandableListView coffeeList, cafeList, sizeList;
		Button payButton;

		SupportToolbar supportToolbar;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Pay);
			// Get our button from the layout resource,
			// and attach an event to it

			supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.order_menu);
			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);


		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				Intent orderActivity = new Intent (this, typeof(OrderActivity));
				NavigateUpTo (orderActivity);
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Layout.order_menu, menu);
			return true;
		}
	}
}

