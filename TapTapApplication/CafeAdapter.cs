using System;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;

namespace TapTapApplication
{
	public class CafeAdapter: BaseAdapter<Cafe>
	{
		List<Cafe> items;
		Activity context;

		public CafeAdapter (Activity context, List<Cafe> items) : base() {
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Cafe this[int position] {  
			get { return items[position]; }
		}

		public override int Count {
			get { return items.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.CafeListItem, null);

			TextView location, cafe, online;

			cafe = view.FindViewById<TextView> (Resource.Id.txtCafeName);
			online = view.FindViewById<TextView> (Resource.Id.txtOpenStatus);
			location = view.FindViewById<TextView> (Resource.Id.txtLocation);

			cafe.Text = items[position].CafeName;
			online.Text = items[position].Online;
			location.Text = items[position].Location;

			return view;
		}
	}
}

