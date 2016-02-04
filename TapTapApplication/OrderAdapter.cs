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
			TextView coffee, cafe, orderDate;

			coffee = lin.FindViewById<TextView> (Resource.Id.txtItemCoffee);
			cafe = lin.FindViewById<TextView> (Resource.Id.txtItemCafe);
			orderDate = lin.FindViewById<TextView> (Resource.Id.txtItemOrder);


			if (items [position].OrderStatus != "black") {
				if (items [position].OrderStatus == "red") {
					colour.SetBackgroundColor (Android.Graphics.Color.Red);
					orderDate.Text = string.Concat("Expected delivery time: ", items[position].OrderTime.AddMinutes(TempStorage.GetPrepTime (items [position].CafeId)).ToShortTimeString());
				} else if (items [position].OrderStatus == "yellow") {
					colour.SetBackgroundColor (Android.Graphics.Color.Yellow);
					orderDate.Text = string.Concat("Expected delivery time: ", items[position].OrderTime.AddMinutes(TempStorage.GetPrepTime (items [position].CafeId)).ToShortTimeString());
				} else if (items [position].OrderStatus == "green") {
					colour.SetBackgroundColor (Android.Graphics.Color.Green);
					orderDate.Text = string.Concat("Expected delivery time: ", items[position].OrderTime.AddMinutes(TempStorage.GetPrepTime (items [position].CafeId)).ToShortTimeString());
				}
			} else {
				colour.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#FFFFFF"));
				orderDate.Text = items [position].OrderTime.ToString ("F");
			}



			coffee.Text = string.Concat(items[position].Size, " ", items [position].Coffee, " ($", items[position].Price , ")");
			cafe.Text = items [position].CafeName;



			return view;
		}
	}
}

