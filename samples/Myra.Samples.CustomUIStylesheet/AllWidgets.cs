/* Generated by Myra UI Editor at 11/16/2017 2:38:36 AM */

using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.ColorPicker;

namespace Myra.Samples.CustomUIStylesheet
{
	public partial class AllWidgets: HorizontalSplitPane
	{
		public AllWidgets()
		{
			BuildUI();

			_button.Click += (sender, args) =>
			{
			};

			_textButton.Click += (sender, args) =>
			{
				var debugWindow = new ColorPickerDialog();
				debugWindow.ShowModal(Desktop);

				debugWindow.Closed += (s, a) =>
				{
					if (!debugWindow.Result)
					{
						return;
					}

					_textButtonLabel.TextColor = debugWindow.Color;
				};
			};

			_imageButton.Click += (sender, args) =>
			{
				var debugWindow = new DebugOptionsDialog();
				debugWindow.ShowModal(Desktop);
			};

			var tree = new Tree
			{
				HasRoot = false,
				GridColumn = 1,
				GridRow = 8
			};
			var node1 = tree.AddSubNode("node1");
			var node2 = node1.AddSubNode("node2");
			var node3 = node2.AddSubNode("node3");
			node3.AddSubNode("node4");
			node3.AddSubNode("node5");
			node2.AddSubNode("node6");
			_gridRight.Widgets.Add(tree);
		}
	}
}