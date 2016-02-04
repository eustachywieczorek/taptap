using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using Android.OS;


using FireSharp;
using FireSharp.Config;
using FireSharp.Exceptions;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;

using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;

using Android.Net;
using Android.Net.Wifi;
using Android.Telephony;
using Java.Lang;
using System.Threading;


namespace TapTapApplication
{
/*
 * Reminder: Don't forget to hide the button on loading!
*/
	[Activity (Name = "damiangrasso.taptapapplication.MainActivity", MainLauncher = true, Theme="@android:style/Theme.Light.NoTitleBar.Fullscreen")]
	public class MainActivity : Activity, IFacebookCallback
	{
		ICallbackManager callback;
		//IFirebaseClient fbClient;
		//IFirebaseConfig fbConfig;
		JObject userObj;

		WifiManager wifi;

		ProgressBar loadingBar;
		Button loginButton;
		AlertDialog.Builder alertDialog, networkDialog;
		MyProfileTracker mProfileTracker;

		protected override void OnCreate (Bundle savedInstanceState) {
		//	Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);
			Stripe.StripeClient.DefaultPublishableKey = TempStorage.PublishableKey;

			TempStorage.SetupDb ();
			TempStorage.GetMilks();
			TempStorage.GetSizes();

			SetContentView (Resource.Layout.Main);
			SetupUi ();//might move to else statement

			if (NetworkAvailable ()) {
				SetupFacebook ();

				if (Profile.CurrentProfile != null) {
					TempStorage.ProfileId = string.Concat ("facebook:", Profile.CurrentProfile.Id);
					StartLogin ();
				}

			} else {
				SetupNetworkPrompt ();
				networkDialog.Show ();
			}
		}

		public void StartLogin() {
			try {
				loadingBar.Visibility = Android.Views.ViewStates.Visible;
				loginButton.Visibility = Android.Views.ViewStates.Invisible;
				new System.Threading.Thread (new ThreadStart (() => {
					LoadEverything();

					RunOnUiThread(() => {
						StartActivity(typeof(HomeActivity));			
					});
						
				})).Start ();
			} catch (System.Exception e) {
				loginButton.Visibility = Android.Views.ViewStates.Visible;
				//then say, there was an issue, show error message.
				//you might want to put an alert dialog here
			}
		}

		public void LoadEverything() {
			TempStorage.GetOrders();
			TempStorage.GetCafes();
			TempStorage.GetFavourites();
			TempStorage.GetCoffees ();
		}

		private void SetupUi() {
			loginButton = FindViewById<Button> (Resource.Id.login_button);
			alertDialog = new AlertDialog.Builder (this);
			loadingBar = FindViewById<ProgressBar> (Resource.Id.pbLoading);
			loadingBar.Visibility = Android.Views.ViewStates.Invisible;

			loginButton.Click += LoginButton_Click;
		}

		public void SetupFacebook() {
			FacebookSdk.SdkInitialize (this.ApplicationContext);
			callback = CallbackManagerFactory.Create ();
			LoginManager.Instance.RegisterCallback (callback, this);

			mProfileTracker = new MyProfileTracker ();
			mProfileTracker.mOnProfileChanged += mProfileTracker_mOnProfileChanged;
			mProfileTracker.StartTracking ();
		}

		public void SetupNetworkPrompt() {
			networkDialog = new AlertDialog.Builder (this);
			networkDialog.SetTitle ("WiFi and Mobile Data Disabled");
			networkDialog.SetMessage ("Please activate WiFi or Mobile Data before continuing.");

			networkDialog.SetPositiveButton ("Activate WiFi", (senderAlert, args) => {
				if (ActivateNetwork(1) != -1) {
					SetupFacebook();
				}
			});

			networkDialog.SetNegativeButton ("Activate Data", (senderAlert, args) => {
				if (ActivateNetwork(0) != -1) {
					SetupFacebook();
				}
			});
		}

		public int ActivateNetwork(int networkCode) {
			if (networkCode == 0) {
				//Turn on mobile data
				return 0;
			} else if (networkCode == 1) {
				wifi = (WifiManager)GetSystemService (WifiService);
				wifi.SetWifiEnabled (true);
				return 1;
			}

			return -1;
		}
	
		void LoginButton_Click (object sender, EventArgs e) {
			if (NetworkAvailable ()) {
				if (Profile.CurrentProfile == null) {
					LoginManager.Instance.LogInWithReadPermissions (this, new List<string> () { "public_profile" });
				} else {
					TempStorage.ProfileId = string.Concat ("facebook:", Profile.CurrentProfile.Id);
					StartLogin ();
				}
			} else {
				Toast.MakeText (BaseContext, "Network currently unavailable", ToastLength.Long).Show();
			}
		}

		public void PhysicalLogin() {
			string name = Profile.CurrentProfile.Name;
			string facebookId = string.Concat ("facebook:", Profile.CurrentProfile.Id);

			try {
				FirebaseResponse fbResponse = TempStorage.FbClient.Get ("users/" + facebookId);
				if (fbResponse.ResultAs<JObject>() == null) {
					userObj = JObject.Parse (@"{ 'fullName' : '" + Profile.CurrentProfile.Name + "', 'favourites' : { }, 'loyalty_tracking' : { } }");
					SetResponse userSet = TempStorage.FbClient.Set ("users/" + facebookId, userObj);
					JObject result = userSet.ResultAs<JObject> ();
				}
			} catch (FirebaseException e) {
				//NB Harness the exception somehow, maybe try to get the user profile data somewhow
			}
		}

		void mProfileTracker_mOnProfileChanged(object sender, OnProfileChangedEventArgs e) {
			if (Profile.CurrentProfile != null) {
				TempStorage.ProfileId = string.Concat("facebook:", Profile.CurrentProfile.Id);
				PhysicalLogin();
			}
		}

		public void OnCancel() {
			//alertDialog.SetMessage ("Login process was cancelled");
			//alertDialog.Show ();
		}

		public void OnError(FacebookException e) {
			//alertDialog.SetMessage (e.Message);
			//alertDialog.Show();
		}

		public void OnSuccess(Java.Lang.Object result) {
			LoginResult loginResult = result as LoginResult;
			TempStorage.ProfileId = string.Concat("facebook:", loginResult.AccessToken.UserId);

			PhysicalLogin ();
			StartLogin ();
		}

		public bool NetworkAvailable() {
			ConnectivityManager connectivityManager = (ConnectivityManager) GetSystemService(ConnectivityService);
			NetworkInfo wifiInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi);
			NetworkInfo mobDataInfo = connectivityManager.GetNetworkInfo (ConnectivityType.Mobile);

			if (wifiInfo.IsConnected || mobDataInfo.IsConnected) {
				return true;
			}

			return false;
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
			base.OnActivityResult (requestCode, resultCode, data);
			callback.OnActivityResult (requestCode, (int)resultCode, data);
		}

		protected override void OnStop() {
			base.OnStop ();
			mProfileTracker.StopTracking ();
			GC.Collect ();
		}
	}
		
	public class MyProfileTracker : ProfileTracker {
		public EventHandler<OnProfileChangedEventArgs> mOnProfileChanged;

		protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile) {
			if (mOnProfileChanged != null) {
				mOnProfileChanged.Invoke (this, new OnProfileChangedEventArgs (newProfile));
			}
		}
	}

	public class OnProfileChangedEventArgs: EventArgs {
		public Profile mProfile;

		public OnProfileChangedEventArgs(Profile profile) {
			mProfile = profile;
		}
	}
}