
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

using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Wallet.FirstParty;
using Android.Gms.Wallet.Fragment;
using Android.Gms.Wallet.Wobs;
using Android.Gms.Wallet;
using Android.Runtime;

namespace TapTapApplication
{
	public class BillingFragment : Fragment
	{
		Stripe.StripeView stripeView;
		Button btnPay;
		const int LOAD_MASKED_WALLET_REQ_CODE = 1000;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.BillingFragment, null);



			return v;
		}
	}
}

