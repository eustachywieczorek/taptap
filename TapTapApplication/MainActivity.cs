using Android.App;
using Android.Widget;
using Android.OS;

using System;
using System.Collections.Generic;


using Newtonsoft.Json.Linq;

using FireSharp.Response;
using FireSharp;
using FireSharp.Config;
using Android.Content;

using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Android.Content.PM;
using Java.Security;
using Xamarin.Facebook.Login.Widget;

using FireSharp.Exceptions;

namespace TapTapApplication
{
	[Activity (Name = "damiangrasso.taptapapplication.MainActivity", MainLauncher = true, Theme="@android:style/Theme.Light.NoTitleBar.Fullscreen")]
	public class MainActivity : Activity, IFacebookCallback
	{
		ICallbackManager callback;
		FirebaseClient fbClient;
		LoginButton loginButton;
		AlertDialog.Builder alertDialog;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);

			FacebookSdk.SdkInitialize (this.ApplicationContext);

			if (AccessToken.CurrentAccessToken.Token == null) {
				StartActivity (typeof(HomeActivity));
				FinishActivity (0);
			} else {
				SetContentView (Resource.Layout.Main);

				loginButton = FindViewById<LoginButton> (Resource.Id.login_button);

				callback = CallbackManagerFactory.Create ();
				loginButton.RegisterCallback (callback, this);

				fbClient = new FirebaseClient (new FirebaseConfig {
					AuthSecret = "FO85aksCBndB5fXAykNFQstlLqqYrHsiq4myZTQW", 
					BasePath = "https://scorching-heat-9815.firebaseio.com/"
				});

				alertDialog = new AlertDialog.Builder (this);//Need a theme?
				alertDialog.SetTitle("Error");
			}
		}

		public void OnCancel() {
			alertDialog.SetMessage ("Login process was cancelled");
			alertDialog.Show ();
		}

		public void OnError(FacebookException e) {
			alertDialog.SetMessage (e.Message);
			alertDialog.Show();
		}

		public void OnSuccess(Java.Lang.Object result) {
			LoginResult loginResult = result as LoginResult;

			string name = string.Concat (Profile.CurrentProfile.FirstName, " ", Profile.CurrentProfile.LastName);
			string facebookId = Profile.CurrentProfile.Id;

			PhysicalLogin (name, facebookId);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
			base.OnActivityResult (requestCode, resultCode, data);
			callback.OnActivityResult (requestCode, (int)resultCode, data);
		}

		public async void PhysicalLogin(string name, string facebookId) {
			try {	
				FirebaseResponse fbResponse = fbClient.Get("users/" + facebookId);
				JObject current = fbResponse.ResultAs<JObject>();

				//If a user is not in the database, assuming they've never logged in before
				if (current == null) {
					JObject userObj = JObject.Parse (@"{ 'fullName' : '" + name + "' }");
					SetResponse set = await fbClient.SetAsync ("users/" + facebookId , userObj);
					JObject result = set.ResultAs<JObject> ();
				}

				loginButton.Visibility = Android.Views.ViewStates.Invisible;
				StartActivity(typeof(HomeActivity));
			} catch (System.AggregateException e) {
				alertDialog.SetMessage (e.Message);
				alertDialog.Show ();
			} catch (FirebaseException e) {
				alertDialog.SetMessage (e.Message);
				alertDialog.Show ();
			} catch (Exception e) {
				alertDialog.SetMessage (e.Message);
				alertDialog.Show ();
			}
		}
	}
}