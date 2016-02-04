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
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using FireSharp.Config;
using FireSharp;
using Android.Media;

namespace TapTapApplication
{
	public class CoffeeFragment : ListFragment, ExpandableListView.IOnChildClickListener
	{
		ExpandableListView coffeeLv;
		ExpandableListAdapter coffeeAdapter;
		Button payButton;
		string original;
		string price, coffee, milk, size;

		bool favChecked;

	//	List<string> coffeeDataHeaders;
		Dictionary<string, List<string>> coffeeData;
		IMenuItem favourite;
		Android.Support.V7.App.AlertDialog.Builder orderDialog;
		List<string> headers;
		ImageView chosenLogo;

		public CoffeeFragment(Dictionary<string, List<string>> coffeeData, ExpandableListAdapter coffeeAdapter) {
			this.CoffeeData = coffeeData;
			this.ExpandableListAdapter = coffeeAdapter;
		}

		public Dictionary<string, List<string>> CoffeeData { get; set; }
		public ExpandableListAdapter ExpandableListAdapter { get; set; }

		public override void OnActivityCreated(Bundle savedInstanceState) {
			base.OnActivityCreated (savedInstanceState);
			ExpandableListAdapter = this.ExpandableListAdapter;
		}

		public void ProcessPayments() {//string coffee, string milk, string size) {
			//sort the input stuff out later
			if (headers [0] != "Coffee" && headers [1] != "Milk" && headers [2] != "Size") {
				Intent payActivity = new Intent (this.Activity, typeof(PayActivity));
				payActivity.PutExtra ("Favourite", favChecked);

				StartActivity (payActivity);
				//Don't forget to implement this
				//payActivity.PutExtra("CafeId", "");
				//There needs to be more data set via the intent e.g. cafe etc.
			} else {
				orderDialog.Show ();
			}



		}

		public override void OnCreate(Bundle saveStateInstance) {
			base.OnCreate (saveStateInstance);
			SetHasOptionsMenu (true);

			//make sure you have a dismiss button or something for devices
			orderDialog = new Android.Support.V7.App.AlertDialog.Builder (this.Activity);
			orderDialog.SetTitle ("Woah There!");
			orderDialog.SetMessage ("You forgot to fill in every field.");
			orderDialog.SetPositiveButton ("Thanks!", delegate {

			});
		}

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater menuInflater) {
			menuInflater.Inflate (Resource.Layout.order_menu, menu);
			favourite = menu.FindItem (Resource.Id.fav);

			favourite.SetVisible (false);

			base.OnCreateOptionsMenu (menu, menuInflater);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View v = inflater.Inflate (Resource.Layout.ChooseCoffee, null);

			favChecked = false;

			v.FindViewById<ImageView> (Resource.Id.chosen_logo).SetBackgroundResource (TempStorage.ChosenLogo);
			coffeeLv = v.FindViewById<ExpandableListView> (Android.Resource.Id.List);
			payButton = v.FindViewById<Button> (Resource.Id.btnPay);

			payButton.Click += delegate {
				ProcessPayments();
			};

			coffeeAdapter = new ExpandableListAdapter (this.Activity, CoffeeData);

			coffeeLv.SetAdapter(coffeeAdapter);
			coffeeLv.SetOnChildClickListener (this);

			//don't forget!
			price = "4.50";

			return v;
		}
			
		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				StartActivity(new Intent(this.Activity, typeof(OrderActivity)));



				break;
			case Resource.Id.fav: 
				if (favChecked) {
					item.SetIcon (Resources.GetDrawable (Resource.Drawable.fav));
					favChecked = false;
				} else {
					item.SetIcon (Resources.GetDrawable (Resource.Drawable.favchecked));
					favChecked = true;
				}
				break;
			}

			base.OnOptionsItemSelected (item);
			return true;
		}

	public bool OnChildClick(ExpandableListView parent, View v,
			int groupPosition, int childPosition, long id) {

			//Make a second selection, it adds that into the listitem...

			headers = new List<string>(CoffeeData.Keys);



			List<string> defaultHeaders = new List<string> ();
			defaultHeaders.Add ("Coffee");
			defaultHeaders.Add ("Size");
			defaultHeaders.Add ("Milk");

			string currentKey = headers [groupPosition];

			List<string> values = CoffeeData [currentKey];

			string fixedKey;
			int cutOff = currentKey.IndexOf (":");
			string newGroupText = "";
	
			if (cutOff > 0) {
				fixedKey = currentKey.Substring (0, cutOff);
				newGroupText = string.Empty;
				newGroupText = string.Concat (fixedKey, ": ", 
					v.FindViewById<TextView> (Resource.Id.txtItemContent).Text);
			} else {
				
				original = string.Empty;

				if (groupPosition == 0) {
					original = "Coffee";
				} else if (groupPosition == 1) {
					original = "Milk";
				} else if (groupPosition == 2) {
					original = "Size";
				}

				newGroupText = string.Concat (original, ": ", 
					v.FindViewById<TextView> (Resource.Id.txtItemContent).Text);
			}

			headers [groupPosition] = newGroupText;

			CoffeeData.Remove (currentKey);
			CoffeeData.Add (headers [groupPosition], values);

			coffeeLv.CollapseGroup (groupPosition);
			coffeeAdapter.UpdateData (CoffeeData);

			if (groupPosition == 0) {
				TempStorage.Coffee = v.FindViewById<TextView> (Resource.Id.txtItemContent).Text;
			} else if (groupPosition == 1) {
				TempStorage.Milk = v.FindViewById<TextView> (Resource.Id.txtItemContent).Text;
			} else if (groupPosition == 2) {
				TempStorage.Size = v.FindViewById<TextView> (Resource.Id.txtItemContent).Text;
			}

			if (headers [0] != "Coffee" && headers [1] != "Milk" && headers [2] != "Size") {
				favourite.SetVisible (true);
			}

			return true;
		}
	}
}