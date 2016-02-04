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

using Xamarin.Facebook;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using System.Threading.Tasks;
using System.Threading;

namespace TapTapApplication
{
	[Activity (Label = "ActivePastActivity", Theme="@style/MyTheme")]			
	public class ActivePastActivity : ActionBarActivity
	{
		SupportToolbar supportToolbar;

		FirebaseClient fbClient;
		IFirebaseConfig fbConfig;
		string filter;

		Android.Support.V7.App.AlertDialog.Builder confirmedDialog;

		FirebaseResponse response;

		JObject ordersJson;

		List<Order> orders;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.ActivePast);

			supportToolbar = this.FindViewById<SupportToolbar> (Resource.Layout.back_menu);
			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);
			SupportActionBar.Title = "TapTap Coffee";

			filter = Intent.GetStringExtra ("Query");


			confirmedDialog = new Android.Support.V7.App.AlertDialog.Builder (this);
			confirmedDialog.SetTitle ("Order Received");
			confirmedDialog.SetMessage ("Your order was received and paid!");
			confirmedDialog.SetPositiveButton ("Great!", delegate {
				
			});

			orders = GetOrders (Intent.GetStringExtra ("Query"));

			if (orders.Count > 0) {
				//frame layout slow
				ActivePastFragment activePast = new ActivePastFragment (orders, new OrderAdapter (this, orders));
				FragmentManager.BeginTransaction ().Replace (Resource.Id.variable_frlayout, activePast).Commit ();
			} else {
				//framelayout slow
				NoOrdersFragment noOrders = new NoOrdersFragment ();
				FragmentManager.BeginTransaction ().Replace (Resource.Id.variable_frlayout, noOrders).Commit ();
			}

			if (Intent.GetStringExtra ("Past") == "pay") {
				confirmedDialog.Show ();
			}
		}

		protected override void OnStart() {
			base.OnStart ();
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Layout.back_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		public List<Order> GetOrders(string orderType) {

			List<Order> orders = new List<Order>();
			List<Order> yellows, greens;

			if (TempStorage.Orders.Count > 0) {
				yellows = new List<Order> ();
				greens = new List<Order> ();

				for (int i = 0; i < TempStorage.Orders.Count; i++) {
					if (orderType == "black") {
						if (TempStorage.Orders [i].OrderStatus == orderType) {
							orders.Add (TempStorage.Orders [i]);
						}
					} else if (orderType == "not") {
						if (TempStorage.Orders [i].OrderStatus == "green") {
							greens.Add (TempStorage.Orders [i]);
						} else if (TempStorage.Orders [i].OrderStatus == "yellow") {
							yellows.Add (TempStorage.Orders [i]);
						} else if (TempStorage.Orders [i].OrderStatus == "red") {
							orders.Add (TempStorage.Orders [i]);
						}
					}
				}
					
				if (yellows.Count > 0) {
					orders.InsertRange (0, yellows);
				}

				if (greens.Count > 0) {
					orders.InsertRange (0, greens);
				}

				for (int i = 0; i < orders.Count; i++) {
					for (int j = 0; j < TempStorage.Cafes.Count; j++) {
						if (orders[i].CafeId == TempStorage.Cafes[j].CafeId) {
							orders [i].CafeName = TempStorage.Cafes [j].CafeName;
						}
					}
				}
			}

			return orders;
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			
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

		/*protected override void OnPostResume ()
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

