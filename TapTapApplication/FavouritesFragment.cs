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
	public class FavouritesFragment : ListFragment
	{
		ListView listView;

		public FavouritesFragment(List<Favourite> orders, FavouritesAdapter favouritesAdapter) {
			this.Orders = orders;
			this.FavouritesAdapter = favouritesAdapter;
		}

		public List<Favourite> Orders { get; set; }
		public FavouritesAdapter FavouritesAdapter { get; set; }

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
			ListAdapter = this.FavouritesAdapter;
		}

		public override void OnListItemClick (ListView l, View v, int position, long id)
		{
			//add: grab the data

			string coffee = View.FindViewById<TextView> (Resource.Id.txtCoffeeFav).Text;

			string size;
			int sizeStart = coffee.IndexOf ('(');

			size = coffee.Substring (sizeStart);
			coffee = coffee.Remove (sizeStart - 1);


			size = size.Trim ('(', ')');
			//size = size.ToLower ();

			Intent payActivity = new Intent(this.Activity, typeof(PayActivity));
			payActivity.PutExtra ("Favourite", true);
			payActivity.PutExtra("Coffee", coffee);
				payActivity.PutExtra("Milk",  View.FindViewById<TextView>(Resource.Id.txtMilk).Text);
			payActivity.PutExtra("Size", size);
			StartActivity(payActivity);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate (Resource.Layout.FavouritesFragment, container, false);
		}
	}
}

