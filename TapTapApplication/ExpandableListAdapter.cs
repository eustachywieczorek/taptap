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

		public ExpandableListAdapter (Activity context, Dictionary<string, List<string>> data)
		{
			this.context = context;
			this.data = data;
		}
			
		public override Java.Lang.Object GetChild (int groupPosition, int childPosition)
		{
			return null;
		}

		public override long GetChildId (int groupPosition, int childPosition)
		{
			return childPosition;
		}

		public override int GetChildrenCount (int groupPosition)
		{
			List<string> keyList = new List<string> (data.Keys);
			return data [keyList[groupPosition]].Count;
		}

		public override View GetChildView (int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			View row = convertView;
			if (row == null) {
				row = context.LayoutInflater.Inflate (Resource.Layout.ExpandableListItem, null);
			}

			TextView txtContent = row.FindViewById<TextView> (Resource.Id.txtItemContent);

			List<string> keyList = new List<string>(data.Keys);
			List<string> children = data [keyList [groupPosition]];

			txtContent.Text = children [childPosition];


			return row;
		}

		public override Java.Lang.Object GetGroup (int groupPosition)
		{
			return null;
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

			List<string> keyList = new List<string>(data.Keys);

			groupView.FindViewById<TextView> (Resource.Id.txtListHeader).Text = keyList[groupPosition];

			return groupView;
		}

		public override bool IsChildSelectable (int groupPosition, int childPosition)
		{
			return true;
		}

		public void UpdateData (Dictionary<string, List<string>> data) {
			this.data = data;
			this.NotifyDataSetChanged ();   
		}

		public override int GroupCount {
			get {
				return data.Count;
			}
		}

		public override bool HasStableIds {
			get {
				return true; //What is this?
			}
		}
	}
} 