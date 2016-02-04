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
			int d;
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.CafeListItem, null);
			
			view.FindViewById<TextView>(Resource.Id.txtCafeName).Text = items[position].CafeName;
			view.FindViewById<TextView>(Resource.Id.txtLocation).Text = items[position].Location;

			if (TempStorage.DrawableLogos == null) {
				TempStorage.DrawableLogos = new int[3];
			}

			switch (position) {
			case 0:
				d = Resource.Drawable.physiollogo;
				view.FindViewById<RelativeLayout> (Resource.Id.cafe_logo).SetBackgroundResource (d);
				TempStorage.DrawableLogos [position] = d;
				break;
			case 1:
				d = Resource.Drawable.darwinslogo;
				view.FindViewById<RelativeLayout> (Resource.Id.cafe_logo).SetBackgroundResource (d);
				TempStorage.DrawableLogos [position] = d;
				break;
			case 2:
				d = Resource.Drawable.pcafelogo;
				view.FindViewById<RelativeLayout> (Resource.Id.cafe_logo).SetBackgroundResource (d);
				TempStorage.DrawableLogos [position] = d;
				break;
			}



			//I'm going to assume that if prep_time = 0 or -1, the cafe is offline
			if (items[position].PrepTime == "0" || items[position].PrepTime == "-1") {
				view.FindViewById<TextView> (Resource.Id.txtOpenStatus).SetTextColor (Android.Graphics.Color.Red);
				view.FindViewById<TextView>(Resource.Id.txtOpenStatus).Text = "Offline";
			} else {
				view.FindViewById<TextView> (Resource.Id.txtOpenStatus).SetTextColor (Android.Graphics.Color.DarkGreen);
				view.FindViewById<TextView>(Resource.Id.txtOpenStatus).Text = string.Concat("Approx. ", items[position].PrepTime , " minutes prep");
			}

			return view;
		}
	}
}

