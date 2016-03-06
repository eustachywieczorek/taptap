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


using Stripe;
using System.Threading.Tasks;

using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Wallet.FirstParty;
using Android.Gms.Wallet.Fragment;
using Android.Gms.Wallet.Wobs;
using Android.Gms.Wallet;
using Android.Runtime;

using nsoftware.InPayPal;

namespace TapTapApplication
{
	[Activity (Label = "PayActivity", Theme="@style/MyTheme")]
	public class PayActivity : Android.Support.V4.App.FragmentActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
	{
		List<string> cafes, coffees, sizes;
		ProgressBar loadingBar;



		Directpayment directpayment;



		ExpandableListView coffeeList, cafeList, sizeList;
		Button btnPay;
		TextView emailAddress, order;
		bool favChecked;
		StripeView stripeView;
		SupportToolbar supportToolbar;

		GoogleApiClient googleApiClient;

		const int LOAD_MASKED_WALLET_REQ_CODE = 1000;
		const int LOAD_FULL_WALLET_REQ_CODE = 2000;

		static string apiUsername = "damian.grasso123-buyer-1@gmail.com";
		static string apiPassword = "awesome12345";
		static string apiSignature = "An5ns1Kso7MWUdW4ErQKJJJ4qi4-AjQcqCnqZPYRBv2FbPRkZQEu7D7M";
			static string cardNumber = "4239532182772787";		
		static string cardExpMonth = "02";
		static string cardExpYear = "2021";
		static string cardCVVData = "533";

		protected override async void OnCreate (Bundle savedInstanceState)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);

			directpayment = new Directpayment(this);
			directpayment.OnSSLServerAuthentication += (object sender, DirectpaymentSSLServerAuthenticationEventArgs e) => {
				e.Accept = true;
			};

			//Stripe.StripeClient.DefaultPublishableKey = "sk_test_1sTZuEyHl6nZFhZJ3lnEgF8Y";

			SetContentView (Resource.Layout.Pay);

			EditText txtFirstName = FindViewById<EditText> (Resource.Id.txtFirstName);
			EditText txtLastName = FindViewById<EditText> (Resource.Id.txtLastName);
			EditText txtAddress = FindViewById<EditText> (Resource.Id.txtAddress);
			EditText txtCity = FindViewById<EditText> (Resource.Id.txtCity);
			EditText txtState = FindViewById<EditText> (Resource.Id.txtState);
			EditText txtZip = FindViewById<EditText> (Resource.Id.txtZip);
			EditText txtIPAddress = FindViewById<EditText> (Resource.Id.txtIPAddress);
			EditText txtAmount = FindViewById<EditText> (Resource.Id.txtAmount);
			EditText txtDescription = FindViewById<EditText> (Resource.Id.txtDescription);
			Button btnConfigure = FindViewById<Button> (Resource.Id.btnConfigure);
			Button btnCardInfo = FindViewById<Button> (Resource.Id.btnCardInfo);
			Button btnAuthorize = FindViewById<Button> (Resource.Id.btnAuthorize);

			btnAuthorize.Click += (object sender, EventArgs e) => {
				try {
					directpayment.URL = "https://api-3t.sandbox.paypal.com/nvp"; // Test Server URL
					directpayment.User = apiUsername;
					directpayment.Password = apiPassword;
					directpayment.Signature = apiSignature;

					directpayment.OrderTotal = txtAmount.Text;
					directpayment.OrderDescription = txtDescription.Text;

					nsoftware.InPayPal.Card card = new nsoftware.InPayPal.Card ();
					card.CardType = CardTypes.ccVisa;
					card.Number = cardNumber;
					card.ExpMonth = Convert.ToInt32 (cardExpMonth);
					card.ExpYear = Convert.ToInt32 (cardExpYear);
					card.CVV = cardCVVData;
					directpayment.Card = card;

					DirectPaymentPayer payer = new DirectPaymentPayer ();
					payer.FirstName = txtFirstName.Text;
					payer.LastName = txtLastName.Text;
					payer.Street1 = txtAddress.Text;
					payer.City = txtCity.Text;
					payer.State = txtState.Text;
					payer.Zip = txtZip.Text;
					payer.IPAddress = txtIPAddress.Text;
					directpayment.Payer = payer;

					directpayment.Sale ();

					/*string results = "Ack  : " + directpayment.Ack + "\r\n";
					results += "Amt  : " + directpayment.Response.Amount + "\r\n";
					results += "AVS  : " + directpayment.Response.AVS + "\r\n";
					results += "CVV  : " + directpayment.Response.CVV + "\r\n";
					results += "TxnId: " + directpayment.Response.TransactionId + "\r\n";
					txtResults.Text = results;*/


					//ShowResultsView();

				} catch (InPayPalDirectpaymentException ex) {
					//ShowMessage ("Error", ex.Message);
				}
			};

			/*var b = new WalletClass.WalletOptions.Builder ()
				.SetEnvironment (WalletConstants.EnvironmentSandbox)
				.SetTheme (WalletConstants.ThemeLight)
				.Build ();

			googleApiClient = new GoogleApiClient.Builder (this)
				.AddConnectionCallbacks (this)
				.AddOnConnectionFailedListener (this)
				.AddApi (WalletClass.API, b)
				.Build ();


			//var token = await StripeClient.CreateToken (c, MainActivity.STRIPE_PUBLISHABLE_KEY);
			var walletFragment = SupportWalletFragment.NewInstance (WalletFragmentOptions.NewBuilder ()
				.SetEnvironment (WalletConstants.EnvironmentSandbox)
				.SetMode (WalletFragmentMode.BuyButton)
				.SetTheme (WalletConstants.ThemeLight)
				.SetFragmentStyle (new WalletFragmentStyle ()
					.SetBuyButtonText (BuyButtonText.BuyWithGoogle)
					.SetBuyButtonAppearance (BuyButtonAppearance.Classic)
					.SetBuyButtonWidth (Dimension.MatchParent))
				.Build ());

			var maskedWalletRequest = MaskedWalletRequest.NewBuilder ()

				// Request credit card tokenization with Stripe by specifying tokenization parameters:
				.SetPaymentMethodTokenizationParameters (PaymentMethodTokenizationParameters.NewBuilder ()
					.SetPaymentMethodTokenizationType (PaymentMethodTokenizationType.PaymentGateway)
					.AddParameter ("gateway", "stripe")
					.AddParameter ("stripe:publishableKey", Stripe.StripeClient.DefaultPublishableKey)
					.AddParameter ("stripe:version", "1.15.1")
					.Build ())

				// You want the shipping address:
				.SetShippingAddressRequired (false)

				.SetMerchantName ("Llamanators")
				.SetPhoneNumberRequired (false)
				.SetShippingAddressRequired (false)

				// Price set as a decimal:
				.SetEstimatedTotalPrice ("20.00")
				.SetCurrencyCode ("USD")

				.Build();

			// Set the parameters:  
			var initParams = WalletFragmentInitParams.NewBuilder ()
				.SetMaskedWalletRequest (maskedWalletRequest)
				.SetMaskedWalletRequestCode (LOAD_MASKED_WALLET_REQ_CODE)
				.Build ();

			// Initialize the fragment:
			walletFragment.Initialize (initParams);

			SupportFragmentManager.BeginTransaction ().Replace (Resource.Id.frame_action, walletFragment).Commit ();*/

			//supportToolbar = FindViewById<SupportToolbar> (Resource.Layout.order_menu);
			//SetSupportActionBar (supportToolbar);
			//SupportActionBar.SetHomeButtonEnabled(true);
			//SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			//SupportActionBar.Title = "TapTap Coffee";





			//order = FindViewById<TextView> (Resource.Id.txtListHeader);
			//order.Text = string.Concat (TempStorage.Size, " ", TempStorage.Coffee, " ", "($", TempStorage.Price, ")");

			//loadingBar = FindViewById<ProgressBar> (Resource.Id.pbLoading);
			//loadingBar.Visibility = ViewStates.Invisible;*/

			favChecked = Intent.GetBooleanExtra ("Favourite", false); 
			//stripeView = FindViewById<Stripe.StripeView> (Resource.Id.stripeView);
			//btnPay = FindViewById<Button> (Resource.Id.btnPay);
			//btnPay.Click += delegate {
				
			/*	var c = stripeView.Card;

				if (!c.IsCardValid) {
					var errorMsg = "Invalid Card Information";

					if (!c.IsNumberValid)
						errorMsg = "Invalid Card Number";
					else if (!c.IsValidExpiryDate)
						errorMsg = "Invalid Card Expiry Date";
					else if (!c.IsValidCvc)
						errorMsg = "Invalid CVC";

					Toast.MakeText(this, errorMsg, ToastLength.Short).Show();
				} else {
				//	c.Name = name.Text;
				//	c.AddressLine1 = address1.Text;
				//	c.AddressLine2 = address2.Text;
				//	c.AddressCity = city.Text;
				//	c.AddressState = state.Text;
				//	c.AddressZip = zip.Text;
				//	c.AddressCountry = country.Text;

					try {
						var token = StripeClient.CreateToken (c, TempStorage.PublishableKey);

						if (token != null) {



							//TODO: Send token to your server to process a payment with

							var msg = string.Format ("Good news! Stripe turned your credit card into a token: \n{0} \n\nYou can follow the instructions in the README to set up Parse as an example backend, or use this token to manually create charges at dashboard.stripe.com .", 
								token.Id);

							Toast.MakeText(this, msg, ToastLength.Long).Show();
						} else {
							Toast.MakeText(this, "Failed to create Token", ToastLength.Short).Show();
						}
					} catch (Exception ex) {
						Toast.MakeText (this, "Failure: " + ex.Message, ToastLength.Short).Show ();
					}
				}*/

		}

		public void OnConnected(Bundle something) {
			Console.WriteLine ("Say so");
		}

		public void OnConnectionSuspended(int cause) {
			Console.WriteLine ("Suspended");
		}

		public void OnConnectionFailed(ConnectionResult result) {
			Console.WriteLine (result.ErrorMessage);
		}
			
		public async void PayOrder() {



		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{            
			if (requestCode == LOAD_MASKED_WALLET_REQ_CODE) { // Unique, identifying constant
				if (resultCode == Result.Ok) {

					var maskedWallet = data.GetParcelableExtra (WalletConstants.ExtraMaskedWallet).JavaCast<MaskedWallet> ();



					var fullWalletRequest = FullWalletRequest.NewBuilder ()
						.SetCart (Cart.NewBuilder ()
							.SetCurrencyCode ("USD")
							.SetTotalPrice ("20.00")
							.AddLineItem (LineItem.NewBuilder () // Identify item being purchased
								.SetCurrencyCode ("USD")
								.SetQuantity ("1")
								.SetDescription ("Premium Llama Food")
								.SetTotalPrice ("20.00")
								.SetUnitPrice ("20.00")
								.Build ())
							.Build ())
						.SetGoogleTransactionId (maskedWallet.GoogleTransactionId)
						.Build ();

					WalletClass.Payments.LoadFullWallet (googleApiClient, fullWalletRequest, LOAD_FULL_WALLET_REQ_CODE);

				} else {
					base.OnActivityResult (requestCode, resultCode, data);
				}
			} else if (requestCode == LOAD_FULL_WALLET_REQ_CODE) { // Unique, identifying constant

				if (resultCode == Result.Ok) {
					var fullWallet = data.GetParcelableExtra (WalletConstants.ExtraFullWallet).JavaCast<FullWallet> ();
					var tokenJson = fullWallet.PaymentMethodToken.Token;

					var stripeToken = Stripe.Token.FromJson (tokenJson);

					var msg = string.Empty;

					if (stripeToken != null) {

						//TODO: Send token to your server to process a payment with

						msg = string.Format ("Good news! Stripe turned your credit card into a token: \n{0} \n\nYou can follow the instructions in the README to set up Parse as an example backend, or use this token to manually create charges at dashboard.stripe.com .", 
							stripeToken.Id);

					} else {
						msg = "Failed to create Token";
					}

					new Android.Support.V7.App.AlertDialog.Builder (this)
						.SetTitle ("Stripe Response")
						.SetMessage (msg)
						.SetCancelable (true)
						.SetNegativeButton ("OK", delegate { })
						.Show ();                    
				}

			} else {
				base.OnActivityResult (requestCode, resultCode, data);
			}
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

