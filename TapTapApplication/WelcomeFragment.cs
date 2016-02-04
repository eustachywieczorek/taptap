
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TapTapApplication
{
	public class WelcomeFragment : Fragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.WelcomeFragment, container, false);
			Button btnCraft = v.FindViewById<Button> (Resource.Id.btnCraft);

			btnCraft.Click += delegate {
				OnClick();
			};

			return v;
		}

		public void OnClick() {
			Intent orderActivity = new Intent(this.Activity, typeof(OrderActivity));
			StartActivity(orderActivity);
		}
	}
}

