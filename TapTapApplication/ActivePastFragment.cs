
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
	public class ActivePastFragment : ListFragment
	{
		ListView ordersList;

		public ActivePastFragment(List<Order> orders, OrderAdapter orderAdapter) {
			this.Orders = orders;
			this.OrderAdapter = orderAdapter;
		}

		public List<Order> Orders { get; set; }
		public OrderAdapter OrderAdapter { get; set; }

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
			ListAdapter = this.OrderAdapter;
		}


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate (Resource.Layout.ActivePastFragment, null);
		}
	}
}

