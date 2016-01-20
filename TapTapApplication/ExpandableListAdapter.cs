using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android;
using Android.Database;
using System.Linq;

namespace TapTapApplication
{
	public class ExpandableListAdapter: BaseExpandableListAdapter
	{
		Activity context;
		Dictionary<string, List<string>> data;
		List<string> dataHeaders;

		public ExpandableListAdapter (Activity context, Dictionary<string, List<string>> data, List<string> headers)
		{
			this.context = context;
			this.data = data;
			this.dataHeaders = new List<string> (this.data.Keys);
		}
			
		public override Java.Lang.Object GetChild (int groupPosition, int childPosition)
		{
			return data[this.dataHeaders[groupPosition]][childPosition];
		}

		public override long GetChildId (int groupPosition, int childPosition)
		{
			return childPosition;
		}

		public override int GetChildrenCount (int groupPosition)
		{
			return data [this.dataHeaders [groupPosition]].Count;
		}

		public override View GetChildView (int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			View row = convertView;
			if (row == null) {
				row = context.LayoutInflater.Inflate (Resource.Layout.ExpandableListItem, null);
			}

			TextView txtContent = row.FindViewById<TextView> (Resource.Id.txtItemContent);
			txtContent.Text = data[this.dataHeaders[groupPosition]][childPosition];

			return row;
		}

		public override Java.Lang.Object GetGroup (int groupPosition)
		{
			return dataHeaders [groupPosition];
		}

		public override long GetGroupId (int groupPosition)
		{
			return groupPosition;
		}

		public override View GetGroupView (int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			View groupView = convertView;

			if (groupView == null) 
				groupView = context.LayoutInflater.Inflate (Resource.Layout.ExpandableListView, null);


			/*if (isExpanded) {
				group.FindViewById<ImageView> (Resource.Id.arrow).SetImageResource(Resource.Drawable.arrow_up);

			} else {
				group.FindViewById<ImageView> (Resource.Id.arrow).SetImageResource(Resource.Drawable.arrow_down);
			}*/

			groupView.FindViewById<TextView> (Resource.Id.txtListHeader).Text = dataHeaders[groupPosition];
			return groupView;
		}

		public override bool IsChildSelectable (int groupPosition, int childPosition)
		{
			return true;
		}

		public void UpdateData (Dictionary<string, List<string>> data) {
			this.data = data;
			this.dataHeaders = data.Keys.ToList ();
			this.NotifyDataSetChanged ();   
		}

		public override int GroupCount {
			get {
				return dataHeaders.Count;
			}
		}

		public override bool HasStableIds {
			get {
				return true; //What is this?
			}
		}
	}
} 