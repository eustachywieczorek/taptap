using System;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;
using Android.Graphics;

namespace TapTapApplication
{
	public class ListAdapter: BaseAdapter<string>
	{
		List<string> items;
		Activity context;

		public ListAdapter (Activity context, List<string> items) : base() {
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override string this[int position] {  
			get { return items[position]; }
		}

		public override int Count {
			get { return items.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);

		
			TextView t = view.FindViewById<TextView> (Android.Resource.Id.Text1);
			t.Text = items [position];
			t.SetTextColor (Color.ParseColor ("#BDBDBD"));
			return view;
		}
	}
}

