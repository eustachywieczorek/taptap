using Android.App;
using Android.Widget;
using Android.OS;

using System.Collections.Generic;

using FireSharp;
using FireSharp.Config;
using FireSharp.Response;

using Newtonsoft.Json.Linq;
using Android.Content;

using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using System;
using Xamarin.Facebook;
using System.Threading;

using nsoftware.InPayPal;
using Stripe;
using System.Threading.Tasks;

namespace TapTapApplication
{
	[Activity (Label = "PayActivity", Theme="@style/MyTheme")]
	public class PayActivity : ActionBarActivity
	{
		List<string> cafes, coffees, sizes;
		ProgressBar loadingBar;

		EditText name, address1, address2, city, state, zip, country;


		ExpandableListView coffeeList, cafeList, sizeList;
		Button buttonToken;
		TextView emailAddress, order;
		bool favChecked;
		StripeView stripeView;
		SupportToolbar supportToolbar;



		protected override void OnCreate (Bundle savedInstanceState)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);



			SetContentView (Resource.Layout.Pay);

			supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.order_menu);
			SetSupportActionBar (supportToolbar);
			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.Title = "TapTap Coffee";

			stripeView = FindViewById<Stripe.StripeView> (Resource.Id.stripeView);

			buttonToken = FindViewById<Button> (Resource.Id.buttonToken);

			order = FindViewById<TextView> (Resource.Id.txtListHeader);
			order.Text = string.Concat (TempStorage.Size, " ", TempStorage.Coffee, " ", "($", TempStorage.Price, ")");

			loadingBar = FindViewById<ProgressBar> (Resource.Id.pbLoading);
			loadingBar.Visibility = ViewStates.Invisible;

			favChecked = Intent.GetBooleanExtra ("Favourite", false); 


			buttonToken.Click += delegate {
				PayOrder();
			};
				
		}
			
		public async void PayOrder() {



		}

		public void SendOrder() {
			Intent activeOrders;

			string fullName = Profile.CurrentProfile.Name;
			string facebookId = string.Concat("facebook:", Profile.CurrentProfile.Id);

			string orderTime = ((Int32)(DateTime.UtcNow.AddSeconds(5).Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

			loadingBar.Visibility = ViewStates.Visible;

			//Add pricing to the mix...
			new Thread(new ThreadStart(new ThreadStart (() => {

				TempStorage.CafeId = TempStorage.GetCafeId(TempStorage.Cafe);
				TempStorage.Price = TempStorage.GetPrice (TempStorage.Coffee);

				TempStorage.Orders.Insert (0, new Order {
					CafeId = TempStorage.CafeId,
					Coffee = TempStorage.Coffee, 
					OrderStatus = "red", 
					OrderTime = TempStorage.GetDate(orderTime), 
					Size = TempStorage.Size, 
					Price = TempStorage.Price
				});

				JObject order = JObject.Parse(@"{ 'cafe_id' : '" + TempStorage.CafeId + "', 'coffee' : '" + TempStorage.Coffee + "', 'order_status' : 'red', 'order_time' : '" + orderTime + 
					"', 'price' : '" + TempStorage.Price + "', 'size' : '" + TempStorage.Size + "', 'user_fullName' : '" + Profile.CurrentProfile.Name + 
					"', 'user_id' : '" + string.Concat("facebook:", Profile.CurrentProfile.Id) + "', 'milk' : '" + TempStorage.Milk + "' }");



				FirebaseResponse orderSend = TempStorage.FbClient.Push ("orders/", order);

				if (favChecked) {
					JObject favouritesSend = JObject.Parse (@"{ 'cafe_id' : '" + TempStorage.CafeId + "', 'coffee' : '" + TempStorage.Coffee + "', 'price' : '" + TempStorage.Price + "', 'size' : '" + TempStorage.Size + "', 'milk' : '" + 
						TempStorage.Milk + "'}");

					orderSend = TempStorage.FbClient.Push(string.Concat("users/", facebookId, "/favourites"), favouritesSend);
					favouritesSend = orderSend.ResultAs<JObject> ();

					TempStorage.Favourites.Add (new Favourite{ Coffee = TempStorage.Coffee, Milk = TempStorage.Milk, Size = TempStorage.Size, Price = TempStorage.Price });
				}

				activeOrders = new Intent (this, typeof(ActivePastActivity));
				activeOrders.PutExtra ("Past", "pay");
				activeOrders.PutExtra ("Query", "not");

				RunOnUiThread(() => {
					StartActivity (activeOrders);
				});
			}))).Start();
		}



		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				Intent orderActivity = new Intent (this, typeof(OrderActivity));
				NavigateUpTo (orderActivity);
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Layout.order_menu, menu);
			//Not sure about false as default

			if (favChecked) {
				menu.GetItem (0).SetIcon (Resource.Drawable.favchecked);
			} else if (!favChecked) {
				menu.GetItem (0).SetIcon (Resource.Drawable.fav);
			}

			return true;
		}
	}
}

