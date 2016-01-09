﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android;

namespace TapTapApplication
{
	public class ExpandableListAdapter: BaseExpandableListAdapter
	{
		private Activity _context;

		public ExpandableListAdapter (Activity context, List<string> dataList, string listName)
		{
			_context = context;
			DataList = dataList;
			ListName = listName;
		}

		public List<string> DataList { get; set; }
		public string ListName { get; set; }

		public override View GetGroupView (int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			View group = convertView;

			if (group == null) {
				group = _context.LayoutInflater.Inflate (Resource.Layout.ExpandableListView, null);
			}

			if (isExpanded) {
				
				group.FindViewById<TextView> (Resource.Id.txtListHeader).Text = ListName;
			} else {
				

			}

			return group;
		}

		public override View GetChildView (int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			View row = convertView;
			if (row == null) {
				row = _context.LayoutInflater.Inflate (Resource.Layout.ExpandableListItem, null);
			}

			LinearLayout cover = _context.FindViewById<LinearLayout> (Resource.Id.linItemContainer);

			row.FindViewById<TextView> (Resource.Id.txtItemContent).Text = DataList [childPosition];

			return row;
			//throw new NotImplementedException ();
		}

		public override int GetChildrenCount (int groupPosition) {
			return DataList.Count;
		}

		public override int GroupCount {
			get { return 1; } //Don't know what this is...
		}

		public override Java.Lang.Object GetChild (int groupPosition, int childPosition)
		{
			throw new NotImplementedException ();
		}

		public override long GetChildId (int groupPosition, int childPosition)
		{
			return childPosition;
		}

		public override Java.Lang.Object GetGroup (int groupPosition)
		{
			throw new NotImplementedException ();
		}

		public override long GetGroupId (int groupPosition)
		{
			return groupPosition;
		}

		public override bool IsChildSelectable (int groupPosition, int childPosition)
		{
			throw new NotImplementedException ();
		}

		public override bool HasStableIds {
			get { return true; }
		}
	}
}
