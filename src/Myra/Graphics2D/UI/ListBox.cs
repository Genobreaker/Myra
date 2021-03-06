﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Myra.Attributes;
using Myra.Graphics2D.UI.Styles;
using Newtonsoft.Json;
using static Myra.Graphics2D.UI.Grid;

namespace Myra.Graphics2D.UI
{
	public class ListBox : SingleItemContainer<Grid>
	{
		private ListItem _selectedItem;
		private readonly ObservableCollection<ListItem> _items = new ObservableCollection<ListItem>();

		[EditCategory("Data")]
		public ObservableCollection<ListItem> Items
		{
			get { return _items; }
		}

		[HiddenInEditor]
		[JsonIgnore]
		public ListBoxStyle ListBoxStyle { get; set; }

		[HiddenInEditor]
		[JsonIgnore]
		public int? SelectedIndex
		{
			get
			{
				if (_selectedItem == null)
				{
					return null;
				}

				return _items.IndexOf(_selectedItem);
			}

			set
			{
				if (value == null || value.Value < 0 || value.Value >= Items.Count)
				{
					SelectedItem = null;
					return;
				}

				SelectedItem = Items[value.Value];
			}
		}

		[HiddenInEditor]
		[JsonIgnore]
		public ListItem SelectedItem
		{
			get { return _selectedItem; }

			set
			{
				if (value == _selectedItem)
				{
					return;
				}

				if (_selectedItem != null)
				{
					((Button)_selectedItem.Widget).IsPressed = false;
				}

				_selectedItem = value;

				if (_selectedItem != null)
				{
					((Button)_selectedItem.Widget).IsPressed = true;
					_selectedItem.FireSelected();
				}

				var ev = SelectedIndexChanged;
				if (ev != null)
				{
					ev(this, EventArgs.Empty);
				}
			}
		}

		public event EventHandler SelectedIndexChanged;

		public ListBox(ListBoxStyle style)
		{
			InternalChild = new Grid();

			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;

			if (style != null)
			{
				ApplyListBoxStyle(style);
			}

			_items.CollectionChanged += ItemsOnCollectionChanged;
		}

		public ListBox(string style)
			: this(Stylesheet.Current.ListBoxStyles[style])
		{
		}

		public ListBox()
			: this(
				Stylesheet.Current.ListBoxStyle)
		{
		}

		private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Add:
					{
						var index = args.NewStartingIndex;
						foreach (ListItem item in args.NewItems)
						{
							InsertItem(item, index);
							++index;
						}
						break;
					}

				case NotifyCollectionChangedAction.Remove:
					{
						foreach (ListItem item in args.OldItems)
						{
							RemoveItem(item);
						}
						break;
					}

				case NotifyCollectionChangedAction.Reset:
					{
						while (InternalChild.Widgets.Count > 0)
						{
							RemoveItem((ListItem)InternalChild.Widgets[0].Tag);
						}
						break;
					}
			}

			InvalidateMeasure();
		}

		private void ItemOnChanged(object sender, EventArgs eventArgs)
		{
			var item = (ListItem)sender;

			var button = (Button)item.Widget;
			button.Text = item.Text;
			button.TextColor = item.Color ?? ListBoxStyle.ListItemStyle.LabelStyle.TextColor;

			InvalidateMeasure();
		}

		private void UpdateGridPositions()
		{
			for (var i = 0; i < Items.Count; ++i)
			{
				var widget = InternalChild.Widgets[i];
				widget.GridRow = i;
			}
		}

		private void InsertItem(ListItem item, int index)
		{
			item.Changed += ItemOnChanged;

			Widget widget = null;

			if (!item.IsSeparator)
			{
				widget = new ListButton(ListBoxStyle.ListItemStyle)
				{
					Text = item.Text,
					TextColor = item.Color ?? ListBoxStyle.ListItemStyle.LabelStyle.TextColor,
					Tag = item,
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Stretch,
					Image = item.Image,
					ImageTextSpacing = item.ImageTextSpacing
				};

				((Button)widget).Click += ButtonOnClick;
			}
			else
			{
				widget = new HorizontalSeparator(ListBoxStyle.SeparatorStyle);
			}

			InternalChild.RowsProportions.Insert(index, new Proportion(ProportionType.Auto));
			InternalChild.Widgets.Insert(index, widget);

			item.Widget = widget;

			UpdateGridPositions();
		}

		private void RemoveItem(ListItem item)
		{
			item.Changed -= ItemOnChanged;

			var index = InternalChild.Widgets.IndexOf(item.Widget);
			InternalChild.RowsProportions.RemoveAt(index);
			InternalChild.Widgets.RemoveAt(index);

			if (SelectedItem == item)
			{
				SelectedItem = null;
			}

			UpdateGridPositions();
		}

		private void ButtonOnClick(object sender, EventArgs eventArgs)
		{
			var item = (Button)sender;
			var index = InternalChild.Widgets.IndexOf(item);
			SelectedIndex = index;
		}

		public void ApplyListBoxStyle(ListBoxStyle style)
		{
			ApplyWidgetStyle(style);

			ListBoxStyle = style;

			foreach (var item in Items)
			{
				var asButton = item.Widget as Button;
				if (asButton != null)
				{
					asButton.ApplyButtonStyle(style.ListItemStyle);
				}

				var asSeparator = item.Widget as SeparatorWidget;
				if (asSeparator != null)
				{
					asSeparator.ApplyMenuSeparatorStyle(style.SeparatorStyle);
				}
			}
		}

		protected override void SetStyleByName(Stylesheet stylesheet, string name)
		{
			ApplyListBoxStyle(stylesheet.ListBoxStyles[name]);
		}

		internal override string[] GetStyleNames(Stylesheet stylesheet)
		{
			return stylesheet.ListBoxStyles.Keys.ToArray();
		}
	}
}