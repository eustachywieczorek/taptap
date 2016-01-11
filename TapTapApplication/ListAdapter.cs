using System;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;

namespace TapTapApplication
{
	public class LeftDrawerAdapter: BaseAdapter<string>
	{
		List<string> items;
		Activity context;

		public LeftDrawerAdapter (Activity context, List<string> items) : base() {
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
			
			view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];
			return view;
		}
	}
}

