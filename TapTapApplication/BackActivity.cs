using System;
using System.Collections.Generic;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using TapTap;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.OS;

namespace TapTapApplication
{
	public class BackActivity: ActionBarActivity
	{
			private SupportToolbar supportToolbar;
			public Intent nextActivity;

			protected override void OnCreate (Bundle bundle)
			{
				base.OnCreate (bundle);

				SetContentView (Resource.Layout.Order);

				supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.action_menu);
				SetSupportActionBar (supportToolbar);
				SupportActionBar.SetHomeButtonEnabled(true);
				SupportActionBar.SetDisplayHomeAsUpEnabled (true);
				SupportActionBar.SetDisplayShowTitleEnabled (true);
			}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			return base.OnOptionsItemSelected (item);
		}

			public override bool OnCreateOptionsMenu (IMenu menu)
			{
				MenuInflater.Inflate (Resource.Layout.order_menu, menu);
				return base.OnCreateOptionsMenu (menu);
			}
			
	}
}

