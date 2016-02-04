
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

using FireSharp;
using FireSharp.Response;
using FireSharp.Config;
using Newtonsoft.Json.Linq;

namespace TapTapApplication
{
	public class CafeFragment : ListFragment
	{
		ListView lvCafes;
		Dictionary<string, List<string>> coffeeData;
		IMenuItem favourite;
	//Button payButton;

		public CafeFragment(List<Cafe> cafes, CafeAdapter cafeAdapter) {
			this.Cafes = cafes;
			this.CafeAdapter = cafeAdapter;
		}

		public List<Cafe> Cafes { get; set; }
		public CafeAdapter CafeAdapter { get; set; }

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			coffeeData = new Dictionary<string, List<string>> ();
			SetHasOptionsMenu (true);

			GetCoffeeData ();
		}

		public void GetCoffeeData() {
			List<string> derivedCoffees = new List<string>();

			for (int i = 0; i < TempStorage.Coffees.Count; i++) {
				derivedCoffees.Add (TempStorage.Coffees [i].CoffeeType);
			}

			coffeeData.Add ("Coffee", derivedCoffees);
			coffeeData.Add ("Milk", TempStorage.Milks);
			coffeeData.Add ("Size", TempStorage.Sizes);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.ChooseCafe, null);

			lvCafes = v.FindViewById<ListView> (Resource.Id.cafes);
			lvCafes.Adapter = new CafeAdapter (this.Activity, Cafes);
		//	payButton = v.FindViewById<Button> (Resource.Id.btnPay);

			lvCafes.ItemClick += delegate {
				//ListClick();
			};

			//payButton.Click += delegate {

			//};

			return inflater.Inflate (Resource.Layout.ChooseCafe, container, false);
		}

		public override void OnActivityCreated(Bundle savedInstanceState) {
			base.OnActivityCreated (savedInstanceState);
			this.ListAdapter = this.CafeAdapter;
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater menuInflater) {
			menuInflater.Inflate (Resource.Layout.order_menu, menu);
			favourite = menu.FindItem (Resource.Id.fav);

			favourite.SetVisible (false);

			base.OnCreateOptionsMenu (menu, menuInflater);
		}

		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				StartActivity (new Intent(this.Activity, typeof(HomeActivity)));
				break;
			}

			base.OnOptionsItemSelected (item);
			return true;
		}

		public override void OnListItemClick (ListView l, View v, int position, long id)
		{
			OrderActivity.CoffeeFragPresented = true;
			CoffeeFragment coffeeFragment = new CoffeeFragment (coffeeData, new ExpandableListAdapter(this.Activity, coffeeData));
			FragmentManager.BeginTransaction ().Replace (Resource.Id.frame_action, coffeeFragment).Commit();

			TempStorage.ChosenLogo = TempStorage.DrawableLogos [position];

			TempStorage.Cafe = v.FindViewById<TextView> (Resource.Id.txtCafeName).Text;
		}
	}
}