using System;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.App.ActionBarDrawerToggle;

namespace TapTap
{
	public class MyActionBarDrawerToggle : ActionBarDrawerToggle
	{
		private ActionBarActivity mHost;
		private int mOpenedResource, mClosedResource;

		public MyActionBarDrawerToggle (ActionBarActivity host, DrawerLayout drawerLayout, int openedResource, int closedResource) 
			: base(host, drawerLayout, openedResource, closedResource) {
			mHost = host;
			mOpenedResource = openedResource;
			mClosedResource = closedResource;
		}

		public override void OnDrawerOpened(Android.Views.View drawerView) {
			int drawerType = (int)drawerView.Tag;

			if (drawerType == 0)
			{
				base.OnDrawerOpened (drawerView);
			}
		}

		public override void OnDrawerClosed(Android.Views.View drawerView) {

			int drawerType = (int)drawerView.Tag;

			if (drawerType == 0)
			{
				base.OnDrawerClosed (drawerView);
			}
		}

		public override void OnDrawerSlide(Android.Views.View drawerView, float slideOffset) {
			int drawerType = (int)drawerView.Tag;

			if (drawerType == 0)
			{
				base.OnDrawerSlide (drawerView, slideOffset);
			}
		}
	}
}

