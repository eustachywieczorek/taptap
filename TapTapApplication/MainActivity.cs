using Android.App;
using Android.Widget;
using Android.OS;

using System;
using System.Collections.Generic;

using Auth0.SDK;

namespace TapTapApplication
{
	[Activity (Label = "TapTapApplication", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		Auth0Client sc;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Main);



			sc = new Auth0Client ("testappforportfolio.au.auth0.com", 
				                 "ht0ONVPDNc1PxHT98ruZFRHzUVH2Q7iO");

			Button btnLogin = FindViewById <Button> (Resource.Id.btnLogin);
			TextView txtUsername = FindViewById<TextView> (Resource.Id.txtUsername);
			TextView Password = FindViewById<TextView> (Resource.Id.txtPassword);

			btnLogin.Click += delegate {
				LogMeIn ();				
			};

		}

		public async void LogMeIn() {
			await sc.LoginAsync ("facebook", , );
		}


	}
}