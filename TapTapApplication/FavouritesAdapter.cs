using System;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;
using Java.Net;
using Java.IO;
using Android.Graphics.Drawables;

namespace TapTapApplication
{
	public class FavouritesAdapter: BaseAdapter<Favourite>
	{
		List<Favourite> items;
		Activity context;

		public FavouritesAdapter (Activity context, List<Favourite> items) : base() {
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Favourite this[int position] {  
			get { return items[position]; }
		}

		public override int Count {
			get { return items.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.FavouritesListItem, null);
			//	view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
			
			view.FindViewById<TextView>(Resource.Id.txtCoffeeFav).Text = string.Concat(items[position].Coffee, " (", items[position].Size, ")");
			view.FindViewById<TextView>(Resource.Id.txtMilk).Text = items[position].Milk;
			view.FindViewById<TextView>(Resource.Id.txtPrice).Text = items[position].Price;

			return view;
		}
	}
}

