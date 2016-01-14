using System;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;

namespace TapTapApplication
{
	public class OrderAdapter: BaseAdapter<Order>
	{
		List<Order> items;
		Activity context;

		public OrderAdapter (Activity context, List<Order> items) : base() {
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Order this[int position] {  
			get { return items[position]; }
		}

		public override int Count {
			get { return items.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.ListItem, null);

			LinearLayout lin = view.FindViewById<LinearLayout> (Resource.Id.list_item);
			LinearLayout colour = view.FindViewById<LinearLayout> (Resource.Id.order_colour);

			if (items [position].OrderStatus == "red") {
				colour.SetBackgroundColor (Android.Graphics.Color.Red);
			} else if (items [position].OrderStatus == "yellow") {
				colour.SetBackgroundColor (Android.Graphics.Color.Yellow);
			} else if (items [position].OrderStatus == "green") {
				colour.SetBackgroundColor (Android.Graphics.Color.Green);
			} else if (items [position].OrderStatus == "black") {
				colour.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#FFFFFF"));
			}

			TextView coffee, cafe, orderDate;

			coffee = lin.FindViewById<TextView> (Resource.Id.txtItemCoffee);
			cafe = lin.FindViewById<TextView> (Resource.Id.txtItemCafe);
			orderDate = lin.FindViewById<TextView> (Resource.Id.txtItemOrder);

			coffee.Text = string.Concat(items [position].Coffee, " - ", items[position].Size);
			cafe.Text = items [position].Cafe;
			orderDate.Text = items [position].OrderTime.ToLongDateString();


			return view;
		}
	}
}

