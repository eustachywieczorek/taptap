using Android.App;
using Android.Widget;
using Android.OS;

using System;
using System.Collections.Generic;

using Auth0.SDK;
using Newtonsoft.Json.Linq;

using FireSharp.Response;
using FireSharp;
using FireSharp.Config;
using Android.Content;

namespace TapTapApplication
{
	[Activity (Name = "damiangrasso.taptapapplication.MainActivity", MainLauncher = true, Theme="@android:style/Theme.Light.NoTitleBar.Fullscreen")]
	public class MainActivity : Activity
	{
		Auth0Client sc;
		FirebaseClient fbClient;
		Button btnLogin;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Main);

			btnLogin = FindViewById <Button> (Resource.Id.btnLogin);

			fbClient = new FirebaseClient (new FirebaseConfig {
				AuthSecret = "FO85aksCBndB5fXAykNFQstlLqqYrHsiq4myZTQW", 
				BasePath = "https://scorching-heat-9815.firebaseio.com/"
			});
					
			sc = new Auth0Client ("testappforportfolio.au.auth0.com", 
				                 "ht0ONVPDNc1PxHT98ruZFRHzUVH2Q7iO");

			btnLogin.Click += delegate {
				Login ();
			};
		}

		public async void Login() {
			try {
				Intent change;

				var user = await sc.LoginAsync (this, "facebook");



				string fullName = user.Profile ["name"].ToString ();
				string userId = user.Profile ["user_id"].ToString ();
				userId = userId.Replace ('|', ':');

				FirebaseResponse fbResponse = fbClient.Get("users/" + userId);
				JObject current = fbResponse.ResultAs<JObject>();

				if (current == null) {
					JObject userObj = JObject.Parse (@"{ 'fullName' : '" + fullName + "' }");
					SetResponse set = await fbClient.SetAsync ("users/" + userId , userObj);
					JObject result = set.ResultAs<JObject> ();

					if (result != null) {
						change = new Intent(this, typeof(HomeActivity));
						StartActivity(change);
					}
				} else {
					change = new Intent(this, typeof(HomeActivity));
					StartActivity(change);
				}
			} catch (System.AggregateException e) {
				//username = e.Message;
			} catch (Exception e) {
		//		username = e.Message;
			}
		}
	}
}